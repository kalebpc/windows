Add-Type -AssemblyName System.Windows.Forms, PresentationFramework, System.Drawing

$iconFile = "./images/icon.png"

$xamlFile = "./xaml/MainWindow.xaml"

$xamlContent = Get-Content $xamlFile

$xamlContent = $xamlContent -replace 'mc:Ignorable="d"', '' -replace 'x:N', 'N' -replace 'x:Class=".*?"', ''

[xml]$xmlNodes = $xamlContent

$reader = New-Object System.Xml.XmlNodeReader $xmlNodes

$window = [Windows.Markup.XamlReader]::Load($reader)

# Set env vars for named elements from xaml file
$xmlNodes.SelectNodes("//*[@Name]") | ForEach-Object {Set-Variable -Name ($_.Name) -Value $window.FindName($_.Name)}

$icon.Source = $iconFile

$menuBarFileOpen.Add_Click({
    explorer.exe
})

$menuPanel.Add_MouseDown({
    $window.SizeToContent = "Manual"
    $windowControlMaximize.Header = "1"
    $window.WindowState = "Normal"
    $window.DragMove()
})

$windowControlMinimize.Add_Click({
    $window.WindowState = "Minimized"
})

$windowControlMaximize.Add_Click({
    $window.SizeToContent = "Manual"
    if ($window.WindowState -eq "Maximized") {
	$windowControlMaximize.Header = "1"
	$window.WindowState = "Normal"
    } else {
	$windowControlMaximize.Header = "2"
        $window.WindowState = "Maximized"
    }
})

$windowControlClose.Add_Click({
    $window.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
})

$menuBarFileExit.Add_Click({
    $window.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
})

$window.ShowDialog() | Out-Null
