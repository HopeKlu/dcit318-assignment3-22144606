using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public interface IInventoryEntity
{
    int Id { get; }
}

public record InventoryItem(
    int Id,
    string Name,
    int Quantity,
    DateTime DateAdded
) : IInventoryEntity;

public class InventoryLogger<T> where T : IInventoryEntity
{
    private List<T> _log;
    private string _filePath;

    public InventoryLogger(string filePath)
    {
        _filePath = filePath;
        _log = new List<T>();
    }

    public void Add(T item)
    {
        _log.Add(item);
    }

    public List<T> GetAll()
    {
        return _log;
    }

    public void SaveToFile()
    {
        try
        {
            string json = JsonSerializer.Serialize(
                _log,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.Write(json);
            }

            Console.WriteLine("Inventory data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving inventory data: {ex.Message}");
        }
    }

    public void LoadFromFile()
    {
        try
        {
            using (StreamReader reader = new StreamReader(_filePath))
            {
                string json = reader.ReadToEnd();

                List<T>? loadedItems =
                    JsonSerializer.Deserialize<List<T>>(json);

                _log = loadedItems ?? new List<T>();
            }

            Console.WriteLine("Inventory data loaded successfully.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading inventory data: {ex.Message}");
        }
    }

    public void Clear()
    {
        _log.Clear();
    }
}

public class InventoryApp
{
    private InventoryLogger<InventoryItem> _logger;

    public InventoryApp()
    {
        _logger = new InventoryLogger<InventoryItem>("inventory.json");
    }

    public void SeedSampleData()
    {
        _logger.Add(
            new InventoryItem(
                1,
                "Laptop",
                5,
                DateTime.Now
            )
        );

        _logger.Add(
            new InventoryItem(
                2,
                "Phone",
                10,
                DateTime.Now
            )
        );

        _logger.Add(
            new InventoryItem(
                3,
                "Keyboard",
                15,
                DateTime.Now
            )
        );

        _logger.Add(
            new InventoryItem(
                4,
                "Mouse",
                20,
                DateTime.Now
            )
        );
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void ClearMemory()
    {
        _logger.Clear();
        Console.WriteLine("Memory cleared.");
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        Console.WriteLine("\nInventory Items:");

        foreach (InventoryItem item in _logger.GetAll())
        {
            Console.WriteLine(
                $"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded}"
            );
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        InventoryApp app = new InventoryApp();

        app.SeedSampleData();
        app.SaveData();

        app.ClearMemory();

        app.LoadData();
        app.PrintAllItems();
    }
}
