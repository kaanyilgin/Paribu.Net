using Paribu.Net.Enums;
using System;
using System.Linq;

namespace Paribu.Net.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Rest Api Client
            var api = new ParibuClient();

            /* Public Endpoints */
            var p01 = api.GetInitials();
            var p02 = api.GetTickers();
            var p03 = api.GetMarketData("btc-tl");
            var p04 = api.GetChartData("btc-tl");
            var p05 = api.Register("John Doe", "a@b.com", "532XXXXXXX", "Pa55w0rd");
            var p06 = api.RegisterTwoFactor(p05.Data.Token, "---CODE---");
            var p07 = api.Login("532XXXXXXX", "Pa55w0rd");
            var p08 = api.LoginTwoFactor(p07.Data.Token, "---CODE---");
            api.SetAccessToken(p08.Data.Token);

            /* Private Endpoints */
            var p11 = api.GetUserInitials();
            var p12 = api.GetOpenOrders();
            var p13 = api.PlaceOrder("usdt-tl", OrderSide.Sell, OrderType.Limit, 110.0m, 10.0m, 11.0m);
            var p14 = api.CancelOrder("j1kwxq9l-eyr6-7yzg-ogkd-6gp843dzvn5o");
            var p15 = api.CancelOrders("usdt-tl");
            var p16 = api.CancelOrders("all");
            var p21 = api.GetAlerts();
            var p22 = api.SetAlert("usdt-tl", 9.25m);
            var p23 = api.SetAlert("usdt-tl", 10.25m);
            var p24 = api.SetAlert("btc-tl", 620000m);
            var p25 = api.SetAlert("btc-tl", 660000m);
            var p26 = api.CancelAlert("1z4r65mv-qe3l-29oj-l40d-278ydpnxj90g");
            var p27 = api.CancelAlerts("eth-tl");
            var p28 = api.CancelAlerts("all");
            var p31 = api.GetBalances();
            var p32 = api.GetDepositAddresses();
            var p33 = api.Withdraw("tl", 1000.0m, "---IBAN---");
            var p34 = api.Withdraw("usdt", 100.0m, "---USDT-ADDRESS---", "", "trx");
            var p35 = api.CancelWithdrawal(p34.Data.Id);

            // Web Socket Feeds Client
            var ws = new ParibuSocketClient();
            ws.SetPusherApplicationId("a68d528f48f652c94c88"); // Dont Change Application Id

            // Tickers
            var sub01 = ws.SubscribeToTickers((data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Ticker State >> {data.Pair} " +
                        (data.Open.HasValue ? $"O:{data.Open} " : "") +
                        (data.High.HasValue ? $"H:{data.High} " : "") +
                        (data.Low.HasValue ? $"L:{data.Low} " : "") +
                        (data.Close.HasValue ? $"C:{data.Close} " : "") +
                        (data.Volume.HasValue ? $"V:{data.Volume} " : "") +
                        (data.Change.HasValue ? $"CH:{data.Change} " : "") +
                        (data.ChangePercent.HasValue ? $"CP:{data.ChangePercent} " : "") +
                        (data.Average24H.HasValue ? $"Avg:{data.Average24H} " : "") +
                        (data.VolumeQuote.HasValue ? $"G:{data.VolumeQuote} " : "") +
                        (data.Bid.HasValue ? $"Bid:{data.Bid} " : "") +
                        (data.Ask.HasValue ? $"Ask:{data.Ask} " : "") +
                        (data.EBid.HasValue ? $"EBid:{data.EBid} " : "") +
                        (data.EAsk.HasValue ? $"EAsk:{data.EAsk} " : "")
                        );
                }
            }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Ticker Prices >> {data.Pair} C:{data.Prices.Count()} P:{string.Join(',', data.Prices)}");
                }
            });

            // Order Book & Trades
            var sub02 = ws.SubscribeToMarketData("btc-tl", (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Book Update >> {data.Pair} " +
                        $"AsksToAdd:{data.AsksToAdd.Count} " +
                        $"BidsToAdd:{data.BidsToAdd.Count} " +
                        $"AsksToRemove:{data.AsksToRemove.Count} " +
                        $"BidsToRemove:{data.BidsToRemove.Count} "
                        );
                }
            }, (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"New Trade >> {data.Pair} T:{data.Timestamp} P:{data.Price} A:{data.Amount} S:{data.Side}");
                }
            });

            // Unsubscribe
            _ = ws.UnsubscribeAsync(sub02.Data);

            // Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}