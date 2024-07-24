namespace OutOfOffice.Models.Models;

public partial class ApprovalRequest : IEntity<int>
{
    public int Id { get; set; }

    public int Approver { get; set; }

    public int LeaveRequest { get; set; }

    public string Status { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual Employee ApproverNavigation { get; set; } = null!;

    public virtual LeaveRequest LeaveRequestNavigation { get; set; } = null!;
}
