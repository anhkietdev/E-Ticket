using BAL.DTOs;
using BAL.Extension;
using BAL.Helpers;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Config;
using BAL.Services.ZaloPay.Request;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BAL.Services.Implement
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly ZaloPayConfig _zaloPayConfig;
        public ZaloPayService(IOptions<ZaloPayConfig> zaloPayConfig)
        {
            _zaloPayConfig = zaloPayConfig.Value;
        }

        public async Task<(int, string)> CallBackPayment(CallBackPaymentDTO request)
        {
            var reqMac = request.Mac;
            var mac = HashHelper.HmacSha256(_zaloPayConfig.Key2, request.Data);

            Console.WriteLine("mac = {0}", mac);

            if (!reqMac.Equals(mac))
            {
                return (-1, "mac not equal");
            }
            else
            {
                var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.Data);
                Console.WriteLine("update order's status = success where apptransid = {0}", dataJson["apptransid"]);

                return (1, "mac not equal");
            }
        }

        public async Task<string> CreateZalopayPayment(PaymentDTO request)
        {
            //todo add expandoobj for embeded data
            var zalopayRequest = new CreateZalopayRequest
            {
                AppId = _zaloPayConfig.AppId,
                AppUser = _zaloPayConfig.AppUser,
                AppTime = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                Amount = (long)request.RequiredAmount,
                AppTransId = DateTime.Now.ToString("yymmdd"),
                Description = request.PaymentContent,
            };
            zalopayRequest.MakeSignature(_zaloPayConfig.Key1);
            (bool createZaloPayLinkResult, string createZaloPayMessage) = zalopayRequest.GetLink(_zaloPayConfig.PaymentUrl);
            if (createZaloPayLinkResult)
            {
                return createZaloPayMessage;
            }
            else
            {
                return "Fail";
            }
        }
    }
}
