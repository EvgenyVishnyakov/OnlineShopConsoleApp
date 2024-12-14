using System.Collections.Generic;
using System;
using System.Numerics;
using System.Xml.Linq;


namespace OnlineShopConsoleApp;

public class User
{
    public string UserName { get; set; }
    public List<Order> OrderList { get; set; }
    public List<Product> Basket { get; set; }

    public User(string name)
    {
        UserName = name;
        Basket = new List<Product>();
        OrderList = new List<Order>();
    }    
}
public class InputInspector
{
    Store store;
    Store onlineStore = new Store();
    public int korrectCount = 1;
    public bool GetUserAnswer(string answer)
    {
        bool isNotNull = false;
        bool Flag = false;
        Console.WriteLine("");
        do
        {
            if (answer == "да")
            {
                isNotNull = true;
                Flag = true;
                break;
            }
            if (answer == "нет")
            {
                isNotNull = true;
                Flag = false;
                break;
            }
            
        } while (isNotNull != true);
        if (Flag) return true;
        return false;

    }
    public bool GetAnswerAddProduct()
    {
        bool Flag = false;
        bool isNotNull = false;
        Console.WriteLine("");
        do
        {
            Console.WriteLine("Хотите добавить продукт? Нажмите Да или нет");
            var adminAnswer = Console.ReadLine().ToLower();

            if (adminAnswer == "да")
            {
                isNotNull = true;
                Flag = true;
                break;
            }
            if (adminAnswer == "нет")
            {
                isNotNull = true;
                Flag = false;
                break;
            }
        } while (isNotNull != true);
        if (Flag) return true;
        return false;
    }
    public bool GetAnswerDeleteProduct()
    {
       
        bool Flag = false;
        bool isNotNull = false;
        Console.WriteLine("");
        do
        {
            Console.WriteLine("Хотите удалить продукт? Нажмите Да или нет");
            var adminAnswer = Console.ReadLine().ToLower();            
            if (adminAnswer == "да")
            {
                isNotNull = true;
                Flag = true;
                break;
            }
            if (adminAnswer == "нет")
            {
                isNotNull = true;
                Flag = false;
                
                break;
            }
        } while (isNotNull != true);
        if (Flag) return true;
        return false;
    }
    public int GetNumber()
    {
        var number = 0;
        while (!TryGetNumber(Console.ReadLine(), out number, out string errorMessage))
        {
            Console.WriteLine(errorMessage);
        }
        return number;
    }    
    public bool TryGetNumber(string input, out int number, out string errorMessage)
    {
        try
        {
            number = Convert.ToInt32(input);
            errorMessage = "";
            if (number > 0 && number <= onlineStore.Products.Count)
            {
                errorMessage = "";
                return true;
            }
            else if (number < 0)
            {
                errorMessage = "Введите неотрицательное число";
                return false;
            }
            else if (number > onlineStore.Products.Count )
            {
                errorMessage = "Нет такого номера в списке товаров. Введите существующий номер";
                return false;
            }
            return true;
        }
        catch (FormatException)
        {
            errorMessage = "Пожалуйста, введите число";
            number = 0;
            return false;
        }
        catch (OverflowException)
        {
            errorMessage = "Пожалуйста, введите число от 0 до 2*10^9";
            number = 0;
            return false;
        }
    }    
}

public class Admin
{
    InputInspector inspector = new InputInspector();
    Store onlineStore = new Store();
    public int Password { get; set; }

    public Admin() 
    {
        Password = 345;
    }
    public void ConfirmAdmin()
    {       
        Console.WriteLine("Введите пароль для подтвреждения прав администратора.");

        var userPassword = Console.ReadLine();
        var passwordForShop = GetNum(userPassword);
        if (passwordForShop == Password)
        {
            Console.WriteLine("Ваши права подтверждены!");
            Console.WriteLine();

            if (inspector.GetAnswerAddProduct())
            {
                Console.WriteLine("Напишите название добавляемого продукта.");
                Console.WriteLine();
                var adminAddProductName = Console.ReadLine();
                Console.WriteLine("Напишите цену добавляемого продукта.");
                Console.WriteLine();

                var adminAddProductPrice = Convert.ToInt32(Console.ReadLine());
                onlineStore.AddProductAdmin(adminAddProductName, adminAddProductPrice);
                Console.WriteLine();

            }
            if (inspector.GetAnswerDeleteProduct())
            {
                Console.WriteLine("Выберете номер продукта для удаления из каталога.");
                onlineStore.RemoveProduct(inspector.GetNumber());
            }
            else
            {
                Console.WriteLine("Возвращаемся в каталог");
            }
        }
        else
        {
            Console.WriteLine("В доступе отказано!");
        }
    }
    private int GetNum(string userPassword)
    {        
        bool Flag = true;
        while (Flag)
        {
            if (int.TryParse(userPassword, out var number))
            {
                Flag = false;
            }
            else
            {
                Console.WriteLine("Введите число, пожалуйста! Спасибо!");
                userPassword = Console.ReadLine();
                Flag =  true;
            }        
        }
        return Convert.ToInt32(userPassword);       
    }
}
public class Product
{
    public List<Product> Products;
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price; 
        Products = new List<Product>();
    }
    public void Print()
    {
        Console.WriteLine($"{Name} {Price}");
    }
    
}
public class Order
{
    public static int Id = 0;
    public int OrderNumber { get; set;}

