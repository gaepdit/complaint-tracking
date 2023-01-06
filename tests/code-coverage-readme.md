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
[coverlet commands from "sonarcloud-scan.yml" file]
reportgenerator -reports:coverage.xml -targetdir:coveragereport
```

The ReportGenerator tool creates an HTML report and saves it in the specified directory ("coveragereport").
