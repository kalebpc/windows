function Create-Shortcut {

    $WsShell = New-Object -ComObject WScript.Shell

    $shortcut = $WsShell.CreateShortcut("$pwd\..\Task Widget.lnk")

    $shortcut.TargetPath = "$pwd\..\run.bat"

    $shortcut.WorkingDirectory = "$pwd\..\"

    $shortcut.IconLocation = "%SystemRoot%\System32\imageres.dll,290"

    $shortcut.Description = "Open Task Widget Window"

    $shortcut.save()
}
Create-Shortcut | Out-Null
