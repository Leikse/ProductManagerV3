using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;
using ProductManager.Models;
using ProductManager.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductManager
{
    enum MainMenu
    { 
        AddProduct,
        SearchProduct,
        AddCategory,
        AddProductToCategory,
        ListCategories,
        AddCategoryToCategory,
        Logout,
        Exit
    }

    class Program
    {
        // TODO: Remove
        static string connectionString;

        static void Main(string[] args)
        {
            var isAuthenticated = false;
            var isRunning = true;

            do
            {
                while (!isAuthenticated)
                {
                    isAuthenticated = AuthenticateUser();
                }

                Clear();

                CursorVisible = false;

                WriteLine("1. Add product");
                WriteLine("2. Search product");
                WriteLine("3. Add category");
                WriteLine("4. Add product to category");
                WriteLine("5. List categories");
                WriteLine("6. Add Category to category");
                WriteLine("7. Logout");
                WriteLine("8. Exit");

                MainMenu menuChoice = AwaitUserChoice(new Dictionary<ConsoleKey, MainMenu>
                {
                    { ConsoleKey.D1, MainMenu.AddProduct },
                    { ConsoleKey.NumPad1, MainMenu.AddProduct },
                    { ConsoleKey.D2, MainMenu.SearchProduct },
                    { ConsoleKey.NumPad2, MainMenu.SearchProduct },
                    { ConsoleKey.D3, MainMenu.AddCategory },
                    { ConsoleKey.NumPad3, MainMenu.AddCategory },
                    { ConsoleKey.D4, MainMenu.AddProductToCategory },
                    { ConsoleKey.NumPad4, MainMenu.AddProductToCategory },
                    { ConsoleKey.D5, MainMenu.ListCategories },
                    { ConsoleKey.NumPad5, MainMenu.ListCategories },
                    { ConsoleKey.D6, MainMenu.AddCategoryToCategory },
                    { ConsoleKey.NumPad6, MainMenu.AddCategoryToCategory },
                    { ConsoleKey.D7, MainMenu.Logout },
                    { ConsoleKey.NumPad7, MainMenu.Logout },
                    { ConsoleKey.D8, MainMenu.Exit },
                    { ConsoleKey.NumPad8, MainMenu.Exit }
                });

                CursorVisible = true;

                Clear();

                switch (menuChoice)
                {
                    case MainMenu.AddProduct:
                    {
                        AddProduct();
                    }
                        break;

                    case MainMenu.SearchProduct:
                    {
                        // TODO: Need fixes 
                        ListProduct();
                    }
                        break;

                    case MainMenu.AddCategory:
                    {
                        AddCategory();
                    }
                        break;

                    case MainMenu.AddProductToCategory:
                    {
                        AddProductToCategory();
                    }
                        break;

                    case MainMenu.ListCategories:
                    {
                        ListCategories();
                    }
                        break;

                    case MainMenu.AddCategoryToCategory:
                    {
                        AddCategoryToCategory();
                    }
                        break;

                    case MainMenu.Logout:
                    {
                        isAuthenticated = false;
                    }
                        break;

                    case MainMenu.Exit:
                    {
                        isRunning = false;
                    }
                        break;
                }
            } while (isRunning);
        }

        private static MainMenu AwaitUserChoice(Dictionary<ConsoleKey, MainMenu> userChoice)
        {
            ConsoleKeyInfo input;

            bool invalidChoice;

            do
            {
                input = ReadKey(true);

                invalidChoice = !userChoice.ContainsKey(input.Key);

            } while (invalidChoice);

            return userChoice[input.Key];
        }

        static bool AuthenticateUser()
        {
            bool isRunning = true;

            do
            {
                Clear();

                WriteLine("Username: ");

                var username = ReadLine();

                WriteLine("\nPassword: ");

                var password = ReadLine();

                Login login = FindLogin(username);

                var usernameInvalid = username == null;
                var passwordInvalid = password == null;
                var usernameCorrect = username == login.Username;
                var passwordCorrect = password == login.Password;

                if (!usernameInvalid && !passwordInvalid && usernameCorrect && passwordCorrect)
                {
                    isRunning = false;
                }
                else
                {
                    Clear();

                    CursorVisible = false;

                    WriteLine("Invalid credentials, please try again");

                    Thread.Sleep(2000);

                    CursorVisible = true;
                }

            } while (isRunning);

            return true;
        }

        private static Login FindLogin(string username)
        {
            using var context = new ProductManagerContext();

            var login = context.Logins
                .FirstOrDefault(x => x.Username == username);
            
            return login;
        }

        private static void AddProduct()
        {
            Product product = CreateProduct();

            WriteLine("Is this correct? (Y)es (N)o");

            CursorVisible = false;

            ConsoleKeyInfo input;

            bool invalidChoice;

            do
            {
                input = ReadKey(true);

                invalidChoice = !(input.Key == ConsoleKey.Y || input.Key == ConsoleKey.N);

            }
            while (invalidChoice);

            Product productExist = FindProduct(product.ArticleNumber);

            var productNotExists = productExist == null;

            Clear();

            if (input.Key == ConsoleKey.Y && !productNotExists)
            {
                WriteLine("Product already exists!");

                Thread.Sleep(2000);
            }
            else if (input.Key == ConsoleKey.Y && productNotExists)
            {
                SaveProduct(product);

                WriteLine("Product saved");

                Thread.Sleep(2000);
            }
            CursorVisible = true;

            Clear();
        }

        static Product FindProduct(string articleNumber)
        {
            using var context = new ProductManagerContext();

            Product product = context.Products
                .FirstOrDefault(x => x.ArticleNumber == articleNumber);

            return product;
        }

        private static Product CreateProduct()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Write("Name: ");

            var name = ReadLine();

            Write("Description: ");

            var description = ReadLine();

            Write("Image URL: ");

            var url = ReadLine();

            Write("Price: ");

            var price = int.Parse(ReadLine());

            Product product = new(articleNumber: articleNumber, name: name, description: description, url: url, price: price);

            return product;
        }

        private static void SaveProduct(Product product)
        {
            using var context = new ProductManagerContext();

            context.Products.Add(product);

            context.SaveChanges();
        }

        static void ListProduct()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Clear();
            
            var products = FindProductList(articleNumber);

            var productNotExists = products == null;

            var isRunning = true;

            do
            {
                if (!productNotExists)
                {
                    bool isRunningAgain = true;

                    do
                    {
                        foreach (var product in products)
                        {
                            WriteLine($"Article number: {product.ArticleNumber}");
                            WriteLine($"Name: {product.Name}");
                            WriteLine($"Description: {product.Description}");
                            WriteLine($"Url: {product.Url}");
                            WriteLine($"Price: {product.Price}");
                            WriteLine("\n(D)elete");

                            CursorVisible = false;
                        }

                        ConsoleKeyInfo inputAgain;

                        bool invalidChoiceAgain;

                        do
                        {
                            inputAgain = ReadKey(true);

                            invalidChoiceAgain = !(inputAgain.Key == ConsoleKey.Escape || inputAgain.Key == ConsoleKey.D);
                        }
                        while (invalidChoiceAgain);

                        switch (inputAgain.Key)
                        {
                            case ConsoleKey.D:

                                bool isRunningThird = true;

                                do
                                {
                                    Clear();

                                    foreach (var product in products)
                                    {
                                        WriteLine($"Article number: {product.ArticleNumber}");
                                        WriteLine($"Name: {product.Name}");
                                        WriteLine($"Description: {product.Description}");
                                        WriteLine($"Url: {product.Url}");
                                        WriteLine($"Price: {product.Price}");
                                    }

                                    WriteLine("\nAre you sure you want to delete? (Y)es (N)o");

                                    ConsoleKeyInfo inputThird;

                                    bool invalidChoiceThird;

                                    do
                                    {
                                        inputThird = ReadKey(true);

                                        invalidChoiceThird = !(inputThird.Key == ConsoleKey.Y || inputThird.Key == ConsoleKey.N);

                                    }
                                    while (invalidChoiceThird);

                                    Clear();

                                    switch (inputThird.Key)
                                    {
                                        case ConsoleKey.Y:

                                        // TODO: Fix so products delete from category after product to category works
                                            //foreach (var product in products)
                                            //{
                                            //    DeleteCategory(product.Id);
                                            //}

                                            DeleteProduct(articleNumber);

                                            WriteLine("Product deleted");

                                            Thread.Sleep(2000);

                                            isRunningThird = false;
                                            isRunningAgain = false;
                                            break;

                                        case ConsoleKey.N:

                                            isRunningThird = false;

                                            break;
                                    }
                                } while (isRunningThird);

                                break;

                            case ConsoleKey.Escape:

                                isRunningAgain = false;

                                break;
                        }
                    } while (isRunningAgain);

                    isRunning = false;
                }
                else
                {
                    CursorVisible = false;

                    WriteLine("Product not found");

                    Thread.Sleep(2000);

                    isRunning = false;
                }
                Clear();

            } while (isRunning);
        }

        private static void DeleteProduct(string articleNumber)
        {
            using var context = new ProductManagerContext();

            var productToRemove = context.Products.SingleOrDefault(x => x.ArticleNumber == articleNumber);

            if (productToRemove != null)
            { 
                context.Products.Remove(productToRemove);
                context.SaveChanges();
            }
        }

        // TODO: REmove
        //private static void DeleteCategory(int productId)
        //{
        //    // TODO: Change to EF Core
        //    using var context = new ProductManagerContext();

        //    var productToRemove = context.CategoryProduct.SingleOrDefault(x => x.ProductId == productId);

        //    if (productToRemove != null)
        //    {
        //        context.CategoryProduct.Remove(productToRemove);
        //        context.SaveChanges();
        //    }
        //}

        private static IList<Product> FindProductList(string inputArticleNumber)
        {
            using var context = new ProductManagerContext();

            var productList = context.Products.ToList();

            return productList;
        }

        static void AddCategory()
        {
            var isRunning = true;

            Category category = CreateCategory();

            do
            {
                WriteLine("Is this correct? (Y)es (N)o");

                CursorVisible = false;

                ConsoleKeyInfo input;

                bool invalidChoice;

                do
                {
                    input = ReadKey(true);

                    invalidChoice = !(input.Key == ConsoleKey.Y || input.Key == ConsoleKey.N);

                }
                while (invalidChoice);

                Clear();

                if (input.Key == ConsoleKey.Y)
                {
                    SaveCategory(category);

                    WriteLine("Category added");

                    Thread.Sleep(2000);

                    isRunning = false;
                }
                else
                {
                    isRunning = false;
                }
                Clear();

            } while (isRunning);
        }

        static Category CreateCategory()
        {
            Write("Name: ");

            var name = ReadLine();

            Write("Description: ");

            var description = ReadLine();

            Write("Image URL: ");

            var url = ReadLine();

            var category = new Category(name, description, url);

            return category;
        }

        private static void SaveCategory(Category category)
        {
            using var context = new ProductManagerContext();

            context.Categories.Add(category);

            context.SaveChanges();
        }

        static void AddProductToCategory()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Clear();
            
            Product product = FindProduct(articleNumber);

            var productNotExists = product == null;

            if (!productNotExists)
            {
                WriteLine("Category name: ");

                var name = ReadLine();

                Category category = FindCategory(name);

                var categoryNotExists = category == null;

                Clear();

                CursorVisible = false;

                if (!categoryNotExists)
                {
                    SaveProductToCategory(category, product);

                    WriteLine("Product added to category");

                    Thread.Sleep(2000);
                }
                else
                {
                    WriteLine("Category not found");

                    Thread.Sleep(2000);
                }
            }
            else
            {
                CursorVisible = false;

                WriteLine("Product not found");

                Thread.Sleep(2000);
            }
            Clear();
        }

        private static void SaveProductToCategory(Category category, Product product)
        {
            using var context = new ProductManagerContext();

            context.Categories.Attach(category);
            context.Products.Attach(product);

            category.Products.Add(product);

            context.SaveChanges();

            //string sql = @"INSERT INTO CategoryProduct (
            //             ProductId,
            //             CategoryId
            //             ) VALUES (
            //             @ProductId,
            //             @CategoryId)";

            //using SqlConnection connection = new(connectionString);
            //using SqlCommand command = new(sql, connection);

            //command.Parameters.AddWithValue("@ProductId", product.Id);
            //command.Parameters.AddWithValue("@CategoryId", category.Id);

            //connection.Open();

            //command.ExecuteNonQuery();

            //connection.Close();
        }

        static Category FindCategory(string name)
        {
            using var context = new ProductManagerContext();

            Category category = context.Categories
                .FirstOrDefault(x => x.Name == name);

            return category;

            //string sql = @"
            //            SELECT Id,
            //                   Name,
            //                   Description,
            //                   Url,
            //                   ParentCategoryId
            //            FROM Categorys
            //            WHERE Name = @Name
            //";

            //using var connection = new SqlConnection(connectionString);

            //using var command = new SqlCommand(sql, connection);

            //command.Parameters.AddWithValue("@Name", name);

            //connection.Open();

            //var dataReader = command.ExecuteReader();

            //Category category = null;

            //if (dataReader.Read())
            //{
            //    category = new Category(
            //        id: (int) dataReader["Id"],
            //        name: (string) dataReader["Name"],
            //        description: (string) dataReader["Description"],
            //        url: (string) dataReader["Url"],
            //        parentCategoryId: (int) (dataReader["ParentCategoryId"] as int?).GetValueOrDefault());
            //}

            //connection.Close();
        }

        static void ListCategories()
        {
            CursorVisible = false;

            WriteLine("Name\t\tPrice");
            WriteLine("------------------------------------------------------------------------");

            List <Category> rootCategories = FindRootCategory();

            rootCategories.ForEach(rootCategory => 
            {
                var childCategories = FindChildCategoriesList(rootCategory.Id);
                rootCategory.CategoryInCategory = childCategories;

                var amountOfProducts = FindAmountOfProducts(rootCategory);

                WriteLine($"{rootCategory.Name} ({amountOfProducts})");

                PrintChildCategory(rootCategory);
            });

            while (ReadKey(true).Key != ConsoleKey.Escape);

            Clear();
        }

        public static void PrintChildCategory(Category category, int level = 0)
        {
            var childCategories = category.CategoryInCategory;
            if (childCategories == null) return;

            var spaces = "";

            for (int i = 0; i < level; i++)
            {
                spaces += "  ";
            }

            childCategories.ForEach(category =>
            {
                WriteLine($"{spaces}  {category.Name}");

                PrintProducts(category, level + 1);

                if (category.CategoryInCategory == null || category.CategoryInCategory.Count() < 1)
                {
                    return;
                }
               
                PrintChildCategory(category, level + 1);
            });
        }

        public static void PrintProducts(Category category, int level = 0)
        {
            var childProducts = category.Products;
            if (childProducts == null) return;

            var spaces = "";

            for (int i = 0; i < level; i++)
            {
                spaces += "   ";
            }

            childProducts.ForEach(product =>
            {
                WriteLine($"{spaces}  {product.Name}      {product.Price}");
            });
        }

        public static int FindAmountOfProducts(Category category)
        {
            var childCategories = category.CategoryInCategory;
            if (childCategories == null) return 0;

            var totalProductsCounter = 0;

            childCategories.ForEach(category =>
            {
                var products = category.Products;

                var numberOfProducts = products.Count();

                var numberOfChildProducts = FindAmountOfProducts(category);

                totalProductsCounter += numberOfProducts + numberOfChildProducts;
            });

            return totalProductsCounter;
        }

        private static List<Product> FindProductsForCategories(int categoryId)
        {
            using var context = new ProductManagerContext();

            List<Product> childProductsList = new List<Product>();

            // TODO: Fix finding categoryProducts
            var products = context.Products.FirstOrDefault();

            childProductsList.Add(products);

            if (products != null)
            {
                return childProductsList;
            }
            else
            {
                return null;
            }

            return childProductsList;

            //string sql = @"
            //            SELECT Products.Id,
            //                   Products.ArticleNumber,
            //                   Products.Name,
            //                   Products.Description,
            //                   Products.Url,
            //                   Products.Price
            //            FROM Products
            //            INNER JOIN CategoryProduct 
            //                    ON CategoryProduct.ProductId = Products.Id
            //                 WHERE CategoryProduct.CategoryId = @categoryId
            //";

            //using var connection = new SqlConnection(connectionString);
            //using var command = new SqlCommand(sql, connection);

            //command.Parameters.AddWithValue("@category", categoryId);

            //connection.Open();

            //var dataReader = command.ExecuteReader();

            //List<Product> childProductsList = new List<Product>();

            //while (dataReader.Read())
            //{
            //    var id = (int)dataReader["Id"];
            //    var articleNumber = (string)dataReader["ArticleNumber"];
            //    var name = (string)dataReader["Name"];
            //    var description = (string)dataReader["Description"];
            //    var url = (string)dataReader["Url"];
            //    var price = (int)dataReader["Price"];

            //    Product product = new Product(id, articleNumber, name, description, url, price);

            //    childProductsList.Add(product);
            //}

            //connection.Close();

            //return childProductsList;
        }

        private static List<Category> FindChildCategoriesList(int parentId)
        {
            using var context = new ProductManagerContext();

            List<Category> parentCategoryList = new List<Category>();

            var childCategory = context.Categories.FirstOrDefault(x => x.ParentCategoryId == parentId);

            parentCategoryList.Add(childCategory);

            //string sql = @"
            //            SELECT Id,
            //                   Name,
            //                   Description,
            //                   Url,
            //                   ParentCategoryId
            //            FROM Categorys
            //            WHERE ParentCategoryId = @parentId
            //";

            //using var connection = new SqlConnection(connectionString);
            //using var command = new SqlCommand(sql, connection);

            //command.Parameters.AddWithValue("@parentId", parentId);

            //connection.Open();

            //var dataReader = command.ExecuteReader();

            //List<Category> parentCategoryList = new List<Category>();

            //while (dataReader.Read())
            //{
            //    var id = (int)dataReader["Id"];
            //    var name = (string)dataReader["Name"];
            //    var description = (string)dataReader["Description"];
            //    var url = (string)dataReader["Url"];
            //    var parentCategoryId = (int)(dataReader["ParentCategoryId"] as int?).GetValueOrDefault();

            //    Category category = new Category(name, description, url, id, parentCategoryId);

            //    parentCategoryList.Add(category);
            //}

            if (parentCategoryList == null) return null;

            parentCategoryList.ForEach(category => 
            {
                var parentCategoryList = FindChildCategoriesList(category.Id);
                category.CategoryInCategory = parentCategoryList;

                var products = FindProductsForCategories(category.Id);
                category.Products = products;
            });

            //connection.Close();

            return parentCategoryList;
        }

        private static List<Category> FindRootCategory()
        {
            using var context = new ProductManagerContext();

            List<Category> rootCategoryList = new List<Category>();

            var rootCategory = context.Categories.FirstOrDefault(x => (x.Id as int?).GetValueOrDefault() == null);

            rootCategoryList.Add(rootCategory);

            if (rootCategoryList == null)
            {
                return rootCategoryList;
            }
            else
            {
                return null;
            }

            //string sql = @"
            //            SELECT Id,
            //                   Name,
            //                   Description,
            //                   Url,
            //                   ParentCategoryId
            //            FROM Categorys
            //            WHERE ParentCategoryId IS NULL
            //";

            //using var connection = new SqlConnection(connectionString);
            //using var command = new SqlCommand(sql, connection);

            //connection.Open();

            //var dataReader = command.ExecuteReader();

            //List<Category> parentCategoryList = new List<Category>();

            //while (dataReader.Read())
            //{
            //    var id = (int)dataReader["Id"];
            //    var name = (string)dataReader["Name"];
            //    var description = (string)dataReader["Description"];
            //    var url = (string)dataReader["Url"];
            //    var parentCategoryId = (int)(dataReader["ParentCategoryId"] as int?).GetValueOrDefault();

            //    Category category = new Category(name, description, url, id, parentCategoryId);

            //    parentCategoryList.Add(category);
            //}

            //connection.Close();

            //return parentCategoryList;
        }

        private static void AddCategoryToCategory()
        {
            bool isRunning = true;

            do
            {
                Write("Parent category: ");

                var parentCategory = ReadLine();

                Category parentCategoryExist = FindIfCategoryExist(parentCategory);

                if (parentCategoryExist == null)
                {
                    Clear();

                    WriteLine("Could not find Parent Category");

                    Thread.Sleep(2000);

                    continue;
                }

                Write("Child category: ");

                var childCategory = ReadLine();

                Category childCategoryExist = FindIfCategoryExist(childCategory);

                if (childCategoryExist == null)
                {
                    Clear();

                    WriteLine("Could not find Child Category");

                    Thread.Sleep(2000);

                    continue;
                }

                CursorVisible = false;

                Write("\nIs this correct? (Y)es (N)o");

                ConsoleKeyInfo input;

                bool invalidChoice;

                do
                {
                    input = ReadKey(true);

                    invalidChoice = !(input.Key == ConsoleKey.Y || input.Key == ConsoleKey.N);

                }
                while (invalidChoice);

                Clear();

                if (input.Key == ConsoleKey.Y)
                {
                    SaveCategoryToCategory(parentCategoryExist, childCategoryExist);

                    WriteLine("Categories connected");

                    Thread.Sleep(2000);

                    isRunning = false;
                }
                else
                {
                    isRunning = false;
                }

                Clear();

            } while (isRunning);
        }

        private static Category FindIfCategoryExist(string categoryName)
        {
            using var context = new ProductManagerContext();

            var category = context.Categories.FirstOrDefault(x => x.Name == categoryName);

            if (category != null)
            {
                return category;
            }
            else
            {
                return null;
            }

            //string sql = @"
            //            SELECT *
            //            FROM Categorys
            //            WHERE Name = @CategoryName 
            //";

            //using var connection = new SqlConnection(connectionString);
            //using var command = new SqlCommand(sql, connection);

            //command.Parameters.AddWithValue("@CategoryName", categoryName);

            //connection.Open();

            //var dataReader = command.ExecuteReader();

            //if (dataReader.Read())
            //{
            //    Category category = new Category(
            //        id: (int) dataReader["Id"],
            //        name: (string) dataReader["Name"],
            //        description: (string) dataReader["Description"],
            //        url: (string) dataReader["Url"],
            //        parentCategoryId: (int) (dataReader["ParentCategoryId"] as int?).GetValueOrDefault());
                
            //    connection.Close();

            //    return category;
            //}
            //else
            //{
            //    connection.Close();

            //    return null;
            //}
        }

        private static void SaveCategoryToCategory(Category parentCategory, Category childCategory)
        {
            using var context = new ProductManagerContext();

            var categoryToCategory = context.Categories.SingleOrDefault(x => x.Id == childCategory.Id);

            if (categoryToCategory != null)
            { 
                categoryToCategory.ParentCategoryId = parentCategory.Id;

                context.SaveChanges();
            }

            //string sql = @"
            //             UPDATE Categorys 
            //             SET ParentCategoryId = @Id
            //             WHERE Id = @childCategory
            //                            ";

            //using SqlConnection connection = new(connectionString);
            //using SqlCommand command = new(sql, connection);

            //command.Parameters.AddWithValue("@Id", parentCategory.Id);
            //command.Parameters.AddWithValue("@childCategory", childCategory.Id);

            //connection.Open();

            //command.ExecuteNonQuery();

            //connection.Close();
        }
    }
}