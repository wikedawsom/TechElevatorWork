using System;
using System.Collections.Generic;
using System.Text;

namespace ParkGeek.Business_Logic
{
    public class TempConverter
    {
        //temps are in F by default. Converter needs to change them to C.
        public int FarenheitToCelsuis(int degreesF)
        {
            return (int)((degreesF - 32)*(5.0/9));
        }
    }
}
