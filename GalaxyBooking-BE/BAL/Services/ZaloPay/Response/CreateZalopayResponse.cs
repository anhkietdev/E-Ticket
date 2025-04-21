namespace BAL.Services.ZaloPay.Response
{
    public class CreateZalopayResponse
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public string OrderUrl { get; set; }
    }
}
