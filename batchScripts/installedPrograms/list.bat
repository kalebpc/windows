@REM List Installed Programs © 2025 https://github.com/kalebpc/windows

@REM -- What this code does.
@REM -- Opens file explorer to choose where to save.
@REM -- Creates txt file with list of installed programs.
@REM -- Opens file explorer to folder and opens txt file in notepad.

@REM -- Does NOT list all installed programs. Just the ones listed under Uninstall in Registry.

@ECHO OFF
SETLOCAL EnableDelayedExpansion
SET FILENAME="getItemPropertyInstalled"
FOR /f "delims=" %%I IN ('POWERSHELL "&{Add-Type -AssemblyName System.Windows.Forms; $saveFileDialog=New-Object System.Windows.Forms.SaveFileDialog; $saveFileDialog.RestoreDirectory = true; $saveFileDialog.InitialDirectory='!USERPROFILE!'; $saveFileDialog.Filename=!FILENAME!; $saveFileDialog.Filter='Txt files (*.txt)|*.txt'; $saveFileDialog.FilterIndex=2; $saveFileDialog.CheckPathExists; $saveFileDialog.ShowDialog(); ECHO $saveFileDialog.Filename}"') do set "FILEPATH=%%I"
IF /i "!FILEPATH!" NEQ "Cancel" (
    FOR /f %%A IN ("!FILEPATH!") DO SET "TEMPPATH=%%~dpA" & SET "TEMPEXT=%%~xA"
    IF /i "!TEMPEXT!" NEQ ".txt" (
        SET FILEPATH="!FILEPATH!.txt"
    )
    IF EXIST !TEMPPATH! (
        POWERSHELL.EXE -Command "& { Get-ItemProperty HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*,HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | Select-Object DisplayName, DisplayVersion, Publisher, InstallDate, URLUpdateInfo, InstallSource | Format-Table -Autosize | Out-File -width 999999 -Encoding utf8 '!FILEPATH!' }"
        START notepad.exe "!FILEPATH!"
    ) ELSE ( ECHO System cannot find path. & PAUSE )
)
ENDLOCAL
EXIT
