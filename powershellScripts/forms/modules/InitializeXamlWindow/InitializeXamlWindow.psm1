function Initialize-XamlWindow {
[CmdletBinding()]
param (
    [string]$Path
    )

if ( $(Try { Test-Path -Path $Path.trim() } Catch { $false }) ) {

    [string]$xamlContent = Get-Content $Path
    [string]$xamlContent = $xamlContent -replace 'mc:Ignorable="d"', '' -replace 'x:N', 'N' -replace 'x:Class=".*?"', ''
    [xml]$xmlNodes = $xamlContent

    $reader = New-Object System.Xml.XmlNodeReader $xmlNodes

    $window = [Windows.Markup.XamlReader]::Load($reader)

    $variables = @{}

    ## Set env vars for named elements from xaml file
    $xmlNodes.SelectNodes("//*[@Name]") | ForEach-Object { Set-Variable -Name ($_.Name) -Value $window.FindName($_.Name) }

    Get-Variable xaml* | ForEach-Object { $variables[$_.Name] = $_.Value }

    [hashtable]$Window = @{
        window = [System.Windows.Window]$window
        variables = [hashtable]$variables
    }
    return $Window
}

Else {
    Write-Host "Could not locate xaml file at : $Path"
    Return
}

}
