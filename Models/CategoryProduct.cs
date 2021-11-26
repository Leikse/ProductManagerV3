using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Models
{
    public class CategoryProduct
    {
        [Key]
        public int ProductId { get; protected set; }

        public int CategoryId { get; protected set; }

        [ForeignKey("ProductId")]
        public Product product { get; protected set; }

        [ForeignKey("CategoryId")]
        public Category category { get; protected set; }

        public CategoryProduct(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}
