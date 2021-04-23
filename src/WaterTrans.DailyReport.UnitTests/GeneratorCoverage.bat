cd /d %~dp0
dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./bin/
reportgenerator -reports:./bin/coverage.cobertura.xml -targetdir:./bin/coveragereport -reporttypes:Html
