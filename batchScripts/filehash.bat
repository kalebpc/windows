:: Quick file hash compare in downloads folder

:: Help with commands : <command> /?

@ECHO OFF

CD /users/%username%/downloads

SETLOCAL EnableDelayedExpansion

SET /P alg=Hash Algorithm ^( MD5 ^| SHA1 ^| SHA256 ^| SHA512 ^) : 

SET count=0

IF NOT DEFINED alg (COLOR 00 & GOTO err)

FOR %%a IN (MD5 SHA1 SHA256 SHA512) DO (IF /I %alg%==%%a (SET /a count=!count! + 1))

IF %count%==0 (COLOR 00 & GOTO err) ELSE (SET /P hash=Hash Value : )

IF NOT DEFINED hash (ECHO No Hash received. Hash set to 0. & SET hash=0)

ECHO.

ECHO ==================

ECHO.

ECHO %CD%

ECHO.

DIR /B /O:D

ECHO.

ECHO ==================

ECHO.

SET /P file=Which file to compare? ^( newdownload.txt ^) : 

IF NOT EXIST %file% (COLOR 00 & GOTO err)

:: Using FINDSTR to parse output for hash value /v to exclude outputs with ":"

FOR /F %%a IN ('CERTUTIL -HASHFILE %file% %alg% ^| FINDSTR /v ":"') DO SET hashed=%%a

ECHO.

IF /I %hash%==%hashed% (ECHO Match!) ELSE (ECHO NO Match!)

ECHO.

ECHO User provided hash : %hash%

ECHO.

ECHO 	 File hash : %hashed%

GOTO end

:ERR

ECHO Error occured. Filehash aborted.

:END

ECHO.

ECHO Exit %errorlevel%

PAUSE

EXIT
