using Newtonsoft.Json;

namespace Paribu.Net.CoreObjects
{
    public class ParibuApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("notice")]
        public string Notice { get; set; }
        
        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
