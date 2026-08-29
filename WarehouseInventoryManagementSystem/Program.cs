using System;
using System.Collections.Generic;
using System.Linq;

public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

public class ElectronicItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public string Brand { get; }
    public int WarrantyMonths { get; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}

public class GroceryItem : IInventoryItem
{
    public int Id { get; }
    public string Name { get; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}

public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message)
    {
    }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message)
    {
    }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message)
    {
    }
}

public class InventoryRepository<T> where T : IInventoryItem
{
    private Dictionary<int, T> _items = new Dictionary<int, T>();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
        {
            throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
        }

        _items.Add(item.Id, item);
    }

    public T GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException($"Item with ID {id} was not found.");
        }

        return _items[id];
    }

    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
        {
            throw new ItemNotFoundException($"Item with ID {id} was not found.");
        }

        _items.Remove(id);
    }

    public List<T> GetAllItems()
    {
        return _items.Values.ToList();
    }

    public void UpdateQuantity(int id, int newQuantity)
    {
        if (newQuantity < 0)
        {
            throw new InvalidQuantityException("Quantity cannot be negative.");
        }

        T item = GetItemById(id);
        item.Quantity = newQuantity;
    }
}

public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics =
        new InventoryRepository<ElectronicItem>();

    private InventoryRepository<GroceryItem> _groceries =
        new InventoryRepository<GroceryItem>();

    public void SeedData()
    {
        _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "HP", 24));
        _electronics.AddItem(new ElectronicItem(2, "Phone", 10, "Samsung", 12));
        _electronics.AddItem(new ElectronicItem(3, "Television", 4, "LG", 18));

        _groceries.AddItem(new GroceryItem(101, "Rice", 20, DateTime.Now.AddMonths(6)));
        _groceries.AddItem(new GroceryItem(102, "Milk", 15, DateTime.Now.AddDays(30)));
        _groceries.AddItem(new GroceryItem(103, "Bread", 12, DateTime.Now.AddDays(7)));
    }

    public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
    {
        foreach (T item in repo.GetAllItems())
        {
            Console.WriteLine(
                $"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}"
            );
        }
    }

    public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity)
        where T : IInventoryItem
    {
        try
        {
            T item = repo.GetItemById(id);
            repo.UpdateQuantity(id, item.Quantity + quantity);

            Console.WriteLine(
                $"Stock updated successfully. New quantity: {item.Quantity}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void RemoveItemById<T>(InventoryRepository<T> repo, int id)
        where T : IInventoryItem
    {
        try
        {
            repo.RemoveItem(id);
            Console.WriteLine("Item removed successfully.");
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void RunTests()
    {
        Console.WriteLine("\nTesting Exceptions:");

        try
        {
            _electronics.AddItem(
                new ElectronicItem(1, "Duplicate Laptop", 2, "Dell", 12)
            );
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine($"Duplicate Error: {ex.Message}");
        }

        try
        {
            _groceries.RemoveItem(999);
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"Not Found Error: {ex.Message}");
        }

        try
        {
            _electronics.UpdateQuantity(2, -5);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine($"Quantity Error: {ex.Message}");
        }
    }

    public void Run()
    {
        SeedData();

        Console.WriteLine("Grocery Items:");
        PrintAllItems(_groceries);

        Console.WriteLine("\nElectronic Items:");
        PrintAllItems(_electronics);

        RunTests();
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        WareHouseManager manager = new WareHouseManager();
        manager.Run();
    }
}
