using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kassasystemet2
{ 
    public class Program
    {
        static List<Product> products = new List<Product>();
        static Receipt currentReceipt = new Receipt();
        static int receiptId;

        static void Main(string[] args)
        {
            string fullPath = Path.Combine(Environment.CurrentDirectory, "../../../productdata.txt");
            LoadProductData(fullPath);

             currentReceipt = new Receipt();
            while (true)
            {
                Console.WriteLine("CASH");
                Console.WriteLine("1. New customer");
                Console.WriteLine("0. Finish");
                Console.Write("Choose an alternative: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartNewSale();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid selection.Try again please.");
                        break;
                }
            }
        }


       
        static void LoadProductData(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] data = reader.ReadLine().Split(' ');
                        if (data.Length == 5)
                        {
                            int productId = int.Parse(data[0]);
                            string productName = data[1];
                            double price = double.Parse(data[2]);
                            string priceType = data[3];

                            products.Add(new Product
                            {
                                ProductId = productId,
                                ProductName = productName,
                                Price = price,
                                PriceType = priceType
                            });
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("The product database file was not found");
            }
        }

        static int receiptCounter = 0;
        static void StartNewSale()
        {
            int receiptId = receiptCounter++;
            currentReceipt = new Receipt();
            currentReceipt.CreationTime = DateTime.Now;
            Console.WriteLine("RECEIPT " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("Receipt identification: " + receiptCounter);

            AddProductsToReceipt(receiptId);
            PrintReceipt();
            SaveReceiptToFile();
            
        }


        static void AddProductsToReceipt(int receiptId)
        {
            while (true)
            {
                Console.Write("Command: ");
                string input = Console.ReadLine();
                
                if (input.Equals("PAY", StringComparison.OrdinalIgnoreCase))
                {
                    SaveReceiptToFile();
                    break;
                }

                string[] command = input.Split(' ');
                if (command.Length == 2 && int.TryParse(command[0], out int productId) && int.TryParse(command[1], out int quantity))
                {
                    
                    Product product = products.Find(p => p.ProductId == productId);
                    if (product != null)
                    {
                        currentReceipt.PurchasedProducts.Add(product);
                        currentReceipt.PurchasedQuantities.Add(quantity);
                        currentReceipt.TotalPrice += product.Price * quantity;
                     
                    }
                    else
                    {
                        Console.WriteLine("The product was not found");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command. Try again please.");
                }
            }
        }

        static void SaveReceiptToFile()
        {
            string fileName = "RECEIPT_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_ID" + receiptId + ".txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("RECEIPT " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine("Receipt identification: " + receiptId);
                foreach (Product product in currentReceipt.PurchasedProducts)
                {
                    
                    writer.WriteLine($"{product.ProductName} {currentReceipt.PurchasedProducts.Count(p => p.ProductName == product.ProductName)} * {product.Price:F2} = {product.Price * currentReceipt.PurchasedProducts.Count(p => p.ProductName == product.ProductName):F2}");
                    
                }
                writer.WriteLine("Total: " + currentReceipt.TotalPrice.ToString("F2"));
                
            }
            Console.WriteLine("Receipt saved on file " + fileName);
        }

        static void PrintReceipt()
        {
            Console.WriteLine("CASH");
            Console.WriteLine("RECEIPT " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            
            for (int i = 0; i < currentReceipt.PurchasedProducts.Count; i++)
            {
                Product product = currentReceipt.PurchasedProducts[i];  
                int quantity = currentReceipt.PurchasedQuantities[i];
                double totalForProduct = product.Price * quantity;

                Console.WriteLine($"{product.ProductName} {quantity} * {product.Price:F2} = {totalForProduct:F2}" + " kr");
                
            }
            Console.WriteLine("Total: " + currentReceipt.TotalPrice.ToString("F2") + " kr");
            Console.WriteLine("_________________________");

        }

    }
}