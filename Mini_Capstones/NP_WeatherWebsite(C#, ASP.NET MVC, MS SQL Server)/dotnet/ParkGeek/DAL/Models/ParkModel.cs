using System;
using System.Collections.Generic;
using System.Text;

namespace ParkGeek.DAL.Models
{
    public class ParkModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public long Acres { get; set; }
        public int Elevation { get; set; }
        public int TrailMiles { get; set; }
        public int NumCampsites { get; set; }
        public string Climate { get; set; }
        public int YearFounded { get; set; }
        public long AnnualVisitors { get; set; }
        public string Quote { get; set; }
        public string QuoteSource { get; set; }
        public string Description { get; set; }
        public decimal EntryFee { get; set; }
        public int NumAnimalSpecies { get; set; }
    }
}
