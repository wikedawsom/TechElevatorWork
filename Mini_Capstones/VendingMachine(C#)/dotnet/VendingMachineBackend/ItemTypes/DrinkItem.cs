using System;
using System.Collections.Generic;
using System.Text;
using VendingMachineBackend.Interfaces;

namespace VendingMachineBackend.ItemTypes
{
    public class DrinkItem : VendingMachineItem, IMessage
    {
        public DrinkItem(string itemName, string slotIdentifier, decimal price) : base(itemName, slotIdentifier, price)
        {
        }

        public string ItemMessage()
        {
            return "Glug Glug, Yum!";
        }
    }
}
