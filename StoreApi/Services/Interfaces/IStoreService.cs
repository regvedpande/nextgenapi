using System.Collections.Generic;
using StoreApi.Models;

namespace StoreApi.Services.Interfaces
{
    public interface IStoreService
    {
        List<Store> GetStores(int pageNumber, int pageSize);
        Store GetStoreById(int id);
        void CreateStore(Store store);
        void UpdateStore(int id, Store store);
        void DeleteStore(int id);
    }
}
