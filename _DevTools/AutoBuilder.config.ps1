
#solution name to build
$solutionName = "DGDataModel"

#set version
$versionMajor = "1"
$versionMinor = "2"
$versionBuild = GetVersionBuild
$versionRevision = "3"
#build version number
$assemblyVersion = GetVersion $versionMajor $versionMinor $versionBuild $versionRevision
$fileVersion = $assemblyVersion

#base folder for of the solution
$baseDir  = Resolve-Path .\..\

#release folder for of the solution
$releaseDir = Resolve-Path .\..\..\Release

#builder parameters
$buildDebugAndRelease = $false
$treatWarningsAsErrors = $true
$releaseDebugFiles = $false

#remove elder release builds for the actual version
$removeElderReleaseWithSameVersion = $true

#folders and files to exclude from the packaged release Source
$releaseSrcExcludeFolders = @('"_DevTools"', '".git"');
$releaseSrcExcludeFiles = @('".git*"');

#builds array
#include here all the solutions file to build	
$builds = @(
	@{
		#solutions filename (.sln)
		Name = "DGDataModel";
		#msbuild optionals contants
		Constants = "";
		#projects to exclude from the release binary package
		ReleaseBinExcludeProjects = @(
			@{
				Name = "DGDataModelSampleModel";
			},
			@{
				Name = "DGDataModelSampleModel.Test";
			},
			@{
				Name = "DGDataModelSampleModel.TestConsole";
			}
		);
		#files to include in the release binary package
		ReleaseBinIncludeFiles = @();
		#unit tests to run
		Tests = @(
			@{
				Name = "DGDataModelSampleModel.Test";
				TestDll = "DGDataModelSampleModel.Test.dll"
			}
		);
		#commands to run before packaging of the release source
		ReleaseSrcCmd = @(
			@{
				Cmd = ".\dbsql-backuptsql.bat ..\..\ empty"
			},
			@{
				Cmd = ".\dbsql-backupsqlschema.bat ..\..\"
			},
			@{
				Cmd = "xcopy ..\..\_DBDump\* Working\Src\_DBDump\ /s /e /y"
			}			
		);
		#commands to run before packaging of the release source
		ReleaseBinCmd = @(
			@{
				Cmd = ".\copylicense.bat"
			},
			@{
				Cmd = "del Working\Bin\DGDataModel\EntityFramework.dll"
			},
			@{
				Cmd = "del Working\Bin\DGDataModel\EntityFramework.SqlServer.dll"
			}
		);
	};
)