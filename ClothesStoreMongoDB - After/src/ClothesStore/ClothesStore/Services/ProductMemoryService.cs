﻿using ClothesStore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.Services
{
    public class ProductMemoryService : IProductService
    {
        public static List<Product> Products { get; set; } = new List<Product>();

        public void Create(Product product)
        {
            product.Id = (Products.Count() + 1).ToString();
            Products.Add(product);
        }

        public void Update(string id, Product product)
        {
            Product originalProduct = Products.SingleOrDefault(p => p.Id == id);

            originalProduct.Name = product.Name;
        }

        public void Remove(string id)
        {
            Product product = Products.SingleOrDefault(p => p.Id == id);
            Products.Remove(product);
        }

        public List<Product> Get()
        {
            return Products;
        }

        public Product Get(string id)
        {
            return Products.SingleOrDefault(p => p.Id == id);
        }

        public void UpdateImage(string id, Product product, byte[] fileBytes)
        {
            throw new NotImplementedException();
        }

        public void UpdateImage(string id, IFormFile file)
        {
            throw new NotImplementedException();
        }

        public FileModel GetImage(string id)
        {
            throw new NotImplementedException();
        }
    }
}
