using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models.ViewModels
{
    public class ApprovalRequestViewModel
    {
        public int Id { get; set; }

        public string Approver { get; set; } = null!;

        [Display(Name = "Leave request id")]
        public int LeaveRequest { get; set; }

        public string Status { get; set; } = null!;

        public string? Comment { get; set; }
        public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "New", "Approved", "Rejected" });
    }
}
