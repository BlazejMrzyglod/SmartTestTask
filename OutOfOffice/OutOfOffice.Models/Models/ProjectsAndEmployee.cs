using System;
using System.Collections.Generic;

namespace OutOfOffice.Models;

public partial class ProjectsAndEmployee
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
