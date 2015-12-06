using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Authentication.Constant;

namespace Authentication.Service.Helpers
{
    public class ValidateClientAndRedirectUri
    {
        public static string Validate(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return Message.RequiredRedirectURI;
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return Message.InvalidRedirectURI;
            }

            var clientId = GetQueryString(request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Message.RequiredClientId;
            }

            // TODO
            //var client = _repo.FindClient(clientId);

            //if (client == null)
            //{
            //    return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            //}

            //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            //{
            //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            //}

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private static string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }
    }

    
}