using System.Collections.Generic;
using ClothesStore.Models;

namespace ClothesStore.Services
{
    public interface IProductService
    {
        void Create(Product product);
        void Update(string id, Product product);
        void Remove(string id);
        List<Product> Get();
        Product Get(string id);
    }
}