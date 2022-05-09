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
        policies.AddCustomHeader("Reporting-Endpoints",
            $"default=\"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.Raygun.ApiKey}\",csp-endpoint=\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}\"");
        policies.AddCustomHeader("Report-To",
            $"{{\"group\":\"default\",\"max_age\":10886400,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports?apikey={ApplicationSettings.Raygun.ApiKey}\"}}]}},{{\"group\":\"csp-endpoint\",\"max_age\":10886400,\"endpoints\":[{{\"url\":\"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}\"}}]}}");
        policies.AddCustomHeader("NEL", 
            "{\"report_to\":\"default\", \"max_age\":2592000}");
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
            .ReportSample();
        builder.AddImgSrc()
            .Self()
            .Data()
            .From("https://www.gravatar.com/avatar/");
        builder.AddConnectSrc()
            .Self()
            .From("https://api.raygun.io");
        builder.AddFontSrc().Self();
        builder.AddFormAction().Self();
        builder.AddManifestSrc().Self();
        builder.AddFrameAncestors().None();
        builder.AddReportUri()
            .To($"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.Raygun.ApiKey}");
        builder.AddCustomDirective("report-to", "csp-endpoint");
    }
#pragma warning restore S1075
}
