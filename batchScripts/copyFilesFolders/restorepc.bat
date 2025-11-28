@REM -- Restorepc © 2025 https://github.com/kalebpc/windows

@REM -- ECHO /?
@ECHO OFF
ECHO.
ECHO Restorepc © 2025 https://github.com/kalebpc/windows
SETLOCAL ENABLEDELAYEDEXPANSION
ECHO.
ECHO Choose Source folder.
ECHO.
ECHO Opening Folder Explorer...
TIMEOUT 5
FOR /f "delims=" %%A IN (
    'POWERSHELL "&{"^
    "Add-Type -AssemblyName System.Windows.Forms;"^
    "$folderBrowserDialog=New-Object System.Windows.Forms.FolderBrowserDialog;"^
    "$folderBrowserDialog.Description='Choose Source Folder to Copy from.';"^
    "$folderBrowserDialog.InitialDirectory=[Environment]::GetFolderPath([System.Environment+SpecialFolder]::UserProfile);"^
    "$folderBrowserDialog.Filter='All files (*.*)|*.*';"^
    "$folderBrowserDialog.FilterIndex=2;"^
    "$folderBrowserDialog.RestoreDirectory=true;"^
    "$folderBrowserDialog.ShowDialog();"^
    "ECHO $folderBrowserDialog.SelectedPath"^
    "}"'
) do set "PATHSOURCE=%%A"
IF /i "!PATHSOURCE!" EQU "Cancel" ( EXIT )
ECHO Source folder : !PATHSOURCE!
ECHO.
ECHO Choose Destination folder.
ECHO.
ECHO Opening Folder Explorer...
TIMEOUT 5
FOR /f "delims=" %%B IN (
    'POWERSHELL "&{"^
    "Add-Type -AssemblyName System.Windows.Forms;"^
    "$folderBrowserDialog=New-Object System.Windows.Forms.FolderBrowserDialog;"^
    "$folderBrowserDialog.Description='Choose Source Folder to Copy from.';"^
    "$folderBrowserDialog.InitialDirectory=[Environment]::GetFolderPath([System.Environment+SpecialFolder]::UserProfile);"^
    "$folderBrowserDialog.Filter='All files (*.*)|*.*';"^
    "$folderBrowserDialog.FilterIndex=2;"^
    "$folderBrowserDialog.RestoreDirectory=true;"^
    "$folderBrowserDialog.ShowDialog();"^
    "ECHO $folderBrowserDialog.SelectedPath"^
    "}"'
) do set "PATHDEST=%%B"
IF /i "!PATHDEST!" EQU "Cancel" ( EXIT )
ECHO Destination folder : !PATHDEST!
ECHO.
ECHO You are about to copy ALL folders and files from SOURCE : "!PATHSOURCE!" to DESTINATION : "!PATHDEST!"
CHOICE /C YN /T 30 /D N /M "Are you sure you want to continue?     ('Y'es|'N'o[Default])"
IF !ERRORLEVEL! NEQ 1 ( EXIT )
@REM -- Copy from Source to Destination.
@REM -- FOR /? for help
@REM -- ROBOCOPY /? for help
SET THREADZ=4
SET NAMELOGFILE="restorepc.log"
SET PATHLOGSDIR="!USERPROFILE!\AppData\Local\Programs\Backup_PC\logs"
IF EXIST !PATHLOGSDIR! ( 
    ROBOCOPY "!PATHSOURCE!" "!PATHDEST!" /tee /e /mt:!THREADZ! /z /xjd /unicode /unilog+:"!USERPROFILE!\AppData\Local\Programs\Backup_PC\logs\!NAMELOGFILE:"=!"
) ELSE (
    ECHO System could not find "!PATHLOGSDIR:"=!\!NAMELOGFILE:"=!"
    ECHO Choose Log folder.
    ECHO.
    ECHO Opening Folder Explorer...
    TIMEOUT 5
    FOR /f "delims=" %%C IN (
        'POWERSHELL "&{"^
        "Add-Type -AssemblyName System.Windows.Forms;"^
        "$folderBrowserDialog=New-Object System.Windows.Forms.FolderBrowserDialog;"^
        "$folderBrowserDialog.Description='Choose Source Folder to Copy from.';"^
        "$folderBrowserDialog.InitialDirectory=[Environment]::GetFolderPath([System.Environment+SpecialFolder]::UserProfile);"^
        "$folderBrowserDialog.Filter='All files (*.*)|*.*';"^
        "$folderBrowserDialog.FilterIndex=2;"^
        "$folderBrowserDialog.RestoreDirectory=true;"^
        "$folderBrowserDialog.ShowDialog();"^
        "ECHO $folderBrowserDialog.SelectedPath"^
        "}"'
    ) do set "PATHLOG=%%C"
    IF /i "!PATHLOG!" EQU "Cancel" ( EXIT )
    ECHO Log folder : !PATHLOG!
    ECHO.
    ROBOCOPY "!PATHSOURCE!" "!PATHDEST!" /tee /e /mt:!THREADZ! /z /xjd /unicode /unilog+:"!PATHLOG:"=!\!NAMELOGFILE:"=!"
)
@REM -- /e       : copies subdirs includes empty dirs.
@REM -- /z       : Copies files in restartable mode.
@REM -- /unilog+ : Append to end of unicode log file.
ECHO.
IF !ERRORLEVEL! GEQ 16 ( ECHO "Copy Failed." & GOTO END )
IF !ERRORLEVEL! GEQ 8 ( ECHO Several files didn't copy. & GOTO END )
IF !ERRORLEVEL! EQU 7 ( ECHO Files were copied, a file mismatch was present, and additional files were present. & GOTO END )
IF !ERRORLEVEL! EQU 6 ( ECHO Additional files and mismatched files exist. No files were copied and no failures were encountered meaning that the files already exist in the destination directory. & GOTO END )
IF !ERRORLEVEL! EQU 5 ( ECHO Some files were copied. Some files were mismatched. No failure was encountered. & GOTO END )
IF !ERRORLEVEL! EQU 3 ( ECHO Some files were copied. Additional files were present. No failure was encountered. & GOTO END )
IF !ERRORLEVEL! EQU 2 ( ECHO There are some additional files in the destination directory that aren't present in the source directory. No files were copied. & GOTO END )
IF !ERRORLEVEL! EQU 1 ( ECHO All files were copied successfully. & GOTO END )
IF !ERRORLEVEL! EQU 0 ( ECHO "No files were copied. No failure was encountered. No files were mismatched. The files already exist in the destination directory; therefore, the copy operation was skipped." )
:END
ECHO.
ECHO Exit !ERRORLEVEL!
ENDLOCAL
PAUSE
EXIT
