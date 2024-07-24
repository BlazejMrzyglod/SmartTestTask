using Microsoft.AspNetCore.Mvc.Rendering;

namespace OutOfOffice.Models.ViewModels
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }

        public string Employee { get; set; } = null!;

        public string AbscenceReason { get; set; } = null!;
        public static SelectList AbscenceReasonOptions { get; set; } = new SelectList(new List<string> { "Sickness", "Vacation", "Family Emergency", "Other" });

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string? Comment { get; set; }

        public string Status { get; set; } = null!;
        public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "New", "Approved", "Rejected", "Canceled", "Submitted" });

    }
}
