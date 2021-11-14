using CryptoExchange.Net.Converters;
using Paribu.Net.Enums;
using System.Collections.Generic;

namespace Paribu.Net.Converters
{
    public class TradeRoleConverter : BaseConverter<TradeRole>
    {
        public TradeRoleConverter() : this(true) { }
        public TradeRoleConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TradeRole, string>> Mapping => new List<KeyValuePair<TradeRole, string>>
        {
            new KeyValuePair<TradeRole, string>(TradeRole.Maker, "maker"),
            new KeyValuePair<TradeRole, string>(TradeRole.Taker, "taker"),
        };
    }
}