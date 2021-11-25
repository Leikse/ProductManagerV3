using System.ComponentModel.DataAnnotations;

namespace ProductManager.Models
{
    public class CategoryProduct
    {
        [Key]
        public int ProductId { get; protected set; }
        public int CategoryId { get; protected set; }

        public CategoryProduct(int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }
    }
}
