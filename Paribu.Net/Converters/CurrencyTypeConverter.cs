﻿using CryptoExchange.Net.Converters;
using Paribu.Net.Enums;
using System.Collections.Generic;

namespace Paribu.Net.Converters
{
    public class CurrencyTypeConverter : BaseConverter<CurrencyType>
    {
        public CurrencyTypeConverter() : this(true) { }
        public CurrencyTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<CurrencyType, string>> Mapping => new List<KeyValuePair<CurrencyType, string>>
        {
            new KeyValuePair<CurrencyType, string>(CurrencyType.Fiat, "fiat"),
            new KeyValuePair<CurrencyType, string>(CurrencyType.Crypto, "crypto"),
        };
    }
}