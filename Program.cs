using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public enum SmartphoneBrand
{
    Apple,
    Samsung,
    Xiaomi,
    Google,
    OnePlus
}

public class Smartphone
{
    private string _modelName = string.Empty;
    private SmartphoneBrand _brand;
    private double _cpuFrequency;
    private int _ram;
    private int _storage;
    private DateTime _releaseDate;
    private int _batteryCapacity;
    private decimal _launchPrice;
    private bool _inBasket = false; 

    public string ModelName
    {
        get => _modelName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Назва не може бути порожньою.");
            if (value.Length < 5 || value.Length > 40)
                throw new ArgumentException("Назва має містити 5–40 символів.");
            if (!Regex.IsMatch(value, @"^[a-zA-Z0-9\s]+$"))
                throw new ArgumentException("Лише латинські літери, цифри та пробіли.");
            _modelName = value;
        }
    }

    public SmartphoneBrand Brand
    {
        get => _brand;
        set
        {
            if (!Enum.IsDefined(typeof(SmartphoneBrand), value))
                throw new ArgumentException("Некоректний бренд.");
            _brand = value;
        }
    }

    public double CpuFrequency
    {
        get => _cpuFrequency;
        set
        {
            if (value < 1.0 || value > 4.0)
                throw new ArgumentException("CPU має бути 1.0–4.0 ГГц.");
            _cpuFrequency = value;
        }
    }

    public int RAM
    {
        get => _ram;
        set
        {
            if (value < 2 || value > 24)
                throw new ArgumentException("RAM 2–24 ГБ.");
            _ram = value;
        }
    }

    public int Storage
    {
        get => _storage;
        set
        {
            if (value < 32 || value > 1024)
                throw new ArgumentException("Памʼять 32–1024 ГБ.");
            _storage = value;
        }
    }

    public DateTime ReleaseDate
    {
        get => _releaseDate;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("Дата релізу не може бути у майбутньому.");
            _releaseDate = value;
        }
    }

    public int BatteryCapacity
    {
        get => _batteryCapacity;
        set
        {
            if (value < 2000 || value > 8000)
                throw new ArgumentException("Акумулятор 2000–8000 мА·год.");
            _batteryCapacity = value;
        }
    }

    public decimal LaunchPrice
    {
        get => _launchPrice;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Ціна має бути більше 0.");
            _launchPrice = value;
        }
    }

    public void PrintInfo(int index)
    {
        Console.WriteLine($"{index,-3} {ModelName,-25} {Brand,-10} {RAM,-5} {Storage,-7} {LaunchPrice}$ {(_inBasket ? "[У кошику]" : "")}");
    }

    public void YearsSinceRelease()
    {
        Console.WriteLine($"{ModelName}: років з релізу — {DateTime.Now.Year - ReleaseDate.Year}");
    }

    public void AddToBasket()
    {
        if (!_inBasket)
        {
            _inBasket = true;
            Console.WriteLine($"{ModelName} додано до кошика ");
        }
        else
        {
            Console.WriteLine($"{ModelName} вже знаходиться в кошику");
        }
    }

    public void RemoveFromBasket()
    {
        if (_inBasket)
        {
            _inBasket = false;
            Console.WriteLine($"{ModelName} видалено з кошика ");
        }
        else
        {
            Console.WriteLine($"{ModelName} не було в кошику");
        }
    }
}

class Program
{
    static List<Smartphone> phones = new List<Smartphone>();

    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1 - Додати об'єкт");
            Console.WriteLine("2 - Переглянути всі об'єкти");
            Console.WriteLine("3 - Знайти об'єкт");
            Console.WriteLine("4 - Продемонструвати поведінку");
            Console.WriteLine("5 - Видалити об'єкт");
            Console.WriteLine("0 - Вийти з програми");
            Console.Write("Ваш вибір: ");

            switch (Console.ReadLine())
            {
                case "1": AddPhone(); break;
                case "2": ShowAll(); break;
                case "3": FindPhone(); break;
                case "4": DemonstrateBehavior(); break;
                case "5": DeletePhone(); break;
                case "0": return;
                default: Console.WriteLine("Невірний пункт меню."); break;
            }
        }
    }

    static void AddPhone()
    {
        try
        {
            Smartphone p = new Smartphone();

            Console.Write("Модель: ");
            p.ModelName = Console.ReadLine();

            Console.WriteLine("Бренд: 0-Apple 1-Samsung 2-Xiaomi 3-Google 4-OnePlus");
            p.Brand = (SmartphoneBrand)int.Parse(Console.ReadLine());

            Console.Write("CPU (ГГц): ");
            p.CpuFrequency = double.Parse(Console.ReadLine());

            Console.Write("RAM (ГБ): ");
            p.RAM = int.Parse(Console.ReadLine());

            Console.Write("Storage (ГБ): ");
            p.Storage = int.Parse(Console.ReadLine());

            Console.Write("Battery (mAh): ");
            p.BatteryCapacity = int.Parse(Console.ReadLine());

            Console.Write("Дата релізу (yyyy-mm-dd): ");
            p.ReleaseDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Ціна на релізі: ");
            p.LaunchPrice = decimal.Parse(Console.ReadLine());

            phones.Add(p);
            Console.WriteLine("Смартфон додано ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static void ShowAll()
    {
        if (phones.Count == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        Console.WriteLine("№   Модель                     Бренд      RAM   Storage Ціна  Статус");
        for (int i = 0; i < phones.Count; i++)
            phones[i].PrintInfo(i + 1);
    }

    static void FindPhone()
    {
        Console.Write("Введіть бренд для пошуку: ");
        string brand = Console.ReadLine();

        bool found = false;
        for (int i = 0; i < phones.Count; i++)
        {
            if (phones[i].Brand.ToString().Equals(brand, StringComparison.OrdinalIgnoreCase))
            {
                phones[i].PrintInfo(i + 1);
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("Обʼєкти не знайдені.");
    }

    static void DemonstrateBehavior()
    {
        if (phones.Count == 0)
        {
            Console.WriteLine("Немає обʼєктів.");
            return;
        }

        Console.WriteLine("Оберіть смартфон для демонстрації поведінки:");
        for (int i = 0; i < phones.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {phones[i].ModelName} ({(phones[i].RAM)} ГБ RAM, {(phones[i].Storage)} ГБ Storage) {(phones[i].ToString() != null ? "" : "")}");
        }
        Console.Write("Ваш вибір (номер): ");
        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > phones.Count)
        {
            Console.WriteLine("Невірний вибір.");
            return;
        }

        Smartphone p = phones[choice - 1];

        while (true)
        {
            Console.WriteLine($"\n--- Поведінка смартфона {p.ModelName} ---");
            Console.WriteLine("1 - Додати до кошика");
            Console.WriteLine("2 - Видалити з кошика");
            Console.WriteLine("3 - Показати скільки років з релізу");
            Console.WriteLine("0 - Повернутися в головне меню");
            Console.Write("Ваш вибір: ");

            string action = Console.ReadLine();
            switch (action)
            {
                case "1": p.AddToBasket(); break;
                case "2": p.RemoveFromBasket(); break;
                case "3": p.YearsSinceRelease(); break;
                case "0": return;
                default: Console.WriteLine("Невірний пункт меню."); break;
            }
        }
    }

    static void DeletePhone()
    {
        if (phones.Count == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        Console.Write("Введіть номер обʼєкта: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < phones.Count)
        {
            phones.RemoveAt(index);
            Console.WriteLine("Обʼєкт видалено.");
        }
        else
        {
            Console.WriteLine("Невірний номер.");
        }
    }
}