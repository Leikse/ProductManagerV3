namespace ProductManager
{
    public class Product
    {
        public string ArticleNumber { get; }

        public string Name { get; }

        public string Description { get; } 

        public string Url { get; }

        public int Price { get; }

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