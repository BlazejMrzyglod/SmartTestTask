namespace OutOfOffice.Models;

public partial class Employee : IEntity<int>
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Subdivision { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int PeoplePartner { get; set; }

    public int OutOfOfficeBalance { get; set; }

    public byte[]? Photo { get; set; }

    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();

    public virtual ICollection<Employee> InversePeoplePartnerNavigation { get; set; } = new List<Employee>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual Employee PeoplePartnerNavigation { get; set; } = null!;
	public virtual ApplicationUser ApplicationUser { get; set; } = null!;

	public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
