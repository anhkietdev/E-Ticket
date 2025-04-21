using BAL.DTOs;
using BAL.Extension;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Config;
using BAL.Services.ZaloPay.Request;
using Microsoft.Extensions.Options;

namespace BAL.Services.Implement
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly ZaloPayConfig _zaloPayConfig;
        public ZaloPayService(IOptions<ZaloPayConfig> zaloPayConfig)
        {
            _zaloPayConfig = zaloPayConfig.Value;
        }
        public async Task<string> CreateZalopayPayment(PaymentDTO request)
        {
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
