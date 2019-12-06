using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Capstone.DAL;
using Capstone.Models;
using System.Threading;
using System.Globalization;

namespace NPDBCommandLineInterface
{
    public class ParkMenu
    {
        private ParkDBDAL dal;
        private string MonthName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        /// <summary>
        /// Introduction screen of program
        /// </summary>
        /// <param name="connectionString">JSON file settings to connect to SQL DB</param>
        public void MainMenu(string connectionString)
        {
            //Console.ForegroundColor = ConsoleColor.Cyan;
            dal = new ParkDBDAL(connectionString);
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("National Park CampGround Reservation System");
                Console.WriteLine("\n1) Login");
                //Option 2 not implemented
                //Console.WriteLine("2) Register");
                Console.WriteLine("0) Quit");
                int userChoice = GetInteger("\nPlease Choose Menu Item:");

                if (userChoice == 1)
                {
                    // Loads List of Parks
                    ParkListMenu();
                }
                else if (userChoice == 2)
                {
                    //Do something
                    //Not implemented currently
                }
                else if (userChoice == 0)
                {
                    //End program
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Presents list of all parks from SQL DB for user selection
        /// </summary>
        /// <param name="">No arguments for method</param>
        private void ParkListMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                var parks = dal.GetParkList();
                int counter = 0;

                Console.WriteLine("Please Choose a Park\n");

                foreach (Park park in parks)

                {
                    counter++;
                    Console.WriteLine($"{counter}) {park.Name}");
                }
                Console.WriteLine($"\nPress 0 to Logout");
                int userInput = int.Parse(Console.ReadLine());

                try
                {
                    if (userInput == 0)
                    {
                        exit = true;
                    }
                    else
                    {
                        int parkNum = userInput;
                        SingleParkMenu(parks[parkNum - 1]);
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine("\nWrong.");
                    //Console.WriteLine(ex.Message);
                    //Console.WriteLine(ex.StackTrace);
                    Console.ReadKey();
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Shows info for selected park from SQL DB
        /// </summary>
        /// <param name="park">The current selected park</param>
        private void SingleParkMenu(Park park)
        {
            bool exit = false;
            while(!exit)
            {
                Console.Clear();
                ParkDisplay(park);
                Console.WriteLine();
                Console.WriteLine("1) View Campgrounds");
                //Option 2 not implemented
                //Console.WriteLine("2) Search for Reservation");
                Console.WriteLine("0) Return to Last Screen");

                int userChoice = GetInteger("\nSelect an option number: ");

                if (userChoice == 1)
                {
                    CampGroundMenu(park);
                }
                //else if (userChoice == 2)
                //{
                //    Loads reservation sub-sub-menu for whole park
                //    Extra
                //}
                else if (userChoice == 0)
                {
                    exit = true;
                }
            }
        }
        /// <summary>
        /// Shows campgrounds list for given park from SQL DB
        /// </summary>
        /// <param name="park">The current selected park</param>
        private void CampGroundMenu(Park park)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                CampgroundDisplay(park);
                //string userInput = Console.ReadLine();
                //chosen park is: parks[parkNum - 1];

                Console.WriteLine("\n1) Search for Reservation");
                Console.WriteLine("0) Return to Last Screen");

                int userChoice = GetInteger("\nSelect an option number: ");

                if (userChoice == 1)
                {
                    ReservationScreen(park);
                }
                else if (userChoice == 0)
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Shows campgrounds list for given park with reservation cost and other options
        /// </summary>
        /// <param name="park">The current selected park</param>
        private void CampgroundDisplay(Park park)
        {
            Console.WriteLine("Search for Campground Reservation\n");
            Console.WriteLine("Name".PadRight(40) + "Open".PadRight(10) + "Close".PadRight(10) + "Daily Fee\n".PadRight(10));

            var camplist = dal.GetCampgroundList(park);
            int counter = 0;
            foreach (Campground campground in camplist)
            {
                counter++;
                Console.WriteLine($"{counter}){campground.Name}".PadRight(40) + 
                    $"{MonthName(campground.OpenFromMonth)}".PadRight(10) +
                    $"{MonthName(campground.OpenToMonth)}".PadRight(10) + $"{campground.DailyFee:C}".PadRight(10));
            }
        }

        /// <summary>
        /// Shows the park info screen on the Command Window
        /// </summary>
        /// <param name="park">Current park to display</param>
        private void ParkDisplay(Park park)
        {
            //Console.Clear();
            Console.WriteLine($"Park Information For:".PadRight(25) + $"{park.Name}");
            Console.WriteLine($"Location:".PadRight(25) + $"{park.Location}");
            Console.WriteLine($"Established:".PadRight(25) + $"{park.EstablishDate.ToShortDateString()}");
            Console.WriteLine($"Area:".PadRight(25) + $"{park.Area}");
            Console.WriteLine($"Annual Visitors:".PadRight(25) + $"{park.Visitors}");
            Console.WriteLine($"\n{park.Description}");
        }
        /// <summary>
        /// Standard method to force a user to enter an integer value
        /// </summary>
        /// <param name="message">Input prompt message</param>
        /// <returns>User Input as whole number</returns>
        private int GetInteger(string message)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }
                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;
        }
        /// <summary>
        /// Submenu prompts user to enter dates to search for sites available for reservation
        /// </summary>
        /// <param name="park">The current selected park</param>
        private void ReservationScreen(Park park)
        {
            bool exit = false;
            while (!exit)
            {

                var campgroundList = dal.GetCampgroundList(park);
                int campgroundChoice = GetInteger("Which campground (enter 0 to cancel)? ");
                if (campgroundChoice == 0)
                {
                    exit = true;
                }
                else
                {
                    try
                    {
                        //Console.Clear();
                        //CampGroundMenu(park);
                        var campground = campgroundList[campgroundChoice - 1];
                        List<Site> availableSites;

                        Console.WriteLine("What is the arrival date? MM/DD/YYYY");
                        DateTime arrival = Convert.ToDateTime(Console.ReadLine());

                        Console.WriteLine("What is the departure date? MM/DD/YYYY");
                        DateTime departure = Convert.ToDateTime(Console.ReadLine());

                        // Logic check on user date entry.
                        if (arrival >= departure)
                        {
                            throw new Exception("Please enter a valid date range. Arrival date should be before departure.");
                        }
                        else if(arrival <DateTime.Now)
                        {
                            throw new Exception("Please enter a valid date. Arrival date must be after today's date.");
                        }

                        availableSites = dal.SearchAvailableReservations(campground, arrival, departure);
                        decimal dailyFee = campground.DailyFee;

                        ChooseSite(availableSites, arrival, departure, dailyFee);
                        return;

                    }
                    catch (FormatException) // Only caught if date is entered in an invalid format
                    {
                        Console.WriteLine("Please Enter Date in MM/DD/YYYY Format.");
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Shows available sites for user to reserve, reservation date must be after today's date.
        /// After information is confirmed, reservation confirmation number and info presented to user
        /// </summary>
        /// <param name="List (sites)">list of sites to display to user</param>
        /// <param name="arrival">first arrival date as input by user</param>
        /// <param name="departure">last arrival date as input by user</param>
        /// <param name="dailyFee">site cost per day from SQL DB</param>
        private void ChooseSite(List<Site> sites, DateTime arrival, DateTime departure, decimal dailyFee)
        {
            int lengthOfStay = (departure - arrival).Days;
            decimal totalCost = (lengthOfStay * dailyFee);

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Site No.".PadRight(20) + "Max Occup.".PadRight(20) +
                    "Accessible".PadRight(20) + "RV Len".PadRight(20) + "Utility".PadRight(20) + "Cost");
                foreach (Site site in sites)
                {
                    string hasAccessible = site.Accessible ? "Yes" : "No";
                    string hasUtility = site.Utilities ? "Yes" : "No";
                    string showRVLength = site.MaxRVLength > 0 ? site.MaxRVLength.ToString() : "N/A";

                    Console.WriteLine($"{site.SiteNumber}".PadRight(20) + $"{site.MaxOccupancy}".PadRight(20) + 
                                        $"{hasAccessible}".PadRight(20) +
                                      $"{showRVLength}".PadRight(20) + $"{hasUtility}".PadRight(20) + $"{totalCost:C}");
                }

                Reservation res = new Reservation();
                try
                {
                    int siteNum = GetInteger("Which site should be reserved (or 0 to cancel)? ");
                    if (siteNum == 0)
                    {
                        exit = true;
                    }
                    else
                    {
                        bool siteExists = false;
                        Site chosenSite = null;
                        foreach(Site site in sites)
                        {
                            if(siteNum == site.SiteNumber)
                            {
                                siteExists = true;
                                chosenSite = site;
                            }
                        }
                        if (siteExists)
                        {

                            Console.Write("\nWhat name should the reservation be under? ");
                            string userName = Console.ReadLine();

                            res.Name = userName;
                            res.SiteID = chosenSite.SiteID;
                            res.FromDate = arrival;
                            res.ToDate = departure;

                            Console.WriteLine($"\nReservation ID: {dal.SaveReservation(res)} for {res.Name} at site {res.SiteID} from dates {res.FromDate.ToString().Substring(0, 10)}" +
                                              $" to {res.ToDate.ToString().Substring(0, 10)} ({lengthOfStay} days for {totalCost:C}).");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Please choose an available site from the list.");
                            Console.ReadKey();
                        }
                    }
                }
                catch (IndexOutOfRangeException) // Only caught if the selected campsite index is beyond the list size
                {
                    Console.WriteLine("Choose a number present in the campsite list.");
                }
            }
        }
    }
}
