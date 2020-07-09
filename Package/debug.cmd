XCOPY "..\Client\bin\Debug\netstandard2.1\DNF.Projects.Client.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Client\bin\Debug\netstandard2.1\DNF.Projects.Client.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Server\bin\Debug\netcoreapp3.1\DNF.Projects.Server.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Server\bin\Debug\netcoreapp3.1\DNF.Projects.Server.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Server\bin\Debug\netcoreapp3.1\RestSharp.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Shared\bin\Debug\netstandard2.1\DNF.Projects.Shared.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Shared\bin\Debug\netstandard2.1\DNF.Projects.Shared.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\netcoreapp3.1\" /Y
XCOPY "..\Server\wwwroot\Modules\DNF.Projects\*" "..\..\oqtane.framework\Oqtane.Server\wwwroot\Modules\DNF.Projects\" /Y /S /I
