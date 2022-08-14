using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Paribu.Net.Enums;
using Paribu.Net.RestObjects;

namespace Paribu.Net.Contracts
{
    public interface IParibuClient
    {
        void SetAccessToken(string token);

        /// <summary>
        /// Gets Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuInitials> GetInitials(CancellationToken ct = default);

        /// <summary>
        /// Gets Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuInitials>> GetInitialsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets all tickers
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<Dictionary<string, ParibuTicker>> GetTickers(CancellationToken ct = default);

        /// <summary>
        /// Gets all tickers
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, ParibuTicker>>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets Market Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuMarketData> GetMarketData(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default);

        /// <summary>
        /// Gets Market Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuMarketData>> GetMarketDataAsync(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default);

        /// <summary>
        /// Gets Chart Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuChartData> GetChartData(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default);

        /// <summary>
        /// Gets Chart Data for a specific Symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="interval">Chart Term</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuChartData>> GetChartDataAsync(string symbol, ChartInterval interval = ChartInterval.OneDay, CancellationToken ct = default);

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="name">Name Surname</param>
        /// <param name="email">Email Address</param>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuTwoFactorToken> Register(string name, string email, string mobile, string password, CancellationToken ct = default);

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="name">Name Surname</param>
        /// <param name="email">Email Address</param>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuTwoFactorToken>> RegisterAsync(string name, string email, string mobile, string password, CancellationToken ct = default);

        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuTwoFactorToken> Login(string mobile, string password, CancellationToken ct = default);

        /// <summary>
        /// Login Method
        /// </summary>
        /// <param name="mobile">10 digits Mobile Number (532xxxxxxx)</param>
        /// <param name="password">Account Password</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuTwoFactorToken>> LoginAsync(string mobile, string password, CancellationToken ct = default);

        /// <summary>
        /// TFA Method for Register
        /// </summary>
        /// <param name="token">Register TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuTwoFactorUser> RegisterTwoFactor(string token, string code, CancellationToken ct = default);

        /// <summary>
        /// Register TFA Method
        /// </summary>
        /// <param name="token">Register TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuTwoFactorUser>> RegisterTwoFactorAsync(string token, string code, CancellationToken ct = default);

        /// <summary>
        /// TFA Method for Login
        /// </summary>
        /// <param name="token">Login TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuTwoFactorUser> LoginTwoFactor(string token, string code, bool setAccessToken = true, CancellationToken ct = default);

        /// <summary>
        /// TFA Method for Login
        /// </summary>
        /// <param name="token">Login TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuTwoFactorUser>> LoginTwoFactorAsync(string token, string code, bool setAccessToken = true, CancellationToken ct = default);

        /// <summary>
        /// TFA Method for TFA Toggle Action
        /// </summary>
        /// <param name="token">Toggle TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuTwoFactorToggle> ToggleTwoFactor(string token, string code, CancellationToken ct = default);

        /// <summary>
        /// TFA Method for TFA Toggle Action
        /// </summary>
        /// <param name="token">Toggle TFA Token</param>
        /// <param name="code">PIN Code</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuTwoFactorToggle>> ToggleTwoFactorAsync(string token, string code, CancellationToken ct = default);

        /// <summary>
        /// Gets Open Orders
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<ParibuOrder>> GetOpenOrders(CancellationToken ct = default);

        /// <summary>
        /// Gets Open Orders
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<ParibuOrder>>> GetOpenOrdersAsync(CancellationToken ct = default);

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
        WebCallResult<ParibuOrder> PlaceOrder(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal? total = null,
            decimal? amount = null,
            decimal? price = null,
            decimal? condition = null,
            CancellationToken ct = default);

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
        Task<WebCallResult<ParibuOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal? total = null,
            decimal? amount = null,
            decimal? price = null,
            decimal? condition = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancels Order with specific Order Id
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>List of order Ids for canceled orders</returns>
        WebCallResult<IEnumerable<string>> CancelOrder(string orderId, CancellationToken ct = default);

        /// <summary>
        /// Cancels Order with specific Order Id
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>List of order Ids for canceled orders</returns>
        Task<WebCallResult<IEnumerable<string>>> CancelOrderAsync(string orderId, CancellationToken ct = default);

        /// <summary>
        /// symbol olarak market sembolü (btc-tl, usdt-tl gibi) iletilirse o marketteki tüm açık emirleri iptal eder. 
        /// symbol olarak "all" iletilirse tüm marketteki tüm açık emirleri iptal eder. 
        /// Cancels all open orders for a specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<string>> CancelOrders(string symbol, CancellationToken ct = default);

        /// <summary>
        /// symbol olarak market sembolü (btc-tl, usdt-tl gibi) iletilirse o marketteki tüm açık emirleri iptal eder. 
        /// symbol olarak "all" iletilirse tüm marketteki tüm açık emirleri iptal eder. 
        /// Cancels all open orders for a specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<string>>> CancelOrdersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets User Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<ParibuUserInitials> GetUserInitials(CancellationToken ct = default);

        /// <summary>
        /// Gets User Initials
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<ParibuUserInitials>> GetUserInitialsAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets Balances
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<Dictionary<string, ParibuAssetBalance>> GetBalances(CancellationToken ct = default);

        /// <summary>
        /// Gets Balances
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, ParibuAssetBalance>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets Alerts
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<ParibuAlert>> GetAlerts(CancellationToken ct = default);

        /// <summary>
        /// Gets Alerts
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<ParibuAlert>>> GetAlertsAsync(CancellationToken ct = default);

        /// <summary>
        /// Create new Price Alert
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="price">Alert Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<bool> SetAlert(string symbol, decimal price, CancellationToken ct = default);

        /// <summary>
        /// Create new Price Alert
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="price">Alert Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<bool>> SetAlertAsync(string symbol, decimal price, CancellationToken ct = default);

        /// <summary>
        /// Cancels Price Alert with Alert Id
        /// </summary>
        /// <param name="alertId">Alert Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<string>> CancelAlert(string alertId, CancellationToken ct = default);

        /// <summary>
        /// Cancels Price Alert with Alert Id
        /// </summary>
        /// <param name="alertId">Alert Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<string>>> CancelAlertAsync(string alertId, CancellationToken ct = default);

        /// <summary>
        /// Cancels all Price Alerts with specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<string>> CancelAlerts(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Cancels all Price Alerts with specific market symbol
        /// </summary>
        /// <param name="symbol">Market Symbol</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<string>>> CancelAlertsAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Gets Deposit Addresses
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<ParibuAddress>> GetDepositAddresses(CancellationToken ct = default);

        /// <summary>
        /// Gets Deposit Addresses
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<ParibuAddress>>> GetDepositAddressesAsync(CancellationToken ct = default);

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
        WebCallResult<ParibuTransaction> Withdraw(
            string currency,
            decimal amount,
            string address,
            string tag = "",
            string network = "",
            CancellationToken ct = default);

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
        Task<WebCallResult<ParibuTransaction>> WithdrawAsync(
            string currency,
            decimal amount,
            string address,
            string tag = "",
            string network = "",
            CancellationToken ct = default);

        /// <summary>
        /// Cancels Withdrawal Request
        /// </summary>
        /// <param name="withdrawalId">Withdrawal Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        WebCallResult<IEnumerable<string>> CancelWithdrawal(string withdrawalId, CancellationToken ct = default);

        /// <summary>
        /// Cancels Withdrawal Request
        /// </summary>
        /// <param name="withdrawalId">Withdrawal Id</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<string>>> CancelWithdrawalAsync(string withdrawalId, CancellationToken ct = default);
    }
}