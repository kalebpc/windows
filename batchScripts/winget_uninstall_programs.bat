@ECHO OFF

SETLOCAL

:: For now, must normalize installed.txt down to a list of names MANUALLY; then use this script

FOR %%b IN ('FINDSTR /v :: %USERPROFILE%\desktop\installed.txt ') DO (WINGET UNINSTALL --name "%%b")

ECHO Exit %ERRORLEVEL%

PAUSE

ENDLOCAL

EXIT