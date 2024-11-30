:: Instructions to Backup pc

:: Put backup folder/file names in backup.txt located at c:\users\%username%\backup.txt directory

:: ex. backup.txt ; List directories and/or files

:: Desktop\,
:: Documents\,
:: Downloads\,
:: example.txt,

:: Or be more specific when listing folders, add subdirectories

:: Desktop\exact\path\to\folderOrfolder\I\want\backed\up,
:: Documents\exact\path\to\folderOrfolder\I\want\backed\up.txt,

:: ECHO /? for help

@ECHO OFF

:: COLOR /? for help

COLOR f0

:: SETLOCAL /? for help

SETLOCAL

:: SET /? for help

SET homedisc=c

SET homedirectory=users\%username%

:: IF /? for help

IF NOT EXIST %homedisc%:\%homedirectory% (ECHO The system cannot find the path specified. & COLOR 00 & GOTO end) ELSE (SET /P backupdirectory=Enter backup disc letter ^( c ^| d ^| f ^| ... ^) : )

IF %backupdirectory%==c (ECHO Backup disc must not be same as Source disc & COLOR 00 & GOTO end) ELSE (

IF NOT EXIST %backupdirectory%:\ (ECHO The system cannot find the path specified. & COLOR 00 & GOTO end) ELSE (SET backupdirectory=%backupdirectory%:\pcBackup)

)

SET loglocation=%homedisc%:\%homedirectory%\logs\pcbackup.log

IF NOT EXIST %homedisc%:\%homedirectory%\logs\ (MKDIR %homedisc%:\%homedirectory%\logs) ELSE (ECHO.)

:: CD /? for help

CD %homedisc%:\%homedirectory%\logs

ECHO - >> pcbackup.log

ECHO ====== %DATE% %TIME% ====== >> pcbackup.log

ECHO - >> pcbackup.log

CD %homedisc%:\%homedirectory%

:: FOR /? for help

:: ROBOCOPY /? for help

FOR /F "delims=," %%a IN (%homedisc%:\%homedirectory%\backup.txt) DO (ROBOCOPY "\%homedirectory%\%%a " "%backupdirectory%\%%a " /B /MIR /LOG+:%loglocation%)

IF %errorlevel% GEQ 16 (ECHO Copy Failed. & GOTO end) ELSE (
IF %errorlevel% GEQ 8 (ECHO Several files didn't copy. & GOTO end) ELSE (
IF %errorlevel% EQU 7 (ECHO Files were copied, a file mismatch was present, and additional files were present. & GOTO end) ELSE (
IF %errorlevel% EQU 6 (ECHO Additional files and mismatched files exist. No files were copied and no failures were encountered meaning that the files already exist in the destination directory. & GOTO end) ELSE (
IF %errorlevel% EQU 5 (ECHO Some files were copied. Some files were mismatched. No failure was encountered. & GOTO end) ELSE (
IF %errorlevel% EQU 3 (ECHO Some files were copied. Additional files were present. No failure was encountered. & GOTO end) ELSE (
IF %errorlevel% EQU 2 (ECHO There are some additional files in the destination directory that aren't present in the source directory. No files were copied. & GOTO end) ELSE (
IF %errorlevel% EQU 1 (ECHO All files were copied successfully. & GOTO end) ELSE (
IF %errorlevel% EQU 0 (ECHO No files were copied. No failure was encountered. No files were mismatched. The files already exist in the destination directory; therefore, the copy operation was skipped.)))))))))

:END

ECHO Exit %errorlevel%

:: ENDLOCAL /? for help

ENDLOCAL

:: PAUSE /? for help

PAUSE

:: EXIT /? for help

EXIT
