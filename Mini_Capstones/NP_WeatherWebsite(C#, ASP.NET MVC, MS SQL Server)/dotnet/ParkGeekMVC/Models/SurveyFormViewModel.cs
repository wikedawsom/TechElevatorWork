using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkGeekMVC.Models
{
    public class SurveyFormViewModel
    {
        [Required]
        public string ParkCode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ActivityLevel { get; set; }

        public List<SelectListItem> AllParkCodes { get; set; } = new List<SelectListItem>();
    }
}
