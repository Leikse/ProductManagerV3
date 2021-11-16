using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;
using Microsoft.Data.SqlClient;
using ProductManager.Models;

namespace ProductManager
{
    class Program
    {
        static string connectionString = "Server=.;Database=ProductManager;Integrated Security=True";

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


                ConsoleKeyInfo userInput;

                bool invalidChoice;

                do
                {
                    userInput = ReadKey(true);

                    invalidChoice = !(userInput.Key == ConsoleKey.D1 || userInput.Key == ConsoleKey.NumPad1 
                                                                     || userInput.Key == ConsoleKey.D2 || userInput.Key == ConsoleKey.NumPad2
                                                                     || userInput.Key == ConsoleKey.D3 || userInput.Key == ConsoleKey.NumPad3
                                                                     || userInput.Key == ConsoleKey.D4 || userInput.Key == ConsoleKey.NumPad4
                                                                     || userInput.Key == ConsoleKey.D5 || userInput.Key == ConsoleKey.NumPad5
                                                                     || userInput.Key == ConsoleKey.D6 || userInput.Key == ConsoleKey.NumPad6
                                                                     || userInput.Key == ConsoleKey.D7 || userInput.Key == ConsoleKey.NumPad7
                                                                     || userInput.Key == ConsoleKey.D8 || userInput.Key == ConsoleKey.NumPad8);

                } while (invalidChoice);

                CursorVisible = true;

                Clear();

                switch (userInput.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                    {
                        AddProduct();
                    }
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                    {
                        ListProduct();
                    }
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                    {
                        AddCategory();
                    }
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                    {
                        AddProductToCategory();
                    }
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                    {
                        ListCategories();
                    }
                        break;

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                    {
                        AddCategoryToCategory();
                    }
                        break;

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                    {
                        isAuthenticated = false;
                    }
                        break;

                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                    {
                        isRunning = false;
                    }
                        break;
                }
            } while (isRunning);
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
            string sql = @"
                SELECT Username,
                       Password
                FROM Logins
               WHERE Username = @Username
            ";

            using var connection = new SqlConnection(connectionString);

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Username", username);

            connection.Open();

            var dataReader = command.ExecuteReader();

            Login login = null;

            if (dataReader.Read())
            {
                login = new Login(
                    username: (string) dataReader["Username"],
                    password: (string) dataReader["Password"]);
            }

            connection.Close();

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
            string sql = @"
                        SELECT Id,
                               ArticleNumber,
                               Name,
                               Description,
                               Url,
                               Price
                        FROM Products
                        WHERE ArticleNumber = @ArticleNumber
            ";

            using var connection = new SqlConnection(connectionString);

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ArticleNumber", articleNumber);

            connection.Open();

            var dataReader = command.ExecuteReader();

            Product product = null;

            if (dataReader.Read())
            {
                product = new Product(
                    id: (int) dataReader["Id"],
                    articleNumber: (string) dataReader["ArticleNumber"],
                    name: (string) dataReader["Name"],
                    description: (string) dataReader["Description"],
                    url: (string) dataReader["Url"],
                    price: (int) dataReader["Price"]);
            }

            connection.Close();

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
            string sql = @"
                         INSERT INTO Products (
                                     ArticleNumber, 
                                     Name, 
                                     Description, 
                                     Url, 
                                     Price
                                ) VALUES (
                                     @ArticleNumber, 
                                     @Name, 
                                     @Description, 
                                     @Url, 
                                     @Price
                                )";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);

            command.Parameters.AddWithValue("@ArticleNumber", product.ArticleNumber);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Url", product.Url);
            command.Parameters.AddWithValue("@Price", product.Price);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        static void ListProduct()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Clear();
            
            var products = FindProductList();

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

                                            DeleteProduct(articleNumber);

                                            DeleteCategory(articleNumber);

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
            string sql = @"
                         DELETE FROM Products WHERE ArticleNumber = @ArticleNumber
                                ";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);

            command.Parameters.AddWithValue("@ArticleNumber", articleNumber);
            
            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        private static void DeleteCategory(string articleNumber)
        {
            string sql = @"
                         DELETE FROM CategoryProduct WHERE ProductArticleNumber = @ProductArticleNumber
                                ";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);

            command.Parameters.AddWithValue("@ProductArticleNumber", articleNumber);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        private static IList<Product> FindProductList()
        {
            string sql = @"
                SELECT Id,
                       ArticleNumber,
                       Name,
                       Description,
                       Url,
                       Price
                  FROM Products
            ";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            var dataReader = command.ExecuteReader();

            List<Product> productList = new List<Product>();

            while (dataReader.Read())
            {
                var id = (int) dataReader["Id"];
                var articleNumber = (string) dataReader["ArticleNumber"];
                var name = (string) dataReader["Name"];
                var description = (string) dataReader["Description"];
                var url = (string) dataReader["Url"];
                var price = (int) dataReader["Price"];

                Product product = new Product(id, articleNumber, name, description, url, price);

                productList.Add(product);
            }

            connection.Close();

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
                    //categoryList.Add(category);
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
            string sql = @"INSERT INTO Categorys (
                         Name,
                         Description,
                         Url
                         ) VALUES (
                         @Name,
                         @Description,
                         @Url)";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);

            command.Parameters.AddWithValue("@Name", category.Name);
            command.Parameters.AddWithValue("@Description", category.Description);
            command.Parameters.AddWithValue("@Url", category.Url);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
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
            string sql = @"INSERT INTO CategoryProduct (
                         ProductId,
                         CategoryId
                         ) VALUES (
                         @ProductId,
                         @CategoryId)";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);

