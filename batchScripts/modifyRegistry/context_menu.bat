
:: **Windows 11**

:: Update registry to force right click menu straight to "show more options" menu

@ECHO OFF

reg add HKCU\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32 /ve /d "" /f

PAUSE

EXIT