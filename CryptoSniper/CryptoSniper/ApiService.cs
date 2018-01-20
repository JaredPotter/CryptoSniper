using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using CryptoSniper.Models;
using CryptoSniper.Models.CEX.IO_api;
using CryptoSniper.Config.Models;

namespace CryptoSniper
{
    public static class ApiService
    {
        #region Methods

        // returns the account balance 
        public static BalanceResult GetAccountBalance(CexIoApiInfo credential)
        {
            //target to hit
            var endpoint = "https://cex.io/api/balance/";

            var jsonResponse = ExecutePostRequest(credential, endpoint);

            // Json convert
            var balance = JsonConvert.DeserializeObject<BalanceResult>(jsonResponse);

            return balance;
        }

        // TODO: Not tested yet.
        public static List<OrdersResult> GetOpenOrders(CexIoApiInfo credential)
        {
            //target to hit
            var endpoint = " https://cex.io/api/open_orders/";

            var jsonResponse = ExecutePostRequest(credential, endpoint);

            var orders = JsonConvert.DeserializeObject<List<OrdersResult>>(jsonResponse);

            return orders;
        }

        // TODO: Not tested yet.
        public static ActiveOrderStatusResult GetActiveOrderStatus(CexIoApiInfo credential)
        {
            var endpoint = "https://cex.io/api/active_orders_status";

            var jsonResponse = ExecutePostRequest(credential, endpoint);

            var activeOrderStatus = JsonConvert.DeserializeObject<ActiveOrderStatusResult>(jsonResponse);

            return activeOrderStatus;
        }

        // TODO: Not tested.
        public static ArchivedOrdersResults GetArchivedOrders(
            CexIoApiInfo credential,
            string curr1, 
            string curr2,
            string limit, 
            string dateTo,
            string dateFrom,
            string lastTxDateTo,
            string lastTxDateFrom, 
            string status)
        {
            var endpointBase = "https://cex.io/api/archived_orders/";
            var endpoint = endpointBase + curr1 + "/" + curr2;

            var formParameters = new Dictionary<string, string>() {
                { "limit", limit },
                { "dateTo", dateTo },
                { "dateFrom", dateFrom },
                { "lastTxDateTo", lastTxDateTo },
                { "lastTxDateFrom", lastTxDateFrom },
                { "status", status }
            };

            var jsonResponse = ExecutePostRequest(credential, endpoint, formParameters);

            var archivedOrders = JsonConvert.DeserializeObject<ArchivedOrdersResults>(jsonResponse);

            return archivedOrders;
        }

        // TODO: Not Tested
        public static bool CancelOrder(CexIoApiInfo credential, string orderId)
        {
            var endpoint = "https://cex.io/api/cancel_order/";

            var formParameters = new Dictionary<string, string>() {
                { "orderId", orderId }
            };

            var jsonResponse = ExecutePostRequest(credential, endpoint, formParameters);

            var cancelled = Convert.ToBoolean(jsonResponse);

            return cancelled;
        }

        // TODO: Not Tested
        public static CancelAllOrdersForGivenPairResult CancelAllOrdersForGivenPair(CexIoApiInfo credential, string curr1, string curr2)
        {
            var endpointBase = "https://cex.io/api/cancel_orders/BTC/USD";
            var endpoint = endpointBase + curr1 + "/" + curr2;

            var response = ExecutePostRequest(credential, endpoint);

            var result = JsonConvert.DeserializeObject<CancelAllOrdersForGivenPairResult>(response);

            return result;
        }

        // TODO: Not testsd
        public static PlaceOrderResult PlaceOrder(CexIoApiInfo credential, string curr1, string curr2, string type, string amount, string price)
        {
            var endpointBase = "https://cex.io/api/place_order/";
            var endpoint = endpointBase + curr1 + "/" + curr2;

            var formParameters = new Dictionary<string, string>() {
                { "type", type },
                { "amount", amount },
                { "price", price }
            };

            var jsonResponse = ExecutePostRequest(credential, endpoint, formParameters);

            var placeOrderResult = JsonConvert.DeserializeObject<PlaceOrderResult>(jsonResponse);

            return placeOrderResult;
        }

