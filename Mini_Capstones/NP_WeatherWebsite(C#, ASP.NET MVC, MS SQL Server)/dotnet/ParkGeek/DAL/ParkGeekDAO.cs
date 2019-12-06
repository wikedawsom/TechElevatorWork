using ParkGeek.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ParkGeek.DAL
{
    public class ParkGeekDAO : IParkGeekDAO
    {
        private string _connectionString;

        public ParkGeekDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<ParkModel> GetAllParks()
        {
            IList<ParkModel> parkList = new List<ParkModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Fill List
                string sql = "SELECT * FROM park;";
                SqlCommand cmd = new SqlCommand(sql, conn);

                var reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    parkList.Add(GetParkFromReader(reader));
                }
            }

            return parkList;
        }

        /// <summary>
        /// Returns a specific park, identified by code
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns></returns>
        public ParkModel GetParkByCode(string parkCode)
        {

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Fill List
                string sql = "SELECT * FROM park " +
                             "WHERE parkCode = @parkCode;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                var reader = cmd.ExecuteReader();
                reader.Read();

                return GetParkFromReader(reader);                
            }
        }

        /// <summary>
        /// Uses parkCode to find all the forecast weather data associated with the park
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns></returns>
        public IList<WeatherModel> GetWeatherByParkCode(string parkCode)
        {
            IList<WeatherModel> forecast = new List<WeatherModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Fill List
                string sql = "SELECT * FROM weather " +
                             "WHERE parkCode = @parkCode " +
                             "ORDER BY fiveDayForecastValue ASC;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                var reader = cmd.ExecuteReader();
               
                while (reader.Read())
                {
                    forecast.Add(GetWeatherFromReader(reader));
                }
            }
            return forecast;
        }

        /// <summary>
        /// Returns a List which contains the data from all rows of the survey_result table in the database
        /// </summary>
        /// <returns></returns>
        public IList<Survey> GetAllSurveys()
        {
            IList<Survey> list = new List<Survey>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Fill List
                string sql = "SELECT * FROM survey_result;";
                SqlCommand cmd = new SqlCommand(sql, conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Survey
                    {
                        ActivityLevel = Convert.ToString(reader["activityLevel"]),
                        ParkCode = Convert.ToString(reader["parkCode"]),
                        Email = Convert.ToString(reader["emailAddress"]),
                        State = Convert.ToString(reader["state"]),
                        SurveyId = Convert.ToInt32(reader["surveyId"])
                    });
                }
            }
            return list;
        }

        public IList<SurveyResult> GetSurveyResults()
        {
            IList<SurveyResult> resultList = new List<SurveyResult>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                // Fill List
                string sql = "SELECT park.parkCode, parkName, COUNT(*) AS 'votes' from park " +
                             "JOIN survey_result ON park.parkCode = survey_result.parkCode " +
                             "GROUP BY parkName, park.parkCode " +
                             "ORDER BY votes DESC;";
                SqlCommand cmd = new SqlCommand(sql, conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    resultList.Add(GetResultFromReader(reader));
                }
            }
            return resultList;
        }
        
        private SurveyResult GetResultFromReader(SqlDataReader reader)
        {
            return new SurveyResult
            {
                ParkCode = Convert.ToString(reader["parkCode"]),
                ParkName = Convert.ToString(reader["parkName"]),
                Votes = Convert.ToInt32(reader["votes"]),
            };
        }

        /// <summary>
        /// Saves a survey to the database and returns its ID
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public int SaveNewSurvey(Survey survey)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sqlInsert = "INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) " +
                                   "VALUES (@parkCode, @emailAddress, @state, @activityLevel); " +
                                   "SELECT CAST(SCOPE_IDENTITY() AS INT); ";
                SqlCommand cmd = new SqlCommand(sqlInsert, conn);
                cmd.Parameters.AddWithValue("@parkCode", survey.ParkCode);
                cmd.Parameters.AddWithValue("@emailAddress", survey.Email);
                cmd.Parameters.AddWithValue("@state", survey.State);
                cmd.Parameters.AddWithValue("@activityLevel", survey.ActivityLevel);

                return (int)cmd.ExecuteScalar();
            }
        }

        private ParkModel GetParkFromReader(SqlDataReader reader)
        {
            var output = new ParkModel();

            output.Code = Convert.ToString(reader["parkCode"]);
            output.Name = Convert.ToString(reader["parkName"]);
            output.State = Convert.ToString(reader["state"]);
            output.Acres = Convert.ToInt64(reader["acreage"]);
            output.Elevation = Convert.ToInt32(reader["elevationInFeet"]);
            output.TrailMiles = Convert.ToInt32(reader["milesOfTrail"]);
            output.NumCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
            output.Climate = Convert.ToString(reader["climate"]);
            output.YearFounded = Convert.ToInt32(reader["yearFounded"]);
            output.AnnualVisitors = Convert.ToInt64(reader["annualVisitorCount"]);
            output.Quote = Convert.ToString(reader["inspirationalQuote"]);
            output.QuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
            output.Description = Convert.ToString(reader["parkDescription"]);
            output.EntryFee = Convert.ToDecimal(reader["entryFee"]);
            output.NumAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);

            return output;
        }

        private WeatherModel GetWeatherFromReader(SqlDataReader reader)
        {
            var output = new WeatherModel();

            output.ParkCode = Convert.ToString(reader["parkCode"]);
            output.Day = Convert.ToInt32(reader["fiveDayForecastValue"]);
            output.LowTemp = Convert.ToInt32(reader["low"]);
            output.HighTemp = Convert.ToInt32(reader["high"]);
            output.Forecast = Convert.ToString(reader["forecast"]);

            return output;
        }
    }
}
