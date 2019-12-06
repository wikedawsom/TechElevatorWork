using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkGeek;
using ParkGeek.Business_Logic;
using ParkGeekMVC.Models;
using Security.DAO;

namespace ParkGeekMVC.Controllers
{
    public class ParkController : AuthenticationController
    {
        private IParkGeekDAO _db;
        public ParkController(IParkGeekDAO db, IUserSecurityDAO sec_db, IHttpContextAccessor httpContext) : base(sec_db, httpContext)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var parkList = _db.GetAllParks();
            return GetAuthenticatedView("Index", parkList);
        }

        public IActionResult ParkDetail(string parkCode)
        {
            var vm = MakeDetailViewModel(parkCode, GetSessionData<bool>("UseCelsius"));
            return GetAuthenticatedView("ParkDetail", vm);
        }

        [HttpGet]
        public IActionResult UseCelsius(ParkDetailViewModel form)
        {
            SetSessionData("UseCelsius", true);
            var vm = MakeDetailViewModel(form.ParkCode, GetSessionData<bool>("UseCelsius"));
            return GetAuthenticatedView("ParkDetail", vm);
        }

        [HttpGet]
        public IActionResult UseFarenheit(ParkDetailViewModel form)
        {
            SetSessionData("UseCelsius", false);
            var vm = MakeDetailViewModel(form.ParkCode, GetSessionData<bool>("UseCelsius"));
            return GetAuthenticatedView("ParkDetail", vm);
        }

        private ParkDetailViewModel MakeDetailViewModel(string parkCode, bool useCelsius)
        {
            var vm = new ParkDetailViewModel();
            var park = _db.GetParkByCode(parkCode);
            vm.Park = park;
            vm.ParkCode = parkCode;
            vm.Forecast = _db.GetWeatherByParkCode(parkCode);
            if (useCelsius == true)
            {
                var converter = new TempConverter();
                foreach (var forecast in vm.Forecast)
                {
                    forecast.HighTemp = converter.FarenheitToCelsuis(forecast.HighTemp);
                    forecast.LowTemp = converter.FarenheitToCelsuis(forecast.LowTemp);
                }
            }
            return vm;
        }

    }
}
