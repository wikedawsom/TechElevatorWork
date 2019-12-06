using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using VendingMachineBackend.Exceptions;
using VendingMachineBackend.Interfaces;
using VendingMachineBackend.ItemTypes;

namespace VendingMachineBackend
{
    public class VendingMachine
    {
        public Dictionary<string, VendingMachineItem> Items { get; set; }
        public decimal Balance { get; private set; } = 0m;
        public decimal TotalRevenue { get; private set; } = 0m;
        

        public VendingMachine()
        {
            MakeItemList();
        }

        private void MakeItemList()
        {
            Items = new Dictionary<string, VendingMachineItem>();

            using (StreamReader sr = new StreamReader("VendingMachine.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    AddItemToList(line);
                }
            }
        }
        
        private void AddItemToList(string inLineItem)
        {
            // Split line on |
            string[] splitItems = inLineItem.Split("|");
            var name = splitItems[1];
            var slotNumber = splitItems[0];
            var price = decimal.Parse(splitItems[2]);
            var type = splitItems[3];

            if(type == "Chip")
            {
                var chipItem = new ChipItem(name, slotNumber, price);
                Items.Add(slotNumber,chipItem);
            }
            else if(type == "Candy")
            {
                var candyItem = new CandyItem(name, slotNumber, price);
                Items.Add(slotNumber, candyItem);
            }
            else if(type == "Drink")
            {
                var drinkItem = new DrinkItem(name, slotNumber, price);
                Items.Add(slotNumber, drinkItem);
            }
            else if(type == "Gum")
            {
                var gumItem = new GumItem(name, slotNumber, price);
                Items.Add(slotNumber, gumItem);
            }
        }

        public string DisplayItems()
        {
            string output = "";
            foreach(var item in Items.Values)
            {
                if (item.IsSoldOut)
                {
                    output += "SOLD OUT\n";
                }
                else
                {
                    output += item.SlotItentifier + ", ";
                    output += item.ItemName + ", ";
                    output += $"{item.Price:C}\n";
                }
            }
            return output;
        }
        
        public string PurchaseItem(string slotID)
        {
            string output = "No message";
            slotID = slotID.ToUpper();
            var chosenItem = Items[slotID];
            if (Balance <= 0m)
            {
                output = "Please deposit money before making a selection.";
            }
            else if (!Items.ContainsKey(slotID))
            {
                output = "Invalid item selection.";
            }
            else if (chosenItem.IsSoldOut)
            {
                output = "SOLD OUT";
            }
            else if (Balance < chosenItem.Price)
            {
                output = "Insufficient Funds";
            }
            else
            {
                if (chosenItem is IMessage)
                {
                    output = ((IMessage)chosenItem).ItemMessage();
                }
                Balance -= chosenItem.Price;
                TotalRevenue += chosenItem.Price;
                chosenItem.SellItem();
                LogReport($"{chosenItem.ItemName} {slotID} {chosenItem.Price:C} {Balance:C}");
            }
            return output;
        }

        public string FeedMoney(string money)
        {
            try
            {
                int temp = (int)decimal.Parse(money);
                if ((int)(decimal.Parse(money)*100) == temp*100)
                {
                    Balance += temp;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                throw new BadMoneyInputException();
            }
            var decimalMoney = decimal.Parse(money);
            LogReport($"FEED MONEY: {decimalMoney:C} {Balance:C}");
            return Balance.ToString();
        }
        public void LogReport(string report)
        {
            DateTime currentTime = DateTime.Now;
            using (StreamWriter sw = new StreamWriter("Log.txt", true))
            {
                sw.WriteLine(currentTime + " " + report);
            }
        }
        public string GiveChange()
        {
            decimal output = Balance;
            Balance = 0;
            LogReport($"GIVE CHANGE: {output:C} {Balance:C}");
            string stringOutput = "";
            int quarters = 0;
            int dimes = 0;
            int nickels = 0;
            quarters = (int)(output / .25m);
            output = output % .25m;
            dimes = (int)(output / .10m);
            output = output % .10m;
            nickels = (int)(output / .05m);
            output = output % .05m;
            
            stringOutput = ($" Quarters: {quarters}\n Dimes: {dimes}\n Nickels: {nickels}");

            return stringOutput;
            
        }
        public void SalesReport()
        {
            List<int> salesAmounts = new List<int>();
            decimal currentSalesTotal = 0;
            if (File.Exists("SalesReport.txt"))
            {
                string currentSalesReport = ReadSalesReport();
                string[] splitReport = currentSalesReport.Split("\n");
                for (int i = 0; i < splitReport.Length; i++)
                {
                    if (i < splitReport.Length - 3)
                    {
                        salesAmounts.Add(int.Parse(splitReport[i].Substring(splitReport[i].IndexOf("|") + 1)));
                    }
                    else if(splitReport[i].Contains("*"))
                    {
                        currentSalesTotal += decimal.Parse(splitReport[i].Substring(splitReport[i].IndexOf("$") + 1));
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter("SalesReport.txt", false))
            {
                int i = 0;
                foreach (var item in Items.Values)
                {
                    if (salesAmounts.Count > 0)
                    {
                        sw.WriteLine(item.ItemName + "|" + (5 - item.Quantity + salesAmounts[i]));
                        i++;
                    }
                    else
                    {
                        sw.WriteLine(item.ItemName + "|" + (5 - item.Quantity));
                    }
                }
                sw.WriteLine();
                sw.WriteLine($"**TOTAL SALES** {currentSalesTotal + TotalRevenue:C}");
            }
        }
        public string ReadSalesReport()
        {
            string total = "";
            using (StreamReader sr = new StreamReader("SalesReport.txt"))
            {
                while (!sr.EndOfStream)
                {
                    total += sr.ReadLine() + "\n";
                }
            }
            return total;
        }
    }
}
