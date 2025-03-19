using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidPrinciple
{
    // Single Responsibility Principle 
    class Order
    {
        public int Id { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public double TotalAmount { get; set; }
    }

    class OrderRepository
    {
        public void Save(Order order)
        {
            Console.WriteLine($"Order {order.Id} saved to database.");
        }
    }

    // Open/Closed Principle 
    interface IDiscount
    {
        double ApplyDiscount(double price);
    }

    class NoDiscount : IDiscount
    {
        public double ApplyDiscount(double price) => price;
    }

    class PercentageDiscount : IDiscount
    {
        private readonly double _percentage;
        public PercentageDiscount(double percentage) => _percentage = percentage;
        public double ApplyDiscount(double price) => price - (price * _percentage / 100);
    }

    // Liskov Substitution Principle
    abstract class PaymentProcessor
    {
        public abstract void ProcessPayment(double amount);
    }

    class CreditCardPayment : PaymentProcessor
    {
        public override void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing credit card payment of ${amount}.");
        }
    }

    class PayPalPayment : PaymentProcessor
    {
        public override void ProcessPayment(double amount)
        {
            Console.WriteLine($"Processing PayPal payment of ${amount}.");
        }
    }

    // Interface Segregation Principle 
    interface IOrderProcessor
    {
        void Process(Order order);
    }

    interface IEmailNotification
    {
        void SendEmail(string message);
    }

    class EmailNotification : IEmailNotification
    {
        public void SendEmail(string message)
        {
            Console.WriteLine($"Sending email: {message}");
        }
    }

    // Dependency Inversion Principle 
    class OrderProcessor : IOrderProcessor
    {
        private readonly OrderRepository _repository;
        private readonly PaymentProcessor _paymentProcessor;
        private readonly IEmailNotification _emailNotification;

        public OrderProcessor(OrderRepository repository, PaymentProcessor paymentProcessor, IEmailNotification emailNotification)
        {
            _repository = repository;
            _paymentProcessor = paymentProcessor;
            _emailNotification = emailNotification;
        }

        public void Process(Order order)
        {
            _paymentProcessor.ProcessPayment(order.TotalAmount);
            _repository.Save(order);
            _emailNotification.SendEmail("Your order has been processed successfully.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose an item to order (1: Donut - $20, 2: Burger - $40):");
            int itemChoice = int.Parse(Console.ReadLine());
            double price = itemChoice == 1 ? 20.0 : 40.0;
            string itemName = itemChoice == 1 ? "Donut" : "Burger";

            Console.WriteLine("Choose a payment method (1: Credit Card, 2: PayPal):");
            string paymentChoice = Console.ReadLine(); 

            PaymentProcessor paymentProcessor; 

            if (paymentChoice == "1")
                paymentProcessor = new CreditCardPayment();
            else
                paymentProcessor = new PayPalPayment();

            Order order = new Order() { Id = 1, Items = new List<string> { itemName }, TotalAmount = price };
            OrderProcessor processor = new OrderProcessor(new OrderRepository(), paymentProcessor, new EmailNotification());
            processor.Process(order);

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

    }
}