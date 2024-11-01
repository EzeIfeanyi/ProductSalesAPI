namespace ProductSalesAPI.Application.ApiUtilities.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginAsync(string username, string password);
        Task<int> RegisterAsync(string username, string password);
    }
}