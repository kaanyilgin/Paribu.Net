﻿using CryptoExchange.Net.Converters;
using Paribu.Net.Enums;
using System.Collections.Generic;

namespace Paribu.Net.Converters
{
    public class TransactionDirectionConverter : BaseConverter<TransactionDirection>
    {
        public TransactionDirectionConverter() : this(true) { }
        public TransactionDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TransactionDirection, string>> Mapping => new List<KeyValuePair<TransactionDirection, string>>
        {
            new KeyValuePair<TransactionDirection, string>(TransactionDirection.Deposit, "deposit"),
            new KeyValuePair<TransactionDirection, string>(TransactionDirection.Withdraw, "withdraw"),
        };
    }
}