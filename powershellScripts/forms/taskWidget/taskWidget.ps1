Add-Type -AssemblyName System.Windows.Forms, PresentationFramework

# ****** Load xaml ******

$xamlFile = "./xaml/MainWindow.xaml"

$xamlContent = Get-Content $xamlFile

$xamlContent = $xamlContent -replace 'mc:Ignorable="d"', '' -replace 'x:N', 'N' -replace 'x:Class=".*?"', ''

[xml]$xmlNodes = $xamlContent

$reader = New-Object System.Xml.XmlNodeReader $xmlNodes

$window = [Windows.Markup.XamlReader]::Load($reader)

# Set env vars for named elements from xaml file
$xmlNodes.SelectNodes("//*[@Name]") | ForEach-Object {Set-Variable -Name ($_.Name) -Value $window.FindName($_.Name)}

# ****** Window Controls ******

$menuPanel.Add_MouseDown({
    $window.SizeToContent = "Manual"
    $window.WindowState = "Normal"
    $window.DragMove()
})

$menuWindowClose.Add_Click({
    $window.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
})

$windowControlMinimize.Add_Click({
    $window.WindowState = "Minimized"
})

# ****** Task List Controls ******

$tasksClearAll.Add_Click({
    Get-Variable checkbox* -valueOnly | ForEach-Object {$_.IsChecked = $False}
    Get-Variable textbox* -valueOnly | ForEach-Object {$_.Text = ""}
})

$tasksClearChecks.Add_Click({
    Get-Variable checkbox* -valueOnly | ForEach-Object {$_.IsChecked = $False}
})

$tasksClearTasks.Add_Click({
    Get-Variable textbox* -valueOnly | ForEach-Object {$_.Text = ""}
})

$window.ShowDialog() | Out-Null
