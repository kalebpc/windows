Add-Type -AssemblyName System.Windows.Forms, PresentationFramework

Import-Module -Name ..\modules\InitializeXamlWindow

Try {
    $Window = Initialize-XamlWindow -Path "./xaml/MainWindow.xaml"

    $Window.variables.xamlMenuPanel.Add_MouseDown({
        $Window.window.SizeToContent = "Manual"
        $Window.window.WindowState = "Normal"
        $Window.window.DragMove()
    })

    $Window.variables.xamlMenuWindowClose.Add_Click({
        $Window.window.Close()
    })

    $Window.variables.xamlWindowControlMinimize.Add_Click({
        $Window.window.WindowState = "Minimized"
    })

    $Window.variables.xamlTasksClearAll.Add_Click({
        $Window.variables.keys | ForEach-Object { if ($_ -like "xamlcheckbox*") {$Window.variables.$_.IsChecked = $False} }
        $Window.variables.keys | ForEach-Object { if ($_ -like "xamltextbox*") {$Window.variables.$_.Text = ""} }
    })

    $Window.variables.xamlTasksClearChecks.Add_Click({
        $Window.variables.keys | ForEach-Object { if ($_ -like "xamlcheckbox*") {$Window.variables.$_.IsChecked = $False} }
    })

    $Window.variables.xamlTasksClearTasks.Add_Click({
        $Window.variables.keys | ForEach-Object { if ($_ -like "xamltextbox*") {$Window.variables.$_.Text = ""} }
    })

    $Window.variables.xamlAboutLink.Add_Click({
        [System.Diagnostics.Process]::Start("firefox",$Window.variables.xamlAboutLink.Header.substring(13))
    })

    $Window.window.ShowDialog() | Out-Null
} 
Catch [System.Exception] {
    Write-Host "Error occurred: $($_.Exception.Message)"
}
Finally {
    EXIT
}
