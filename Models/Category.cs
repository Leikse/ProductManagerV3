using System.Collections.Generic;

namespace ProductManager
{
    public class Category
    {
        public Category(string name, string description, string url)
        {
            Name = name;
            Description = description;
            Url = url;
            ProductList = new Dictionary<string, Product>();
        }

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
