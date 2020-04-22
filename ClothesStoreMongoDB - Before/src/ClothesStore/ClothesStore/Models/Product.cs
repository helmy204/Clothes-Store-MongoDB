namespace ClothesStore.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
