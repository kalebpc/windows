:: Dont show cmd runs

@ECHO OFF

:: Get user input

ECHO What is your name?

SET /P q=

:: Clear terminal

CLS

:: Print user input

ECHO Your name is %q%

:: Keep window open until any key press

PAUSE

EXIT
