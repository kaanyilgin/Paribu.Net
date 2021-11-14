using CryptoExchange.Net.Converters;
using Paribu.Net.Enums;
using System.Collections.Generic;

namespace Paribu.Net.Converters
{
    public class AlertDirectionConverter : BaseConverter<AlertDirection>
    {
        public AlertDirectionConverter() : this(true) { }
        public AlertDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<AlertDirection, string>> Mapping => new List<KeyValuePair<AlertDirection, string>>
        {
            new KeyValuePair<AlertDirection, string>(AlertDirection.Up, "up"),
            new KeyValuePair<AlertDirection, string>(AlertDirection.Down, "down"),
        };
    }
}