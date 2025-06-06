$PROJECT_NAME = "ASFEnhance"
$PLUGIN_NAME = "ASFEnhance.dll"
$PROJECT_NAME2 = "ASFEnhance.IPC"
$PLUGIN_NAME2 = "ASFEnhance.IPC.dll"

dotnet publish $PROJECT_NAME -o ./publish/ -c Release
dotnet publish $PROJECT_NAME2 -o ./publish/IPC/ -c Release


if (-Not (Test-Path -Path ./tmp)) {
    New-Item -ItemType Directory -Path ./tmp
}
else {
    Remove-Item -Path ./tmp/* -Recurse -Force
}

Copy-Item -Path .\publish\$PLUGIN_NAME -Destination .\tmp\ 
Copy-Item -Path .\publish\IPC\$PLUGIN_NAME2 -Destination .\tmp\ 

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