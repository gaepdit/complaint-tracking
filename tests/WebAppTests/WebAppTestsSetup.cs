using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests;

[SetUpFixture]
public static class WebAppTestsSetup
{
    internal static TempDataDictionary PageTempData() =>
        new(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());

    internal static PageContext PageContextWithUser() =>
        new() { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() } };
}
