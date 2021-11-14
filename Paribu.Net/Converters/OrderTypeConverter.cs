using CryptoExchange.Net.Converters;
using Paribu.Net.Enums;
using System.Collections.Generic;

namespace Paribu.Net.Converters
{
    public class OrderTypeConverter : BaseConverter<OrderType>
    {
        public OrderTypeConverter() : this(true) { }
        public OrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderType, string>> Mapping => new List<KeyValuePair<OrderType, string>>
        {
            new KeyValuePair<OrderType, string>(OrderType.Limit, "limit"),
            new KeyValuePair<OrderType, string>(OrderType.Market, "market"),
        };
    }
}