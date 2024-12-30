Add-Type -AssemblyName System.Windows.Forms, PresentationFramework

Import-Module -Name $(Join-Path -Path $(Split-Path -Path "..\modules\InitializeXamlWindow\InitializeXamlWindow.psm1" -Parent -Resolve) -ChildPath "InitializeXamlWindow.psm1")

$MainWindow = Initialize-XamlWindow $(Join-Path -Path $(Split-Path -Path "./MainWindow.xaml" -Parent -Resolve) -ChildPath "MainWindow.xaml")

$DataXml = [xml](Get-Content $(Join-Path -Path $(Split-Path -Path "./data.xml" -Parent -Resolve) -ChildPath "data.xml"))

$DataTable = @{
    inputImageFullPath = " "
    chosenTransformation = " "
    chosenOption = " "
    bitmapImage = " "
}

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
    $openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
    $openFileDialog.FilterIndex = 2
    $openFileDialog.CheckPathExists
    If ($openFileDialog.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
        $MainWindow.variables.xamlBrowseTextBox.Text = $openFileDialog.FileName
        $DataTable.inputImageFullPath = $openFileDialog.FileName
        $bitmapImage = New-Object System.Windows.Media.Imaging.BitmapImage
        $bitmapImage.BeginInit()
        $bitmapImage.DecodePixelWidth = $MainWindow.variables.xamlInputImage.Width
        $bitmapImage.UriSource = New-Object System.Uri($DataTable.inputImageFullPath)
        $bitmapImage.EndInit()
        $MainWindow.variables.xamlInputImage.Source = $bitmapImage
    }
})

# Populate TransformationComboBox
ForEach ($function in $( $DataXml.SelectNodes("//function") | % {$_.name} )) {
    $MainWindow.variables.xamlTransformationComboBox.Items.Add($function)
}

$MainWindow.variables.xamlTransformationComboBox.Add_DropDownClosed({
    $DataTable.chosenTransformation = $MainWindow.variables.xamlTransformationComboBox.SelectedItem
    $MainWindow.variables.xamlPreviewButton.Visibility = "Visible"
    If ($DataTable.chosenTransformation -eq $( $DataXml.SelectNodes("//function[1]") | % {$_.name} )) {
        $MainWindow.variables.xamlOptionLabel.Visibility = "Visible"
        $MainWindow.variables.xamlOptionsComboBox.Visibility = "Visible"
    } Else {
        $MainWindow.variables.xamlOptionLabel.Visibility = "Collapsed"
        $MainWindow.variables.xamlOptionsComboBox.Visibility = "Collapsed"
    }
})

# Populate OptionsComboBox
ForEach ($item in $( $DataXml.SelectNodes("//function[1]") | % {$_.option} )) {
    $MainWindow.variables.xamlOptionsComboBox.Items.Add($item)
}

$MainWindow.variables.xamlOptionsComboBox.Add_DropDownClosed({
    $DataTable.chosenOption = $MainWindow.variables.xamlOptionsComboBox.SelectedItem
})

$MainWindow.variables.xamlPreviewButton.Add_Click({
    If ($DataTable.inputImageFullPath -ne " ") {
        $bitmapImage = New-Object System.Windows.Media.Imaging.BitmapImage
        $formatConvertedBitmapImage = New-Object System.Windows.Media.Imaging.FormatConvertedBitmap

        $bitmapImage.BeginInit()
        $bitmapImage.UriSource = New-Object System.Uri($DataTable.inputImageFullPath)
        If ($DataTable.chosenTransformation -eq $( $DataXml.SelectNodes("//function[1]") | % {$_.name} )) {
            $bitmapImage.Rotation = $DataTable.chosenOption
        }
        $bitmapImage.EndInit()

        If ($DataTable.chosenTransformation -eq $( $DataXml.SelectNodes("//function[1]") | % {$_.name} )) { 
            $MainWindow.variables.xamlPreviewImage.Source = $bitmapImage
            $DataTable.bitmapImage = $bitmapImage
        } ElseIf ($DataTable.chosenTransformation -eq $( $DataXml.SelectNodes("//function[2]") | % {$_.name} )) {
            $formatConvertedBitmapImage.BeginInit()
            $formatConvertedBitmapImage.Source = $bitmapImage
            $formatConvertedBitmapImage.DestinationFormat = $( $DataXml.SelectNodes("//function[2]") | % {$_.option} )
            $formatConvertedBitmapImage.EndInit()
            $MainWindow.variables.xamlPreviewImage.Source = $formatConvertedBitmapImage
            $DataTable.bitmapImage = $formatConvertedBitmapImage
        }
    }
})

$MainWindow.variables.xamlSaveButton.Add_Click({
    If ($DataTable.inputImageFullPath -ne " ") {
        $saveFileDialog = New-Object System.Windows.Forms.SaveFileDialog
        $saveFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"
        $saveFileDialog.FilterIndex = 2
        $saveFileDialog.OverwritePrompt
        If ($DataTable.chosenTransformation -ne " ") {
            $saveFileDialog.FileName = -join ( $([System.IO.Path]::GetFileNameWithoutExtension($DataTable.inputImageFullPath)), "-", $DataTable.chosenTransformation, [System.IO.Path]::GetExtension($DataTable.inputImageFullPath) )
        } Else {
            $saveFileDialog.FileName = $([System.IO.Path]::GetFileNameWithoutExtension($DataTable.inputImageFullPath)), "(copy)"
        }
        If ($saveFileDialog.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) {
            $ext = [System.IO.Path]::GetExtension($DataTable.inputImageFullPath) -replace "\.",""
            If ($ext -eq "jpg") {
                $ext = "Jpeg"
            } ElseIf ($ext -eq "png") {
                $ext = "Png"
            } ElseIf ($ext -eq "bmp") {
                $ext = "Bmp"
            } ElseIf ($ext -eq "gif") {
                $ext = "Gif"
            } Else {
                $ext = "Jpeg"
            }
            $encode = -join ($ext, "BitmapEncoder")
            $fileNameExt = $saveFileDialog.FileName
            If ($fileNameExt) {
                $savePath = $saveFileDialog.FileName
            } Else {
                $savePath = -join ($saveFileDialog.FileName, [System.IO.Path]::GetExtension($DataTable.inputImageFullPath))
            }
            $encoder = New-Object System.Windows.Media.Imaging.$encode
            $frame = [System.Windows.Media.Imaging.BitmapFrame]::Create($DataTable.bitmapImage)
            $encoder.Frames.Add($frame)
            $stream = [System.IO.FileStream]::new($savePath, [System.IO.FileMode]::Create)
            $encoder.Save($stream)
            $stream.Close()
        }
    }
})

$MainWindow.window.ShowDialog() | Out-Null
