$projectName = "ASFEnhance";

$tmpFolder = ".\tmp";
$distFolder = ".\dist";
$objFolder = ".\$projectName\obj";
$local = "Localization"
$localizationFolder = ".\$projectName\$local";
$backupFolder = "$tmpFolder\$local";
$slnFileName = ".\$projectName.sln";

$languages = "en-US", "zh-Hans";

#判断工作目录
if (!(Test-Path $slnFileName)) {
  Write-Output "please run at $projectName's root path";
  [Console]::Readkey() | Out-Null;
  Exit;
}

if (!(Test-Path $tmpFolder)) {
  Write-Output "Create folder $tmpFolder";
  New-Item -ItemType Directory -Path $tmpFolder -Force
}

if (!(Test-Path $distFolder)) {
  Write-Output "Create folder $distFolder";
  New-Item -ItemType Directory -Path $distFolder -Force;
}

Write-Output "Backup localizationization files";

Copy-Item -Path "$localizationFolder" -Destination "$tmpFolder" -Force -Recurse;

# Write-Output "Clear language resx files";

# Remove-Item -Path "$localizationFolder\Langs.[a-z]*-[a-z]*.resx" -Recurse -Force;

foreach ($lang in $languages) {

  Write-Output "Start to build $lang Version";

  $outFolder = "$tmpFolder\$lang";
  $distFile = "$outFolder\$projectName.dll";

  if ((Test-Path $objFolder)) {
    # Remove-Item -Path "$outFolder" -Recurse -Force;
    &cmd.exe /c rd /s /q $objFolder;
  }

  if ((Test-Path $outFolder)) {
    # Remove-Item -Path "$outFolder" -Recurse -Force;
    &cmd.exe /c rd /s /q $outFolder;
  }

  Copy-Item -Path "$backupFolder\Langs.$lang.resx" -Destination "$localizationFolder\Langs.resx" -Force;

  dotnet publish $projectName -c "Release" -f "net6.0" -o "$outFolder";

  if ((Test-Path $distFile)) {

    $version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$distFile").FileVersion;

    Copy-Item -Path "$outFolder\$projectName.dll" -Destination "$distFolder\$projectName-$lang.dll" -Force;
    # Remove-Item -Path "$outFolder" -Recurse -Force;
    Write-Output "Build Language $lang Version $version complete";
  }
  else {
    Write-Output "Build Language $lang Version UNKNOWN failed";
  }

  Write-Output "###############################################################";
}

Write-Output "Restore localization files";

Move-Item -Path "$backupFolder\*" -Destination "$localizationFolder" -Force;

# Create the final zip file
Get-ChildItem $distFolder -Filter *.dll | ForEach-Object -Process { 
  $fileName = $_.Name;
  $pureName = $fileName.Substring(0, $fileName.Length - 4);
 
  Write-Output "Ziping $fileName to $projectName.dll";

  Copy-Item -Path "$distFolder\$fileName" -Destination "$distFolder\$projectName.dll" -Force;

  7z a -bd -slp -tzip -mm=Deflate -mx=9 -mfb=258 -mpass=15 "$distFolder\$pureName.zip" "$distFolder\$projectName.dll"
}

Remove-Item -Path "$distFolder\$projectName.dll" -Force;

Write-Output "Script run finished";

Exit;