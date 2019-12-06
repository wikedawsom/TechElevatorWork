using ParkGeek.DAL.Models;
using System;
using System.Collections.Generic;

namespace ParkGeek
{
    public interface IParkGeekDAO
    {
        IList<ParkModel> GetAllParks();
        ParkModel GetParkByCode(string parkCode);
        IList<WeatherModel> GetWeatherByParkCode(string parkCode);
        IList<Survey> GetAllSurveys();
        int SaveNewSurvey(Survey survey);
        IList<SurveyResult> GetSurveyResults();
    }
}
