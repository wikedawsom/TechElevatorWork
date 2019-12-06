using System;
using System.Collections.Generic;
using System.Text;
using VendingMachineBackend.Interfaces;

namespace VendingMachineBackend.ItemTypes
{
    public class ChipItem : VendingMachineItem, IMessage
    {
        public ChipItem(string itemName, string slotIdentifier, decimal price) : base(itemName, slotIdentifier, price)
        {
        }

        public string ItemMessage()
        {
            return "Crunch Crunch, Yum!";
        }
    }
}
