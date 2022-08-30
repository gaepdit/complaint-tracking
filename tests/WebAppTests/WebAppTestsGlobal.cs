using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests;

[SetUpFixture]
public static class WebAppTestsGlobal
{
    internal static TempDataDictionary GetPageTempData() =>
        new(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
}
