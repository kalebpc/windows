:: Clean terminal for better ui look
@ECHO OFF

:: Get user input
SET /P disc=What disc do you want to tree? (c ^| d ^| f) ^:

CD %disc%:/

IF %errorlevel% NEQ 0 (GOTO showerrors) ELSE (SET /p pathtofolder=Enter path to folder. ^(^/path^/to^/folder^) ^:)

IF NOT DEFINED pathtofolder (ECHO No path defined && GOTO showerrors) ELSE (CD %pathtofolder%/)

IF %errorlevel% NEQ 0 (GOTO showerrors) ELSE (tree)

:showerrors

ECHO %errorlevel% errors encounted.

:: Keep terminal open
PAUSE

EXIT