            command.Parameters.AddWithValue("@ProductId", product.Id);
            command.Parameters.AddWithValue("@CategoryId", category.Id);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        static Category FindCategory(string name)
        {
            string sql = @"
                        SELECT Id,
                               Name,
                               Description,
                               Url
                        FROM Categorys
                        WHERE Name = @Name
            ";

            using var connection = new SqlConnection(connectionString);

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", name);

            connection.Open();

            var dataReader = command.ExecuteReader();

            Category category = null;

            if (dataReader.Read())
            {
                category = new Category(
                    id: (int) dataReader["Id"],
                    name: (string) dataReader["Name"],
                    description: (string) dataReader["Description"],
                    url: (string) dataReader["Url"],
                    parentCategoryId: (int) dataReader["ParentCategoryId"]);
            }

            connection.Close();

            return category;
        }

        static void ListCategories()
        {
            CursorVisible = false;

            WriteLine("Name\t\tPrice");
            WriteLine("------------------------------------------------------------------------------");

            var parentCategorys = FindParentCategory();

            var parentCategoryDoesntExist = parentCategorys == null;

            if (!parentCategoryDoesntExist)
            {
                foreach (var parentCategory in parentCategorys)
                {
                    WriteLine($"{parentCategory.Name}");

                    var childCategorys = FindChildCategory();

                    var childCategoryDoesntExist = childCategorys == null;

                    if (!childCategoryDoesntExist)
                    {
                        foreach (var childCategory in childCategorys)
                        {
                            WriteLine($"  {childCategory.Name}");
                        }
                        
                    }
                }
            }
            else
            {
                WriteLine("Parent category doesn't exist");
            }

            while (ReadKey(true).Key != ConsoleKey.Escape) ;

            Clear();
        }

        private static IList<Category> FindChildCategory()
        {
            string sql = @"
                        SELECT Id,
                               Name,
                               Description,
                               Url,
                               ParentCategoryId
                        FROM Categorys
                        WHERE ParentCategoryId IS NOT NULL
            ";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();

            var dataReader = command.ExecuteReader();

            List<Category> parentCategoryList = new List<Category>();

            while (dataReader.Read())
            {

                var id = (int)dataReader["Id"];
                var name = (string)dataReader["Name"];
                var description = (string)dataReader["Description"];
                var url = (string)dataReader["Url"];
                var parentCategoryId = (int)(dataReader["ParentCategoryId"] as int?).GetValueOrDefault();

                Category category = new Category(name, description, url, id, parentCategoryId);

                parentCategoryList.Add(category);
            }

            connection.Close();

            return parentCategoryList;
        }

        private static IList<Category> FindParentCategory()
        {
            string sql = @"
                        SELECT Id,
                               Name,
                               Description,
                               Url,
                               ParentCategoryId
                        FROM Categorys
                        WHERE ParentCategoryId IS NULL
            ";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();

            var dataReader = command.ExecuteReader();

            List<Category> parentCategoryList = new List<Category>();

            while (dataReader.Read())
            {

                var id = (int) dataReader["Id"];
                var name = (string) dataReader["Name"];
                var description = (string)dataReader["Description"];
                var url = (string)dataReader["Url"];
                var parentCategoryId = (int)(dataReader["ParentCategoryId"] as int?).GetValueOrDefault();

                Category category = new Category(name, description, url, id, parentCategoryId);

                parentCategoryList.Add(category);
            }

            connection.Close();

            return parentCategoryList;
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
            string sql = @"
                        SELECT *
                        FROM Categorys
                        WHERE Name = @CategoryName 
            ";

            using var connection = new SqlConnection(connectionString);

            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@CategoryName", categoryName);

            connection.Open();

            var dataReader = command.ExecuteReader();

            if (dataReader.Read())
            {
                Category category = new Category(
                    id: (int) dataReader["Id"],
                    name: (string) dataReader["Name"],
                    description: (string) dataReader["Description"],
                    url: (string) dataReader["Url"],
                    parentCategoryId: (int) (dataReader["ParentCategoryId"] as int?).GetValueOrDefault());
                
                connection.Close();

                return category;
            }
            else
            {
                connection.Close();

                return null;
            }
        }

        private static void SaveCategoryToCategory(Category parentCategory, Category childCategory)
        {
            string sql = @"
                         UPDATE Categorys 
                         SET ParentCategoryId = @Id
                         WHERE Id = @childCategory
                                        ";

            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(sql, connection);

            command.Parameters.AddWithValue("@Id", parentCategory.Id);
            command.Parameters.AddWithValue("@childCategory", childCategory.Id);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
