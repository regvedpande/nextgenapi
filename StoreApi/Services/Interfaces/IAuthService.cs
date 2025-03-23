using StoreApi.Models;

namespace StoreApi.Services.Interfaces
{
    public interface IAuthService
    {
        User Authenticate(string username, string passwordHash);
        string GetToken(int userId);
        void SaveToken(int userId, string token);
    }
}
