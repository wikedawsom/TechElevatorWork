using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class DBDALIntegrationTests
    {
        private TransactionScope transaction;
        private string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=npcampground;Integrated Security=True";
        private ParkDBDAL dal;

        /// <summary>
        /// Automatically sets up a temporary dummy database that is initialized with known values
        /// </summary>
        [TestInitialize]
        public void Start()
        {
            transaction = new TransactionScope();
            dal = new ParkDBDAL(_connectionString);
            // Get the SQL Script to run
            string sql = File.ReadAllText("TempTestDB.sql");

            // Execute the script
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Automatically rolls back database to the state it was in before testing began
        /// </summary>
        [TestCleanup]
        public void Finish()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetParkListTest()
        {
            var list = dal.GetParkList();
            Assert.AreEqual(1, list.Count);
        }
        [TestMethod]
        public void GetCampgroundListTest()
        {
            Park park = dal.GetParkList()[0];
            var list = dal.GetCampgroundList(park);
            Assert.AreEqual(1, list.Count);
        }
        [TestMethod]
        public void GetReservationsTest()
        {
            var park = dal.GetParkList()[0];
            var campground = dal.GetCampgroundList(park)[0];
            var reservationList = dal.GetReservations(campground);
            Assert.AreEqual(1, reservationList.Count);
        }
        [TestMethod]
        public void SearchAvailableReservationsTest()
        {
            // The only reservation in the list is from 10/21/2019 to 10/25/2019
            // Any other date range should display one available site (the only site in the DB)
            DateTime StartDate = new DateTime(2019, 11, 1);
            DateTime EndDate = new DateTime(2019, 11, 5);

            var park = dal.GetParkList()[0];
            var campground = dal.GetCampgroundList(park)[0];

            var list = dal.SearchAvailableReservations(campground, StartDate, EndDate);
            Assert.AreEqual(1, list.Count);
        }
        [TestMethod]
        public void GetAllSitesInCampgroundTest()
        {
            var park = dal.GetParkList()[0];
            var campground = dal.GetCampgroundList(park)[0];
            var siteList = dal.GetAllSitesInCampground(campground);
            Assert.AreEqual(1, siteList.Count);
        }
        [TestMethod]
        public void SaveReservationTest()
        {
            var park = dal.GetParkList()[0];
            var campground = dal.GetCampgroundList(park)[0];
            var site = dal.GetAllSitesInCampground(campground)[0];

            Reservation res = new Reservation
            {
                CreateDate = DateTime.Now,
                FromDate = new DateTime(2019, 11, 2),
                ToDate = new DateTime(2019, 11, 5),
                Name = "Customer1",
                SiteID = site.SiteID
            };
            res.ReservationID = dal.SaveReservation(res);

            var resList = dal.GetReservations(campground);
            Assert.AreEqual(2, resList.Count, "The reservation list should now have 2 entries.");
            Assert.AreNotEqual(0, res.ReservationID, "Reservation ID should not be 0.");
        }
    }
}
