using Newtonsoft.Json;
using Paribu.Net.Converters;
using Paribu.Net.Enums;
using System;

namespace Paribu.Net.RestObjects
{
    public class ParibuTwoFactorToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("subject"), JsonConverter(typeof(TwoFactorSubjectConverter))]
        public TwoFactorSubject Subject { get; set; }

        [JsonProperty("method"), JsonConverter(typeof(TwoFactorMethodConverter))]
        public TwoFactorMethod Method { get; set; }

        [JsonIgnore]
        public string Message { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
