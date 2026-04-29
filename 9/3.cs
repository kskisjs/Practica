using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// Модель товара (такая же, как в задании 2)
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}

// Класс для чтения товаров из двоичного файла
public class ProductFileReader
{
    private readonly string _filePath = "file.data";

    public List<Product> ReadProducts()
    {
        var products = new List<Product>();

        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"Файл {_filePath} не найден");
        }

        using (BinaryReader reader = new BinaryReader(File.OpenRead(_filePath), Encoding.UTF8))
        {
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var product = new Product
                {
                    Name = reader.ReadString(),
                    Price = reader.ReadDecimal(),
                    Category = reader.ReadString()
                };
                products.Add(product);
            }
        }

        return products;
    }
}

public class ProductProcessor
{
    public List<Product> SortByPrice(List<Product> products, bool ascending)
    {
        if (ascending)
            return products.OrderBy(p => p.Price).ToList();
        else
            return products.OrderByDescending(p => p.Price).ToList();
    }

    public void PrintProducts(List<Product> products)
    {
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Name} | {p.Price:F2} | {p.Category}");
        }
    }
}

// Демонстрация работы
class Program
{
    static void Main()
    {
        // === СНАЧАЛА ЗАПИШЕМ ТОВАРЫ (если файла нет) ===
        if (!File.Exists("file.data"))
        {
            var productsToWrite = new List<Product>
            {
                new Product { Name = "Ноутбук", Price = 1200.99m, Category = "Электроника" },
                new Product { Name = "Мышь", Price = 25.50m, Category = "Электроника" },
                new Product { Name = "Книга", Price = 15.00m, Category = "Литература" },
                new Product { Name = "Монитор", Price = 350.00m, Category = "Электроника" },
                new Product { Name = "Ручка", Price = 2.50m, Category = "Канцелярия" }
            };

            using (BinaryWriter writer = new BinaryWriter(File.Open("file.data", FileMode.Create), Encoding.UTF8))
            {
                foreach (var p in productsToWrite)
                {
                    writer.Write(p.Name);
                    writer.Write(p.Price);
                    writer.Write(p.Category);
                }
            }
            Console.WriteLine("Товары записаны в file.data\n");
        }

        // === ОСНОВНАЯ ЧАСТЬ ===
        var reader = new ProductFileReader();
        var processor = new ProductProcessor();

        // Читаем товары из файла
        List<Product> products = reader.ReadProducts();

        Console.WriteLine("Исходный список товаров:");
        processor.PrintProducts(products);

        // Сортировка по возрастанию цены
        var sortedAsc = processor.SortByPrice(products, true);
        Console.WriteLine("\nСортировка по цене (возрастание):");
        processor.PrintProducts(sortedAsc);

        // Сортировка по убыванию цены
        var sortedDesc = processor.SortByPrice(products, false);
        Console.WriteLine("\nСортировка по цене (убывание):");
        processor.PrintProducts(sortedDesc);
    }
}