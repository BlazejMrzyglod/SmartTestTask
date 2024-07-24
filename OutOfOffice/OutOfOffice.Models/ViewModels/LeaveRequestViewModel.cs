using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models.ViewModels
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }

        public string Employee { get; set; } = null!;

        [Display(Name = "Abscence reason")]
        public string AbscenceReason { get; set; } = null!;
        public static SelectList AbscenceReasonOptions { get; set; } = new SelectList(new List<string> { "Sickness", "Vacation", "Family Emergency", "Other" });

        [Display(Name = "Start date")]
        public DateOnly StartDate { get; set; }

        [Display(Name = "End date")]
        public DateOnly EndDate { get; set; }

        public string? Comment { get; set; }

        public string Status { get; set; } = null!;
        public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "New", "Approved", "Rejected", "Canceled", "Submitted" });

    }
}
