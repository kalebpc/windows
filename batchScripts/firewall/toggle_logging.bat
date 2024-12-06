
:: Toggle firewall logging of Public connection

@ECHO OFF

ECHO Checking current logging status...

:: Check current logging status then procede to toggle opposite

FOR /F "delims=" %%a IN ('POWERSHELL "& {Get-NetFirewallProfile -Name Public | Select-Object -ExpandProperty LogAllowed}" ') DO (

IF /I %%a==False (ECHO Enabling firewall logging... & SET logging=True & GOTO toggleallowedon) ELSE (ECHO Disabling firewall logging... & SET logging=False & GOTO toggleallowedoff)

)

:blocked

FOR /F "delims=" %%b IN ('POWERSHELL "& {Get-NetFirewallProfile -Name Public | Select-Object -ExpandProperty LogBlocked}" ') DO (

IF /I %%b==False (GOTO toggleblockedon) ELSE (GOTO toggleblockedoff)

)

:end

IF /I %logging%==TRUE (TYPE NUL >%USERPROFILE%\desktop\loggingTrue.txt & ECHO Firewall logging Enabled.) ELSE (DEL %USERPROFILE%\desktop\loggingTrue.txt & ECHO Firewall logging Disabled.)

ECHO Exit %ERRORLEVEL%

PAUSE

EXIT

:toggleallowedon

POWERSHELL Set-NetFirewallProfile -Profile Public -LogFileName "C:\Windows\System32\LogFiles\Firewall\pfirewall.log" -LogAllowed True

GOTO blocked

:toggleallowedoff

POWERSHELL Set-NetFirewallProfile -Profile Public -LogFileName "C:\Windows\System32\LogFiles\Firewall\pfirewall.log" -LogAllowed False

GOTO blocked

:toggleblockedon

POWERSHELL Set-NetFirewallProfile -Profile Public -LogFileName "C:\Windows\System32\LogFiles\Firewall\pfirewall.log" -LogBlocked True

GOTO end

:toggleblockedoff

POWERSHELL Set-NetFirewallProfile -Profile Public -LogFileName "C:\Windows\System32\LogFiles\Firewall\pfirewall.log" -LogBlocked False

GOTO end

