using System;
using System.Collections.Generic;

namespace Supermarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Supermarket supermarket = new Supermarket();
            supermarket.Work();
        }
    }

    class Supermarket
    {
        private List<Product> _products = new List<Product>();
        private Queue<Buyer> _buyer = new Queue<Buyer>();
        private int _moneyInCashbox;

        public Supermarket()
        {
            _buyer = new Queue<Buyer>();
            CreateProducts();
        }

        public void Work()
        {
            const string AddBuyerToQueueCommand = "1";
            const string ServeBuyerCommand = "2";
            const string ShowMoneyInCashboxCommand = "3";
            const string ExitCommand = "4";

            bool isWork = true;

            while (isWork)
            {
                Console.WriteLine("Супермаркет");
                Console.WriteLine($"{AddBuyerToQueueCommand} - Добавить покупателя в очередь");
                Console.WriteLine($"{ServeBuyerCommand} - Обслужить покупателя");
                Console.WriteLine($"{ShowMoneyInCashboxCommand} - Показать деньги в кассе");
                Console.WriteLine($"{ExitCommand} - Закрыть программу");
                Console.Write("Выберите команду: ");

                string command = Console.ReadLine();

                switch (command)
                {
                    case AddBuyerToQueueCommand:
                        AddBuyer();
                        break;

                    case ServeBuyerCommand:
                        ServeAllBuyers();
                        break;

                    case ShowMoneyInCashboxCommand:
                        Console.WriteLine($"Денег в кассе: {_moneyInCashbox}");
                        break;

                    case ExitCommand:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Нет такой команды");
                        break;
                }
            }
        }

        private void AddBuyer()
        {
            int minRandomMoney = 340;
            int maxRandomMoney = 1000;
            int randomMoney = Utilite.GenerateRandomNumber(minRandomMoney, maxRandomMoney + 1);

            Buyer buyer = new Buyer(randomMoney,AddProductsInCart());

            _buyer.Enqueue(buyer);

            Console.WriteLine("Новый покупатель добавлен в очередь");
        }

        private void ServeAllBuyers()
        {
            {
                while (_buyer.Count > 0)
                {
                    Buyer buyer = _buyer.Dequeue();
                    ServeBuyer(buyer);
                }

                Console.WriteLine("Нет покупателей в очереди");
                Console.ReadKey();
            }
        }

        private void ServeBuyer(Buyer buyer)
        {

            int totalCost = buyer.GetSumPriceProductInCart();

            while (totalCost > buyer.Money)
            {
                Console.WriteLine("Покупатель ничего не купил");
                buyer.RemoveRandomItem();
                totalCost = buyer.GetSumPriceProductInCart();
            }

            _moneyInCashbox += buyer.BuyItems();
            Console.ReadKey();
        }

        public List<Product> AddProductsInCart()
        {
            List<Product> products = new List<Product>();

            int minQuantityProducts = 1;
            int maxQuantityProducts = 10;
            int quantityProducts = Utilite.GenerateRandomNumber(minQuantityProducts, maxQuantityProducts + 1);

            for (int i = 0; i > quantityProducts; i++)
            {
                int randomIndex = Utilite.GenerateRandomNumber(1, _products.Count + 1);
                products.Add(_products [randomIndex]);
            }

            return products;
        }
        private void CreateProducts()
        {
            _products.Add(new Product("Хлеб", 50));
            _products.Add(new Product("Вода", 30));
            _products.Add(new Product("Масло", 340));
            _products.Add(new Product("Молоко", 105));
            _products.Add(new Product("Кефир", 116));
        }
    }

    class Buyer
    {
       private List<Product> _bag = new List<Product>();

        public Buyer(int money, List<Product> products)
        {
            Money = money;
            Cart = products;
        }

        public List<Product> Cart { get; private set; }
        public int Money { get; private set; }
        
        public int GetSumPriceProductInCart()
        {
            int sum = 0;

            for (int i = 0; i < Cart.Count; i++)
            {
                sum += Cart[i].Price;
            }

            return sum;
        }

        public void RemoveRandomItem()
        {
            int index = Utilite.GenerateRandomNumber(0, Cart.Count + 1);
            Product removed = Cart[index];
            Cart.RemoveAt(index);

            Console.WriteLine($"Не хватает денег! Продукт {removed.Name} удалён из тележки.");
            Console.ReadKey();
        }

        public int BuyItems()
        {
            int money = 0;
            int totalCost = GetSumPriceProductInCart();

            Console.Clear();

            money += totalCost;
            _bag.AddRange(Cart);
            Cart.Clear();

            Console.WriteLine($"Покупатель купил товары на сумму: {totalCost}");
            ShoyPurchases();

            return money;
        }

        private void ShoyPurchases()
        {
            foreach (Product product in _bag)
            {
                product.ShowInfo();
            }
        }
    }

    class Product
    {
        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public int Price { get; private set; }

        public void ShowInfo()
        {
            Console.Write($"Товар: {Name}, Цена: {Price}\n");
        }
    }

    class Utilite
    {
        private static Random s_random = new Random();

        public static int GenerateRandomNumber(int lowerLimitRangeRandom, int upperLimitRangeRandom)
        {
            int numberRandom = s_random.Next(lowerLimitRangeRandom, upperLimitRangeRandom);
            return numberRandom;
        }

        public static int GetNumberInRange(int lowerLimitRangeNumbers = Int32.MinValue, int upperLimitRangeNumbers = Int32.MaxValue)
        {
            bool isEnterNumber = true;
            int enterNumber = 0;
            string userInput;

            while (isEnterNumber)
            {
                Console.WriteLine($"Введите число.");

                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out enterNumber) == false)
                    Console.WriteLine("Не корректный ввод.");
                else if (VerifyForAcceptableNumber(enterNumber, lowerLimitRangeNumbers, upperLimitRangeNumbers))
                    isEnterNumber = false;
            }

            return enterNumber;
        }

        private static bool VerifyForAcceptableNumber(int number, int lowerLimitRangeNumbers, int upperLimitRangeNumbers)
        {
            if (number < lowerLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за нижний предел допустимого значения.");
                return false;
            }
            else if (number > upperLimitRangeNumbers)
            {
                Console.WriteLine($"Число вышло за верхний предел допустимого значения.");
                return false;
            }

            return true;
        }
    }
}
