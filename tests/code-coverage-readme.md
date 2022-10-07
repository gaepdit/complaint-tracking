# Unit Test Code Coverage Reports

The SonarCloud GitHub workflow automatically generates a code coverage report for each pull request viewable on the SonarCloud project website. Optionally, there are a couple of ways to generate an HTML coverage report locally with detailed information.

If you're using Visual Studio, you can install the [Fine Code Coverage](https://marketplace.visualstudio.com/items?itemName=FortuneNgwenya.FineCodeCoverage) extension, which generates a report within VS each time the unit tests are run.

If you're not using Visual Studio or if you want to save a copy of the report, first install the Coverlet and ReportGenerator CLI tools:

```
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool
```

Then run the following commands from the root directory (the coverlet commands should be the same as those in the "sonarcloud-scan.yml" file):

```
dotnet build
coverlet .\.artifacts\DomainTests\bin\Debug\net6.0\DomainTests.dll --target "dotnet" --targetargs "test tests/DomainTests --no-build" --exclude "[TestData]*"
coverlet .\.artifacts\IntegrationTests\bin\Debug\net6.0\IntegrationTests.dll --target "dotnet" --targetargs "test tests/IntegrationTests --no-build" --exclude "[TestData]*" --exclude "[Infrastructure]MyAppRoot.Infrastructure.Migrations.*" --merge-with "coverage.json"
coverlet .\.artifacts\LocalRepositoryTests\bin\Debug\net6.0\LocalRepositoryTests.dll --target "dotnet" --targetargs "test tests/LocalRepositoryTests --no-build" --exclude "[TestData]*" --merge-with "coverage.json"
coverlet .\.artifacts\AppServicesTests\bin\Debug\net6.0\AppServicesTests.dll --target "dotnet" --targetargs "test tests/AppServicesTests --no-build" --exclude "[TestData]*" --merge-with "coverage.json"
coverlet .\.artifacts\WebAppTests\bin\Debug\net6.0\WebAppTests.dll --target "dotnet" --targetargs "test tests/WebAppTests --no-build" --exclude "[TestData]*" --exclude "[Infrastructure]MyAppRoot.Infrastructure.Migrations.*" --merge-with "coverage.json" -f=opencover -o="coverage.xml"
reportgenerator -reports:coverage.xml -targetdir:coveragereport
```

The ReportGenerator tool creates an HTML report and saves it in the specified directory ("coveragereport").
