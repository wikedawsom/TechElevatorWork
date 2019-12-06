using System;
using System.Collections.Generic;
using System.Text;
using VendingMachineBackend.Interfaces;

namespace VendingMachineBackend.ItemTypes
{
    public class GumItem : VendingMachineItem, IMessage
    {
        public GumItem(string itemName, string slotIdentifier, decimal price) : base(itemName, slotIdentifier, price)
        {
        }

        public string ItemMessage()
        {
            return "Chew Chew, Yum!";
        }
    }
}
