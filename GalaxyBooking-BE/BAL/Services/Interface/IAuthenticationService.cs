namespace BAL.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(string email, string password);
    }
}
