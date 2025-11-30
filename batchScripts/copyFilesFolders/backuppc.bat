@REM Backuppc © 2025 https://github.com/kalebpc/windows

@REM -- What this code does.
@REM -- 1. Check and/or create Backup_PC folder in !USERPROFILE!\AppData\Local\Programs\ directory.
@REM -- 2. Check and/or create logs directory in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
@REM -- 3. Check and/or create !BACKUPLISTFILE! in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
@REM -- 4. Check and/or create backup destination folder.
@REM -- 5. Finds list of folders\files in !BACKUPLISTFILE! to copy.
@REM -- 6. Copy directories and files to destination folder.
@REM -- 7. Displays location of log file, exit message and exit code to terminal.

@REM -- !!!!!!!!!!!! CHECK BACKUP DRIVE FORMAT !!!!!!!!!!!!
@REM -- Format backup drive to exFat for platform cross compatibility and support for files larger than 4GB.

@REM -- ECHO /? for help
@ECHO OFF
ECHO Backuppc © 2025 https://github.com/kalebpc/windows
@REM -- SETLOCAL /? for help
SETLOCAL EnableDelayedExpansion
SET THREADZ=4
SET "INSTALLDIRNAME=Backup_PC"
SET "PATHINSTALLDIR=!USERPROFILE!\AppData\Local\Programs\!INSTALLDIRNAME!"
SET "PATHENVFILE=!PATHINSTALLDIR!\.env"
@REM -- Create .env file if it does not exist.
IF NOT EXIST !PATHENVFILE! (
    SET "BACKUPDESTNAME=pcBackup"
    SET "BACKUPLISTFILE=backup_list.txt"
    SET "MIRBACKUPLISTFILE=mirror_backup_list.txt"
    SET "FILELOG=!BACKUPDESTNAME!.log"
    SET "PATHLOGSDIR=!PATHINSTALLDIR!\logs"
    SET "PATHBACKUPLISTFILE=!PATHINSTALLDIR!\!BACKUPLISTFILE!"
    SET "PATHMIRBACKUPLISTFILE=!PATHINSTALLDIR!\!MIRBACKUPLISTFILE!"
    ECHO ^|
    CHOICE /C YN /T 15 /D N /M "Do you want to set custom Backup Folder name? ('N' for Default Setup)"
    IF !ERRORLEVEL! EQU 1 (
        :SETBACKUPDESTNAME
        SET /p "TEMPBACKUPDESTNAME=Set Backup Folder name : "
        IF /i "!TEMPBACKUPDESTNAME!" EQU "" ( GOTO SETBACKUPDESTNAME )
        CHOICE /C YN /T 15 /D N /M "Are you sure you want to change Backup Folder name to '!TEMPBACKUPDESTNAME!'?"
        IF !ERRORLEVEL! EQU 2 ( GOTO SETBACKUPDESTNAME ) ELSE ( SET "BACKUPDESTNAME=TEMPBACKUPDESTNAME" )
    )
    (   ECHO "BACKUPDESTNAME=!BACKUPDESTNAME!"
        ECHO "BACKUPLISTFILE=backup_list.txt"
        ECHO "MIRBACKUPLISTFILE=mirror_backup_list.txt"
        ECHO "FILELOG=!BACKUPDESTNAME!.log"
        ECHO "PATHLOGSDIR=!PATHINSTALLDIR!\logs"
        ECHO "PATHBACKUPLISTFILE=!PATHINSTALLDIR!\!BACKUPLISTFILE!"
        ECHO "PATHMIRBACKUPLISTFILE=!PATHINSTALLDIR!\!MIRBACKUPLISTFILE!"
    ) > !PATHENVFILE!
    TIMEOUT /s 5
)
@REM -- Set env vars from .env
FOR /F "tokens=* eol=#" %%I IN (!PATHENVFILE!) DO SET %%I
ECHO ^|
@REM -- Create !INSTALLDIRNAME! folder in !USERPROFILE!\AppData\Local\Programs\ directory.
IF NOT EXIST !PATHINSTALLDIR! (
    @REM -- User prompt
    ECHO ^| & ECHO !PATHINSTALLDIR! does not exist. & ECHO ^|
    CHOICE /C YNC /T 15 /D Y /M "Do you want to create it now?  ('Y'-Yes[Default] | 'N'-No | 'C'-Cancel)"
    IF !ERRORLEVEL! NEQ 1 ( GOTO END )
    SET ERRORLEVEL=0
    MKDIR !PATHINSTALLDIR!
    IF NOT EXIST !PATHINSTALLDIR! ( ECHO ^| & ECHO Could not create !PATHINSTALLDIR!. & GOTO END )
)
@REM -- Create logs directory in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
IF NOT EXIST !PATHLOGSDIR! (
    @REM -- User prompt
    ECHO ^| & ECHO !PATHLOGSDIR! does not exist. & ECHO ^|
    CHOICE /C YNC /T 15 /D Y /M "Do you want to create it now?  ('Y'-Yes[Default] | 'N'-No | 'C'-Cancel)"
    IF !ERRORLEVEL! NEQ 1 ( GOTO END )
    SET ERRORLEVEL=0
    MKDIR !PATHLOGSDIR!
    IF NOT EXIST !PATHLOGSDIR! ( ECHO ^| & ECHO Could not create !PATHLOGSDIR!. & GOTO END )
)
@REM -- Create !BACKUPLISTFILE! in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
IF NOT EXIST !PATHBACKUPLISTFILE! (
    @REM -- User prompt
    ECHO ^| & ECHO !PATHBACKUPLISTFILE! does not exist. & ECHO ^|
    CHOICE /C YNC /T 15 /D Y /M "Do you want to create it now?  ('Y'-Yes[Default] | 'N'-No | 'C'-Cancel)"
    IF !ERRORLEVEL! NEQ 1 ( GOTO END )
    SET ERRORLEVEL=0
    GOTO MKBACKUPFILE
)
@REM ** Added for mirror backup
IF NOT EXIST !PATHMIRBACKUPLISTFILE! (
    @REM -- User prompt
    ECHO ^| & ECHO !PATHMIRBACKUPLISTFILE! does not exist. & ECHO ^|
    CHOICE /C YNC /T 15 /D Y /M "Do you want to create it now?  ('Y'-Yes[Default] | 'N'-No | 'C'-Cancel)"
    IF !ERRORLEVEL! NEQ 1 ( GOTO END )
    SET ERRORLEVEL=0
    GOTO MKMIRBACKUPFILE
)
SET "LASTBACKUP=!PATHLOGSDIR!\LastBackup_!FILELOG!"
@REM -- Change the character encoding of the terminal shown
@CHCP 65001 > !LASTBACKUP!
FOR /f "tokens=*" %%A IN ('TIME /t') DO SET "BACKUPTIME=%%A"
(   ECHO -------------------------------------------------------------------------------
    ECHO    ROBOCOPY     ::     Robust File Copy for Windows
    ECHO -------------------------------------------------------------------------------
    ECHO    Started  : !DATE! !BACKUPTIME!
) > !LASTBACKUP!
:SETDESTINATION
ECHO ^|
SET X=0
SET "RESULT="
FOR /f "delims=" %%N IN (
    'POWERSHELL "&{"^
    "Add-Type -AssemblyName System.Windows.Forms;"^
    "$folderBrowserDialog=New-Object System.Windows.Forms.FolderBrowserDialog;"^
    "$folderBrowserDialog.Description='Choose backup drive ( C | D | F | ... ).';"^
    "$folderBrowserDialog.ShowDialog();"^
    "ECHO $folderBrowserDialog.SelectedPath"^
    "}"'
) DO ( FOR %%O IN ("%%N") DO ( IF !X! EQU 1 ( SET "USERDRIVEINPUT=%%~dN" ) ELSE ( SET "RESULT=%%N" & SET /a X+=1 ) ) )
IF /i "!RESULT!" EQU "Cancel" ( EXIT )
ECHO !USERDRIVEINPUT!
ECHO ^|
@REM -- IF /? for help
IF /I !USERDRIVEINPUT!==!HOMEDRIVE! ( ECHO Backup drive must not be same as source drive. & GOTO SETDESTINATION )
@REM -- Checking for drive existence
IF NOT EXIST !USERDRIVEINPUT! ( ECHO The system cannot find the path specified. & GOTO SETDESTINATION )
SET "DESTINATION=!USERDRIVEINPUT!\!BACKUPDESTNAME!"
@REM -- Checking for existing backup folder on backup drive. Create if not existing
IF NOT EXIST !DESTINATION! ( MKDIR !DESTINATION! ) ELSE ( ECHO !DESTINATION!    Backup located. )
@REM -- FOR /? for help
@REM -- ROBOCOPY /? for help
FOR /f "delims=" %%B IN ('FINDSTR /V :: !PATHBACKUPLISTFILE!') DO (
    SET "TEMP=%%~dB"
    SET "TEMP=!TEMP::=!"
    ROBOCOPY "%%B" "!DESTINATION!\!TEMP!%%~pnxB" /tee /e /mt:!THREADZ! /z /xjd /unicode /unilog+:"!PATHLOGSDIR!\%%~nB_!FILELOG!"
)
@REM -- /e    : copies subdirs includes empty dirs.
@REM -- /z    : Copies files in restartable mode.
@REM -- /unilog+ : Append to end of unicode log file.
@REM ** Added for mirror backup
FOR /f "delims=" %%C IN ('FINDSTR /V :: !PATHMIRBACKUPLISTFILE!') DO (
    SET "TEMP=%%~dC"
    SET "TEMP=!TEMP::=!"
    ROBOCOPY "%%C" "!DESTINATION!\!TEMP!%%~pnxC" /tee /mt:!THREADZ! /z /mir /xjd /unicode /unilog+:"!PATHLOGSDIR!\%%~nC_!FILELOG!"
)
@REM -- /mir  : Mirror destination to source. This WILL delete extra files in destination.
FOR /f "tokens=*" %%D IN ('TIME /t') DO SET "BACKUPTIME=%%D"
(   ECHO    Finished : !DATE! !BACKUPTIME!
    ECHO.
    ECHO -------------------------------------------------------------------------------
) >> !LASTBACKUP!
ECHO ^|
IF !ERRORLEVEL! GEQ 16 ( ECHO "Copy Failed." & GOTO END )
IF !ERRORLEVEL! GEQ 8 ( ECHO Several files didn't copy. & GOTO END )
IF !ERRORLEVEL! EQU 7 ( ECHO Files were copied, a file mismatch was present, and additional files were present. & GOTO END )
IF !ERRORLEVEL! EQU 6 ( ECHO Additional files and mismatched files exist. No files were copied and no failures were encountered meaning that the files already exist in the destination directory. & GOTO END )
IF !ERRORLEVEL! EQU 5 ( ECHO Some files were copied. Some files were mismatched. No failure was encountered. & GOTO END )
IF !ERRORLEVEL! EQU 3 ( ECHO Some files were copied. Additional files were present. No failure was encountered. & GOTO END )
IF !ERRORLEVEL! EQU 2 ( ECHO There are some additional files in the destination directory that aren't present in the source directory. No files were copied. & GOTO END )
IF !ERRORLEVEL! EQU 1 ( ECHO All files were copied successfully. & GOTO END )
IF !ERRORLEVEL! EQU 0 ( ECHO "No files were copied. No failure was encountered. No files were mismatched. The files already exist in the destination directory; therefore, the copy operation was skipped." )
GOTO END
:MKBACKUPFILE
ECHO ^|
CHOICE /C YN /T 15 /D N /M "Do you want to add custom paths to !BACKUPLISTFILE!? ('N'[Default Setup])"
IF !ERRORLEVEL! EQU 1 (
    SET "LISTBACKUPPATHS="
    :ADDPATHSLOOP
    SET X=0
    FOR /f "delims=" %%L IN (
        'POWERSHELL "&{"^
        "Add-Type -AssemblyName System.Windows.Forms;"^
        "$folderBrowserDialog=New-Object System.Windows.Forms.FolderBrowserDialog;"^
        "$folderBrowserDialog.Description='Choose Folder to Copy.';"^
        "$folderBrowserDialog.ShowDialog();"^
        "ECHO $folderBrowserDialog.SelectedPath"^
        "}"'
    ) DO ( FOR %%M IN ("%%L") DO ( IF !X! EQU 1 ( SET "LISTBACKUPPATHS=!LISTBACKUPPATHS! %%M" ) ELSE ( SET /a X+=1 ) ) )
    IF /i "!TEMPPATH!" EQU "Cancel" (
        CHOICE /C YN /T 15 /D Y /M "Are you done adding folders?"
        IF !ERRORLEVEL! EQU 1 ( GOTO DONEADDINGPATHS )
    )
    ECHO ^| & ECHO Current list of folders to backup. & ECHO ^|
    FOR %%M IN (!LISTBACKUPPATHS!) DO ( ECHO %%~M )
    ECHO ^|
    CHOICE /C YN /T 15 /D N /M "Add another folder to list?"
    IF !ERRORLEVEL! EQU 1 ( GOTO ADDPATHSLOOP )
    :DONEADDINGPATHS
    ECHO ^|
) ELSE ( SET "LISTBACKUPPATHS=!USERPROFILE!\Music !USERPROFILE!\Pictures !USERPROFILE!\Videos" )
(   ECHO :: ** List fully qualified paths you want backed up. **
    ECHO ::
    ECHO !PATHINSTALLDIR!
    ECHO ::
    ECHO :: ** LINES BEGINING WITH ":: " WILL NOT BE BACKED UP **
    ECHO ::
    ECHO :: ** Start your list below **
    ECHO ::
) > !PATHBACKUPLISTFILE!
FOR %%N IN (!LISTBACKUPPATHS!) DO ( ECHO %%~N >> !PATHBACKUPLISTFILE! )
IF EXIST !PATHBACKUPLISTFILE! ( START notepad !PATHBACKUPLISTFILE! ) ELSE ( ECHO ^| & ECHO Could not create !PATHBACKUPLISTFILE!. )
@REM ** Added for mirror backup
:MKMIRBACKUPFILE
IF NOT EXIST !PATHMIRBACKUPLISTFILE! (
    ECHO ^|
    CHOICE /C YN /T 15 /D N /M "Do you want to add custom paths to !MIRBACKUPLISTFILE!? ('N'[Default Setup])"
    IF !ERRORLEVEL! EQU 1 (
        SET "LISTBACKUPPATHS="
        :ADDPATHSLOOPMIR
        SET X=0
        FOR /f "delims=" %%L IN (
            'POWERSHELL "&{"^
            "Add-Type -AssemblyName System.Windows.Forms;"^
            "$folderBrowserDialog=New-Object System.Windows.Forms.FolderBrowserDialog;"^
            "$folderBrowserDialog.Description='Choose Folder to Copy (Mirrored).';"^
            "$folderBrowserDialog.ShowDialog();"^
            "ECHO $folderBrowserDialog.SelectedPath"^
            "}"'
        ) DO ( FOR %%M IN ("%%L") DO ( IF !X! EQU 1 ( SET "LISTBACKUPPATHS=!LISTBACKUPPATHS! %%M" ) ELSE ( SET /a X+=1 ) ) )
        IF /i "!TEMPPATH!" EQU "Cancel" (
            CHOICE /C YN /T 15 /D Y /M "Are you done adding folders?"
            IF !ERRORLEVEL! EQU 1 ( GOTO DONEADDINGPATHSMIR )
        )
        ECHO ^| & ECHO Current list of folders to backup. & ECHO ^|
        FOR %%M IN (!LISTBACKUPPATHS!) DO ( ECHO %%~M )
        ECHO ^|
        CHOICE /C YN /T 15 /D N /M "Add another folder to list?"
        IF !ERRORLEVEL! EQU 1 ( GOTO ADDPATHSLOOPMIR )
        :DONEADDINGPATHSMIR
        ECHO ^|
    ) ELSE ( SET "LISTBACKUPPATHS=!USERPROFILE!\AppData !USERPROFILE!\Desktop !USERPROFILE!\Downloads !USERPROFILE!\Documents" )
    (   ECHO :: ** List fully qualified paths you want backed up. **
        ECHO ::
        ECHO !PATHINSTALLDIR!
        ECHO ::
        ECHO :: ** LINES BEGINING WITH ":: " WILL NOT BE BACKED UP **
        ECHO ::
        ECHO :: ** DESTINATION WILL BE MIRRORED OF SOURCE **
        ECHO :: ** - Anything that gets deleted from source will also be PERMANENTLY DELETED from backup destination **
        ECHO ::
        ECHO :: ** Start your list below **
        ECHO ::
    ) > !PATHMIRBACKUPLISTFILE!
    FOR %%N IN (!LISTBACKUPPATHS!) DO ( ECHO %%~N >> !PATHMIRBACKUPLISTFILE! )
    IF EXIST !PATHMIRBACKUPLISTFILE! ( START notepad !PATHMIRBACKUPLISTFILE! & EXIT ) ELSE ( ECHO ^| & ECHO Could not create !PATHMIRBACKUPLISTFILE!. )
)
:END
ECHO ^|
ECHO Exit !ERRORLEVEL!
ECHO List of backed up files/folders located at !PATHBACKUPLISTFILE!.
ECHO List of Mirrored files/folders located at  !PATHMIRBACKUPLISTFILE!.
@REM -- ENDLOCAL /? for help
ENDLOCAL
@REM -- PAUSE /? for help
PAUSE
@REM -- EXIT /? for help
EXIT
