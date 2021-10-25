$folder_tmp = ".\tmp";
$folder_dist = ".\dist";
$folder_obj = ".\ASFEnhance\obj";
$local = "Localization"
$folder_location = ".\ASFEnhance\$local";
$folder_backup = "$folder_tmp\$local";
$file_sln = ".\ASFEnhance.sln";

$languages = "en-US", "zh-CN";

#判断工作目录
if (!(Test-Path $file_sln)) {
  Write-Output "please run at ASFEnchance's root path";
  [Console]::Readkey() | Out-Null;
  Exit;
}

if (!(Test-Path $folder_tmp)) {
  Write-Output "Create folder $folder_tmp";
  New-Item -ItemType Directory -Path $folder_tmp -Force
}

if (!(Test-Path $folder_dist)) {
  Write-Output "Create folder $folder_dist";
  New-Item -ItemType Directory -Path $folder_dist -Force;
}

Write-Output "Backup localization files";

Copy-Item -Path "$folder_location" -Destination "$folder_tmp" -Force -Recurse;

# Write-Output "Clear language resx files";

# Remove-Item -Path "$folder_location\Langs.[a-z]*-[a-z]*.resx" -Recurse -Force;

foreach ($lang in $languages) {

  Write-Output "Start to build $lang Version";

  $folder_out = "$folder_tmp\$lang";
  $file_dist = "$folder_out\ASFEnhance.dll";

  if ((Test-Path $folder_obj)) {
    # Remove-Item -Path "$folder_out" -Recurse -Force;
    &cmd.exe /c rd /s /q $folder_obj;
  }

  if ((Test-Path $folder_out)) {
    # Remove-Item -Path "$folder_out" -Recurse -Force;
    &cmd.exe /c rd /s /q $folder_out;
  }

  Copy-Item -Path "$folder_backup\Langs.$lang.resx" -Destination "$folder_location\Langs.resx" -Force;

  dotnet publish ASFEnhance -c "Release" -f "net5.0" -o "$folder_out";

  if ((Test-Path $file_dist)) {

    $version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$file_dist").FileVersion;

    Copy-Item -Path "$folder_out\ASFEnhance.dll" -Destination "$folder_dist\ASFEnhance-$lang.dll" -Force;
    # Remove-Item -Path "$folder_out" -Recurse -Force;
    Write-Output "Build Language $lang Version $version complete";
  }
  else {
    Write-Output "Build Language $lang Version UNKNOWN failed";
  }

  Write-Output "###############################################################";
}

Write-Output "Restore localization files";

Move-Item -Path "$folder_tmp\Localization\*" -Destination "$folder_location" -Force;

# Create the final zip file
Get-ChildItem $folder_dist -Filter *.dll | ForEach-Object -Process { 
  $file_name = $_.Name;
  $pure_name = $file_name.Substring(0, $file_name.Length - 4);
 
  Write-Output "Ziping $file_name to ASFEnhance.dll";

  Copy-Item -Path "$folder_dist\$file_name" -Destination "$folder_dist\ASFEnhance.dll" -Force;

  7z a -bd -slp -tzip -mm=Deflate -mx=9 -mfb=258 -mpass=15 "$folder_dist\$pure_name.zip" "$folder_dist\ASFEnhance.dll"
}

Remove-Item -Path "$folder_dist\ASFEnhance.dll" -Force;

Write-Output "Script run finished";

Exit;