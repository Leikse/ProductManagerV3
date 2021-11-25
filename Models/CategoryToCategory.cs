
// TODO: Remove
namespace ProductManager.Models
{
    public class CategoryToCategory
    {
        public int ParentCategory { get; }
        public int ChildCategory { get; }

        public CategoryToCategory(int parentCategory, int childCategory)
        {
            ParentCategory = parentCategory;
            ChildCategory = childCategory;
        }
    }
}
