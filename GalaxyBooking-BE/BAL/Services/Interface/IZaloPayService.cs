using BAL.DTOs;
using BAL.DTOs.ZaloPay;

namespace BAL.Services.Interface
{
    public interface IZaloPayService
    {
        Task<(bool, string)> CreateZalopayPayment(PaymentDTO request);
        Task<int> CheckOrderStatus();
    }
}
