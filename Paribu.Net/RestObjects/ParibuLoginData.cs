﻿using Newtonsoft.Json;
using Paribu.Net.Converters;
using Paribu.Net.Enums;
using System.Collections.Generic;

namespace Paribu.Net.RestObjects
{
    public class ParibuLoginData
    {
        [JsonProperty("market")]
        public string Symbol { get; set; }

        [JsonProperty("interval"), JsonConverter(typeof(ChartIntervalConverter))]
        public ChartInterval Interval { get; set; }

        [JsonProperty("t")]
        public IEnumerable<int> OpenTimeData { get; set; }

        [JsonProperty("c")]
        public IEnumerable<decimal> ClosePriceData { get; set; }

        [JsonProperty("v")]
        public IEnumerable<decimal> VolumeData { get; set; }
    }
}