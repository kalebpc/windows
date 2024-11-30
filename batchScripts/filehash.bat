:: Help with commands : <command> /?

@ECHO OFF

CD /users/%username%/downloads

SETLOCAL

SET /P alg=Hash Algorithm ^( MD5 ^| SHA1 ^| SHA256 ^| SHA512 ^) : 

SET /P hash=Hash Value : 

DIR /B /O:D

SET /P file=Which file to compare? ^( newdownload.txt ^) : 

:: Using FINDSTR to parse output for hash value /v to exclude outputs with ":"

FOR /F %%a IN ('CERTUTIL -HASHFILE %file% %alg% ^| FINDSTR /v ":"') DO SET hashed=%%a

IF %hash%==%hashed% (ECHO Match!) ELSE (ECHO NO Match!)

ECHO User provided hash : %hash%

ECHO 	 File hash : %hashed%

PAUSE

EXIT