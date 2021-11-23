using System.Collections.Generic;

namespace ProductManager
{
    public class Category
    {
        public Category(string name, string description, string url, int id, int parentCategoryId)
        {
            Name = name;
            Description = description;
            Url = url;
            Id = id;
            ParentCategoryId = parentCategoryId;
            CategoryInCategory = new List<Category>();
            ProductInCategory = new List<Product>();
        }

        public Category(string name, string description, string url)
        {
            Name = name;
            Description = description;
            Url = url;
            CategoryInCategory = new List<Category>();
            ProductInCategory = new List<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int ParentCategoryId { get; set; }
        public List<Category> CategoryInCategory { get; set; }
        public List<Product> ProductInCategory { get; set; }
    }
}
