
SET WshShell = WScript.CreateObject("WScript.Shell")
WshShell.SendKeys "^{ESC}"
WScript.Sleep(500)
WshShell.SendKeys "{TAB}"
WScript.Sleep(39)
WshShell.SendKeys "{TAB}"
WScript.Sleep(39)
WshShell.SendKeys "{ENTER}"
