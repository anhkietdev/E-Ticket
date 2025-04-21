using BAL.DTOs;
using BAL.Helpers;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Controllers
{
    public class ZalopayController : BaseController
    {
        private string key2 = "eG4r0GcoNtRGbO8";

        private readonly IZaloPayService _service;
        public ZalopayController(IZaloPayService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDTO request)
        {
            var result = await _service.CreateZalopayPayment(request);
            return Ok(new { redirectUrl = result });
        }

        [HttpPost]
        public IActionResult Callback([FromBody] dynamic cbdata)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var dataStr = Convert.ToString(cbdata["data"]);
                var reqMac = Convert.ToString(cbdata["mac"]);

                string mac = HashHelper.HmacSha256(key2, dataStr);

                Console.WriteLine("mac = {0}", mac);

                // kiểm tra callback hợp lệ (đến từ ZaloPay server)
                if (!reqMac.Equals(mac))
                {
                    // callback không hợp lệ
                    result["returncode"] = -1;
                    result["returnmessage"] = "mac not equal";
                }
                else
                {
                    // thanh toán thành công
                    // merchant cập nhật trạng thái cho đơn hàng
                    var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                    Console.WriteLine("update order's status = success where apptransid = {0}", dataJson["apptransid"]);

                    result["returncode"] = 1;
                    result["returnmessage"] = "success";
                }
            }
            catch (Exception ex)
            {
                result["returncode"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result["returnmessage"] = ex.Message;
            }

            // thông báo kết quả cho ZaloPay server
            return Ok(result);
        }
    }
}
