using System.Collections.Generic;

namespace ProductManager
{
    public class Category
    {
        public Category(string name, string description, string url, int id)
        {
            Name = name;
            Description = description;
            Url = url;
            Id = id;

            ProductList = new Dictionary<string, Product>();
        }

        public Category(string name, string description, string url)
        {
            Name = name;
            Description = description;
            Url = url;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        // TODO: When done with SQL, remove these 2
        public Dictionary<string, Product> ProductList { get; }
        public void AddProduct(Product product)
        {
            ProductList.Add(product.ArticleNumber, product);
        }
    }
}
