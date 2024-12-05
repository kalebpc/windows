
:: ******Run from external backup drive******

@ECHO OFF

:: Allows !count! to function
SETLOCAL ENABLEDELAYEDEXPANSION

set meta=0

:: Goto root dir
CD \



:: \/\/\/\/\/\/\/ Options menu \/\/\/\/\/\/\/

:guide

ECHO.

ECHO Choose backup folder : 1

ECHO Create restore.txt file in backup folder enter : 2

ECHO Restore folders\files to homedrive : 3

ECHO.

:choose

ECHO Type "exit" to close window.

SET /P chosen=Enter ^( 1 ^| 2 ^| ...^) : 

IF DEFINED chosen (

IF /I %chosen%==exit (EXIT) ELSE (

IF %chosen%==1 (GOTO folder) ELSE (

IF %chosen%==2 (GOTO textfile) ELSE (

IF %chosen%==3 (GOTO restore) ELSE (GOTO choose))))

) ELSE (GOTO end)

:: /\/\/\/\/\/\/\ Options menu /\/\/\/\/\/\/\



:: \/\/\/\/\/\/\/ Get retorefolder from user \/\/\/\/\/\/\/

:folder

ECHO.

DIR /B /A:D /O:D | FINDSTR /V "restore $"

ECHO.

SET count=0

SET /P restorefolder=Choose backup folder? : 

IF NOT EXIST %restorefolder%\ (

IF /I %restorefolder%==exit (EXIT) ELSE (

IF !count! GTR 2 (ECHO Could not locate folder. & COLOR 00 & GOTO guide) ELSE (

ECHO No folder chosen. & SET /A count=!count! + 1 & GOTO folder)))

IF %meta%==2 (GOTO textfile)

IF %meta%==3 (GOTO restore)

ECHO Backup folder chosen.

GOTO guide

:: /\/\/\/\/\/\/\ Get retorefolder from user /\/\/\/\/\/\/\



:: \/\/\/\/\/\/\/ Create restore.txt file and populate from backup.txt on user's pc \/\/\/\/\/\/\/

:textfile

IF NOT DEFINED restorefolder (SET meta=2 & GOTO folder) ELSE (SET meta=0 & SET list=%restorefolder%\restore.txt)

:: Create file with list of folders/files to restore if not existing, enclose instructions
IF NOT EXIST %list% (ECHO :: **LINES BEGINING WITH :: WILL NOT BE RESTORED** > %list%) ELSE (ECHO File already exists. Find at %CD%%list%  & GOTO guide)

ECHO :: \/**Folders and files that will be restored listed below**\/ >> %list%

IF NOT EXIST %USERPROFILE%\backup.txt (ECHO :: %USERPROFILE%\backup.txt does not exist. Populated with list of backed up folders. >> %list%) ELSE (

FOR /F "delims=" %%a IN ('FINDSTR /V ":: log" "%USERPROFILE%\backup.txt"') DO (ECHO %%a >> %list%)

)

ECHO.

ECHO File created at %CD%%list%

IF %meta%==3 (GOTO restore)

GOTO guide

:: /\/\/\/\/\/\/\ Create restore.txt file and populate from backup.txt on user's pc /\/\/\/\/\/\/\



:: \/\/\/\/\/\/\/ Restore folders to user's pc \/\/\/\/\/\/\/
 
:restore

IF NOT DEFINED restorefolder (SET meta=3 & GOTO folder)

IF NOT EXIST %CD%%restorefolder%\restore.txt (SET meta=3 & GOTO textfile)

SET meta=0

SET list=%restorefolder%\restore.txt

IF NOT EXIST %CD%restore\logs\ (MKDIR %CD%restore\logs\)

SET log=%CD%restore\logs\restore.log


:: ***************************IMPORTANT NOTE: ROBOCOPY NOT USING PURGE WHEN RESTORING***************************
IF EXIST %list% (

FOR /F "delims=" %%a IN ('FINDSTR /v :: %list%') DO (IF EXIST %USERPROFILE%\%%a (ROBOCOPY "%restorefolder%\%%a " "%USERPROFILE%\%%a " *.* /S /E /MT /DCOPY:DA /COPY:DAT /R:1000000 /W:30 /LOG+:%log%) ELSE (MKDIR %USERPROFILE%"\%%a" & ROBOCOPY "%restorefolder%\%%a " "%USERPROFILE%\%%a " *.* /S /E /MT /DCOPY:DA /COPY:DAT /R:1000000 /W:30 /LOG+:%log%))

)

:: /\/\/\/\/\/\/\ Restore folders to user's pc /\/\/\/\/\/\/\



:end

:: \/\/\/\/\/\/\/ Error check \/\/\/\/\/\/\/

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

:: /\/\/\/\/\/\/\ Error check /\/\/\/\/\/\/\



ECHO.

ECHO logs\ was skipped.

ECHO.

ECHO Exit %ERRORLEVEL%

PAUSE

EXIT
