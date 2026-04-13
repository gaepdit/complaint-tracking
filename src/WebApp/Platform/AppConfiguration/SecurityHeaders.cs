using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Platform.AppConfiguration;

internal static class SecurityHeaders
{
    public static IHostApplicationBuilder AddSecurityHeaders(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddHttpsRedirection(options =>
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect);
        }
        else
        {
            // Starting value for HSTS max age is five minutes to allow for debugging.
            // For more info on updating HSTS max age value for production, see:
            // https://gaepdit.github.io/web-apps/use-https.html#how-to-enable-hsts
            builder.Services
                .AddHsts(options => options.MaxAge = TimeSpan.FromDays(730))
                .AddHttpsRedirection(options =>
                {
                    options.HttpsPort = 443;
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                });
        }

        return builder;
    }

    public static WebApplication UseSecurityHeaders(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment() || AppSettings.DevSettings.UseSecurityHeadersInDev)
            app.UseHsts().UseSecurityHeaders(policyCollection => policyCollection.AddSecurityHeaderPolicies());

        return app;
    }

    private static readonly string DatadogReportUri =
        $"https://browser-intake-us3-datadoghq.com/api/v2/logs?" +
        $"dd-api-key={AppSettings.DataDogSettings.ClientToken}" +
        $"&dd-evp-origin=content-security-policy&ddsource=csp-report" +
        $"&ddtags=service%3Acts%2Cenv%3A{AppSettings.ShortEnv}%2Cversion%3A{AppSettings.SimpleVersion}";

    private static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddContentTypeOptionsNoSniff();
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicyReportOnly(builder => builder.CspBuilder());
        policies.AddCrossOriginOpenerPolicy(builder => builder.SameOrigin());
        policies.AddCrossOriginEmbedderPolicy(builder => builder.Credentialless());
        policies.AddCrossOriginResourcePolicy(builder => builder.SameSite());

        if (string.IsNullOrEmpty(AppSettings.DataDogSettings.ClientToken)) return;
        policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint", DatadogReportUri));
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddObjectSrc().None();
        builder.AddScriptSrc().Self()
            .From("https://www.datadoghq-browser-agent.com/us3/v6/")
            .WithHashTagHelper()
            .WithNonce()
            .ReportSample();
        builder.AddStyleSrc().Self()
            .UnsafeInline()
            .ReportSample();
        builder.AddImgSrc().Self().Data();
        builder.AddConnectSrc().Self()
            .From("https://browser-intake-us3-datadoghq.com");
        builder.AddFontSrc().Self().Data();
        builder.AddFormAction().Self()
            .From("https://*.okta.com")
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();
        builder.AddWorkerSrc()
            .From("https://www.datadoghq-browser-agent.com/us3/v6/");
        builder.AddReportTo("csp-endpoint");
    }
#pragma warning restore S1075
}
