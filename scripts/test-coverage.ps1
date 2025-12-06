#!/usr/bin/env pwsh
param(
    [string]$Solution = 'ForkPoint.sln',
    [string]$ResultsDir = 'TestResults',
    [string]$CoverageDir = 'coverage-report'
)

Write-Host "Running coverage for solution: $Solution"

# Restore and build
dotnet restore $Solution

# Build in Release configuration
dotnet build $Solution --configuration Release

# Run tests with XPlat Code Coverage
dotnet test $Solution --configuration Release --logger "trx;LogFileName=results.trx" --collect:"XPlat Code Coverage" --results-directory $ResultsDir

# Find cobertura file
$cobertura = Get-ChildItem -Path $ResultsDir -Recurse -Filter 'coverage.cobertura.xml' -ErrorAction SilentlyContinue | Select-Object -First 1
if (-not $cobertura) {
    Write-Host "No cobertura file found under $ResultsDir. Check TestResults output." -ForegroundColor Yellow
    exit 0
}

Write-Host "Found cobertura: $($cobertura.FullName)"

# Ensure reportgenerator tool is available
if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing reportgenerator global tool..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
    $env:PATH = "$env:PATH;$env:USERPROFILE\.dotnet\tools"
}

# Generate HTML report
reportgenerator -reports:"$($cobertura.FullName)" -targetdir:"$CoverageDir" -reporttypes:Html

Write-Host "Coverage HTML generated at $CoverageDir/index.html"
