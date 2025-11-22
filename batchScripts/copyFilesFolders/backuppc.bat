
@REM -- What this code does.
@REM -- 1. Check and/or create Backup_PC folder in !USERPROFILE!\AppData\Local\Programs\ directory.
@REM -- 2. Check and/or create logs directory in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
@REM -- 3. Check and/or create !BACKUPLISTFILE! in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
@REM -- 4. Check and/or create backup destination folder.
@REM -- 5. Finds list of folders\files in !BACKUPLISTFILE! to copy.
@REM -- 6. Copy directories and files to destination folder.
@REM -- 7. Displays location of log file, exit message and exit code to terminal.

@REM -- ECHO /? for help
@ECHO OFF
@REM -- SETLOCAL /? for help
SETLOCAL EnableDelayedExpansion
SET INSTALLDIRNAME="Backup_PC"
SET BACKUPDESTNAME="pcBackup"
SET BACKUPLISTFILE="backup_list.txt"
SET FILELOG="!BACKUPDESTNAME!.log"
SET PATHINSTALLDIR="!USERPROFILE!\AppData\Local\Programs\!INSTALLDIRNAME:"=!"
SET PATHLOGSDIR="!PATHINSTALLDIR:"=!\logs"
SET PATHBACKUPLISTFILE="!PATHINSTALLDIR:"=!\!BACKUPLISTFILE:"=!"
@REM -- Create !INSTALLDIRNAME! folder in !USERPROFILE!\AppData\Local\Programs\ directory.
IF NOT EXIST !PATHINSTALLDIR! (
    @REM -- User prompt
    ECHO. & ECHO !PATHINSTALLDIR! does not exist. & ECHO.
    SET /P AREYOUSURE="Do you want to create it now? (Y/[N])?"
    IF /I "!AREYOUSURE!" NEQ "Y" GOTO END
    MKDIR !PATHINSTALLDIR!
    IF NOT EXIST !PATHINSTALLDIR! ( ECHO. & ECHO Could not create !PATHINSTALLDIR!. & GOTO END )
)
@REM -- Create logs directory in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
IF NOT EXIST !PATHLOGSDIR! (
    @REM -- User prompt
    ECHO. & ECHO !PATHLOGSDIR! does not exist. & ECHO.
    SET /P AREYOUSURE="Do you want to create it now? (Y/[N])?"
    IF /I "!AREYOUSURE!" NEQ "Y" GOTO END
    MKDIR !PATHLOGSDIR!
    IF NOT EXIST !PATHLOGSDIR! ( ECHO. & ECHO Could not create !PATHLOGSDIR!. & GOTO END )
)
@REM -- Create !BACKUPLISTFILE! in !USERPROFILE!\AppData\Local\Programs\Backup_PC directory.
IF NOT EXIST !PATHBACKUPLISTFILE! (
    @REM -- User prompt
    ECHO. & ECHO !PATHBACKUPLISTFILE! does not exist. & ECHO.
    SET /P AREYOUSURE="Do you want to create it now? (Y/[N])?"
    IF /I "!AREYOUSURE!" NEQ "Y" GOTO END
    GOTO MKBACKUPFILE
)
ECHO ====== %DATE% %TIME% ====== >> "!PATHLOGSDIR:"=!\!FILELOG:"=!"
:SETDESTINATION
SET /P USERDRIVEINPUT=Enter backup drive ^( c ^| d ^| f ^| ... ^) : 
@REM -- IF /? for help
IF /I !USERDRIVEINPUT!==!HOMEDRIVE::=! ( ECHO Backup drive must not be same as source drive. & GOTO SETDESTINATION )
@REM -- Checking for drive existence
IF NOT EXIST !USERDRIVEINPUT!:\ ( ECHO The system cannot find the path specified. & GOTO SETDESTINATION )
SET DESTINATION=!USERDRIVEINPUT!:\!BACKUPDESTNAME:"=!
@REM -- Checking for existing backup folder on backup drive. Create if not existing
IF NOT EXIST !DESTINATION! ( MKDIR !DESTINATION! ) ELSE ( ECHO !DESTINATION! - Drive located. )
@REM -- FOR /? for help
@REM -- ROBOCOPY /? for help
SET i=0
FOR /f "delims=" %%A IN ('FINDSTR /V :: !PATHBACKUPLISTFILE!') DO (
    SET TEMP=%%~dA
    SET TEMP=!TEMP::=!
    ROBOCOPY "%%A" "!DESTINATION!\!TEMP!%%~pnxA" /tee /e /z /xjd /LOG+:"!PATHLOGSDIR:"=!\%%~nA_!FILELOG:"=!"
)
@REM -- /e    : copies subdirs includes empty dirs.
@REM -- /z    : Copies files in restartable mode.
@REM -- /xx   : Excluding extra files doesn't delete files from the destination and suppress extra files on logs.
@REM -- /LOG+ : Append to end of log file.
ECHO.
IF !ERRORLEVEL! GEQ 16 (ECHO "Copy Failed." & GOTO end) ELSE (
IF !ERRORLEVEL! GEQ 8 (ECHO Several files didn't copy. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 7 (ECHO Files were copied, a file mismatch was present, and additional files were present. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 6 (ECHO Additional files and mismatched files exist. No files were copied and no failures were encountered meaning that the files already exist in the destination directory. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 5 (ECHO Some files were copied. Some files were mismatched. No failure was encountered. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 3 (ECHO Some files were copied. Additional files were present. No failure was encountered. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 2 (ECHO There are some additional files in the destination directory that aren't present in the source directory. No files were copied. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 1 (ECHO All files were copied successfully. & GOTO end) ELSE (
IF !ERRORLEVEL! EQU 0 (ECHO No files were copied. No failure was encountered. No files were mismatched. The files already exist in the destination directory; therefore, the copy operation was skipped.)
))))))))
GOTO END
:MKBACKUPFILE
ECHO :: ** List fully qualified paths you want backed up. ** > !PATHBACKUPLISTFILE! & (
    ECHO ::
    ECHO :: ** LINES BEGINING WITH "::" WILL NOT BE BACKED UP **
    ECHO ::
    ECHO :: ** Start your list below **
    ECHO !PATHINSTALLDIR:"=!
    ECHO !USERPROFILE!\AppData
    ECHO !USERPROFILE!\Desktop
    ECHO !USERPROFILE!\Downloads
    ECHO !USERPROFILE!\Documents
    ECHO !USERPROFILE!\Music
    ECHO !USERPROFILE!\Pictures
    ECHO !USERPROFILE!\Videos 
) >> !PATHBACKUPLISTFILE!
IF EXIST !PATHBACKUPLISTFILE! ( START notepad !PATHBACKUPLISTFILE! & EXIT ) ELSE ( ECHO. & ECHO Could not create !PATHBACKUPLISTFILE!. )
:END
ECHO.
ECHO Exit !ERRORLEVEL!
ECHO List of backed up files/folders located at !PATHBACKUPLISTFILE!.
@REM -- ENDLOCAL /? for help
ENDLOCAL
@REM -- PAUSE /? for help
PAUSE
@REM -- EXIT /? for help
EXIT
