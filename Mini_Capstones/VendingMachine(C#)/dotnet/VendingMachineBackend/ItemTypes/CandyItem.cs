using System;
using System.Collections.Generic;
using System.Text;
using VendingMachineBackend.Interfaces;

namespace VendingMachineBackend.ItemTypes
{
    public class CandyItem : VendingMachineItem, IMessage
    {
        public CandyItem(string itemName, string slotIdentifier, decimal price) : base(itemName, slotIdentifier, price)
        {
        }

        public string ItemMessage()
        {
            return "Munch Munch, Yum!";
        }
    }
}
