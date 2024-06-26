param (
    [string]$name = "*"
)

# Path to the KaitaiStruct compiler executable
$compilerPath = "$PSScriptRoot\bin\kaitai-struct-compiler.bat"

# Path to the output directory
# Get the parent directory
$outputDir = Join-Path (Split-Path $PSScriptRoot) "BinaryFormats"

# Recursively search for .ksy files in the KaitaiStruct folder
$ksyFiles = Get-ChildItem -Path $outputDir -Filter "$name.ksy" -Recurse

foreach ($ksyFile in $ksyFiles)
{
    Write-Output "Generating code for: " $ksyFile.Name;

    # Determine the output directory based on the folder the .ksy file is found in
    $outputSubDir = Join-Path $outputDir $ksyFile.Directory.Name

    Write-Output "Output directory: " $outputSubDir;

    # Compile the .ksy file
    Write-Output $compilerPath
    & $compilerPath -t csharp $ksyFile.FullName --outdir $outputSubDir --dotnet-namespace "BoostStudio.Formats"
}

# Path to the PropertySetterRewriter executable
$rewriterPath = "$PSScriptRoot\PropertySetterRewriter.exe"

# Recursively search for .cs files in the KaitaiStruct folder
$csFiles = Get-ChildItem -Path $outputDir -Filter "$name.cs" -Recurse

foreach ($csFile in $csFiles)
{
    Write-Output "Parsing C#: " $csFile;
    # Execute the PropertySetterRewriter with the input and output file paths
    & $rewriterPath $csFile.FullName $csFile.FullName
}