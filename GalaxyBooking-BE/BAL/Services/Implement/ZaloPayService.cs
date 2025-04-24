using BAL.DTOs;
using BAL.DTOs.ZaloPay;
using BAL.Extension;
using BAL.Helpers;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Config;
using BAL.Services.ZaloPay.Request;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Dynamic;

namespace BAL.Services.Implement
{
    public class ZaloPayService : IZaloPayService
    {
        private readonly ZaloPayConfig _zaloPayConfig;
        private readonly IUnitOfWork _unitOfWork;
        public ZaloPayService(IOptions<ZaloPayConfig> zaloPayConfig, IUnitOfWork unitOfWork)
        {
            _zaloPayConfig = zaloPayConfig.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<(int, string)> CallBackPayment(CallBackPaymentDTO request)
        {
            (int returnCode, string returnMessage) = await VerifyPayment(request);
            if (returnCode == -1)
            {
                return (returnCode, returnMessage);
            }

            CallBackObj callBackObj = JsonConvert.DeserializeObject<CallBackObj>(request.Data);
            EmbedDataDTO embedDataDTO = JsonConvert.DeserializeObject<EmbedDataDTO>(callBackObj.EmbedData);

            List<Ticket> ticketLst = new List<Ticket>();

            foreach (var item in embedDataDTO.TicketIds)
            {
                var ticket = await _unitOfWork.TicketRepository.GetAsync(t => t.Id == item);
                ticket.IsPaymentSuccess = true;

                ticketLst.Add(ticket);
            }

            await _unitOfWork.TicketRepository.UpdateRange(ticketLst);
            await _unitOfWork.SaveAsync();

            return (returnCode, returnMessage);
        }

        public async Task<(bool, string)> CreateZalopayPayment(PaymentDTO request)
        {
            //todo add expandoobj for embeded data
            var embedData = new EmbedDataDTO();
            embedData.RedirectUrl = _zaloPayConfig.RedirectUrl.ToString();
            embedData.TicketIds = request.TicketIds;
            embedData.UserId = request.UserId.ToString();
            embedData.ProjectionId = request.ProjectionId.ToString();

            var zalopayRequest = new CreateZalopayRequest
            {
                AppId = _zaloPayConfig.AppId,
                AppUser = _zaloPayConfig.AppUser,
                AppTime = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                Amount = (long)request.RequiredAmount,
                AppTransId = DateTime.Now.ToString("yymmdd"),
                Description = request.PaymentContent,
                EmbedData = JsonConvert.SerializeObject(embedData).ToString(),
                ReturnUrl = _zaloPayConfig.RedirectUrl,
                BankCode = request.BankCode,
                CallbackUrl = _zaloPayConfig.CallbackUrl,
                Item = JsonConvert.SerializeObject(request.Items)
            };
            zalopayRequest.MakeSignature(_zaloPayConfig.Key1);
            (bool createZaloPayLinkResult, string createZaloPayMessage) = zalopayRequest.GetLink(_zaloPayConfig.PaymentUrl);
            if (createZaloPayLinkResult)
            {
                return (true, createZaloPayMessage);
            }
            else
            {
                return (false, string.Empty);
            }
        }

        public async Task<(int, string)> VerifyPayment(CallBackPaymentDTO request)
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
    }
}
