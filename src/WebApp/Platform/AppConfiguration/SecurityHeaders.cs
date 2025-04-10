using Cts.WebApp.Platform.Settings;

namespace Cts.WebApp.Platform.AppConfiguration;

internal static class SecurityHeaders
{
    private static readonly string ReportUri =
        $"https://report-to-api.raygun.com/reports?apikey={AppSettings.RaygunSettings.ApiKey}";

    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddContentTypeOptionsNoSniff();
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicyReportOnly(builder => builder.CspBuilder());
        policies.AddCrossOriginOpenerPolicy(builder => builder.SameOrigin());
        policies.AddCrossOriginEmbedderPolicy(builder => builder.Credentialless());
        policies.AddCrossOriginResourcePolicy(builder => builder.SameSite());

        if (string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) return;
        policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint", ReportUri));
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddObjectSrc().None();
        builder.AddScriptSrc().Self()
            .From("https://cdn.raygun.io/raygun4js/raygun.min.js")
            .WithHashTagHelper()
            .WithNonce()
            .ReportSample();
        builder.AddStyleSrc().Self()
            .UnsafeInline()
            .ReportSample();
        builder.AddImgSrc().Self().Data();
        builder.AddConnectSrc()
            .From("https://api.raygun.com")
            .From("https://api.raygun.io");
        builder.AddFontSrc().Self().Data();
        builder.AddFormAction().Self()
            .From("https://login.microsoftonline.com");
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();

        if (string.IsNullOrEmpty(AppSettings.RaygunSettings.ApiKey)) return;
        builder.AddReportUri().To(ReportUri);
        builder.AddReportTo("csp-endpoint");
    }
#pragma warning restore S1075
}
