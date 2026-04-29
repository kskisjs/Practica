using System;
using System.Collections.Generic;

public class ConfigManager
{
    private static ConfigManager _instance;
    private Dictionary<string, string> _configs = new Dictionary<string, string>();

    private ConfigManager() { }

    public static ConfigManager GetInstance()
    {
        if (_instance == null)
            _instance = new ConfigManager();
        return _instance;
    }

    public void SetConfig(string key, string value)
    {
        _configs[key] = value;
        Console.WriteLine($"{key} = {value}");
    }

    public string GetConfig(string key)
    {
        return _configs.ContainsKey(key) ? _configs[key] : "Не найдено";
    }
}

// Демонстрация
class Program
{
    static void Main()
    {
        ConfigManager cfg = ConfigManager.GetInstance();

        cfg.SetConfig("Theme", "Dark");
        cfg.SetConfig("Language", "Russian");

        Console.WriteLine(cfg.GetConfig("Theme"));
        Console.WriteLine(cfg.GetConfig("Language"));

        // Проверка Singleton
        Console.WriteLine(ConfigManager.GetInstance() == cfg);
    }
}