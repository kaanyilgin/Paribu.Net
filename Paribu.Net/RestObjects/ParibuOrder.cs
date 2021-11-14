using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using Paribu.Net.Converters;
using Paribu.Net.Enums;
using System;

namespace Paribu.Net.RestObjects
{
    public class ParibuOrder
    {
        [JsonProperty("uid")]
        public string Id { get; set; }

        [JsonProperty("market")]
        public string Symbol { get; set; }

        [JsonProperty("trade"), JsonConverter(typeof(OrderSideConverter))]
        public OrderSide Side { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(OrderTypeConverter))]
        public OrderType Type { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("matched")]
        public decimal? Matched { get; set; }

        [JsonProperty("condition")]
        public decimal? Condition { get; set; }

        // Bulabildiğim Örnekler: open, open-nomatch
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime? Timestamp { get; set; }
    }
}
