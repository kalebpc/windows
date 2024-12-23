<#
.SYNOPSIS

Take xaml file and return [hashtable] containing [System.Windows.Window], [hashtable] objects.

.PARAMETER Path

Absolute or relative path to xaml file being used.

.EXAMPLE

Initialize-XamlWindow -Path "$Env:SystemDrive/Absolute/Path/to/XamlFileName.xaml"

Name                           Value
----                           -----
window                         System.Windows.Window
variables                      {xamlCheckbox5, xamlTextbox4, xamlTextbox6, xamlWindowControlMinimize...}

.EXAMPLE

$variable = Initialize-XamlWindow -Path "./Relative/Path/to/XamlFileName.xaml"

$variable.variables

Name                           Value
----                           -----
xamlCheckbox5                  System.Windows.Controls.CheckBox Content: IsChecked:False
xamlTextbox4                   System.Windows.Controls.TextBox
xamlTextbox6                   System.Windows.Controls.TextBox

#>

function Initialize-XamlWindow {
[CmdletBinding()]
[OutputType([psobject])]
param (
    [PARAMETER(Mandatory=$true)]
    [ValidateScript({Test-Path -Path $_.trim()})]
    [string]$Path
    )

    [string]$xamlContent = Get-Content $Path
    [string]$xamlContent = $xamlContent -replace 'mc:Ignorable="d"', '' -replace 'x:N', 'N' -replace 'x:Class=".*?"', ''
    [xml]$xmlNodes = $xamlContent

    $reader = New-Object System.Xml.XmlNodeReader $xmlNodes

    $window = [Windows.Markup.XamlReader]::Load($reader)

    $variables = @{}

    ## Set vars for named elements from xaml file
    $xmlNodes.SelectNodes("//*[@Name]") | ForEach-Object { Set-Variable -Name ("xaml", $_.Name -join "") -Value $window.FindName($_.Name) }

    Get-Variable xaml* | ForEach-Object { $variables[$_.Name] = $_.Value }

    return @{window = [System.Windows.Window]$window; variables = [hashtable]$variables}
}
