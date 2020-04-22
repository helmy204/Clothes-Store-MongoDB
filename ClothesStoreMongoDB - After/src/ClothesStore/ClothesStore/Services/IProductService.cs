using System.Collections.Generic;
using ClothesStore.Models;
using Microsoft.AspNetCore.Http;

namespace ClothesStore.Services
{
    public interface IProductService
    {
        void Create(Product product);
        void Update(string id, Product product);
        void UpdateImage(string id, IFormFile file);
        void Remove(string id);
        List<Product> Get();
        Product Get(string id);
        FileModel GetImage(string id);
    }
}