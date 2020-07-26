#! /usr/bin/pwsh

# COMMON PATHS, VAR

$Build = Get-Location
$Nupkgs = Join-Path $Build "nupkgs"
$Projects = Get-Content ./configuration.json | ConvertFrom-Json

Set-Location ..
$Root = Get-Location

Write-Host '## CLEAR ######################################################################' -ForegroundColor DarkGreen

dotnet clean --configuration Release --verbosity quiet

Write-Host '## RESTORE NUGET PACKAGES #####################################################' -ForegroundColor DarkGreen

dotnet restore

Write-Host '## BUILD SOLUTION #############################################################' -ForegroundColor DarkGreen

dotnet build --configuration Release --verbosity quiet

Write-Host '## PACK #######################################################################' -ForegroundColor DarkGreen

foreach ($Project in $Projects) {
    $SemanticVersioning = $Project.version.Split('.')
    
    $Major = [int]$SemanticVersioning[0]
    $Minor = [int]$SemanticVersioning[1]
    $Patch = [int]$SemanticVersioning[2]
    
    $Version = @($Major, $Minor, $Patch) -Join '.'
    $ProjectPath = $Project.path
    $ProjectName = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath) + " v" + $Version + " Ok"

    dotnet pack $ProjectPath -p:PackageVersion=$Version --output $Nupkgs --configuration Release --verbosity quiet
    
    Write-Host $ProjectName -ForegroundColor Green
}

return
Write-Host '## PUSH PACKAGES ##############################################################' -ForegroundColor DarkGreen

Get-ChildItem $Nupkgs -Filter *.nupkg | 
    Foreach-Object {
    dotnet nuget push $_.FullName `
        --source $configuration.Server `
        --api-key $configuration.Key `
        --skip-duplicate `
        --timeout 1800
    }

Write-Host '## FINALIZE ###################################################################' -ForegroundColor DarkGreen

Set-Location $Build
Remove-Item -Path $Nupkgs -Recurse