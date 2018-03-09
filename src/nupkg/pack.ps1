Push-Location $PSScriptRoot\..\dx
dotnet pack -c Release -o ..\nupkg
Pop-Location