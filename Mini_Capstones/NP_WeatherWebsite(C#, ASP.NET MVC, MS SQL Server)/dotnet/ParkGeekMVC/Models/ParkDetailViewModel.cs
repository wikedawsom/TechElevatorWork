
using ParkGeek.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkGeekMVC.Models
{
    public class ParkDetailViewModel
    {
        public string ParkCode { get; set; }

        public ParkModel Park { get; set; }

        public IList<WeatherModel> Forecast { get; set; }

        public string weatherWarning(int index)
        {
            string output = "";

            if (Forecast[index].HighTemp >= 75)
                output += "WARNING! Temperatures may be high. Bring an extra gallon of water. \n";
            if (Forecast[index].LowTemp <= 20)
                output += "DANGER! Long exposure to frigid temperatures can result in permanent bodily damage and/or frostbite. Dress accordingly. \n" ;
            if ((Forecast[index].HighTemp - Forecast[index].LowTemp) > 20)
                output += "NOTE: There could be a higher gap in temperature. Be sure to wear breathable clothing. \n" ;
            if (Forecast[index].Forecast == "rain")
                output += "Rain expected in the forecast. Pack rain gear and wear rainproof footwear. \n" ;
            if (Forecast[index].Forecast == "snow")
                output += "Snow expected in the forecast. Be sure to wear snowshoes. \n" ;
            if (Forecast[index].Forecast == "thunderstorms")
                output += "Thunderstorms expected in the forecast. Seek shelter and avoid hiking on exposed ridges. \n" ;
            if (Forecast[index].Forecast == "sunny")
                output += "Sun expected in the forecast. Be sure to pack sunscreen. \n" ;


            return output;
        }
     
        
    }


}
