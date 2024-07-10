namespace OutOfOffice.Models;

public partial class LeaveRequest : IEntity<int>
{
    public int Id { get; set; }

    public int Employee { get; set; }

    public string AbscenceReason { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();

    public virtual Employee EmployeeNavigation { get; set; } = null!;
}
