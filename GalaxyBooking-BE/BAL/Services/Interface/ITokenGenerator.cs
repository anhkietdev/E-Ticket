namespace BAL.Services.Interface
{
    public interface ITokenGenerator
    {
        (string, TimeSpan) GenerateOtp(int truncationLevel = 6);
    }
}
