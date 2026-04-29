using System;
using System.IO;
using System.Threading;

class Program
{
    private static bool _isProcessing = false;  // Флаг, чтобы не обрабатывать одно изменение несколько раз

    static void Main()
    {
        string folder = @"D:\study_3\ПРАКТИКА КПиЯП\задание\ЗАДАНИЯ\9";
        Directory.CreateDirectory(folder);

        string backupFolder = Path.Combine(folder, "backup");
        Directory.CreateDirectory(backupFolder);

        Console.WriteLine("=== АВТОМАТИЧЕСКОЕ СОЗДАНИЕ БЭКАПОВ ===\n");
        Console.WriteLine($"Папка для отслеживания: {folder}");
        Console.WriteLine($"Папка для бэкапов: {backupFolder}\n");

        FileSystemWatcher watcher = new FileSystemWatcher(folder);
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Changed += OnFileChanged;
        watcher.EnableRaisingEvents = true;

        string testFile = Path.Combine(folder, "test.txt");
        File.WriteAllText(testFile, "Первая версия текста");
        Console.WriteLine("1. Создан файл: test.txt");

        Thread.Sleep(3000);
        File.WriteAllText(testFile, "Вторая версия текста - файл изменён!");
        Console.WriteLine("2. Файл test.txt изменён (автоматически)");

        Thread.Sleep(1000);

        Console.WriteLine("\n=== ПРОВЕРКА РЕЗУЛЬТАТА ===");

        string[] backups = Directory.GetFiles(backupFolder);
        if (backups.Length > 0)
        {
            Console.WriteLine($"\nСозданы бэкапы ({backups.Length} шт.):");
            foreach (string backup in backups)
            {
                FileInfo fi = new FileInfo(backup);
                Console.WriteLine($"  - {fi.Name} (размер: {fi.Length} байт)");
            }
        }
        else
        {
            Console.WriteLine("\n❌ Бэкапы НЕ созданы");
        }

        Console.WriteLine("\nНажмите Enter для выхода...");
        Console.ReadLine();
    }

    static void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        // Защита от многократного срабатывания
        if (_isProcessing) return;
        _isProcessing = true;

        try
        {
            Thread.Sleep(100);

            string backupFolder = Path.Combine(Path.GetDirectoryName(e.FullPath), "backup");
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupName = $"{Path.GetFileNameWithoutExtension(e.Name)}_{timestamp}{Path.GetExtension(e.Name)}.bak";
            string backupPath = Path.Combine(backupFolder, backupName);

            File.Copy(e.FullPath, backupPath, true);
            Console.WriteLine($"   ✅ Создан бэкап: {backupName}");
        }
        catch (Exception ex)
        {
            // Не выводим ошибку, если проблема с доступом (бэкап уже создан)
            if (!ex.Message.Contains("Access to the path"))
            {
                Console.WriteLine($"   ❌ Ошибка: {ex.Message}");
            }
        }
        finally
        {
            _isProcessing = false;
        }
    }
}