using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Модель товара
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}

// Класс для записи в двоичный файл
public class ProductFileWriter
{
    private readonly string _filePath = "file.data";

    public void WriteProducts(List<Product> products)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(_filePath, FileMode.Create), Encoding.UTF8))
        {
            foreach (var product in products)
            {
                writer.Write(product.Name);
                writer.Write(product.Price);
                writer.Write(product.Category);
            }
        }
    }
}

// Демонстрация работы
class Program
{
    static void Main()
    {
        // Создаём список товаров
        var products = new List<Product>
        {
            new Product { Name = "Ноутбук", Price = 1200.99m, Category = "Электроника" },
            new Product { Name = "Футболка", Price = 25.50m, Category = "Одежда" },
            new Product { Name = "Книга", Price = 15.00m, Category = "Литература" }
        };

        // Записываем в файл
        var writer = new ProductFileWriter();
        writer.WriteProducts(products);

        Console.WriteLine("Товары успешно записаны в file.data в двоичном формате");

        // Для проверки — читаем и выводим (без десериализации, просто демонстрация)
        using (BinaryReader reader = new BinaryReader(File.OpenRead("file.data")))
        {
            Console.WriteLine("\nСодержимое файла:");
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                string name = reader.ReadString();
                decimal price = reader.ReadDecimal();
                string category = reader.ReadString();
                Console.WriteLine($"{name} | {price} | {category}");
            }
        }
    }
}