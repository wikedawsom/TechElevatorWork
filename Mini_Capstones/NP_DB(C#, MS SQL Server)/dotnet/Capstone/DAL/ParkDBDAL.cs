using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkDBDAL
    {
        private const string _getlastIDSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString;

        public ParkDBDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Creates an all inclusive list of Park objects from the 'park' table in the database
        /// </summary>
        /// <param name="park">User specified park</param>
        /// <returns>All parks in the database</returns>
        public List<Park> GetParkList()
        {
            var allParks = new List<Park>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sqlCommand = "SELECT * FROM park " +
                                    "ORDER BY name;";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    allParks.Add(GetParkFromReader(reader));
                }
            }
            return allParks;
        }
        
        /// <summary>
        /// Creates a list of Campground objects from the 'campground' table 
        /// in the database for a given park
        /// </summary>
        /// <param name="park">User specified park</param>
        /// <returns>All campgrounds in the park</returns>
        public List<Campground> GetCampgroundList(Park park)
        {
            var campgroundList = new List<Campground>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // this command will connect all campgrounds with their corresponding park
                string sqlCommand = "SELECT * FROM campground " +
                                   $"INNER JOIN park ON park.park_id = campground.park_id " +
                                   $"WHERE park.park_id = {park.ParkID};";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campgroundList.Add(GetCampgroundFromReader(reader));
                }
            }

            return campgroundList;
        }
        /// <summary>
        /// Creates a List of Reservation objects from the 'reservation' table
        /// the the database that match the given campground. If no campground given, 
        /// then the List includes all reservations in the database
        /// </summary>
        /// <param name="campground"></param>
        /// <returns>All reservations for the campground</returns>
        public List<Reservation> GetReservations(Campground campground)
        {
            var reservationList = new List<Reservation>();
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Pull all reservations, with their associated site and campground
                string sqlCommand = "SELECT * FROM reservation " +
                                   $"INNER JOIN site ON site.site_id = reservation.site_id " +
                                   $"INNER JOIN campground ON campground.campground_id = site.campground_id " +
                                   $"WHERE site.campground_id = {campground.CampgroundID};";
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    reservationList.Add(GetReservationFromReader(reader));
                }
            }
            return reservationList;
        }
        /// <summary>
        /// Finds all campsites on the campgrounds that are available to be reserved in the given timeframe
        /// </summary>
        /// <param name="campground">Specified campground</param>
        /// <param name="StartDate">Desired reservation start date</param>
        /// <param name="EndDate">Desired reservation end date</param>
        /// <returns>A list of campsites available for reservation</returns>
        public List<Site> SearchAvailableReservations(Campground campground, DateTime StartDate, DateTime EndDate)
        {
            var sitelist = GetAllSitesInCampground(campground);
            var reservationList = GetReservations(campground);

            var filteredSiteList = new List<Site>();

            foreach (Site site in sitelist)
            {
                bool resAvailable = true;
                foreach (Reservation reservation in reservationList)
                {
                    if(((StartDate >= reservation.FromDate && StartDate <= reservation.ToDate) || 
                        (EndDate >= reservation.FromDate && EndDate <= reservation.ToDate)) && 
                        reservation.SiteID == site.SiteID)
                    {
                        resAvailable = false;
                    }
                }
                if (resAvailable && filteredSiteList.Count < 5)
                {
                    filteredSiteList.Add(site);
                }
            }
            return filteredSiteList;
        }
        /// <summary>
        /// Retrieves a list of every campsite asociated with a specific campground
        /// </summary>
        /// <param name="campground">Specified campground</param>
        /// <returns>A List of Sites</returns>
        public List<Site> GetAllSitesInCampground(Campground campground)
        {
            var sitelist = new List<Site>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sqlCommand = "SELECT * FROM site " +
                                   $"INNER JOIN campground ON campground.campground_id = site.campground_id AND campground.campground_id = {campground.CampgroundID}" +
                                   $"INNER JOIN park ON park.park_id = campground.park_id AND park.park_id = {campground.ParkID};";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlCommand, conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sitelist.Add(GetSiteFromReader(reader));
                }
            }
            return sitelist;
        }
        /// <summary>
        /// Saves given instance of type Reservation to the database in the 'reservation' table
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns>Reservation ID (should be primary key of the reservation table)</returns>
        public int SaveReservation(Reservation reservation)
        {
            reservation.CreateDate = DateTime.Now;
            int index = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sqlInsert = "INSERT INTO reservation (site_id, name, from_date, to_date, create_date) " +
                                   "VALUES (@SiteID, @Name, CAST(@FromDate AS DATE) , CAST(@ToDate AS DATE), CAST(@CreateDate AS DATETIME));";

                var cmd = new SqlCommand(sqlInsert + _getlastIDSQL, conn);

                cmd.Parameters.AddWithValue("@SiteId", reservation.SiteID);
                cmd.Parameters.AddWithValue("@Name", reservation.Name);
                cmd.Parameters.AddWithValue("@FromDate", reservation.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", reservation.ToDate);
                cmd.Parameters.AddWithValue("@CreateDate", reservation.CreateDate);

                index = (int)cmd.ExecuteScalar();
            }
            return index;
        }
        /// <summary>
        /// Reads the data from one row of the 'park' table and stores it in a Park object
        /// </summary>
        /// <param name="reader">SQL reader pointing to a park row in the database</param>
        /// <returns>A Park object with all the row data stored as properties</returns>
        private Park GetParkFromReader(SqlDataReader reader)
        {
            Park park = new Park();

            park.ParkID = Convert.ToInt32(reader["park_id"]);
            park.Name = Convert.ToString(reader["name"]);
            park.Location = Convert.ToString(reader["location"]);
            park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
            park.Area = Convert.ToInt32(reader["area"]);
            park.Visitors = Convert.ToInt32(reader["visitors"]);
            park.Description = Convert.ToString(reader["description"]);

            return park;
        }
        /// <summary>
        /// Reads the data from one row of the 'campground' table and stores it in a Campground object
        /// </summary>
        /// <param name="reader">SQL reader pointing to a campground row in the database</param>
        /// <returns>A Campground object with all the row data stored as properties</returns>
        private Campground GetCampgroundFromReader(SqlDataReader reader)
        {
            Campground campground = new Campground();

            campground.CampgroundID = Convert.ToInt32(reader["campground_id"]);
            campground.ParkID = Convert.ToInt32(reader["park_id"]);
            campground.Name = Convert.ToString(reader["name"]);
            campground.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
            campground.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
            campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);
            return campground;
        }
        /// <summary>
        /// Reads the data from one row of the 'reservation' table and stores it in a Reservation object
        /// </summary>
        /// <param name="reader">SQL reader pointing to a reservation row in the database</param>
        /// <returns>A Reservation object with all the row data stored as properties</returns>
        private Reservation GetReservationFromReader(SqlDataReader reader)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationID = Convert.ToInt32(reader["reservation_id"]);
            reservation.SiteID = Convert.ToInt32(reader["site_id"]);
            reservation.Name = Convert.ToString(reader["name"]);
            reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
            reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
            reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);
            return reservation;
        }
        /// <summary>
        /// Reads the data from one row of the 'site' table and stores it in a Site object
        /// </summary>
        /// <param name="reader">SQL reader pointing to a site row in the database</param>
        /// <returns>A Site object with all the row data stored as properties</returns>
        private Site GetSiteFromReader(SqlDataReader reader)
        {
            Site site = new Site();

            site.SiteID = Convert.ToInt32(reader["site_id"]);
            site.CampgroundID = Convert.ToInt32(reader["campground_id"]);
            site.SiteNumber = Convert.ToInt32(reader["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
            site.Accessible = Convert.ToBoolean(reader["accessible"]);
            site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
            site.Utilities = Convert.ToBoolean(reader["utilities"]);
            return site;
        }
    }
}
