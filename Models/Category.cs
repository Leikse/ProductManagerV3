using System.Collections.Generic;

namespace ProductManager
{
    public class Category
    {
        public string name;

        public string description;

        public string url;

        public Dictionary<string, Product> productList = new Dictionary<string, Product>();
    }
}
