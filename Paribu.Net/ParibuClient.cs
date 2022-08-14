using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paribu.Net.Converters;
using Paribu.Net.CoreObjects;
using Paribu.Net.Enums;
using Paribu.Net.Helpers;
using Paribu.Net.RestObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Paribu.Net.Contracts;

namespace Paribu.Net
{
    public class ParibuClient : RestClient, IRestClient, IParibuClient
    {
        #region Client Options
        protected static ParibuClientOptions defaultOptions = new ParibuClientOptions();
        protected static ParibuClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region Endpoints
        protected const int ParibuErrorCode = -1;

        protected const string Endpoints_Public_Initials = "initials";                              // GET
        protected const string Endpoints_Public_Ticker = "ticker";                                  // GET
        protected const string Endpoints_Public_Market = "markets/{symbol}";                        // GET
        protected const string Endpoints_Public_Chart = "charts/{symbol}";                          // GET
        protected const string Endpoints_Public_Register = "register";                              // POST
        protected const string Endpoints_Public_TwoFactor = "two-factor";                           // POST
        protected const string Endpoints_Public_Login = "login";                                    // POST
        // protected const string Endpoints_Public_RetrySms = "retry-sms";                          // POST
        // protected const string Endpoints_Public_ResetPasswordInit = "reset/password";            // POST
        // protected const string Endpoints_Public_ResetPasswordAction = "reset/password";          // PUT
        // protected const string Endpoints_Public_EmailConfirmation = "user/email/confirmation";   // POST

        // protected const string Endpoints_Private_IdVerify = "user/id-verify";                    // POST
        // protected const string Endpoints_Private_Logout = "user/logout";                         // POST
        protected const string Endpoints_Private_OpenOrders = "user/orders";                        // GET
        protected const string Endpoints_Private_PlaceOrder = "user/orders";                        // POST
        protected const string Endpoints_Private_CancelOrder = "user/orders/{uid}";                 // DELETE
        protected const string Endpoints_Private_SetUserAlert = "user/alerts";                      // POST
        protected const string Endpoints_Private_DeleteUserAlert = "user/alerts/{uid}";             // DELETE
        // protected const string Endpoints_Private_GetDepositAdress = "user/addresses/assign";     // POST
        // protected const string Endpoints_Private_DeleteWithdrawAdress = "user/addresses/{t}";    // DELETE
        protected const string Endpoints_Private_Withdraw = "user/withdraws";                       // POST
        protected const string Endpoints_Private_CancelWithdrawal = "user/withdraws/{uid}";         // DELETE
        // protected const string Endpoints_Private_IninalWithdraw = "user/ininal/withdraw";        // POST
        // protected const string Endpoints_Private_ChangePassword = "user/password";               // PUT
        // protected const string Endpoints_Private_ChangeEmail = "user/email";                     // PUT
        // protected const string Endpoints_Private_ChangeTwoFactor = "user/two-factor";            // PUT
        // protected const string Endpoints_Private_CreateTicket = "tickets";                       // POST
        // protected const string Endpoints_Private_UserCards = "user/cards";                       // POST
        // protected const string Endpoints_Private_TicketToken = "ticket/token";                   // GET
        // protected const string Endpoints_Private_FenerbahceToken = "user/fb-token";              // POST
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of ParibuClient using the default options
        /// </summary>
        public ParibuClient() : this(DefaultOptions)
        {
        }

        /// <summary>
        /// Create a new instance of the ParibuClient with the provided token
        /// </summary>
        public ParibuClient(string token) : this(DefaultOptions, token)
        {
        }

        /// <summary>
        /// Create a new instance of the ParibuClient with the provided options
        /// </summary>
        public ParibuClient(ParibuClientOptions options, string token = "") : base("Paribu", options, new ParibuAuthenticationProvider(token))
        {
            requestBodyFormat = RequestBodyFormat.FormData;
            arraySerialization = ArrayParametersSerialization.MultipleValues;
        }
        #endregion

        #region Core Methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(ParibuClientOptions options)
        {
            defaultOptions = options;
        }

        public virtual void SetAccessToken(string token)
        {
            SetAuthenticationProvider(new ParibuAuthenticationProvider(token));
        }
        #endregion

        #region Api Methods

