using System;
using System.Collections.Generic;

public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Bank transfer processed: {transaction.Amount:C} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Mobile money processed: {transaction.Amount:C} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Crypto wallet processed: {transaction.Amount:C} for {transaction.Category}");
    }
}

public class Account
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Updated balance: {Balance:C}");
        }
    }
}

public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        SavingsAccount account = new SavingsAccount("SA001", 1000m);

        Transaction transaction1 = new Transaction(
            1,
            DateTime.Now,
            150m,
            "Groceries"
        );

        Transaction transaction2 = new Transaction(
            2,
            DateTime.Now,
            200m,
            "Utilities"
        );

        Transaction transaction3 = new Transaction(
            3,
            DateTime.Now,
            100m,
            "Entertainment"
        );

        ITransactionProcessor mobileMoneyProcessor = new MobileMoneyProcessor();
        ITransactionProcessor bankTransferProcessor = new BankTransferProcessor();
        ITransactionProcessor cryptoWalletProcessor = new CryptoWalletProcessor();

        mobileMoneyProcessor.Process(transaction1);
        account.ApplyTransaction(transaction1);
        _transactions.Add(transaction1);

        bankTransferProcessor.Process(transaction2);
        account.ApplyTransaction(transaction2);
        _transactions.Add(transaction2);

        cryptoWalletProcessor.Process(transaction3);
        account.ApplyTransaction(transaction3);
        _transactions.Add(transaction3);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        FinanceApp app = new FinanceApp();
        app.Run();
    }
}
