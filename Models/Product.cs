using System.ComponentModel.DataAnnotations;

namespace ProductManager.Models
{
    public class Product
    {
        public int Id { get; protected set; }

        [Required]
        [MaxLength(10)]
        public string ArticleNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; protected set; }

        [MaxLength(50)]
        public string Description { get; protected set; } 

        [MaxLength(100)]
        public string Url { get; protected set; }

        [MaxLength(10)]
        public decimal Price { get; protected set; }

        public Product(int id, string articleNumber, string name, string description, string url, decimal price)
        {
            Id = id;
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Url = url;
            Price = price;
        }

        public Product(string articleNumber, string name, string description, string url, decimal price)
        {
            ArticleNumber = articleNumber;
            Name = name;
            Description = description;
            Url = url;
            Price = price;
        }
    }
}