        #region Initials
        /// <summary>
        /// Gets Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuInitials> GetInitials(CancellationToken ct = default)
            => GetInitialsAsync(ct).Result;
        /// <summary>
        /// Gets Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuInitials>> GetInitialsAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<ParibuInitials>>(GetUrl(Endpoints_Public_Initials), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuInitials>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuInitials>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<ParibuInitials>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #region Tickers
        /// <summary>
        /// Gets all tickers
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<Dictionary<string, ParibuTicker>> GetTickers(CancellationToken ct = default)
            => GetTickersAsync(ct).Result;
        /// <summary>
        /// Gets all tickers
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<Dictionary<string, ParibuTicker>>> GetTickersAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<ParibuTickers>>(GetUrl(Endpoints_Public_Ticker), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<Dictionary<string, ParibuTicker>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<Dictionary<string, ParibuTicker>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<Dictionary<string, ParibuTicker>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data.Data, null);
        }
        #endregion

        #region Market Data
        /// <summary>
        /// Gets Market Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuMarketData> GetMarketData(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default)
            => GetMarketDataAsync(symbol, interval, ct).Result;
        /// <summary>
        /// Gets Market Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuMarketData>> GetMarketDataAsync(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "interval", JsonConvert.SerializeObject(interval, new ChartIntervalConverter(false)) }
            };
            var result = await SendRequestAsync<ParibuApiResponse<MarketData>>(GetUrl(Endpoints_Public_Market.Replace("{symbol}", symbol)), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuMarketData>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuMarketData>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            var pmd = new ParibuMarketData
            {
                ChartData = new ParibuChartData(),
                OrderBook = new ParibuOrderBook(),
                MarketMatches = result.Data.Data.MarketMatches,
                UserMatches = result.Data.Data.UserMatches != null ? result.Data.Data.UserMatches : new List<ParibuUserMatch>(),
            };

            // Candles
            var min = result.Data.Data.ChartData.OpenTimeData.Count();
            min = Math.Min(min, result.Data.Data.ChartData.VolumeData.Count());
            min = Math.Min(min, result.Data.Data.ChartData.ClosePriceData.Count());
            pmd.ChartData.Symbol = result.Data.Data.ChartData.Symbol;
            pmd.ChartData.Interval = result.Data.Data.ChartData.Interval;
            for (var i = 0; i < min; i++)
            {
                pmd.ChartData.Candles.Add(new ParibuCandle
                {
                    OpenTime = result.Data.Data.ChartData.OpenTimeData.ElementAt(i),
                    OpenDateTime = result.Data.Data.ChartData.OpenTimeData.ElementAt(i).FromUnixTimeSeconds(),
                    ClosePrice = result.Data.Data.ChartData.ClosePriceData.ElementAt(i),
                    Volume = result.Data.Data.ChartData.VolumeData.ElementAt(i),
                });
            }

            // Order Book
            foreach (var ask in result.Data.Data.OrderBook.Asks.Data) pmd.OrderBook.Asks.Add(new ParibuOrderBookEntry { Price = ask.Key, Amount = ask.Value });
            foreach (var bid in result.Data.Data.OrderBook.Bids.Data) pmd.OrderBook.Bids.Add(new ParibuOrderBookEntry { Price = bid.Key, Amount = bid.Value });

            return new WebCallResult<ParibuMarketData>(result.ResponseStatusCode, result.ResponseHeaders, pmd, null);
        }
        #endregion

        #region Chart Data
        /// <summary>
        /// Gets Chart Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuChartData> GetChartData(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default)
            => GetChartDataAsync(symbol, interval, ct).Result;
        /// <summary>
        /// Gets Chart Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuChartData>> GetChartDataAsync(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "interval", JsonConvert.SerializeObject(interval, new ChartIntervalConverter(false)) }
            };
            var result = await SendRequestAsync<ParibuApiResponse<ChartData>>(GetUrl(Endpoints_Public_Chart.Replace("{symbol}", symbol)), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: false, parameters: parameters).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuChartData>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuChartData>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            // Candles
            var chart = new ParibuChartData();
            var min = result.Data.Data.OpenTimeData.Count();
            min = Math.Min(min, result.Data.Data.VolumeData.Count());
            min = Math.Min(min, result.Data.Data.ClosePriceData.Count());
            chart.Symbol = result.Data.Data.Symbol;
            chart.Interval = result.Data.Data.Interval;
            for (var i = 0; i < min; i++)
            {
                chart.Candles.Add(new ParibuCandle
                {
                    OpenTime = result.Data.Data.OpenTimeData.ElementAt(i),
                    OpenDateTime = result.Data.Data.OpenTimeData.ElementAt(i).FromUnixTimeSeconds(),
                    ClosePrice = result.Data.Data.ClosePriceData.ElementAt(i),
                    Volume = result.Data.Data.VolumeData.ElementAt(i),
                });
            }

            // Return
            return new WebCallResult<ParibuChartData>(result.ResponseStatusCode, result.ResponseHeaders, chart, null);
        }
        #endregion

        #region Register
        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="name">Name Surname</param>
        /// <param name="email">Email Address</param>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuTwoFactorToken> Register(string name, string email, string mobile, string password, CancellationToken ct = default)
            => RegisterAsync(name, email, mobile, password, ct).Result;
        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="name">Name Surname</param>
        /// <param name="email">Email Address</param>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuTwoFactorToken>> RegisterAsync(string name, string email, string mobile, string password, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "name", name},
                { "email", email},
                { "mobile", mobile},
                { "password", password},
            };

            var result = await SendRequestAsync<ParibuApiResponse<ParibuTwoFactorToken>>(GetUrl(Endpoints_Public_Register), method: HttpMethod.Post, ct, parameters, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuTwoFactorToken>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuTwoFactorToken>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            result.Data.Data.Message = result.Data.Message;
            return new WebCallResult<ParibuTwoFactorToken>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #region Login
        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuTwoFactorToken> Login(string mobile, string password, CancellationToken ct = default)
            => LoginAsync(mobile, password, ct).Result;
        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuTwoFactorToken>> LoginAsync(string mobile, string password, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "mobile", mobile},
                { "password", password},
            };

            var result = await SendRequestAsync<ParibuApiResponse<ParibuTwoFactorToken>>(GetUrl(Endpoints_Public_Login), method: HttpMethod.Post, ct, parameters, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuTwoFactorToken>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuTwoFactorToken>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            result.Data.Data.Message = result.Data.Message;
            return new WebCallResult<ParibuTwoFactorToken>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #region TwoFactor
        /// <summary>
        /// TFA Method for Register
        /// </summary>
        /// <param name="token">Register TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuTwoFactorUser> RegisterTwoFactor(string token, string code, CancellationToken ct = default)
            => RegisterTwoFactorAsync(token, code, ct).Result;
        /// <summary>
        /// Register TFA Method
        /// </summary>
        /// <param name="token">Register TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuTwoFactorUser>> RegisterTwoFactorAsync(string token, string code, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "token", token},
                { "code", code},
            };

            var result = await SendRequestAsync<ParibuApiResponse<ParibuTwoFactorUser>>(GetUrl(Endpoints_Public_TwoFactor), method: HttpMethod.Post, ct, parameters, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuTwoFactorUser>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuTwoFactorUser>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<ParibuTwoFactorUser>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// TFA Method for Login
        /// </summary>
        /// <param name="token">Login TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuTwoFactorUser> LoginTwoFactor(string token, string code, bool setAccessToken = true, CancellationToken ct = default)
            => LoginTwoFactorAsync(token, code, setAccessToken, ct).Result;
        /// <summary>
        /// TFA Method for Login
        /// </summary>
        /// <param name="token">Login TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuTwoFactorUser>> LoginTwoFactorAsync(string token, string code, bool setAccessToken = true, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "token", token},
                { "code", code},
            };

            var result = await SendRequestAsync<ParibuApiResponse<ParibuTwoFactorUser>>(GetUrl(Endpoints_Public_TwoFactor), method: HttpMethod.Post, ct, parameters, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuTwoFactorUser>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuTwoFactorUser>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            if (setAccessToken && (result.Data.Data.Subject == TwoFactorSubject.Register || result.Data.Data.Subject == TwoFactorSubject.Login))
            {
                if (!string.IsNullOrEmpty(result.Data.Data.Token))
                    SetAccessToken(result.Data.Data.Token);
            }

            return new WebCallResult<ParibuTwoFactorUser>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// TFA Method for TFA Toggle Action
        /// </summary>
        /// <param name="token">Toggle TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuTwoFactorToggle> ToggleTwoFactor(string token, string code, CancellationToken ct = default)
            => ToggleTwoFactorAsync(token, code, ct).Result;
        /// <summary>
        /// TFA Method for TFA Toggle Action
        /// </summary>
        /// <param name="token">Toggle TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuTwoFactorToggle>> ToggleTwoFactorAsync(string token, string code, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "token", token},
                { "code", code},
            };

            var result = await SendRequestAsync<ParibuApiResponse<ParibuTwoFactorToggle>>(GetUrl(Endpoints_Public_TwoFactor), method: HttpMethod.Post, ct, parameters, checkResult: false, signed: false).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuTwoFactorToggle>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuTwoFactorToggle>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<ParibuTwoFactorToggle>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #region Orders
        /// <summary>
        /// Gets Open Orders
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<ParibuOrder>> GetOpenOrders(CancellationToken ct = default)
            => GetOpenOrdersAsync(ct).Result;
        /// <summary>
        /// Gets Open Orders
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<ParibuOrder>>> GetOpenOrdersAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<Dictionary<string, ParibuOrder>>>(GetUrl(Endpoints_Private_OpenOrders), method: HttpMethod.Get, ct, parameters: null, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<ParibuOrder>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<IEnumerable<ParibuOrder>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<IEnumerable<ParibuOrder>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data.Values.Select(x => x).ToList(), null);
        }

        /// <summary>
        /// Place an Order
        /// </summary>
        /// <param name="symbol">Mandatory</param>
        /// <param name="side">Mandatory</param>
        /// <param name="type">Mandatory</param>
        /// <param name="total">Mandatory</param>
        /// <param name="amount">Mandatory for Limit Orders</param>
        /// <param name="price">Mandatory for Limit Orders</param>
        /// <param name="condition">Condition Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuOrder> PlaceOrder(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal? total = null,
            decimal? amount = null,
            decimal? price = null,
            decimal? condition = null,
            CancellationToken ct = default)
            => PlaceOrderAsync(symbol, side, type, total, amount, price, condition, ct).Result;
        /// <summary>
        /// Place an Order
        /// </summary>
        /// <param name="symbol">Mandatory</param>
        /// <param name="side">Mandatory</param>
        /// <param name="type">Mandatory</param>
        /// <param name="total">Mandatory</param>
        /// <param name="amount">Mandatory for Limit Orders</param>
        /// <param name="price">Mandatory for Limit Orders</param>
        /// <param name="condition">Condition Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal? total = null,
            decimal? amount = null,
            decimal? price = null,
            decimal? condition = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol},
                { "trade", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
            };
            if (amount.HasValue) parameters.AddOptionalParameter("amount", amount.Value.ToString(CultureInfo.InvariantCulture));
            if (total.HasValue) parameters.AddOptionalParameter("total", amount.Value.ToString(CultureInfo.InvariantCulture));
            if (price.HasValue) parameters.AddOptionalParameter("price", price.Value.ToString(CultureInfo.InvariantCulture));
            if (condition.HasValue) parameters.AddOptionalParameter("condition", condition.Value.ToString(CultureInfo.InvariantCulture));

            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<ParibuOrder>>>(GetUrl(Endpoints_Private_PlaceOrder), method: HttpMethod.Post, ct, parameters, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuOrder>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuOrder>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<ParibuOrder>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data.FirstOrDefault(), null);
        }

        /// <summary>
        /// Cancels Order with specific Order Id
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>List of order Ids for canceled orders</returns>
        public virtual WebCallResult<IEnumerable<string>> CancelOrder(string orderId, CancellationToken ct = default)
            => CancelOrderAsync(orderId, ct).Result;
        /// <summary>
        /// Cancels Order with specific Order Id
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>List of order Ids for canceled orders</returns>
        public virtual async Task<WebCallResult<IEnumerable<string>>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<string>>>(GetUrl(Endpoints_Private_CancelOrder.Replace("{uid}", orderId)), method: HttpMethod.Delete, ct, parameters: null, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<IEnumerable<string>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// symbol olarak market sembolü (btc-tl, usdt-tl gibi) iletilirse o marketteki tüm açık emirleri iptal eder. 
        /// symbol olarak "all" iletilirse tüm marketteki tüm açık emirleri iptal eder. 
        /// Cancels all open orders for a specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<string>> CancelOrders(string symbol, CancellationToken ct = default)
            => CancelOrdersAsync(symbol, ct).Result;
        /// <summary>
        /// symbol olarak market sembolü (btc-tl, usdt-tl gibi) iletilirse o marketteki tüm açık emirleri iptal eder. 
        /// symbol olarak "all" iletilirse tüm marketteki tüm açık emirleri iptal eder. 
        /// Cancels all open orders for a specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<string>>> CancelOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<string>>>(GetUrl(Endpoints_Private_CancelOrder.Replace("{uid}", symbol)), method: HttpMethod.Delete, ct, parameters: null, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<IEnumerable<string>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #region User Initials
        /// <summary>
        /// Gets User Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuUserInitials> GetUserInitials(CancellationToken ct = default)
            => GetUserInitialsAsync(ct).Result;
        /// <summary>
        /// Gets User Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuUserInitials>> GetUserInitialsAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<ParibuLoggedInInitials>>(GetUrl(Endpoints_Public_Initials), method: HttpMethod.Get, cancellationToken: ct, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuUserInitials>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuUserInitials>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<ParibuUserInitials>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data.UserInfo, null);
        }
        #endregion

        #region Balances
        /// <summary>
        /// Gets Balances
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<Dictionary<string, ParibuAssetBalance>> GetBalances(CancellationToken ct = default)
            => GetBalancesAsync(ct).Result;
        /// <summary>
        /// Gets Balances
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<Dictionary<string, ParibuAssetBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            var result = await GetUserInitialsAsync(ct);
            if (!result.Success) return WebCallResult<Dictionary<string, ParibuAssetBalance>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data == null) return WebCallResult<Dictionary<string, ParibuAssetBalance>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Error?.Message, null));

            return new WebCallResult<Dictionary<string, ParibuAssetBalance>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.AssetBalances, null);
        }
        #endregion

        #region Alerts
        /// <summary>
        /// Gets Alerts
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<ParibuAlert>> GetAlerts(CancellationToken ct = default)
            => GetAlertsAsync(ct).Result;
        /// <summary>
        /// Gets Alerts
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<ParibuAlert>>> GetAlertsAsync(CancellationToken ct = default)
        {
            var result = await GetUserInitialsAsync(ct);
            if (!result.Success) return WebCallResult<IEnumerable<ParibuAlert>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data == null) return WebCallResult<IEnumerable<ParibuAlert>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Error?.Message, null));

            return new WebCallResult<IEnumerable<ParibuAlert>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Alerts, null);
        }

        /// <summary>
        /// Create new Price Alert
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="price">Alert Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<bool> SetAlert(string symbol, decimal price, CancellationToken ct = default)
            => SetAlertAsync(symbol, price, ct).Result;
        /// <summary>
        /// Create new Price Alert
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="price">Alert Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<bool>> SetAlertAsync(string symbol, decimal price, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "market", symbol},
                { "trigger_price", price.ToString(CultureInfo.InvariantCulture)},
            };

            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<object>>>(GetUrl(Endpoints_Private_SetUserAlert), method: HttpMethod.Post, ct, parameters: parameters, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<bool>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<bool>(result.ResponseStatusCode, result.ResponseHeaders, result.Success, null);
        }

        /// <summary>
        /// Cancels Price Alert with Alert Id
        /// </summary>
        /// <param name="alertId">Alert Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<string>> CancelAlert(string alertId, CancellationToken ct = default)
            => CancelAlertAsync(alertId, ct).Result;
        /// <summary>
        /// Cancels Price Alert with Alert Id
        /// </summary>
        /// <param name="alertId">Alert Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<string>>> CancelAlertAsync(string alertId, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<string>>>(GetUrl(Endpoints_Private_DeleteUserAlert.Replace("{uid}", alertId)), method: HttpMethod.Delete, ct, parameters: null, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<IEnumerable<string>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Cancels all Price Alerts with specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<string>> CancelAlerts(string symbol, CancellationToken ct = default)
            => CancelAlertsAsync(symbol, ct).Result;
        /// <summary>
        /// Cancels all Price Alerts with specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<string>>> CancelAlertsAsync(string symbol, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<string>>>(GetUrl(Endpoints_Private_DeleteUserAlert.Replace("{uid}", symbol)), method: HttpMethod.Delete, ct, parameters: null, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<IEnumerable<string>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #region Deposit
        /// <summary>
        /// Gets Deposit Addresses
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<ParibuAddress>> GetDepositAddresses(CancellationToken ct = default)
            => GetDepositAddressesAsync(ct).Result;
        /// <summary>
        /// Gets Deposit Addresses
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<ParibuAddress>>> GetDepositAddressesAsync(CancellationToken ct = default)
        {
            var result = await GetUserInitialsAsync(ct);
            if (!result.Success) return WebCallResult<IEnumerable<ParibuAddress>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (result.Data == null) return WebCallResult<IEnumerable<ParibuAddress>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Error?.Message, null));

            var addresses = new List<ParibuAddress>();
            if (result.Data != null && result.Data.Addresses != null) addresses = result.Data.Addresses.Where(x => x.Direction == TransactionDirection.Deposit).ToList();
            return new WebCallResult<IEnumerable<ParibuAddress>>(result.ResponseStatusCode, result.ResponseHeaders, addresses, null);
        }
        #endregion

        #region Withdrawal
        /// <summary>
        /// Withdraw Method
        /// </summary>
        /// <param name="currency">Currency Symbol</param>
        /// <param name="amount">Withdrawal Amount</param>
        /// <param name="address">Withdrawal Address</param>
        /// <param name="tag">Withdrawal Tag</param>
        /// <param name="network">Withdrawal Network</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<ParibuTransaction> Withdraw(
            string currency,
            decimal amount,
            string address,
            string tag = "",
            string network = "",
            CancellationToken ct = default)
            => WithdrawAsync(currency, amount, address, tag, network, ct).Result;
        /// <summary>
        /// Withdraw Method
        /// </summary>
        /// <param name="currency">Currency Symbol</param>
        /// <param name="amount">Withdrawal Amount</param>
        /// <param name="address">Withdrawal Address</param>
        /// <param name="tag">Withdrawal Tag</param>
        /// <param name="network">Withdrawal Network</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<ParibuTransaction>> WithdrawAsync(
            string currency,
            decimal amount,
            string address,
            string tag = "",
            string network = "",
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "currency", currency},
                { "amount", amount.ToString(CultureInfo.InvariantCulture)},
                { "address", address},
                { "tag", tag},
                { "network", network},
            };

            var result = await SendRequestAsync<ParibuApiResponse<ParibuTransaction>>(GetUrl(Endpoints_Private_Withdraw), method: HttpMethod.Post, ct, parameters: parameters, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<ParibuTransaction>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<ParibuTransaction>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<ParibuTransaction>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }

        /// <summary>
        /// Cancels Withdrawal Request
        /// </summary>
        /// <param name="withdrawalId">Withdrawal Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<string>> CancelWithdrawal(string withdrawalId, CancellationToken ct = default)
            => CancelWithdrawalAsync(withdrawalId, ct).Result;
        /// <summary>
        /// Cancels Withdrawal Request
        /// </summary>
        /// <param name="withdrawalId">Withdrawal Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<string>>> CancelWithdrawalAsync(string withdrawalId, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<ParibuApiResponse<IEnumerable<string>>>(GetUrl(Endpoints_Private_CancelWithdrawal.Replace("{uid}", withdrawalId)), method: HttpMethod.Delete, ct, parameters: null, checkResult: false, signed: true).ConfigureAwait(false);
            if (!result.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, result.Error);
            if (!result.Data.Success) return WebCallResult<IEnumerable<string>>.CreateErrorResult(result.ResponseStatusCode, result.ResponseHeaders, new ServerError(ParibuErrorCode, result.Data.Message, null));

            return new WebCallResult<IEnumerable<string>>(result.ResponseStatusCode, result.ResponseHeaders, result.Data.Data, null);
        }
        #endregion

        #endregion

        #region Protected Methods
        protected override Error ParseErrorResponse(JToken error)
        {
            return ParibuParseErrorResponse(error);
        }
        protected virtual Error ParibuParseErrorResponse(JToken error)
        {
            if (error["message"] == null)
                return new ServerError(error.ToString());

            return new ServerError(ParibuErrorCode, (string)error["message"], null);
        }

        protected virtual Uri GetUrl(string endpoint)
        {
            return new Uri($"{BaseAddress.TrimEnd('/')}/{endpoint}");
        }
        #endregion

    }
}