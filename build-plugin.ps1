$PROJECT_NAME = "ASFEnhance"
$PLUGIN_NAME = "ASFEnhance.dll"

dotnet publish $PROJECT_NAME -o ./publish/ -c Release
dotnet publish $PROJECT_NAME -o ./publish-ipc/ -c ReleaseWithIpc

if (-Not (Test-Path -Path ./dist)) {
    New-Item -ItemType Directory -Path ./dist
}
else {
    Remove-Item -Path ./dist/* -Recurse -Force
}

# 复制普通版本到 dist
Copy-Item -Path .\publish\$PLUGIN_NAME -Destination .\dist\

# 复制 IPC 版本到 dist/ipc
if (-Not (Test-Path -Path .\dist\ipc)) {
    New-Item -ItemType Directory -Path .\dist\ipc
}
Copy-Item -Path .\publish-ipc\$PLUGIN_NAME -Destination .\dist\ipc\

# 处理普通版本的资源文件
$dirs = Get-ChildItem -Path ./publish -Directory
foreach ($dir in $dirs) {
    $subFiles = Get-ChildItem -Path $dir.FullName -File -Filter *.resources.dll

    foreach ($file in $subFiles) {
        $resourceName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
        $opDir = "./dist/$resourceName"
        if (-Not (Test-Path -Path $opDir)) {
            New-Item -ItemType Directory -Path $opDir
        }

        $destinationPath = ".\dist\$resourceName\$($dir.Name).dll"
        Copy-Item -Path $file -Destination $destinationPath

        Write-Output "Copy resource DLL $($file.FullName) -> $destinationPath"
    }
}

# 处理 IPC 版本的资源文件
$dirsIpc = Get-ChildItem -Path ./publish-ipc -Directory
foreach ($dir in $dirsIpc) {
    $subFiles = Get-ChildItem -Path $dir.FullName -File -Filter *.resources.dll

    foreach ($file in $subFiles) {
        $resourceName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
        $opDir = "./dist/ipc/$resourceName"
        if (-Not (Test-Path -Path $opDir)) {
            New-Item -ItemType Directory -Path $opDir
        }

        $destinationPath = ".\dist\ipc\$resourceName\$($dir.Name).dll"
        Copy-Item -Path $file -Destination $destinationPath

        Write-Output "Copy IPC resource DLL $($file.FullName) -> $destinationPath"
    }
}

Remove-Item -Path ./publish -Recurse -Force
Remove-Item -Path ./publish-ipc -Recurse -Force