using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkGeek;
using ParkGeek.DAL.Models;
using ParkGeekMVC.Models;
using Security.DAO;

namespace ParkGeekMVC.Controllers
{
    public class SurveyController : AuthenticationController
    {
        private IParkGeekDAO _db;
        public SurveyController(IParkGeekDAO parkDB, IUserSecurityDAO db, IHttpContextAccessor httpContext) : base(db, httpContext)
        {
            _db = parkDB;
        }

        public IActionResult SurveyResults()
        {
            IList<SurveyResult> surveyResultList = _db.GetSurveyResults();
            
            return GetAuthenticatedView("SurveyResults", surveyResultList);
        }

        [HttpGet]
        public IActionResult SurveyForm()
        {
            SurveyFormViewModel surveyVM = new SurveyFormViewModel();
            var parkList = _db.GetAllParks();
            surveyVM.AllParkCodes = new List<SelectListItem>();
            foreach (var park in parkList)
            {
                var item = new SelectListItem();
                item.Value = park.Code;
                item.Text = park.Name;
                surveyVM.AllParkCodes.Add(item);
            }
            return GetAuthenticatedView("SurveyForm", surveyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SurveyForm(SurveyFormViewModel model)
        {
            IActionResult result = RedirectToAction("SurveyResults");
            Survey survey = new Survey
            {
                ActivityLevel = model.ActivityLevel,
                Email = model.Email,
                ParkCode = model.ParkCode,
                State = model.State,
            };
            try
            {
                TempData["SurveyConfirmation"] = "Your response was added successfully";
                int id = _db.SaveNewSurvey(survey);
            }
            catch
            {
                TempData["SurveyConfirmation"] = "Failed to save survey response";
                SurveyFormViewModel surveyVM = new SurveyFormViewModel();
                var parkList = _db.GetAllParks();
                surveyVM.AllParkCodes = new List<SelectListItem>();
                foreach (var park in parkList)
                {
                    var item = new SelectListItem();
                    item.Value = park.Code;
                    item.Text = park.Name;
                    surveyVM.AllParkCodes.Add(item);
                }
                result = GetAuthenticatedView("SurveyForm", surveyVM);
            }
            return result;
        }
    }
}