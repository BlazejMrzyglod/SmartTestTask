using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOffice.Models.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.Models.ViewModels;

public partial class ProjectViewModel
{
    public int Id { get; set; }

    [Display(Name = "Type")]
    public string ProjectType { get; set; } = null!;

    [Display(Name = "Start date")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "End date")]
    public DateOnly? EndDate { get; set; }

    [Display(Name = "Project manager id")]
    public int ProjectManager { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ProjectsAndEmployee> ProjectsAndEmployees { get; set; } = new List<ProjectsAndEmployee>();
    public static SelectList ProjectTypeOptions { get; set; } = new SelectList(new List<string> { "Backend", "Frontend", "Database" });
    public static SelectList StatusOptions { get; set; } = new SelectList(new List<string> { "Active", "Inactive" });
    public static SelectList? ProjectManagers { get; set; }

}
