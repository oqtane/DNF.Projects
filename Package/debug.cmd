XCOPY "..\Client\bin\Debug\net9.0\DNF.Projects.Client.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Client\bin\Debug\net9.0\DNF.Projects.Client.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Server\bin\Debug\net9.0\DNF.Projects.Server.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Server\bin\Debug\net9.0\DNF.Projects.Server.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Server\bin\Debug\net9.0\RestSharp.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Shared\bin\Debug\net9.0\DNF.Projects.Shared.Oqtane.dll" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Shared\bin\Debug\net9.0\DNF.Projects.Shared.Oqtane.pdb" "..\..\oqtane.framework\Oqtane.Server\bin\Debug\net9.0\" /Y
XCOPY "..\Server\wwwroot\Modules\DNF.Projects\*" "..\..\oqtane.framework\Oqtane.Server\wwwroot\Modules\DNF.Projects\" /Y /S /I
