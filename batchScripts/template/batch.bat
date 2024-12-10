:: Create new batch file populated with chosen template

@ECHO OFF

SETLOCAL

:: Deal with arguments

SET arg1=%~1

SET arg2=%~2

SET arg3=%3

IF DEFINED arg1 (IF "%arg1%"=="/?" (GOTO usage) ELSE (SET fullpathwfilename=%arg1%)) ELSE (GOTO promptuser)

IF DEFINED arg2 (IF "%arg2%"=="/?" (GOTO usage) ELSE (SET template=%arg2%)) ELSE (GOTO usage)

IF DEFINED arg3 (IF /I "%arg3%"=="/y" (SET arg3=Y) ELSE (SET arg3=N))

:create

:: Complete filepath/name setup for ".bat" file

:: Make sure creating a .bat file
FOR %%a IN ("%fullpathwfilename%") DO (SET pathh=%%~dpa & SET filename="%%~nxa")

FOR /F "delims=." %%a IN (%filename%) DO (SET fullpathwfilename="%pathh:~0,-1%%%~a.bat")

:: Lets user know if about to attempt file creation in whatever path starting from root "drive:\"

SET testpath=%HOMEDRIVE%%1.bat

IF /I "%testpath:/=\%"==%fullpathwfilename% (

IF "%arg3%"=="Y" (GOTO continue) ELSE (

ECHO.You are about to create file at %fullpathwfilename%! Do you want to continue?

CHOICE /C YN /T 30 /D N /M "Press Y for Yes, N for No."

))

:: Error condition
IF %ERRORLEVEL%==255 (GOTO end)

:: "N"
IF %ERRORLEVEL%==2 (GOTO end)

:: "Y"
IF %ERRORLEVEL%==1 (GOTO checkoverwrite)

:checkoverwrite

:: Check if file already exists

IF EXIST %fullpathwfilename% (

IF "%arg3%"=="Y" (GOTO continue) ELSE (

ECHO.%fullpathwfilename% will be overwritten. Do you want to continue?

CHOICE /C YN /T 30 /D N /M "Press Y for Yes, N for No."

))

:: Error condition
IF %ERRORLEVEL%==255 (GOTO end)

:: "N"
IF %ERRORLEVEL%==2 (GOTO end)

:: "Y"
IF %ERRORLEVEL%==1 (GOTO continue)

:continue

:: Use chosen template to create new file

IF "%template%"=="/basic" (

IF EXIST %CD%\basic.txt (COPY %CD%\basic.txt %fullpathwfilename%) ELSE (

ECHO.Could not find %CD%\basic.txt

GOTO end

)

) ELSE (

ECHO.Did not recognize template.

ECHO.

ECHO.batch.bat /? ^(For Help^)

GOTO end

)

IF EXIST %fullpathwfilename% (ECHO.File created at : %fullpathwfilename%) ELSE (ECHO.File not created.)

ECHO.Exit %ERRORLEVEL%

:end

IF NOT DEFINED arg1 (PAUSE)

EXIT /B

:usage

ECHO.USAGE:

ECHO.   batch.bat [drive:][path]filename [Option1] [Option2]

ECHO.

ECHO.   Option1:

ECHO.	/?		Display this help message

ECHO.	/basic		Populates new file with basic template

ECHO.

ECHO.   Option2:

ECHO.	/y		Override location and overwrite protection.

ECHO.

ECHO.	Note:

ECHO.	Must use quotes "" when filename includes one or more spaces.

ECHO.	If path is not specified, new file will be created in same

ECHO.	location of "batch.bat". Must use "/" leading template type.

ECHO.

ECHO.   Examples:

ECHO.	batch.bat foo /basic /y

ECHO.	batch.bat "foo bar" /basic

ECHO.	batch.bat C:\Users\John\Desktop\foo /basic

ECHO.	batch.bat "C:\Users\John\Desktop\foo bar" /basic 

GOTO end

:promptuser

:: Get path from user

SET /P fullpathwfilename=Enter full path ending with new file name. ( %USERPROFILE%\Documents\batchname ) : 

IF NOT DEFINED fullpathwfilename (GOTO promptuser) ELSE (

IF /I "%fullpathwfilename%"=="EXIT" (GOTO end)

)

:: Get template from user

:gettemplate

SET /P template=Enter template type. ( /basic ) : 

IF NOT DEFINED template (GOTO gettemplate) ELSE (

IF /I "%template%"=="EXIT" (GOTO end)

)

GOTO create

ENDLOCAL
