using System;
using System.Collections.Generic;
using System.Text;

namespace ParkGeek.DAL.Models
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string ParkCode { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string ActivityLevel { get; set; }
    }
}
