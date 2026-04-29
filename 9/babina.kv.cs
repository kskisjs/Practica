using System;
using System.IO;
using System.Linq;

public class FileManager
{
    public void CreateFile(string path, string content) => File.WriteAllText(path, content);
    public void DeleteFile(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("Файл не найден");
        File.Delete(path);
    }
    public void CopyFile(string src, string dst) => File.Copy(src, dst, true);
    public void MoveFile(string src, string dst) => File.Move(src, dst);
    public void RenameFile(string oldPath, string newPath) => File.Move(oldPath, newPath);
}

public class FileInfoProvider
{
    public void GetInfo(string path)
    {
        var fi = new FileInfo(path);
        Console.WriteLine($"Размер: {fi.Length} байт | Создан: {fi.CreationTime} | Изменён: {fi.LastWriteTime}");
    }
}

class Program
{
    static void Main()
    {
        string fileName = "babina.kv";
        var fm = new FileManager();
        var fip = new FileInfoProvider();

        // 1
        fm.CreateFile(fileName, "Hello");
        Console.WriteLine(File.ReadAllText(fileName));

        // 2
        if (File.Exists(fileName)) fm.DeleteFile(fileName);

        // 3
        fm.CreateFile(fileName, "Data");
        fip.GetInfo(fileName);

        // 4
        fm.CopyFile(fileName, "copy_" + fileName);
        Console.WriteLine(File.Exists("copy_" + fileName));

        // 5
        Directory.CreateDirectory("NewDir");
        fm.MoveFile(fileName, "NewDir/" + fileName);

        // 6
        fm.RenameFile("NewDir/" + fileName, "NewDir/babina.kv");

        // 7
        try { fm.DeleteFile("notexist.txt"); }
        catch (Exception ex) { Console.WriteLine(ex.Message); }

        // 8
        var f1 = new FileInfo("NewDir/babina.kv");
        var f2 = new FileInfo("copy_babina.kv");
        Console.WriteLine(f1.Length == f2.Length ? "Одинаковы" : "Разные");

        // 9
        foreach (var f in Directory.GetFiles(".", "*.kv")) File.Delete(f);

        // 10
        foreach (var f in Directory.GetFiles(".")) Console.WriteLine(f);

        // 11
        var ro = "readonly.txt";
        fm.CreateFile(ro, "test");
        new FileInfo(ro).IsReadOnly = true;
        try { File.WriteAllText(ro, "new"); }
        catch { Console.WriteLine("Ошибка записи"); }

        // 12
        Console.WriteLine($"ReadOnly: {new FileInfo(ro).IsReadOnly}");
    }
}