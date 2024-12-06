
:: Remove onedrive from folder paths in registry

@ECHO OFF

SETLOCAL ENABLEDELAYEDEXPANSION

SET keyPath=\"Registry::HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders\"

FOR %%a in ({0DDD015D-B06C-45D5-8C4C-F59713854639} {F42EE2D3-909F-4907-8871-4C22FC0BF756} Desktop "My Pictures" Personal) DO (

IF %%a=={0DDD015D-B06C-45D5-8C4C-F59713854639} (SET newPath=\"%USERPROFILE%\Pictures\" & SET valueName=\"%%a\" & powershell -Command Set-ItemProperty -Path %keyPath% -Name !valueName! -Value !newPath!) ELSE (

IF %%a=={F42EE2D3-909F-4907-8871-4C22FC0BF756} (SET newPath=\"%USERPROFILE%\Documents\" & SET valueName=\"%%a\") ELSE (

IF %%a=="My Pictures" (SET newPath=\"%USERPROFILE%\Pictures\" & SET valueName=\"%%~a\") ELSE (

IF %%a==Personal (SET newPath=\"%USERPROFILE%\Documents\" & SET valueName=\"%%a\") ELSE (

SET newPath=\"%USERPROFILE%\%%a\" & SET valueName=\"%%a\")))) & powershell -Command Set-ItemProperty -Path %keyPath% -Name !valueName! -Value !newPath!)

:end

ENDLOCAL

IF %ERRORLEVEL%==0 (ECHO Registry Updated!)

IF NOT EXIST %USERPROFILE%\Desktop (MKDIR %USERPROFILE%\Desktop)

IF NOT EXIST %USERPROFILE%\Pictures (MKDIR %USERPROFILE%\Pictures)

ECHO Exit %ERRORLEVEL%

PAUSE

EXIT
