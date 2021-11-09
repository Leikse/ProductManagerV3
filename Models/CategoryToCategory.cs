namespace ProductManager.Models
{
    public class CategoryToCategory
    {
        public string ParentCategory { get; }
        public string ChildCategory { get; }

        public CategoryToCategory(string parentCategory, string childCategory)
        {
            ParentCategory = parentCategory;
            ChildCategory = childCategory;
        }
    }
}
