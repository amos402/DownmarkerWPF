$root = Get-Location
$build="$root\build.cmd"
$nuget="$root\src\.nuget\nuget.exe"
$artifacts="$root\artifacts"
$content="$artifacts\content\"
$version="1.0.0"

# cleanup old artifacts
if (Test-Path $artifacts) {
	Remove-Item $artifacts -recurse	
}

# update the assembly version from this script
$template = Get-Content src\GlobalAssemblyInfo.template.cs
$template -replace "{version}", $version > src\GlobalAssemblyInfo.cs

# build teh code
& $build Release "Mixed Platforms"

# get build output and copy out to root 
& xcopy "$root\src\MarkPad\bin\Release\*" $content /s /e /Y

# create the package based on this content
& $nuget pack build\MarkPad.nuspec -Version $version -BasePath $artifacts -OutputDirectory $artifacts