        // TODO: not tested
        /// <summary>
        /// 
        /// </summary>
        /// <param name="curr1">The currency we want to buy.</param>
        /// <param name="curr2">The currency we're buying with.</param>
        /// <param name="type">Buy or sell.</param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static PlaceInstantOrderResult PlaceInstantOrder(CexIoApiInfo credential, string curr1, string curr2, string type, string amount)
        {
            var endpointBase = "https://cex.io/api/place_order/";
            var endpoint = endpointBase + curr1 + "/" + curr2;

            var formParameters = new Dictionary<string, string>() {
                { "type", type },
                { "amount", amount },
                { "order_type", "market" }
            };

            var jsonResponse = ExecutePostRequest(credential, endpoint, formParameters);

            var placeInstantOrderResult = JsonConvert.DeserializeObject<PlaceInstantOrderResult>(jsonResponse);

            return placeInstantOrderResult;
        }

        public static OrderDetailsResult GetOrderDetails(CexIoApiInfo credential, int order_id)
        {
            var endpoint = "https://cex.io/api/get_order/";
            var formParameters = new Dictionary<string, string>() {
                { "id", order_id.ToString() }
            };

            var jsonResponse = ExecutePostRequest(credential, endpoint, formParameters);

            var orderDetailsResult = JsonConvert.DeserializeObject<OrderDetailsResult>(jsonResponse);

            return orderDetailsResult;
        }

        public static LastPriceResult GetLastPrice(string curr1, string curr2)
        {
            var endpoint = "https://cex.io/api/last_price/" + curr1 + "/" + curr2;
            var result = ExecuteGetRequest(endpoint);

            var lastPrice = JsonConvert.DeserializeObject<LastPriceResult>(result);

            return lastPrice;
        }

        #endregion

        #region HelperMethods

        //generates the signature of the user in Hex upper case
        private static Int32 GetNextNonce()
        {
            // As recommended by CEX.IO, the current unix time can be used as a nonce. 
            var nonce = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            return nonce;
        }

        private static string GenerateSignature(int nonce, string userId, string key, string secret)
        {
            var byteSecret = Encoding.ASCII.GetBytes(secret);
            var hmac = new HMACSHA256(byteSecret);
            var message = nonce + userId + key;
            var messageByteArray = Encoding.UTF8.GetBytes(message);
            var messageStream = new MemoryStream(messageByteArray);
            var signature = hmac.ComputeHash(messageStream);
            var hexUpperSignature = ByteArrayToString(signature).ToUpper();

            return hexUpperSignature;
        }

        //Converts byte array to a string
        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        //Send the request
        private static string ExecutePostRequest(CexIoApiInfo credential, string endpoint, Dictionary<string, string> parameters = null)
        {
            var request = WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var nonce = GetNextNonce();
            var userId = credential.UserId;
            var key = credential.Key;
            var secret = credential.Secret;

            //users signature
            var signature = GenerateSignature(nonce, userId, key, secret);

            var formParameters = new Dictionary<string, string>() {
                { "key", key },
                { "signature", signature },
                { "nonce", nonce.ToString() }
            };

            if(parameters != null)
            {
                foreach (var param in parameters)
                    formParameters.Add(param.Key, param.Value);
            }

            var postData = "";

            foreach(var param in formParameters)
            {
                var parameterLine = param.Key + "=" + param.Value + "&";
                postData += parameterLine;
            }

            postData = postData.Substring(0, postData.Length - 1);

            var byteArray = Encoding.ASCII.GetBytes(postData);
            
            request.ContentLength = byteArray.Length;
            
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteArray, 0, byteArray.Length);
            }
               
            var response = request.GetResponse();
            var responseStream = response.GetResponseStream();

            var result = "";
            using (var reader = new StreamReader(responseStream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static string ExecuteGetRequest(string url)
        {
            var result = "";
            var request = WebRequest.Create(url);
            request.Method = "GET";
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        #endregion
    }
}
