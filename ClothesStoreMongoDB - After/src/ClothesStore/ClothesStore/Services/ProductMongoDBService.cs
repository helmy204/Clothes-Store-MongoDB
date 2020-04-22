using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClothesStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ClothesStore.Services
{
    public class ProductMongoDBService : IProductService
    {
        private readonly IMongoCollection<Product> products;
        private readonly IGridFSBucket bucket;

        public ProductMongoDBService(IConfiguration config)
        {
            MongoClient client = new MongoClient(config.GetConnectionString("ClothesStoreDbConnection"));
            IMongoDatabase database = client.GetDatabase("ClothesStoreDb");
            products = database.GetCollection<Product>("Products");
            bucket = new GridFSBucket(database);
        }

        public void Create(Product product)
        {
            products.InsertOne(product);
        }

        public void Update(string id, Product productIn)
        {
            products.ReplaceOne(product => product.Id == id, productIn);
        }

        public void Remove(string id)
        {
            products.DeleteOne(product => product.Id == id);
        }

        public List<Product> Get()
        {
            return products.Find(product => true).ToList();
        }

        public Product Get(string id)
        {
            return products.Find(product => product.Id == id).FirstOrDefault();
        }

        public void UpdateImage(string id, IFormFile file)
        {
            var product = Get(id);
            byte[] fileBytes = null;

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }

            GridFSUploadOptions options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "ContentType", file.ContentType }
                }
            };

            var imageId = bucket.UploadFromBytes(file.FileName, fileBytes, options);

            product.ImageId = imageId.ToString();

            products.ReplaceOne(p => p.Id == id, product);
        }

        public FileModel GetImage(string id)
        {
            ObjectId objectId = ObjectId.Parse(id);
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);

            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions
            {
                Limit = 1,
                Sort = sort
            };
            GridFSFileInfo info;
            using (var cursor = bucket.Find(filter, options))
            {
                info = cursor.ToList().FirstOrDefault();
                // fileInfo either has the matching file information or is null
            }

            var bytes = bucket.DownloadAsBytes(objectId);

            FileModel file = new FileModel()
            {
                FileContents = bytes,
                ContentType = info.Metadata["ContentType"].ToString()
            };

            return file;
        }
    }

    public class FileModel
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
    }
}
