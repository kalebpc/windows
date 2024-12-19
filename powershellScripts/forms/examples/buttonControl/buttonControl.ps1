Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName PresentationFramework

$xamlFile = "./MainWindow.xaml"

$xamlContent = Get-Content $xamlFile

$xamlContent = $xamlContent -replace 'mc:Ignorable="d"', '' -replace 'x:N', 'N' -replace 'x:Class=".*?"', ''

[xml]$xmlNodes = $xamlContent

$reader = New-Object System.Xml.XmlNodeReader $xmlNodes

$window = [Windows.Markup.XamlReader]::Load($reader)

# Controls

$checkBox = $window.FindName("checkBox")

$checkBox.Add_Click({})

$okButton = $window.FindName("okButton")

$okButton.Add_Click({
    if ($checkBox.IsChecked) {
	# Close Window
	$window.DialogResult = [System.Windows.Forms.DialogResult]::OK
    } else {
        if ($okButton.Content -eq "OK") {
	    $okButton.Content = "Clicked!"
        } else {
	    $okButton.Content = "OK"
	}
    }
})

$window.ShowDialog()
