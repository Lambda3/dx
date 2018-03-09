Push-Location $PSScriptRoot\..\dx
dotnet pack -c Release -o ..\nupkg
if ($LASTEXITCODE -ne 0) { exit }
Pop-Location
if (Test-Path $HOME\.dotnet\tools\dx.exe) {
    Remove-Item $HOME\.dotnet\tools\dx.exe
}
if (Test-Path $HOME\.dotnet\tools\dx.exe.config) {
    Remove-Item $HOME\.dotnet\tools\dx.exe.config
}
dotnet install tool -g dx