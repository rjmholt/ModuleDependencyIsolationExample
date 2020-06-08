param(
    [ValidateSet('Debug', 'Release')]
    [string]
    $Configuration = 'Debug'
)

Push-Location $PSScriptRoot
try
{
    dotnet publish
    $outPath = "$PSScriptRoot/out/JsonModule"
    if (Test-Path $outPath)
    {
        Remove-Item -Path $outPath -Recurse
    }
    New-Item -Path $outPath -ItemType Directory
    foreach ($path in "$PSScriptRoot/JsonModule.psd1", "$PSScriptRoot/bin/$Configuration/netstandard2.0/publish/*.dll", "$PSScriptRoot/bin/$Configuration/netstandard2.0/publish/*.pdb")
    {
        Copy-Item -Path $path -Destination $outPath
    }
}
finally
{
    Pop-Location
}