$PROJECT_NAME = "ASFEnhance"
$PLUGIN_NAME = "ASFEnhance.dll"

dotnet publish $PROJECT_NAME -o ./publish/ -c Release

if (-Not (Test-Path -Path ./tmp)) {
    New-Item -ItemType Directory -Path ./tmp
}
else {
    Remove-Item -Path ./tmp/* -Recurse -Force
}

Copy-Item -Path .\publish\$PLUGIN_NAME -Destination .\tmp\ 

$dirs = Get-ChildItem -Path ./publish -Directory
foreach ($dir in $dirs) {
    $subFiles = Get-ChildItem -Path $dir.FullName -File -Filter *.resources.dll
    
    foreach ($file in $subFiles) {
        $resourceName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
        $opDir = "./tmp/$resourceName"
        if (-Not (Test-Path -Path $opDir)) {
            New-Item -ItemType Directory -Path $opDir
        }

        $destinationPath = ".\tmp\$resourceName\$($dir.Name).dll"
        Copy-Item -Path $file -Destination $destinationPath

        Write-Output "Copy resource DLL $($file.FullName) -> $destinationPath"
    }
}