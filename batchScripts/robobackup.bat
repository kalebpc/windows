
:: What this code does.

:: 1. Create \logs folder in source directory and save log file of recent backups (once the file is created, if the backup name is the same logs append).

:: 2. Look for backup.txt located in source directory. Create if not existing. Put instructions for listing folders\files in backup.txt

:: 3. Finds listed folders\files in backup.txt and copies to backup directory.

:: 4. Displays location of log file, exit message and exit code to terminal.

:: ECHO /? for help

@ECHO OFF

:: SETLOCAL /? for help

SETLOCAL

:: SET /? for help and DOS string extraction for ":~start,end"

SET drive=%CD:~0,1%

:: ********\/\/\/\/\/\/\/\/*** CHANGE TO users\onedrive\%username% IF SOURCE IS OneDrive **********************
SET source=users\%username%

:: ********\/\/\/\/*********** ENTER BACKUP FOLDER NAME HERE (also changes name of log file)*******************
SET backup=pcBackup


:: \/\/\/\/\/\/ Setup\Verify log file location \/\/\/\/\/\/

IF NOT EXIST %drive%:\%source%\logs\ (MKDIR %drive%:\%source%\logs)

SET log=%drive%:\%source%\logs\%backup%.log

:: CD /? for help

CD %drive%:\%source%\logs

ECHO ====== %DATE% %TIME% ====== >> %backup%.log

:: /\/\/\/\/\/\ Setup\Verify log file location /\/\/\/\/\/\


:: \/\/\/\/\/\/ Verify source and Setup/Verify destination \/\/\/\/\/\/

:: IF /? for help

IF NOT EXIST %drive%:\%source% (ECHO The system cannot find the path specified. & COLOR 00 & GOTO end) ELSE (SET /P destination=Enter backup drive ^( c ^| d ^| f ^| ... ^) : )

IF /I %destination%==%drive% (ECHO Backup drive must not be same as source drive & COLOR 00 & GOTO end) ELSE (

:: Checking for existing drive
IF NOT EXIST %destination%:\ (ECHO The system cannot find the path specified. & COLOR 00 & GOTO end) ELSE (

:: Checking for existing backup folder on drive. Create if not existing
IF NOT EXIST %destination%:\%backup% (MKDIR %destination%:\%backup% )
))

SET destination=%destination%:\%backup%

:: /\/\/\/\/\/\ Verify source and Setup/Verify destination /\/\/\/\/\/\

:: \/\/\/\/\/\/ Setup/Verify backup.txt (List of folders\files to backup) \/\/\/\/\/\/

CD %drive%:\%source%

IF NOT EXIST %CD%\backup.txt (ECHO :: List folders\files from %drive%:\%source% you want backed up. > backup.txt & (

ECHO :: **Example list**

ECHO ::

ECHO :: Documents\

ECHO :: Downloads\

ECHO :: example.txt

ECHO :: Pictures\MyPictures\picture.jpg

ECHO ::

ECHO :: **LINES BEGINING WITH :: WILL NOT BE BACKED UP**

ECHO :: **Start your list below**

) >> backup.txt & START notepad %CD%\backup.txt & GOTO nopauseexit)

:: /\/\/\/\/\/\ Setup/Verify backup.txt (List of folders\files to backup) /\/\/\/\/\/\

:: FOR /? for help

:: \/\/\/\/\/\/ Copy folders\files listed in backup.txt \/\/\/\/\/\/

:: ROBOCOPY /? for help

FOR /F "delims=" %%a IN ('FINDSTR /v :: %CD%\backup.txt') DO (ROBOCOPY "\%source%\%%a " "%destination%\%%a " /MIR /LOG+:%log%)

:: /\/\/\/\/\/\ Copy folders\files listed in backup.txt /\/\/\/\/\/\

:: \/\/\/\/\/\/ Check for errors and print helpful info if needed \/\/\/\/\/\/

IF %errorlevel% GEQ 16 (ECHO Copy Failed. & GOTO end) ELSE (

IF %errorlevel% GEQ 8 (ECHO Several files didn't copy. & GOTO end) ELSE (

IF %errorlevel% EQU 7 (ECHO Files were copied, a file mismatch was present, and additional files were present. & GOTO end) ELSE (

IF %errorlevel% EQU 6 (ECHO Additional files and mismatched files exist. No files were copied and no failures were encountered meaning that the files already exist in the destination directory. & GOTO end) ELSE (

IF %errorlevel% EQU 5 (ECHO Some files were copied. Some files were mismatched. No failure was encountered. & GOTO end) ELSE (

IF %errorlevel% EQU 3 (ECHO Some files were copied. Additional files were present. No failure was encountered. & GOTO end) ELSE (

IF %errorlevel% EQU 2 (ECHO There are some additional files in the destination directory that aren't present in the source directory. No files were copied. & GOTO end) ELSE (

IF %errorlevel% EQU 1 (ECHO All files were copied successfully. & GOTO end) ELSE (

IF %errorlevel% EQU 0 (ECHO No files were copied. No failure was encountered. No files were mismatched. The files already exist in the destination directory; therefore, the copy operation was skipped.)
))))))))

:: /\/\/\/\/\/\ Check for errors and print helpful info if needed /\/\/\/\/\/\

:END

ECHO Exit %errorlevel%

:: ENDLOCAL /? for help

ENDLOCAL

:: PAUSE /? for help

PAUSE

:NOPAUSEEXIT

:: EXIT /? for help

EXIT
