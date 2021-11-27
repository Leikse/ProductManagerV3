using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Models
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

        public int Id { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; protected set; }

        [MaxLength(100)]
        public string Description { get; protected set; }

        [MaxLength(100)]
        public string Url { get; protected set; }

        [MaxLength(10)]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public Category category { get; protected set; }
        public List<Category> CategoryInCategory { get; set; }
        public List<Product> ProductInCategory { get; set; }
    }
}
