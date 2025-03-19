using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidPrinciple
{
 
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
            Console.WriteLine("Processing order...");
            _paymentProcessor.ProcessPayment(order.TotalAmount);
            _repository.Save(order);
            _emailNotification.SendEmail("Your order has been processed successfully.");
            Console.WriteLine("Order processing complete.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting program...");

            Order order = new Order() { Id = 1, TotalAmount = 100.0 };
            Console.WriteLine("Order created.");

            OrderProcessor processor = new OrderProcessor(new OrderRepository(), new CreditCardPayment(), new EmailNotification());
            Console.WriteLine("Order processor initialized.");

            processor.Process(order);
            Console.WriteLine("Order processed.");

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine(); // Keeps the console open
        }
    }
}
