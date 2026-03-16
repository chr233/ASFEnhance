
$runtimes = "linux-x64", "win-x64" #, "win-arm64", "linux-arm64"
$framework = "net10.0"
$config = "Release"
$projectName = "ArchiSteamFarm/ArchiSteamFarm"
$zip = $false

foreach ($runtime in $runtimes) {
    Write-Debug "Publishing for runtime: $runtime and framework: $framework"
    $buildName = "ASF-$runtime-$framework"

    $outputDir = "./dist/$buildName"

    dotnet publish $projectName --output "$outputDir-fde" --self-contained --runtime $runtime --framework $framework --configuration $config --no-restore --nologo -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:ContinuousIntegrationBuild=true -p:UseAppHost=true
    dotnet publish $projectName --output "$outputDir" --no-self-contained --runtime $runtime --framework $framework --configuration $config --no-restore --nologo -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:ContinuousIntegrationBuild=true -p:UseAppHost=true

    if ($true -eq $zip) {
        Write-Debug "Creating zip archive for $buildName"
        7z a -bd -slp -tzip -mm=Deflate -mx=5 -mfb=150 -mpass=10 "./dist/$buildName-fde.zip" "./tmp/$buildName-fde/*"
        7z a -bd -slp -tzip -mm=Deflate -mx=5 -mfb=150 -mpass=10 "./dist/$buildName.zip" "./tmp/$buildName/*"
    }
}

if ($true -eq $zip) {
    Remove-Item -Recurse -Force "./tmp"
}