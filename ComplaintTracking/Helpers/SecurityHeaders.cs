using ComplaintTracking.App;

namespace ComplaintTracking.Helpers;

internal static class SecurityHeaders
{
    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies.AddFrameOptionsDeny();
        policies.AddXssProtectionBlock();
        policies.AddContentTypeOptionsNoSniff();
        policies.AddReferrerPolicyStrictOriginWhenCrossOrigin();
        policies.RemoveServerHeader();
        policies.AddContentSecurityPolicy(builder => builder.CspBuilder());
        if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey))
            policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint",
                $"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.RaygunSettings.ApiKey}"));
    }

#pragma warning disable S1075 // "URIs should not be hardcoded"
    private static void CspBuilder(this CspBuilder builder)
    {
        builder.AddDefaultSrc().None();
        builder.AddBaseUri().None();
        builder.AddScriptSrc()
            .Self()
            .From("https://cdn.raygun.io/raygun4js/raygun.min.js")
            .WithHash256("lyolOjFEpwMenK+1PNbcwjIW7ZjHzw+EN8xe4louCcE=") // Raygun CDN
            .WithHash256("k8lqom5XjWiHpIL9TqKQ7DpRVbQNTtRtBFIKZ0iQaBk=") // Raygun pulse
            .WithHash256("tTnPz8pqP3Qkgh24aJLdbf1uukxpTUbfsHypqPsdkY4=") // no-js helper
            .WithHash256("P7+P37YsIAcgJJLpGI97Gb6NxOEps50FoJUPSqdDAqs=") // Close alerts
            .WithHashTagHelper()
            .ReportSample();
        builder.AddStyleSrc()
            .Self()
            .WithHash256("FDaFJEfUmeNNDmh1mlkOgFQnyhlZzheSSA3oNe8/JRE=")
            .WithHash256("VJk8poT0aZ7cgtqBETrtHQ50XTJbZAOh2WI1W8LG7Xc=") // Fancybox
            .WithHash256("aqNNdDLnnrDOnTNdkJpYlAxKVJtLt9CtFLklmInuUAE=") // Validation summary (requires 'unsafe-hashes')
            .UnsafeHashes()
            .ReportSample();
        builder.AddImgSrc()
            .Self()
            .Data()
            .From("https://www.gravatar.com/avatar/");
        builder.AddConnectSrc()
            .Self()
            .From("https://api.raygun.com")
            .From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction().Self();
        builder.AddManifestSrc().Self();
        builder.AddFrameSrc().Self();
        builder.AddFrameAncestors().Self();
        builder.AddReportUri()
            .To($"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.RaygunSettings.ApiKey}");
        builder.AddReportTo("csp-endpoint");
    }
#pragma warning restore S1075
}
