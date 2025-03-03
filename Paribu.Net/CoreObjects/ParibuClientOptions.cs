﻿using CryptoExchange.Net.Objects;

namespace Paribu.Net.CoreObjects
{
    public class ParibuClientOptions : RestClientOptions
    {
        public ParibuClientOptions() : base("https://v3.paribu.com/app")
        {
        }

        public ParibuClientOptions Copy()
        {
            return Copy<ParibuClientOptions>();
        }
    }
}
