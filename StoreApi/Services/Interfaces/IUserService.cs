using Microsoft.AspNetCore.Mvc;

namespace StoreApi.Services.Interfaces
{
    public interface IUserService
    {
        int Register(string username, string passwordHash, string role);
        void SaveToken(int userId, string token);
    }
}
