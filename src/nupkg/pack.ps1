Push-Location $PSScriptRoot\..\dnx
dotnet pack -c Release -o ..\nupkg
Pop-Location