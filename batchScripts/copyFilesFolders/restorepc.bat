
@REM -- ECHO /?
@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION
SET RESTORESOURCENAME="pcBackup"
@REM -- Get backup drive path from user
:CHOOSESOURCEDRIVE
SET /p SOURCEDRIVEINPUT=Enter backup source drive ^( c ^| d ^| f ^| ... ^) : 
@REM -- IF /? for help
IF /i !SOURCEDRIVEINPUT!==!HOMEDRIVE::=! ( ECHO "Source drive must not be same as destination drive." & GOTO CHOOSESOURCEDRIVE )
@REM -- Checking for drive existence
IF NOT EXIST !SOURCEDRIVEINPUT!: ECHO "The system cannot find the path specified." & GOTO CHOOSESOURCEDRIVE
SET PATHSOURCE="!SOURCEDRIVEINPUT!:\!RESTORESOURCENAME:"=!"
echo !PATHSOURCE!
IF NOT EXIST !PATHSOURCE:"=! ( ECHO "Could not find backup folder in !PATHSOURCE:"=!" & GOTO END ) ELSE ( GOTO END )
:CHOOSEDESTDRIVE
SET /p DESTDRIVEINPUT=Enter destination drive ^( c ^| d ^| f ^| ... ^) : 
@REM -- IF /? for help
IF /i !DESTDRIVEINPUT!==!SOURCEDRIVEINPUT! ( ECHO "Destination drive must not be same as source drive." & GOTO CHOOSEDESTDRIVE )
@REM -- Checking for drive existence
IF NOT EXIST !DESTDRIVEINPUT!:\ ( ECHO "The system cannot find the path specified." & GOTO CHOOSEDESTDRIVE )
@REM -- Checking for backup existence
echo fuck
IF NOT EXIST "!SOURCEDRIVEINPUT!:\!RESTORESOURCENAME:"=!\!DESTDRIVEINPUT!" ( ECHO "Backup not found." & GOTO END)

@REM @REM IF DEFINED chosen (
@REM IF /I %chosen%==exit (EXIT) ELSE (
@REM IF %chosen%==1 (GOTO folder) ELSE (
@REM IF %chosen%==2 (GOTO textfile) ELSE (
@REM IF %chosen%==3 (GOTO restore) ELSE (GOTO choose))))
@REM ) ELSE (GOTO end)
@REM :: /\/\/\/\/\/\/\ Options menu /\/\/\/\/\/\/\
@REM :: \/\/\/\/\/\/\/ Get retorefolder from user \/\/\/\/\/\/\/
@REM :folder
@REM ECHO.
@REM DIR /B /A:D /O:D | FINDSTR /V "restore $"
@REM ECHO.
@REM SET count=0
@REM SET /P restorefolder=Choose backup folder? : 
@REM IF NOT EXIST %restorefolder%\ (
@REM IF /I %restorefolder%==exit (EXIT) ELSE (
@REM IF !count! GTR 2 (ECHO Could not locate folder. & COLOR 00 & GOTO guide) ELSE (
@REM ECHO No folder chosen. & SET /A count=!count! + 1 & GOTO folder)))
@REM IF %meta%==2 (GOTO textfile)
@REM IF %meta%==3 (GOTO restore)
@REM ECHO Backup folder chosen.
@REM GOTO guide
@REM :: /\/\/\/\/\/\/\ Get retorefolder from user /\/\/\/\/\/\/\
@REM :: \/\/\/\/\/\/\/ Create restore.txt file and populate from backup.txt on user's pc \/\/\/\/\/\/\/
@REM :textfile
@REM IF NOT DEFINED restorefolder (SET meta=2 & GOTO folder) ELSE (SET meta=0 & SET list=%restorefolder%\restore.txt)
@REM :: Create file with list of folders/files to restore if not existing, enclose instructions
@REM IF NOT EXIST %list% (ECHO :: **LINES BEGINING WITH :: WILL NOT BE RESTORED** > %list%) ELSE (ECHO File already exists. Find at %CD%%list%  & GOTO guide)
@REM ECHO :: \/**Folders and files that will be restored listed below**\/ >> %list%
@REM IF NOT EXIST %USERPROFILE%\backup.txt (ECHO :: %USERPROFILE%\backup.txt does not exist. Populated with list of backed up folders. >> %list%) ELSE (
@REM FOR /F "delims=" %%a IN ('FINDSTR /V ":: log" "%USERPROFILE%\backup.txt"') DO (ECHO %%a >> %list%)
@REM )
@REM ECHO.
@REM ECHO File created at %CD%%list%
@REM IF %meta%==3 (GOTO restore)
@REM GOTO guide
@REM :: /\/\/\/\/\/\/\ Create restore.txt file and populate from backup.txt on user's pc /\/\/\/\/\/\/\
@REM :: \/\/\/\/\/\/\/ Restore folders to user's pc \/\/\/\/\/\/\/
@REM :restore
@REM IF NOT DEFINED restorefolder (SET meta=3 & GOTO folder)
@REM IF NOT EXIST %CD%%restorefolder%\restore.txt (SET meta=3 & GOTO textfile)
@REM SET meta=0
@REM SET list=%restorefolder%\restore.txt
@REM IF NOT EXIST %CD%restore\logs\ (MKDIR %CD%restore\logs\)
@REM SET log=%CD%restore\logs\restore.log
@REM :: ***************************IMPORTANT NOTE: ROBOCOPY NOT USING PURGE WHEN RESTORING***************************
@REM IF EXIST %list% (
@REM FOR /F "delims=" %%a IN ('FINDSTR /v :: %list%') DO (IF EXIST %USERPROFILE%\%%a (ROBOCOPY "%restorefolder%\%%a " "%USERPROFILE%\%%a " *.* /S /E /MT /DCOPY:DA /COPY:DAT /R:1000000 /W:30 /LOG+:%log%) ELSE (MKDIR %USERPROFILE%"\%%a" & ROBOCOPY "%restorefolder%\%%a " "%USERPROFILE%\%%a " *.* /S /E /MT /DCOPY:DA /COPY:DAT /R:1000000 /W:30 /LOG+:%log%))
@REM )
@REM :: /\/\/\/\/\/\/\ Restore folders to user's pc /\/\/\/\/\/\/\
:END
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
ECHO.
ECHO Exit %ERRORLEVEL%
PAUSE
ENDLOCAL
EXIT
