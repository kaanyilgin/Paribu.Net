using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;

namespace Paribu.Net.CoreObjects
{
    public class ParibuAuthenticationProvider : AuthenticationProvider
    {
        private readonly SecureString AccessToken;

        public ParibuAuthenticationProvider(string token) : base(new ApiCredentials("APIKEY", "APISECRET"))
        {
            AccessToken = token.ToSecureString();
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, HttpMethodParameterPosition parameterPosition, ArrayParametersSerialization arraySerialization)
        {
            var headers = new Dictionary<string, string>
            {
                { "user-agent", "ParibuApp/337 (Android 12)" },
                { "x-app-version", "337" },
            };

            // Check Point
            if (!signed)
                return headers;

            // Authorization
            headers["Authorization"] = "Bearer " + AccessToken.GetString();

            // Return
            return headers;
        }

        public override string Sign(string toSign)
        {
            throw new NotImplementedException();
        }
    }
}