    public decimal FullPrice;
    public Order(int number)
    {
        OrderNumber = number;
        Id++;
    }
    public override string ToString()
    {
        return $"{OrderNumber}";
    }
}
public class Store
{     
    public List<Product> Products;
    public List<Product> Basket = new List<Product>();
    public List<Order> Orders = new List<Order>();    
    public static int numberOrder {  get; set; }   
    public Store()
    {
        Products = new List<Product>
        {
            new Product("Хлеб", 44),
            new Product("Молоко", 68),
            new Product("Масло", 144),
            new Product("Печенье", 244),
            new Product("Йогурт", 96),
            new Product("Сок", 86)
        };       
        Basket = new List<Product>();
        Orders = new List<Order>();        
    }    
    public void RemoveProduct(int choise)
    {
        if (choise <= Products.Count && choise != 0)
        {
            Products.Remove(Products[choise - 1]);
            ShowProducts(Products);            
            Console.WriteLine();
            Console.WriteLine("");
            Console.WriteLine($"=== Продукт под номером {choise} был удален из каталога ===");
        }        
    }
    public void AddProductAdmin(string name, decimal price)
    {
        
        var newProduct = new Product(name, price);        
        Products.Add(newProduct);        
        ShowCatalog();

        Console.WriteLine("");
        Console.WriteLine($"=== Продукт {newProduct.Name} добавлен в каталог ===");
        ShowCatalog();  
    }
    public void ShowCatalog()
    {
        Console.WriteLine("Каталог продуктов");
        ShowProducts(Products);
    }
    public void ShowProducts(List<Product> Products)
    {
        var numberProduct = 1;
        foreach (var product in Products)
        {
            Console.Write(numberProduct + ". ");
            product.Print();
            numberProduct++;
        }
    }
    public void ShowBasket()
    {
        Console.WriteLine();
        Console.WriteLine("Корзина");
        Console.WriteLine();
        if (Basket.Count == 0)
        {
            Console.WriteLine("=======Корзина пока пустая.=======");
        }
        else
        {
            ShowProducts(Basket);
        }
    }
    public void ShowOrder(int numberOrder)
    {        
        if (Orders.Count == 0)
        {
            Console.WriteLine("=======Заказы не созданы.======");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine($"Заказ {numberOrder}");
            Console.WriteLine();
            ShowProducts(Basket);            
            GetAmountOrder();
        }
    }
    private void GetAmountOrder()
    {       
        decimal orderCost = 0;
        for (int i = 0; i < Basket.Count; i++)
        {
            orderCost += Basket[i].Price;
        }
        Console.WriteLine("");
        Console.WriteLine($"=== Заказ на сумму {orderCost} рублей обработан ===");
    }
    public void ShowListOrders(List<Order> Orders)
    {
        var number = 1;
        if (Orders.Count == 0)
        {
            Console.WriteLine("У Вас нет созданных заказов.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("===Список Ваших заказов===");
            foreach (var order in Orders)
            {
                Console.Write(number + ". ");
                Console.WriteLine($"{order.ToString()}");                
                number++;
            }
        }        
    }    
    public void AddProductToBasket(int numberProduct)
    {
        if (numberProduct <= Products.Count && numberProduct != 0)
        {
            Basket.Add(Products[numberProduct - 1]);
            
            ShowCatalog();
            Console.WriteLine();
            Console.WriteLine($"Продукт {Products[numberProduct - 1].Name} успешно добавлен в корзину.");
            Console.WriteLine();
            Console.WriteLine($"Кол-во товаров в корзине {Basket.Count} ");
            ShowBasket();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Такого товара нет!");
        }
    }
    public void DeleteProductFromBasket(int numberProduct)
    {
        if (numberProduct <= Basket.Count && numberProduct != 0)
        {
            Console.WriteLine();
            Console.WriteLine($"Продукт {Basket[numberProduct - 1].Name} успешно удален из корзины.");
            Console.WriteLine();
            Basket.RemoveAt(numberProduct- 1);            
            Console.WriteLine($"Кол-во товаров в корзине {Basket.Count} ");
            ShowBasket();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Такого товара в корзине нет!");
        }
    }
    public void CreateOrder()
    {
        Random random = new Random();
        numberOrder = random.Next(235, 1000000);
        // передать в отдел доставки
        if (Basket.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("===Ваша корзина пустая и оформить заказ, к сожалению, не получится.===");
            Console.WriteLine();
        }
        else
        {
            Order order = new Order(numberOrder);           
            AddOrder(order);
            Console.WriteLine($"Создан заказ номер {numberOrder}. Желаете его посмотреть? Нажмите Да или Нет");
            var userAnswer = Console.ReadLine().ToLower();
            if (userAnswer == "да" )
            {
                ShowOrder(numberOrder);
            }
            if (userAnswer == "нет" )
            {
                ShowCatalog();
            }
            Basket.Clear();
        }
    }
    private void AddOrder(Order newOrder)
    {
        Orders.Add(newOrder);
    }
}
public class Game
{
    Store onlineStore = new Store();

