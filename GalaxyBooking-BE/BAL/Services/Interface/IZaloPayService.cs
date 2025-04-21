using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface IZaloPayService
    {
        Task<string> CreateZalopayPayment(PaymentDTO request);

        Task<(int, string)> CallBackPayment(CallBackPaymentDTO request);
    }
}
