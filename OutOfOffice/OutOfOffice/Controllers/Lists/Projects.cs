using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;

namespace OutOfOffice.Controllers.Lists
{
    [Authorize(Roles = "Administrator, HR_Manager, ProjectManager")]
    public class Projects(ApplicationDbContext context, IMapper mapper) : Controller
    {
        private readonly RepositoryService<Project> _repository = new(context);
        private readonly RepositoryService<ProjectsAndEmployee> _ProjectAndEmployeeRepository = new(context);
        private readonly IMapper _mapper = mapper;

        // GET: Projects
        public ActionResult Index(string sortOrder, int searchString, string typeFilter, int managerFilter, string statusFilter)
        {
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "type_desc" : "Type";
            ViewData["ManagerSortParm"] = sortOrder == "Manager" ? "manager_desc" : "Manager";
            ViewData["StartDateSortParm"] = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
            ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["NumberFilter"] = searchString;
            ViewData["TypeFilter"] = typeFilter;
            ViewData["ManagerFilter"] = managerFilter;
            ViewData["StatusFilter"] = statusFilter;

            IQueryable<Project> projects = _repository.GetAllRecords();

            if (searchString != 0)
            {
                projects = projects.Where(s => s.Id.Equals(searchString));
            }

            if (!string.IsNullOrEmpty(typeFilter))
            {
                projects = projects.Where(s => s.ProjectType.Equals(typeFilter));
            }

            if (managerFilter != 0)
            {
                projects = projects.Where(s => s.ProjectManager.Equals(managerFilter));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                projects = projects.Where(s => s.Status.Equals(statusFilter));
            }

            projects = sortOrder switch
            {
                "id_desc" => projects.OrderByDescending(e => e.Id),
                "Type" => projects.OrderBy(e => e.ProjectType),
                "type_desc" => projects.OrderByDescending(e => e.ProjectType),
                "Manager" => projects.OrderBy(e => e.ProjectManager),
                "manager_desc" => projects.OrderByDescending(e => e.ProjectManager),
                "StartDate" => projects.OrderBy(e => e.StartDate),
                "startdate_desc" => projects.OrderByDescending(e => e.StartDate),
                "EndDate" => projects.OrderBy(e => e.EndDate),
                "enddate_desc" => projects.OrderByDescending(e => e.EndDate),
                "Status" => projects.OrderBy(e => e.Status),
                "status_desc" => projects.OrderByDescending(e => e.Status),
                _ => projects.OrderBy(e => e.Id),
            };
            List<ProjectViewModel> projectsViewModels = [];

            foreach (Project project in projects)
            {
                projectsViewModels.Add(_mapper.Map<ProjectViewModel>(project));
            }
            return View(projectsViewModels);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int id)
        {
            return View(_mapper.Map<ProjectViewModel>(_repository.GetSingle(id)));
        }

        // GET: Projects/Create
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Create([Bind("ProjectType,StartDate,EndDate,ProjectManager,Comment,Status")] ProjectViewModel project)
        {
            try
            {
                _ = _repository.Add(_mapper.Map<Project>(project));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Edit(int id)
        {
            return View(_mapper.Map<ProjectViewModel>(_repository.GetSingle(id)));
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Edit(int id, [Bind("ProjectType,StartDate,EndDate,ProjectManager,Comment,Status")] ProjectViewModel project)
        {
            try
            {
                _ = _repository.Edit(_mapper.Map<Project>(project));
                _ = _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: Projects/AssignEmployee/5
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult AssignEmployee(int id)
        {
            List<int> projects = [.. _repository.GetAllRecords().Where(e => !e.ProjectsAndEmployees.Any(p => p.EmployeeId == id)).Select(e => e.Id)];
            return View(projects);
        }

        // POST: Projects/AssignEmployee/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult AssignEmployee(int id, int projectId)
        {
            try
            {
                _ = _ProjectAndEmployeeRepository.Add(new ProjectsAndEmployee() { EmployeeId = id, ProjectId = projectId });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Projects/Deactivate/5
        [Authorize(Roles = "Administrator, ProjectManager")]
        public ActionResult Deactivate(int id)
        {
            Project? project = _repository.GetSingle(id);
            if(project != null)
            if (project.Status != "Inactive")
            {
                project.Status = "Inactive";
                _ = _repository.Edit(project);
                _ = _repository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
