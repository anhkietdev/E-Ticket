using Azure.Core;
using BAL.Helpers;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Request;
using BAL.Services.ZaloPay.Response;
using Newtonsoft.Json;

namespace BAL.Extension
{
    public static class ZalopayExtension
    {
        public static void MakeSignature(this CreateZalopayRequest request, string key)
        {
            var data = $"{request.AppId}|{request.AppTransId}|{request.AppUser}|{request.Amount}|{request.AppTime}|{request.EmbedData}|{request.Item}";
            request.Mac = HashHelper.HmacSha256(key, data);
        }

        public static Dictionary<string, string> GetContent(this CreateZalopayRequest request)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("appid", request.AppId.ToString());
            keyValuePairs.Add("appuser", request.AppUser.ToString());
            keyValuePairs.Add("apptime", request.AppTime.ToString());
            keyValuePairs.Add("amount", request.Amount.ToString());
            keyValuePairs.Add("apptransid", request.AppTransId);
            keyValuePairs.Add("description", request.Description);
            keyValuePairs.Add("bankcode", "zalopayapp");
            keyValuePairs.Add("embeddata", request.EmbedData);
            keyValuePairs.Add("mac", request.Mac);
            keyValuePairs.Add("callbackurl", request.CallbackUrl);
            keyValuePairs.Add("item", request.Item);

            return keyValuePairs;
        }

        public static int CheckStatus(this IZaloPayService service, int appId, string apptransid, string key1, string orderUrl)
        {
            var param = new Dictionary<string, string>();
            param.Add("appid", appId.ToString());
            param.Add("apptransid", apptransid.ToString());
            var data = appId + "|" + apptransid + "|" + key1;

            param.Add("mac", HashHelper.HmacSha256(key1, data));
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(param);

            var response = client.PostAsync(orderUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<CreateZalopayResponse>(responseContent);

                if (responseData.ReturnCode == Constants.Constant.ZaloPayConfig.ZaloPaymentSuccessStatus)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public static (bool, string) GetLink(this CreateZalopayRequest request, string paymentUrl)
        {
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(GetContent(request));
            var response = client.PostAsync(paymentUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<CreateZalopayResponse>(responseContent);

                if (responseData.ReturnCode == Constants.Constant.ZaloPayConfig.ZaloPaymentSuccessStatus)
                {
                    return (true, responseData.OrderUrl);
                }
                else
                {
                    return (false, responseData.ReturnMessage);
                }
            }
            else
            {
                return (false, response.ReasonPhrase ?? string.Empty);
            }
        }
    }
}
