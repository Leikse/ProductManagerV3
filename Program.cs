using System;
using System.Collections.Generic;
using System.Threading;
using static System.Console;

namespace ProductManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Product> productDictionary = new Dictionary<string, Product>();

            bool isRunning = true;

            do
            {
                CursorVisible = false;

                WriteLine("1. Add product");
                WriteLine("2. Search product");
                WriteLine("3. Exit");

                ConsoleKeyInfo userInput;

                bool invalidChoice;

                do
                {
                    userInput = ReadKey(true);

                    invalidChoice = !(userInput.Key == ConsoleKey.D1 || userInput.Key == ConsoleKey.NumPad1
                                   || userInput.Key == ConsoleKey.D2 || userInput.Key == ConsoleKey.NumPad2
                                   || userInput.Key == ConsoleKey.D3 || userInput.Key == ConsoleKey.NumPad3);

                } while (invalidChoice);

                CursorVisible = true;

                Clear();

                switch (userInput.Key)
                {
                    case ConsoleKey.D1:

                    case ConsoleKey.NumPad1:
                        {
                            AddProduct(productDictionary);
                        }
                        break;

                    case ConsoleKey.D2:

                    case ConsoleKey.NumPad2:
                        {
                            ListProduct(productDictionary);
                        }
                        break;

                    case ConsoleKey.D3:

                    case ConsoleKey.NumPad3:

                        isRunning = false;

                        break;
                }
            } 
            while (isRunning);
        }

        static void AddProduct(Dictionary<string, Product> productDictionary)
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

            bool productNotExists = !productDictionary.ContainsKey(product.articleNumber);

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

        static void ListProduct(Dictionary<string, Product> productDictionary)
        {
            WriteLine("Article number: ");

            var ArticleNumber = ReadLine();

            bool productExists = productDictionary.ContainsKey(ArticleNumber);

            bool invalidChoice;

            if (productExists)
            {
                foreach (var product in productDictionary.Values)
                {
                    WriteLine($"Article number: {product.articleNumber}");
                    WriteLine($"Name: {product.name}");
                    WriteLine($"Description: {product.description}");
                    WriteLine($"Url: {product.url}");
                    WriteLine($"Price: {product.price}");

                    CursorVisible = false;

                    ConsoleKeyInfo input;

                    do
                    {
                        input = ReadKey(true);

                        invalidChoice = !(input.Key == ConsoleKey.Escape);

                    }
                    while (invalidChoice);
                }
            }
            else
            {
                Clear();
                CursorVisible = false;

                WriteLine("Product not found");
                Thread.Sleep(2000);
            }

            Clear();
        }
    }
}
