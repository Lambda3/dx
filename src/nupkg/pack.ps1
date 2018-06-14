Push-Location $PSScriptRoot\..\dx
dotnet pack -c Release -o $PSScriptRoot
$exit = $LASTEXITCODE
Pop-Location
exit $exit
