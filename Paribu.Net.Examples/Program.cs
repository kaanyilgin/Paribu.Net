using Paribu.Net.CoreObjects;
using Paribu.Net.Enums;
using System;
using System.Diagnostics;
using System.Linq;

namespace Paribu.Net.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Rest Api Client
            var api = new ParibuClient(new ParibuClientOptions
            {
                 LogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
            });





            api.SetAccessToken("DSiuNfKP6wQ5FROXEk5B0JWndCcglmMyX2KuYTm5");

            var xxx = api.GetBalances();
            if (xxx.Success)
            {
                foreach (var balance in xxx.Data)
                {
                    Console.WriteLine($"{balance.Key} Total:{balance.Value.Total} Available:{balance.Value.Available}");
                }
            }
            else
            {
                Console.WriteLine("Hata:" + xxx.Error.Message);
            }


















            #region Login Usage
            Console.Write("Telefon Numarasını Giriniz: ");
            var telefon = Console.ReadLine();
            
            Console.Write("Şifrenizi Giriniz: ");
            var sifre = Console.ReadLine();

            var token = "";
            var login = api.Login(telefon, sifre);
            if (login.Success)
            {
                Console.Write("OTP Şifresini Giriniz: ");
                var otpcode = Console.ReadLine();

                var loginOtp = api.LoginTwoFactor(login.Data.Token, otpcode);
                if (loginOtp.Success)
                {
                    token = loginOtp.Data.Token;
                    Console.WriteLine("Giriş İşlemi Başarılı. Paribu Token: " + loginOtp.Data.Token);
                    Debug.WriteLine("Giriş İşlemi Başarılı. Paribu Token: " + loginOtp.Data.Token);
                    Console.ReadLine();


                    var pxx = api.GetBalances();
                    if (pxx.Success)
                    {
                        foreach (var balance in pxx.Data)
                        {
                            Console.WriteLine($"{balance.Key} Total:{balance.Value.Total} Available:{balance.Value.Available}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hata:" + pxx.Error.Message);
                    }
                    Console.ReadLine();

                }
                else
                {
                    Console.WriteLine("Hata Oluştu. Kullanıcı Adı/Şifre yanlış olabilir.");
                    Console.WriteLine("Çıkmak için <ENTER>'a basın.");
                }
            }
            else
            {
                Console.WriteLine("Hata Oluştu. Kullanıcı Adı/Şifre yanlış olabilir.");
                Console.WriteLine("Çıkmak için <ENTER>'a basın.");
            }
            Environment.Exit(0);
            #endregion






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
                    Console.WriteLine($"Ticker State >> {data.Symbol} " +
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
                    Console.WriteLine($"Ticker Prices >> {data.Symbol} C:{data.Prices.Count()} P:{string.Join(',', data.Prices)}");
                }
            });

            // Order Book & Trades
            var sub02 = ws.SubscribeToMarketData("btc-tl", (data) =>
            {
                if (data != null)
                {
                    Console.WriteLine($"Book Update >> {data.Symbol} " +
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
                    Console.WriteLine($"New Trade >> {data.Symbol} T:{data.Timestamp} P:{data.Price} A:{data.Amount} S:{data.Side}");
                }
            });

            // Unsubscribe
            _ = ws.UnsubscribeAsync(sub02.Data);

            // Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}