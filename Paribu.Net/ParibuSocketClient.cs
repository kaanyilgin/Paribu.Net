﻿using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paribu.Net.CoreObjects;
using Paribu.Net.Helpers;
using Paribu.Net.SocketObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paribu.Net
{
    public partial class ParibuSocketClient : SocketClient, ISocketClient
    {
        public string PusherApplicationId { get; protected set; }

        #region Client Options
        protected static ParibuSocketClientOptions defaultOptions = new ParibuSocketClientOptions();
        protected static ParibuSocketClientOptions DefaultOptions => defaultOptions.Copy();
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Create a new instance of ParibuSocketClient with default options
        /// </summary>
        /// <param name="pusherApplicationId">Paribu Pusher Application Id</param>
        public ParibuSocketClient(string pusherApplicationId = "a68d528f48f652c94c88") : this(DefaultOptions, pusherApplicationId)
        {
        }

        /// <summary>
        /// Create a new instance of ParibuSocketClient using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        /// <param name="pusherApplicationId">Paribu Pusher Application Id</param>
        public ParibuSocketClient(ParibuSocketClientOptions options, string pusherApplicationId = "a68d528f48f652c94c88") : base("Paribu", options, null)
        {
            PusherApplicationId = pusherApplicationId;
            AddGenericHandler("Welcome", WelcomeHandler);
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Set the default options to be used when creating new socket clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(ParibuSocketClientOptions options)
        {
            defaultOptions = options;
        }

        /// <summary>
        /// Sets Paribu Pusher Application Id
        /// </summary>
        /// <param name="pusherApplicationId">Paribu Pusher Application Id</param>
        public virtual void SetPusherApplicationId(string pusherApplicationId)
        {
            PusherApplicationId = pusherApplicationId;
        }
        #endregion

        public virtual CallResult<UpdateSubscription> SubscribeToTickers(Action<ParibuSocketTicker> onTickerData, Action<ParibuSocketPriceSeries> onPriceSeriesData) => SubscribeToTickersAsync(onTickerData, onPriceSeriesData).Result;
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToTickersAsync(Action<ParibuSocketTicker> onTickerData, Action<ParibuSocketPriceSeries> onPriceSeriesData)
        {
            var internalHandler = new Action<DataEvent<ParibuSocketResponse>>(data =>
            {
                var json = JsonConvert.DeserializeObject<SocketPatch<SocketMerge<SocketTickers>>>(data.Data.Data);
                foreach (var ticker in json.Patch.Merge.Data)
                {
                    if (ticker.Value.PriceSeries != null && ticker.Value.PriceSeries.Count() > 0)
                    {
                        onPriceSeriesData(new ParibuSocketPriceSeries
                        {
                            Pair = ticker.Key,
                            Prices = ticker.Value.PriceSeries,
                        });
                    }
                    else
                    {
                        ticker.Value.Pair = ticker.Key;
                        onTickerData(ticker.Value);
                    }
                }
            });

            var request = new ParibuSocketRequest<ParibuSocketSubscribeRequest> { Event = "pusher:subscribe", Data = new ParibuSocketSubscribeRequest { Auth = "", Channel = "prb-public" } };
            return await SubscribeAsync(request, null, false, internalHandler).ConfigureAwait(false);
        }

        public virtual CallResult<UpdateSubscription> SubscribeToMarketData(string pair, Action<ParibuSocketOrderBook> onOrderBookData, Action<ParibuSocketTrade> onTradeData) => SubscribeToMarketDataAsync(pair, onOrderBookData, onTradeData).Result;
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToMarketDataAsync(string pair, Action<ParibuSocketOrderBook> onOrderBookData, Action<ParibuSocketTrade> onTradeData)
        {
            var internalHandler = new Action<DataEvent<ParibuSocketResponse>>(data =>
            {
                var patch = JsonConvert.DeserializeObject<SocketPatch<object>>(data.Data.Data);
                if (patch.Index == "orderBook")
                {
                    var pob = new ParibuSocketOrderBook { Pair = pair };
                    var json = JsonConvert.DeserializeObject<SocketPatch<SocketMerge<SocketOrderBook>>>(data.Data.Data.Replace(",\"merge\":[],", ",\"merge\":{},"));

                    if (json.Patch.Merge.Asks != null && json.Patch.Merge.Asks.Data != null && json.Patch.Merge.Asks.Data.Count > 0)
                        foreach (var ask in json.Patch.Merge.Asks.Data)
                            pob.AsksToAdd.Add(new ParibuSocketOrderBookEntry { Price = ask.Key, Amount = ask.Value });

                    if (json.Patch.Merge.Bids != null && json.Patch.Merge.Bids.Data != null && json.Patch.Merge.Bids.Data.Count > 0)
                        foreach (var bid in json.Patch.Merge.Bids.Data)
                            pob.BidsToAdd.Add(new ParibuSocketOrderBookEntry { Price = bid.Key, Amount = bid.Value });

                    if (json.Patch.Unset != null && json.Patch.Unset.Count() > 0)
                        foreach (var unset in json.Patch.Unset)
                        {
                            var unsetrow = unset.Split('/');
                            if (unsetrow.Length == 2)
                            {
                                if (unsetrow[0] == "buy")
                                    pob.BidsToRemove.Add(new ParibuSocketOrderBookEntry { Price = unsetrow[1].ToDecimal(), Amount = 0.0m });
                                if (unsetrow[0] == "sell")
                                    pob.AsksToRemove.Add(new ParibuSocketOrderBookEntry { Price = unsetrow[1].ToDecimal(), Amount = 0.0m });
                            }
                        }

                    onOrderBookData(pob);
                }
                else if (patch.Index == "marketMatches")
                {
                    var json = JsonConvert.DeserializeObject<SocketPatch<SocketMerge<IEnumerable<ParibuSocketTrade>>>>(data.Data.Data);
                    foreach (var trade in json.Patch.Merge)
                    {
                        trade.Pair = pair;
                        onTradeData(trade);
                    }
                }
            });

            var request = new ParibuSocketRequest<ParibuSocketSubscribeRequest> { Event = "pusher:subscribe", Data = new ParibuSocketSubscribeRequest { Auth = "", Channel = "prb-market-" + pair.ToLower() } };
            return await SubscribeAsync(request, null, false, internalHandler).ConfigureAwait(false);
        }

        #region Core Methods

        protected long iterator = 0;
        protected virtual long NextRequestId()
        {
            return ++iterator;
        }

        protected virtual void WelcomeHandler(MessageEvent messageEvent)
        {
            if (messageEvent.JsonData["event"] != null && (string)messageEvent.JsonData["event"] == "pusher:connection_established")
                return;
        }

        protected override SocketConnection GetSocketConnection(string address, bool authenticated)
        {
            return ParibuGetWebsocket(address, authenticated);
        }
        protected virtual SocketConnection ParibuGetWebsocket(string address, bool authenticated)
        {
            address = address.TrimEnd('/').Replace("{appid}", PusherApplicationId);
            var socketResult = sockets.Where(s =>
                s.Value.Socket.Url.TrimEnd('/') == address.TrimEnd('/') &&
                (s.Value.Authenticated == authenticated || !authenticated) &&
                s.Value.Connected).OrderBy(s => s.Value.SubscriptionCount).FirstOrDefault();
            var result = socketResult.Equals(default(KeyValuePair<int, SocketConnection>)) ? null : socketResult.Value;
            if (result != null)
            {
                if (result.SubscriptionCount < SocketCombineTarget || (sockets.Count >= MaxSocketConnections && sockets.All(s => s.Value.SubscriptionCount >= SocketCombineTarget)))
                {
                    // Use existing socket if it has less than target connections OR it has the least connections and we can't make new
                    return result;
                }
            }

            // Create new socket
            var socket = CreateSocket(address);
            var socketConnection = new SocketConnection(this, socket);
            socketConnection.UnhandledMessage += HandleUnhandledMessage;
            foreach (var kvp in genericHandlers)
            {
                var handler = SocketSubscription.CreateForIdentifier(NextId(), kvp.Key, false, kvp.Value);
                socketConnection.AddSubscription(handler);
            }

            return socketConnection;
        }

        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            return ParibuHandleQueryResponse<T>(s, request, data, out callResult);
        }
        protected virtual bool ParibuHandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            callResult = new CallResult<T>(default, null);
            return true;
        }

        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            return ParibuHandleSubscriptionResponse(s, subscription, request, message, out callResult);
        }
        protected virtual bool ParibuHandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            callResult = new CallResult<object>(true, null);

            // Check for Success
            if (request is ParibuSocketRequest<ParibuSocketSubscribeRequest> socRequest)
            {
                if (message["event"] != null && message["channel"] != null)
                {
                    if (socRequest.Data.Channel == (string)message["channel"] && (string)message["event"] == "pusher_internal:subscription_succeeded")
                    {
                        callResult = new CallResult<object>(true, null);
                        return true;
                    }
                    else
                    {
                        callResult = null;
                        return false;
                    }
                }
            }

            return true;
        }

        protected override bool MessageMatchesHandler(JToken data, object request)
        {
            return ParibuMessageMatchesHandler(data, request);
        }
        protected virtual bool ParibuMessageMatchesHandler(JToken data, object request)
        {
            if (request is ParibuSocketRequest<ParibuSocketSubscribeRequest> socRequest)
            {
                if (data["event"] == null || data["channel"] == null)
                    return false;

                // Get Event
                var evt = (string)data["event"];
                var channel = (string)data["channel"];

                // Tickers
                // Market Data
                if (evt == "state-updated" && socRequest.Data.Channel == channel)
                    return true;
            }

            return false;
        }

        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            return ParibuMessageMatchesHandler(message, identifier);
        }
        protected virtual bool ParibuMessageMatchesHandler(JToken message, string identifier)
        {
            return true;
        }

        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription s)
        {
            return await ParibuUnsubscribe(connection, s);
        }
        protected virtual async Task<bool> ParibuUnsubscribe(SocketConnection connection, SocketSubscription s)
        {
            if (s == null || s.Request == null)
                return false;

            var request = new ParibuSocketRequest<ParibuSocketSubscribeRequest> { Event = "pusher:unsubscribe", Data = new ParibuSocketSubscribeRequest { Auth = "", Channel = ((ParibuSocketRequest<ParibuSocketSubscribeRequest>)s.Request).Data.Channel } };
            await connection.SendAndWaitAsync(request, ResponseTimeout, data =>
            {
                return true;
            });

            return false;
        }

        protected override Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection s)
        {
            return ParibuAuthenticateSocket(s);
        }
        protected virtual Task<CallResult<bool>> ParibuAuthenticateSocket(SocketConnection s)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
