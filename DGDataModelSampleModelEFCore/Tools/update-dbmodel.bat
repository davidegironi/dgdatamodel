@echo off
setlocal enabledelayedexpansion enableextensions

pushd "%~dp0" || exit /b 1

rem Scaffold database to model
rem Copyright (c) Davide Gironi, 2024

rem Enviroment dependencies
rem dotnet tool install --global dotnet-ef

rem Project dependencies
rem Microsoft.EntityFrameworkCore.SqlServer
rem Microsoft.EntityFrameworkCore.Tools
rem System.Configuration.ConfigurationManager

rem load config
if not exist config.update-dbmodel.bat goto :failure
call config.update-dbmodel.bat

rem scaffold database
dotnet ef dbcontext scaffold "%CONFIGSTRING%" Microsoft.EntityFrameworkCore.SqlServer --project "%PROJECT%" %SCAFFOLDPARAMS% --no-onconfiguring --context %CONTEXT% --context-dir %CONTEXTDIR% --output-dir %OUTPUTDIR% --force --verbose
if errorlevel 1 (
  echo ERROR: database scaffolding failed; finalization was not run.
  goto :failure
)

rem finalize scaffolded context and make named indexes compatible with each target
echo Finalizing "%CONTEXTBASEDIR%\%CONTEXT%.cs"...
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -File "%~dp0update-dbmodel-finalization.ps1" -ContextFile "%~dp0%CONTEXTBASEDIR%\%CONTEXT%.cs" -ConnectionStringName "%CONNECTIONSTRINGNAME%" -AdditionalOnConfiguringCode "%ADDITIONALONCONFIGURINGCODE%"
if errorlevel 1 (
  echo ERROR: database-model finalization failed.
  goto :failure
)

popd
endlocal
exit /b 0

:failure
pause
set "RESULT=%ERRORLEVEL%"
if "%RESULT%"=="0" set "RESULT=1"
popd
endlocal & exit /b %RESULT%
