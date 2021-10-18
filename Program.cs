using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.Console;

namespace ProductManager
{
    class Program
    {
        static Dictionary<string, Product> productDictionary = new Dictionary<string, Product>();
        static List<Category> categoryList = new List<Category>();

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
                WriteLine("6. Logout");
                WriteLine("7. Exit");


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
                                                                     || userInput.Key == ConsoleKey.D7 || userInput.Key == ConsoleKey.NumPad7);

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
                        isAuthenticated = false;
                    }
                        break;

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
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

                var userName = ReadLine();

                WriteLine("\nPassword: ");

                var password = ReadLine();

                if (userName == "admin" && password == "123")
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

            var productNotExists = !productDictionary.ContainsKey(product.articleNumber);

            Clear();

            if (input.Key == ConsoleKey.Y && !productNotExists)
            {
                WriteLine("Product already exists!");

                Thread.Sleep(2000);
            }
            else if (input.Key == ConsoleKey.Y && productNotExists)
            {
                productDictionary.Add(product.articleNumber, product);

                WriteLine("Product saved");

                Thread.Sleep(2000);
            }
            CursorVisible = true;

            Clear();
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

            Product product = new Product
            {
                articleNumber = articleNumber,
                name = name,
                description = description,
                url = url,
                price = price
            };
            return product;
        }

        static void ListProduct()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Clear();

            var productExists = productDictionary.ContainsKey(articleNumber);

            var isRunning = true;

            do
            {
                if (productExists)
                {
                    bool isRunningAgain = true;

                    do
                    {
                        var product = productDictionary[articleNumber];

                        WriteLine($"Article number: {product.articleNumber}");
                        WriteLine($"Name: {product.name}");
                        WriteLine($"Description: {product.description}");
                        WriteLine($"Url: {product.url}");
                        WriteLine($"Price: {product.price}");
                        WriteLine("\n(D)elete");

                        CursorVisible = false;

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

                                    WriteLine($"Article number: {product.articleNumber}");
                                    WriteLine($"Name: {product.name}");
                                    WriteLine($"Description: {product.description}");
                                    WriteLine($"Url: {product.url}");
                                    WriteLine($"Price: {product.price}");
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

                                            categoryList.ForEach(category =>
                                            {
                                                category.ProductList.Remove(articleNumber);
                                            });

                                            productDictionary.Remove(articleNumber);

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
                    categoryList.Add(category);

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

            Category category = new Category
            (
                name,
                description,
                url
            );
            return category;
        }

        static void AddProductToCategory()
        {
            Write("Article number: ");

            var ArticleNumber = ReadLine();

            Clear();

            var productExists = productDictionary.ContainsKey(ArticleNumber);

            if (productExists)
            {
                WriteLine("Category name: ");

                var name = ReadLine();

                var categoryExists = categoryList.Any(category => category.Name == name);

                Clear();

                CursorVisible = false;

                if (categoryExists)
                {
                    var productToAdd = productDictionary.SingleOrDefault(x => x.Value.articleNumber == ArticleNumber);

                    var categoryToAddTo = categoryList.SingleOrDefault(x => x.Name == name);

                    categoryToAddTo.AddProduct(productToAdd.Value);

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

        static void ListCategories()
        {
            CursorVisible = false;

            WriteLine("Name\t\tPrice");
            WriteLine("------------------------------------------------------------------------------");

            categoryList.ForEach(category =>
            {
                var numberOfProducts = category.ProductList != null ? category.ProductList.Count().ToString() : "0";

                WriteLine($"{category.Name} ({numberOfProducts})");

                foreach (var product in category.ProductList)
                {
                    WriteLine($"  {product.Value.name}\t\t{product.Value.price}");
                }
            });

            while (ReadKey(true).Key != ConsoleKey.Escape) ;

            Clear();
        }
    }
}
