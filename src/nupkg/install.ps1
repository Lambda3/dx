. $PSScriptRoot\pack.ps1
if ($LASTEXITCODE -ne 0) { exit }
if (Test-Path $HOME\.dotnet\tools\dx.exe) {
    Remove-Item $HOME\.dotnet\tools\dx.exe
}
dotnet tool install -g dx
