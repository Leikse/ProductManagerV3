namespace ProductManager
{
    public class Product
    {
        public int Id { get; }
        public string ArticleNumber { get; }

        public string Name { get; }

        public string Description { get; } 

        public string Url { get; }

        public int Price { get; }

        public Product(int id, string articleNumber, string name, string description, string url, int price)
        {
            Id = id;
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Url = url;
            Price = price;
        }

        public Product(string articleNumber, string name, string description, string url, int price)
        {
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Url = url;
            Price = price;
        }
    }
}