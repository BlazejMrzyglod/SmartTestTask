namespace OutOfOffice.Models.Models;

public partial class Project : IEntity<int>
{
    public int Id { get; set; }

    public string ProjectType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int ProjectManager { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee ProjectManagerNavigation { get; set; } = null!;
    public virtual ICollection<ProjectsAndEmployee> ProjectsAndEmployees { get; set; } = new List<ProjectsAndEmployee>();
}
