using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOffice.Models.Models;

namespace OutOfOffice.Models.ViewModels;

public partial class ProjectViewModel
{
    public int Id { get; set; }

    public string ProjectType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int ProjectManager { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ProjectsAndEmployee> ProjectsAndEmployees { get; set; } = new List<ProjectsAndEmployee>();
    public static SelectList ProjectTypeOptions { get; set; } = new SelectList(new List<string> { "Backend", "Frontend", "Database" });
    public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "Active", "Inactive" });
    public static SelectList? ProjectManagers { get; set; }

}
