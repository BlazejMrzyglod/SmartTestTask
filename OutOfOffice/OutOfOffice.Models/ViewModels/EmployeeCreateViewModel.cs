using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Models.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public string Position { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string PeoplePartner { get; set; } = null!;

        public int OutOfOfficeBalance { get; set; }

        public IFormFile? Photo { get; set; }

        public static SelectList Positions { get; set; } = new SelectList(new List<string> { "HR Manager", "Developer", "Project Manager", "Administrator" });
        public static SelectList Subdivisions { get; set; } = new SelectList(new List<string> { "HR", "Development", "Management", "Administration" });
        public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "Active", "Inactive" });
        public static SelectList? HRManagers { get; set; }
    }
}
