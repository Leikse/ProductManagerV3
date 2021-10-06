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
            var isRunning = true;

            do
            {
                CursorVisible = false;

                WriteLine("1. Add product");
                WriteLine("2. Search product");
                WriteLine("3. Add category");
                WriteLine("4. Add product to category");
                WriteLine("5. List categories");
                WriteLine("6. Exit");


                // Gör till en egen metod fram till rad 45
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
                                   || userInput.Key == ConsoleKey.D6 || userInput.Key == ConsoleKey.NumPad6);

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
                            AddCategoryToProduct();
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
                            isRunning = false;
                        }
                        break;
                }
            } 
            while (isRunning);
        }

        static void AddProduct()
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

        static Product CreateProduct()
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
            {
                name = name,
                description = description,
                url = url,
            };
            return category;
        }

        static void ListProduct()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Clear();

            var productExists = productDictionary.ContainsKey(articleNumber);

            if (productExists)
            {
                var product = productDictionary[articleNumber];

                WriteLine($"Article number: {product.articleNumber}");
                WriteLine($"Name: {product.name}");
                WriteLine($"Description: {product.description}");
                WriteLine($"Url: {product.url}");
                WriteLine($"Price: {product.price}");

                CursorVisible = false;

                while (ReadKey(true).Key != ConsoleKey.Escape);
            }
            else
            {
                CursorVisible = false;

                WriteLine("Product not found");

                Thread.Sleep(2000);
            }
            Clear();
        }

        static void AddCategoryToProduct()
        {
            Write("Article number: ");

            var articleNumber = ReadLine();

            Clear();

            var productExists = productDictionary.ContainsKey(articleNumber);

            if (productExists)
            {
                WriteLine("Category name: ");

                var name = ReadLine();

                var categoryExists = categoryList.Any(category => category.name == name);

                Clear();

                CursorVisible = false;

                if (categoryExists)
                {
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

            WriteLine("Name\t\t\t\tPrice");
            WriteLine("------------------------------------------------------------------------------");

            categoryList.ForEach(product =>
            {
                WriteLine($"{product.name} ({product.})");
            });

            //foreach (var category in categoryList)
            //{
            //    WriteLine($"{category.name}  ({})");

                
            //}
            


            while (ReadKey(true).Key != ConsoleKey.Escape);

            Clear();
        }
    }
}
