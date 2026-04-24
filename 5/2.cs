using System;

class Program
{
    static void Main()
    {
        // Создаём библиотеку
        Library library = new Library("Городская библиотека №1", "ул. Пушкина, 10");

        // Создаём авторов
        Author[] authors1 = { new Author("Лев", "Толстой") };
        Author[] authors2 = { new Author("Фёдор", "Достоевский") };
        Author[] authors3 = { new Author("Александр", "Пушкин") };

        // Главы для книг
        string[] chapters1 = { "Вступление", "Глава 1", "Заключение" };
        string[] chapters2 = { "Пролог", "Часть 1", "Часть 2", "Эпилог" };
        string[] chapters3 = { "Введение", "Основная часть", "Финал" };

        // Создаём книги
        Book book1 = new Book("Война и мир", 1869, authors1, chapters1);
        Book book2 = new Book("Преступление и наказание", 1866, authors2, chapters2);
        Book book3 = new Book("Евгений Онегин", 1833, authors3, chapters3);

        // АССОЦИАЦИЯ: привязываем книги к библиотеке
        book1.SetLibrary(library);
        book2.SetLibrary(library);
        book3.SetLibrary(library);

        // Создаём массив книг
        Book[] books = { book1, book2, book3 };

        // Выводим информацию о всех книгах
        Console.WriteLine("=== ИНФОРМАЦИЯ О КНИГАХ ===\n");

        for (int i = 0; i < books.Length; i++)
        {
            books[i].DisplayInfo();
            Console.WriteLine();
        }

        // Выводим информацию о библиотеке
        library.DisplayInfo();
    }
}

// Класс Library (ассоциация с Book)
class Library
{
    private string name;
    private string address;

    public Library(string name, string address)
    {
        this.name = name;
        this.address = address;
    }

    public string GetName()
    {
        return name;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"\n=== БИБЛИОТЕКА ===\nНазвание: {name}\nАдрес: {address}");
    }
}

// Класс Author (агрегация)
class Author
{
    private string name;
    private string surname;

    public Author(string name, string surname)
    {
        this.name = name;
        this.surname = surname;
    }

    public string GetFullName()
    {
        return $"{name} {surname}";
    }
}

// Класс TableOfContents (композиция)
class TableOfContents
{
    private string[] chapters;

    public TableOfContents(string[] chapters)
    {
        this.chapters = chapters;
    }

    public void Display()
    {
        Console.WriteLine("Оглавление:");
        for (int i = 0; i < chapters.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {chapters[i]}");
        }
    }
}

// Класс Book
class Book
{
    private string title;
    private int year;
    private Author[] authors;           // АГРЕГАЦИЯ
    private TableOfContents contents;   // КОМПОЗИЦИЯ
    private Library library;            // АССОЦИАЦИЯ

    public Book(string title, int year, Author[] authors, string[] chapters)
    {
        this.title = title;
        this.year = year;
        this.authors = authors;
        this.contents = new TableOfContents(chapters); // Композиция: создаём внутри
        this.library = null; // Пока не привязана к библиотеке
    }

    // Метод для установки связи с библиотекой (ассоциация)
    public void SetLibrary(Library lib)
    {
        this.library = lib;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Название: {title}");
        Console.WriteLine($"Год: {year}");

        // Вывод авторов (агрегация)
        Console.Write("Авторы: ");
        for (int i = 0; i < authors.Length; i++)
        {
            Console.Write(authors[i].GetFullName());
            if (i < authors.Length - 1) Console.Write(", ");
        }
        Console.WriteLine();

        // Вывод оглавления (композиция)
        contents.Display();

        // Вывод библиотеки (ассоциация)
        if (library != null)
        {
            Console.WriteLine($"Находится в библиотеке: {library.GetName()}");
        }
        else
        {
            Console.WriteLine("Не привязана к библиотеке");
        }
    }
}