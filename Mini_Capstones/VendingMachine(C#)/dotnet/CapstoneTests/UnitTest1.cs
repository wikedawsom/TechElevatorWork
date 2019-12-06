using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachineBackend;
using VendingMachineBackend.ItemTypes;
using System.Collections.Generic;

namespace CapstoneTests
{
    [TestClass()]
    public class VendingMachineTest
    {
        //[TestMethod]
        //public void DisplayItemsTest()
        //{
        //    var machine = new VendingMachine();
        //    machine.Items.Add("D4",new CandyItem ("KitKat" , "D4", 0.75m));
        //    Assert.AreEqual("KitKat, D4, $0.75\n", machine.DisplayItems());

        //}
        //[TestMethod]
        //public void DisplaySoldOutTest()
        //{
        //    var machine = new VendingMachine();
        //    machine.Items.Add("D4", new CandyItem("KitKat", "D4", 0.75m));
        //    machine.Items["D4"].Quantity = 0;
        //    Assert.AreEqual("SOLD OUT", machine.DisplayItems());

        //}
        //[TestMethod]
        //public void FeedMoneyTest()
        //{
        //    var machine = new VendingMachine();
        //    machine.Items.Add("D4", new CandyItem("KitKat", "D4", 0.75m));
        //    machine.FeedMoney("1");
        //    Assert.AreEqual(machine.Balance, 1m);

        //}
        //[TestMethod]
        //public void PurchaseItemTest()
        //{
        //    string machineResponse = "";
        //    var machine = new VendingMachine();
        //    machine.Items.Add("D4", new CandyItem("KitKat", "D4", 0.75m));
        //    machine.FeedMoney("1");
        //    machineResponse = machine.PurchaseItem("D4");
        //    Assert.AreEqual(machine.Balance, .25m);
        //    Assert.AreEqual(machine.Items["D4"].Quantity , 4);
        //    Assert.AreEqual(machineResponse , "Munch Munch, Yum!");

        //}
        //[TestMethod]
        //public void PurchaseItemNotEnoughMoneyTest()
            
        //{
        //    string machineResponse = "";
        //    var machine = new VendingMachine();
        //    machine.Items.Add("D4", new CandyItem("KitKat", "D4", 1.75m));
        //    machine.FeedMoney("1");
        //    machineResponse = machine.PurchaseItem("D4");
        //    Assert.AreEqual(machine.Balance, 1m);
        //    Assert.AreEqual(machine.Items["D4"].Quantity, 5);
        //    Assert.AreEqual(machineResponse, "Insufficient Funds");

        //}
        [TestMethod]
        public void FeedMoneyTest()
        {
            var machine = new VendingMachine();
            machine.FeedMoney("10");
            Assert.AreEqual(10m, machine.Balance);
        }
        [TestMethod]
        public void PurchaseItemTest()
        {
            var machine = new VendingMachine();
            string machineResponse = "";
            machine.FeedMoney("10");
            machineResponse = machine.PurchaseItem("C2");
            Assert.AreEqual(8.5m, machine.Balance);
            Assert.AreEqual(machine.Items["C2"].Quantity, 4);
            Assert.AreEqual(machineResponse, "Glug Glug, Yum!");
        }
        [TestMethod]
        public void GiveChangeTest()
        {
            var machine = new VendingMachine();
            string machineResponse = "";
            machine.FeedMoney("10");
            decimal change = 0;
            machineResponse = machine.PurchaseItem("C2");
            //change = machine.GiveChange();
            Assert.AreEqual(0, machine.Balance, "Balance is not correct");
            Assert.AreEqual(machine.Items["C2"].Quantity, 4);
            Assert.AreEqual(machineResponse, "Glug Glug, Yum!");
            //Assert.AreEqual(change, 8.5m);
        }
        [TestMethod]
        public void TotalSalesTest()
        {
            var machine = new VendingMachine();
            string machineResponse = "";
            machine.FeedMoney("10");
            decimal change = 0;

            machineResponse = machine.PurchaseItem("C2");
            //change = machine.GiveChange();
            Assert.AreEqual(0, machine.Balance, "Balance is not correct");
            Assert.AreEqual(machine.Items["C2"].Quantity, 4);
            Assert.AreEqual(machineResponse, "Glug Glug, Yum!");
        //    Assert.AreEqual(change, 8.5m);
        }

    }
}
