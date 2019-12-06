using ParkGeek.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkGeek.DAL.Models
{
    public class SurveyResult
    {
        public string ParkCode { get; set; }
        public string ParkName { get; set; }
        public int Votes { get; set; }
    }
}
