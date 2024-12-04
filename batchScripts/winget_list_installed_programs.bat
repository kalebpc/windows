@ECHO OFF

IF NOT EXIST %USERPROFILE%\desktop\installed.txt (WINGET LIST > %USERPROFILE%\desktop\installed.txt)

EXIT