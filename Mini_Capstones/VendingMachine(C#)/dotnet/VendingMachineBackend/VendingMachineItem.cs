using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachineBackend
{
    public abstract class VendingMachineItem
    {
        public string ItemName { get; }
        public string SlotItentifier { get; }
        public decimal Price { get; }
        public decimal Quantity { get; private set; } = 5;
        public bool IsSoldOut
        {
            get
            {
                if (Quantity <= 0)
                {
                    return true;
                }
                return false;
            }
        }

        public VendingMachineItem(string itemName, string slotIdentifier, decimal price)
        {
            Price = price;
            ItemName = itemName;
            SlotItentifier = slotIdentifier;
        }

        public void SellItem()
        {
            Quantity--;
        }
    }
}
