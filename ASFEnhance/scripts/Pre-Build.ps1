param(
    [string]$projectDir
)

if ($null -ne $env:CI) {
    exit(0);
}

$assemblyInfoFile = "$projectDir\AssemblyInfo.cs";

$pattern = 'Version\("(\d+\.\d+\.\d+\.)(\d+)"\)';

$fileContent = Get-Content -Path "$assemblyInfoFile";

$newFile = @();

foreach ($line in $fileContent) {
    if ($line -match $pattern) {
        $build = ([int]$Matches[2]) + 1;
        $newVersion = 'Version("{0}{1:d3}")' -f $Matches[1], $build ;
        $oldVersion = $Matches[0];

        $line = $line.Replace($oldVersion, $newVersion);
    }
    $newFile += [string]$line;
}

$newFile | Set-Content -Path "$assemblyInfoFile" -Encoding UTF8;