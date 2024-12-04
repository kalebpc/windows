@ECHO OFF

powershell.exe -command "& { Get-ItemProperty HKLM:\Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*,HKLM:\Software\Microsoft\Windows\CurrentVersion\Uninstall\* | Select-Object DisplayName, DisplayVersion, Publisher, InstallDate, URLUpdateInfo, InstallSource | Format-Table -Autosize | Out-File -width 999999 -Encoding utf8 '%USERPROFILE%\Documents\installedPrograms\installed.txt' }"

EXIT