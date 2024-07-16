using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Models.ViewModels
{
    public class ApprovalRequestViewModel
    {
        public int Id { get; set; }

        public string Approver { get; set; } = null!;

		public int LeaveRequest { get; set; }

        public string Status { get; set; } = null!;

        public string? Comment { get; set; }
		public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "New", "Approved", "Rejected" });
	}
}
