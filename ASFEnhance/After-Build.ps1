param(
    [string]$solutionDir,
    [string]$targetPath,
    [string]$targetFileName,
    [string]$configuration
)

if ($null -ne $env:CI) {
    exit(0);
}

$distPath = "$solutionDir\ArchiSteamFarm\ArchiSteamFarm\bin\$configuration\net6.0\plugins\$targetFileName";

Copy-Item -Path "$targetPath" -Destination "$distPath" -Force