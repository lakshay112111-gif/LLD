// lets you define a family of strategies/algorithm, encapsulate them in there own classes, and make them interchangaable atruntime.

using System;

public interface IPaymentStrategy
{
    void Pay(double amount);
}

// UPI Strategy
public class UPIPaymentStrategy : IPaymentStrategy
{
    public void Pay(double amount)
    {
        Console.WriteLine($"UPI Payment Received: {amount}");
    }
}

// Credit Card Strategy
public class CreditCardPaymentStrategy : IPaymentStrategy
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Credit Card Payment Received: {amount}");
    }
}

// Context
public class PaymentProcessor
{
    private readonly IPaymentStrategy _strategy;

    public PaymentProcessor(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ProcessPayment(double amount)
    {
        _strategy.Pay(amount);
    }
}

// Client
public class Program
{
    public static void Main()
    {
        IPaymentStrategy s1 = new UPIPaymentStrategy();
        IPaymentStrategy s2 = new CreditCardPaymentStrategy();

        PaymentProcessor paymentProcessUPI =
            new PaymentProcessor(s1);

        PaymentProcessor paymentProcessCreditCard =
            new PaymentProcessor(s2);

        paymentProcessUPI.ProcessPayment(200.12);
        paymentProcessCreditCard.ProcessPayment(4000.23);
    }
}
