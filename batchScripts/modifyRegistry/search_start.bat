
:: Prevents online searching when using search in start menu

@ECHO OFF

SETLOCAL

FOR %%a IN (BingSearchEnabled AllowSearchToUseLocation CortanaConsent) DO (

REG ADD "HKCU\Software\Microsoft\Windows\CurrentVersion\Search" /V "%%a" /T REG_DWORD /D "0" /F

)

ECHO Exit %ERRORLEVEL%

ENDLOCAL

PAUSE

EXIT
