param(
    [ValidateSet('Debug', 'Release')]
    [string]
    $Configuration = 'Debug'
)

$netCore = 'netcoreapp3.1'
$netFramework = 'net461'

$outPath = "$PSScriptRoot/out/JsonModule"
$commonPath = "$outPath/Common"
$corePath = "$outPath/Core"
$frameworkPath = "$outPath/Framework"
if (Test-Path $outPath)
{
    Remove-Item -Path $outPath -Recurse
}
New-Item -Path $outPath -ItemType Directory
New-Item -Path $commonPath -ItemType Directory
New-Item -Path $corePath -ItemType Directory
New-Item -Path $frameworkPath -ItemType Directory

Push-Location "$PSScriptRoot/JsonModule.Engine"
try
{
    dotnet publish
}
finally
{
    Pop-Location
}

Push-Location "$PSScriptRoot/JsonModule.Cmdlets"
try
{
    dotnet publish -f $netCore
    dotnet publish -f $netFramework
}
finally
{
    Pop-Location
}

$commonFiles = [System.Collections.Generic.HashSet[string]]::new()
Copy-Item -Path "$PSScriptRoot/JsonModule.psd1" -Destination $outPath
Get-ChildItem -Path "$PSScriptRoot/JsonModule.Engine/bin/$Configuration/netstandard2.0/publish" |
    Where-Object { $_.Extension -in '.dll','.pdb' } |
    ForEach-Object { [void]$commonFiles.Add($_.Name); Copy-Item -LiteralPath $_.FullName -Destination $commonPath }
Get-ChildItem -Path "$PSScriptRoot/JsonModule.Cmdlets/bin/$Configuration/$netCore/publish" |
    Where-Object { $_.Extension -in '.dll','.pdb' -and -not $commonFiles.Contains($_.Name) } |
    ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $corePath }
Get-ChildItem -Path "$PSScriptRoot/JsonModule.Cmdlets/bin/$Configuration/$netFramework/publish" |
    Where-Object { $_.Extension -in '.dll','.pdb' -and -not $commonFiles.Contains($_.Name) } |
    ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $frameworkPath }
