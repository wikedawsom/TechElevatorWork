using System;
using VendingMachineBackend;
using VendingMachineBackend.Exceptions;

namespace VendingMachineCLI
{
    public class Menu
    {
        private static VendingMachine _machine = new VendingMachine();

        public static void MainMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("1.) Display Items");
                Console.WriteLine("2.) Purchase Items");
                Console.WriteLine("3.) Exit");

                var keySelection = Console.ReadKey();

                if (keySelection.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    Console.WriteLine(_machine.DisplayItems());
                    Console.ReadKey();
                }
                else if (keySelection.Key == ConsoleKey.D2)
                {
                    Console.Clear();
                    PurchaseItemsMenu();
                }
                else if (keySelection.Key == ConsoleKey.D3)
                {
                    exit = true;
                }
                else if (keySelection.Key == ConsoleKey.D4)
                {
                    Console.Clear();
                    Console.Write(_machine.ReadSalesReport());
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Invalid Selection");
                    Console.ReadKey();
                }

            }
        }
        
        public static void PurchaseItemsMenu()
        {
            bool finishTransaction = false;
            while (!finishTransaction)
            {
                Console.Clear();
                Console.WriteLine("1.) Feed Money");
                Console.WriteLine("2.) Select Product");
                Console.WriteLine("3.) Finish Transaction");

                var keySelection = Console.ReadKey();

                if (keySelection.Key == ConsoleKey.D1)
                {
                    FeedMoney();
                }
                else if (keySelection.Key == ConsoleKey.D2)
                {
                    SelectProduct();
                }
                else if (keySelection.Key == ConsoleKey.D3)
                {
                    string givechange = "";
                    _machine.SalesReport();
                    givechange = _machine.GiveChange();
                    Console.WriteLine($"\nYour change is: \n{givechange}");
                    Console.ReadKey();
                    finishTransaction = true;

                }
                else
                {
                    Console.WriteLine("Invalid Selection");
                    Console.ReadKey();
                }
            }
        }

        private static void SelectProduct()
        {
            Console.Clear();
            Console.WriteLine(_machine.DisplayItems());
            Console.Write($"Your current balance is {_machine.Balance:C}, please enter the slot ID of your item choice: ");
            string userSelection = Console.ReadLine().ToUpper();
            if (_machine.Items.ContainsKey(userSelection))
            {
                Console.WriteLine($"You selected: {_machine.Items[userSelection].ItemName}");
                Console.WriteLine(_machine.PurchaseItem(userSelection));
                Console.WriteLine($"Your new current balance is {_machine.Balance:C}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid Selection, please try again..");
                Console.ReadKey();
            }
        }
        private static void FeedMoney()
        {
         
            Console.Clear();
            Console.WriteLine($"You have {_machine.Balance:C}, please enter amount you would like to add: ");
            Console.WriteLine("Please enter money in dollar amounts only");
            string moneyEntered = Console.ReadLine();

            decimal decimalmoneyEntered = 0;
            if (decimal.TryParse(moneyEntered, out decimalmoneyEntered))
            {
                Console.WriteLine($"You entered {decimalmoneyEntered:C}");
            }
            else
            {
                Console.WriteLine($"You entered {moneyEntered}");
            }
            try
            {
                _machine.FeedMoney(moneyEntered);
            }
            catch (BadMoneyInputException)
            {
                Console.WriteLine("Invalid entry, please try again ");
            }
            Console.WriteLine($"Your balance is: {_machine.Balance:C}");
            Console.ReadKey();
        }
    }
}
