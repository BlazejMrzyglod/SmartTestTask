using Microsoft.AspNetCore.Identity;

namespace OutOfOffice.Models
{
	public class ApplicationUser : IdentityUser
	{
        public int EmployeeId { get; set; }
		public virtual Employee Employee { get; set; } = null!;
	}
}