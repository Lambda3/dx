Push-Location $PSScriptRoot\..\dnx
dotnet pack -c Release -o ..\nupkg
if ($LASTEXITCODE -ne 0) { exit }
Pop-Location
if (Test-Path $HOME\.dotnet\tools\dnx.exe) {
    Remove-Item $HOME\.dotnet\tools\dnx.exe
}
if (Test-Path $HOME\.dotnet\tools\dnx.exe.config) {
    Remove-Item $HOME\.dotnet\tools\dnx.exe.config
}
dotnet install tool -g dnx