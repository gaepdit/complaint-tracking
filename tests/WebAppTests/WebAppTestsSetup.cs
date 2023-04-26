using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Claims;

namespace WebAppTests;

[SetUpFixture]
public static class WebAppTestsSetup
{
    internal static TempDataDictionary PageTempData() =>
        new(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

    internal static PageContext PageContextWithUser() =>
        new() { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() } };
}
