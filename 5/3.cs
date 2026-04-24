using System;

class Program
{
    static void Main()
    {
        // Создаём массив транспортных средств
        Vehicle[] vehicles = new Vehicle[5];

        vehicles[0] = new ElectricCar("Tesla Model 3", 2023, 500);
        vehicles[1] = new Truck("Volvo FH", 2022, 12.5);
        vehicles[2] = new ElectricCar("Nissan Leaf", 2021, 380);
        vehicles[3] = new Truck("Scania R500", 2020, 14.0);
        vehicles[4] = new ElectricCar("BMW i4", 2024, 520);

        // Выводим информацию о всех транспортных средствах
        Console.WriteLine("=== ВСЕ ТРАНСПОРТНЫЕ СРЕДСТВА ===\n");

        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].DisplayInfo();
            Console.WriteLine();
        }

        // Находим все дизельные машины
        Console.WriteLine("=== ДИЗЕЛЬНЫЕ МАШИНЫ ===\n");

        for (int i = 0; i < vehicles.Length; i++)
        {
            // Проверяем, реализует ли объект интерфейс IDiesel
            if (vehicles[i] is IDiesel)
            {
                vehicles[i].DisplayInfo();
                Console.WriteLine();
            }
        }

        // Находим все электромобили
        Console.WriteLine("=== ЭЛЕКТРОМОБИЛИ ===\n");

        for (int i = 0; i < vehicles.Length; i++)
        {
            // Проверяем, реализует ли объект интерфейс IElectric
            if (vehicles[i] is IElectric)
            {
                vehicles[i].DisplayInfo();
                Console.WriteLine();
            }
        }
    }
}

// Базовый класс Vehicle
abstract class Vehicle
{
    protected string model;
    protected int year;

    public Vehicle(string model, int year)
    {
        this.model = model;
        this.year = year;
    }

    public abstract void DisplayInfo();

    public string GetModel() { return model; }
    public int GetYear() { return year; }
}

// Интерфейс для электромобилей
interface IElectric
{
    int GetRange();      // запас хода в км
    void ChargeBattery(); // зарядить батарею
}

// Интерфейс для дизельных машин
interface IDiesel
{
    double GetFuelConsumption(); // расход топлива л/100км
    void Refuel();               // заправиться
}

// Класс ElectricCar (электромобиль) - реализует IElectric
class ElectricCar : Vehicle, IElectric
{
    private int range;

    public ElectricCar(string model, int year, int range) : base(model, year)
    {
        this.range = range;
    }

    public int GetRange()
    {
        return range;
    }

    public void ChargeBattery()
    {
        Console.WriteLine($"{model}: Батарея заряжается...");
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Электромобиль: {model}, {year} год, запас хода: {range} км");
    }
}

// Класс Truck (грузовик) - реализует IDiesel
class Truck : Vehicle, IDiesel
{
    private double fuelConsumption;

    public Truck(string model, int year, double fuelConsumption) : base(model, year)
    {
        this.fuelConsumption = fuelConsumption;
    }

    public double GetFuelConsumption()
    {
        return fuelConsumption;
    }

    public void Refuel()
    {
        Console.WriteLine($"{model}: Заправка дизельным топливом...");
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Грузовик: {model}, {year} год, расход: {fuelConsumption} л/100км");
    }
}