using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name="Name")]
        public string FullName { get; set; } = null!;

        public string Subdivision { get; set; } = null!;

        public string Position { get; set; } = null!;

        public string Status { get; set; } = null!;

        [Display(Name = "Partner")]
        public string PeoplePartner { get; set; } = null!;

        [Display(Name = "Days off left")]
        public int OutOfOfficeBalance { get; set; }

        public byte[]? Photo { get; set; }

        public static SelectList Positions { get; set; } = new SelectList(new List<string> { "HR Manager", "Developer", "Project Manager", "Administrator" });
        public static SelectList Subdivisions { get; set; } = new SelectList(new List<string> { "HR", "Development", "Management", "Administration" });
        public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "Active", "Inactive" });
        public static SelectList? HRManagers { get; set; }
    }
}
