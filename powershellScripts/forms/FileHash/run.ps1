Add-Type -AssemblyName System.Windows.Forms, PresentationFramework

Import-Module -Name $(Join-Path -Path $(Split-Path -Path "..\modules\InitializeXamlWindow\InitializeXamlWindow.psm1" -Parent -Resolve) -ChildPath "InitializeXamlWindow.psm1")

$MainWindow = Initialize-XamlWindow $(Join-Path -Path $(Split-Path -Path "./MainWindow.xaml" -Parent -Resolve) -ChildPath "MainWindow.xaml")

$DataXml = [xml](Get-Content $(Join-Path -Path $(Split-Path -Path "./data.xml" -Parent -Resolve) -ChildPath "data.xml"))

$MainWindow.variables.xamlMenuPanel.Add_MouseDown({
    $MainWindow.window.SizeToContent = "Manual"
    $MainWindow.window.WindowState = "Normal"
    $MainWindow.window.DragMove()
})

$MainWindow.variables.xamlMenuPanel.Add_MouseUp({
    $MainWindow.window.WindowState = "Normal"
    $MainWindow.window.SizeToContent = "WidthAndHeight"
})

$MainWindow.variables.xamlMenuWindowClose.Add_Click({
    $MainWindow.window.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
})

$MainWindow.variables.xamlWindowControlClose.Add_Click({
    $MainWindow.window.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
})

$MainWindow.variables.xamlAboutLink.Add_Click({
    [System.Diagnostics.Process]::Start($MainWindow.variables.xamlAboutLink.Header.substring(17))
})

$MainWindow.variables.xamlBrowseButton.Add_Click({
    $openFileDialog = New-Object System.Windows.Forms.OpenFileDialog
    $openFileDialog.Filter = "All files (*.*)|*.*"
    $openFileDialog.FilterIndex = 2
    $openFileDialog.CheckPathExists
    If ($openFileDialog.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
        $MainWindow.variables.xamlBrowseTextBox.Text = $openFileDialog.FileName
    }
})

# Populate AlgorithmComboBox
ForEach ($function in $( $DataXml.SelectNodes("//combo") | % {$_.option} )) {
    $MainWindow.variables.xamlAlgorithmComboBox.Items.Add($function)
}

$MainWindow.variables.xamlCalculateButton.Add_Click({
    $hashOutput = Get-FileHash -Algorithm $MainWindow.variables.xamlAlgorithmComboBox.SelectedItem -LiteralPath $MainWindow.variables.xamlBrowseTextBox.Text
    If ($hashOutput.Hash -eq $MainWindow.variables.xamlPasteHashTextBox.Text) {
        $MainWindow.variables.xamlMatchResultLabel.Content = "Match!"
    } Else {
        $MainWindow.variables.xamlMatchResultLabel.Content = "No Match!"
    }
    $MainWindow.variables.xamlInputTextBlock.Text = $MainWindow.variables.xamlPasteHashTextBox.Text.ToUpper()
    $MainWindow.variables.xamlOutputTextBlock.Text = $hashOutput.Hash
    $MainWindow.variables.xamlResultStackPanel.Visibility = "Visible"
})

$MainWindow.window.ShowDialog() | Out-Null