    Admin admin = new Admin();
    public void showGame()
    {
        bool answerYes = false;
        
        do
        {
            Console.WriteLine();
            Console.WriteLine($"{1}. Показать онлайн каталог!");
            Console.WriteLine($"{2}. Добавить продукты в корзину");
            Console.WriteLine($"{3}. Удалить продукты из корзины");
            Console.WriteLine($"{4}. Посмотреть  корзину");
            Console.WriteLine($"{5}. Оформление заказа");
            Console.WriteLine($"{6}. Показать список заказов");
            Console.WriteLine($"{7}. Войти как администратор");
            Console.WriteLine($"{8}. Покинуть магазин");

            var numberAction = Convert.ToInt32(Console.ReadLine());
            switch (numberAction)
            {
                case 1: Console.WriteLine(); onlineStore.ShowCatalog(); break;
                case 2: Console.WriteLine(); AddProductBasket(onlineStore); break;
                case 3: Console.WriteLine(); DeleteProductBasket(onlineStore); break;
                case 4: Console.WriteLine(); LookBasket(onlineStore); break;
                case 5: Console.WriteLine(); MakeOrder(onlineStore); break;
                case 6: Console.WriteLine(); onlineStore.ShowListOrders(onlineStore.Orders); break;
                case 7: Console.WriteLine(); admin.ConfirmAdmin(); break;
                case 8: Console.WriteLine(); ExitStore(); answerYes = true; break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Выберите действие из списка");
                    Console.WriteLine();
                    break;
            }
        } while (!answerYes);
    }
private static void DeleteProductBasket(Store onlineStore)
{
    int answerYes;
    do
    {
        Console.WriteLine("Хотите удалить продукт из корзины? Ответьте Да или нет");
        answerYes = IsYes(Console.ReadLine());
        if (answerYes == 1)
        {
            onlineStore.ShowBasket();
            Console.WriteLine("Напишите номер продукта, который нужно удалить из корзины");
            var userProductChoice = Convert.ToInt32(Console.ReadLine());
            onlineStore.DeleteProductFromBasket(userProductChoice);
            continue;
        }
        if (answerYes == 0)
        {
            break;
        }
        else
            Console.WriteLine("=====Неверное действие=====");

    } while (answerYes != 0);
}

static void AddProductBasket(Store onlineStore)
{
    int answerYes;
    do
    {
        Console.WriteLine("Хотите добавить продукт в корзину? Ответьте Да или нет");
        answerYes = IsYes(Console.ReadLine().ToLower());
        if (answerYes == 1)
        {
            onlineStore.ShowCatalog();
            Console.WriteLine("Напишите номер продукта, который нужно добавить в корзину");
            //onlineStore.RemoveProduct(inspector.GetNumber());
            var userProductChoice = Convert.ToInt32(Console.ReadLine());
            onlineStore.AddProductToBasket(userProductChoice);
            continue;
        }
        if (answerYes == 0)
        {
            break;
        }
        else
            Console.WriteLine("=====Неверное действие=====");
    } while (answerYes != 0);
}
static void MakeOrder(Store onlineStore)
{
    int answerYes;
    do
    {
        if (onlineStore.Basket.Count == 0)
            break;
        Console.WriteLine("Хотите оформить заказ? Ответьте Да или нет");
        answerYes = IsYes(Console.ReadLine());
        if (answerYes == 1)
        {
            onlineStore.CreateOrder();
            continue;
        }
        if (answerYes == 0)
        {
            break;
        }
        else
            Console.WriteLine("=====Неверное действие=====");
    } while (answerYes != 0);
}
static void LookBasket(Store onlineStore)
{
    int answerYes;
    do
    {
        Console.WriteLine("Хотите посмотреть корзину? Ответьте Да или нет");
        answerYes = IsYes(Console.ReadLine());
        if (answerYes == 1)
        {
            onlineStore.ShowBasket();
            continue;
        }
        if (answerYes == 0)
        {
            break;
        }
        else
            Console.WriteLine("=====Неверное действие=====");
    } while (answerYes != 0);
}
static int IsYes(string answer)
{
    if (answer == "да")
    {
        return 1;
    }
    else if (answer == "нет")
    {
        return 0;
    }
    else
        return 2;
}
static void ExitStore()
{
    Console.WriteLine();
    Console.WriteLine("Будем с нетерпением ждать Вас снова! Всего доброго!");
    Console.WriteLine();
}

}
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Приветствуем Вас в нашем онлайн магазине!");
        Console.WriteLine();
        Console.WriteLine("Как Ваше имя?");
        Console.WriteLine();

        var userName = Console.ReadLine();
        User user = new User(userName);
        
        Console.WriteLine();
        Console.WriteLine($"{userName}, пожалуйста,введите номер действия, которое хотите совершить");
        Console.WriteLine();
        Game game = new Game();
        game.showGame();
    }
    
}