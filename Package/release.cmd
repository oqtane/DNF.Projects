"..\..\oqtane.framework\oqtane.package\nuget.exe" pack DNF.Projects.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework\Oqtane.Server\Packages\" /Y
