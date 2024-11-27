:: Clean terminal for better ui look
@ECHO OFF

:: White background with black text
COLOR f0

ECHO What disc do you want to tree? ex. c ^| d ^| f

:: Get user input
SET /P disc=

CD %disc%:

IF %errorlevel% GTR 0 (GOTO showerrors) ELSE (ECHO Enter path to folder. ex. ^/path^/to^/folder)

:: Get user input
SET /P pathtofolder=

IF DEFINED pathtofolder (CD %disc%:%pathtofolder%/) ELSE (CD %disc%:/)

IF %errorlevel% GTR 0 (GOTO showerrors) ELSE (tree)

:showerrors

ECHO %errorlevel% errors encounted.

:: Keep terminal open
PAUSE
