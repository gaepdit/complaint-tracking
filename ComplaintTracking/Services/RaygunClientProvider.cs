using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.AspNetCore;

namespace ComplaintTracking.Services
{
    public class RaygunClientProvider : DefaultRaygunAspNetCoreClientProvider
    {
        public override RaygunClient GetClient(RaygunSettings settings, HttpContext context)
        {
            var client = base.GetClient(settings, context);
            client.IgnoreFormFieldNames("*Password");
            client.ApplicationVersion = typeof(Program).Assembly.GetName().Version.ToString(3);
            client.SendingMessage += (sender, args) =>
            {
                args.Message.Details.Tags = args.Message.Details.Tags ?? new List<string>();
                args.Message.Details.Tags.Add(CTS.CurrentEnvironment.ToString());
            };

            var identity = context?.User?.Identity as ClaimsIdentity;
            if (identity?.IsAuthenticated == true)
            {
                var email = identity.Name;

                client.UserInfo = new RaygunIdentifierMessage(email)
                {
                    IsAnonymous = false
                };
            }

            return client;
        }
    }
}