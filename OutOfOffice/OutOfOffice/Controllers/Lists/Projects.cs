using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;
using System.Collections.Generic;

namespace OutOfOffice.Controllers.Lists
{
	public class Projects : Controller
	{
		private readonly IRepositoryService<Project> _repository;
        private readonly IRepositoryService<ProjectsAndEmployee> _ProjectAndEmployeeRepository;
		private readonly IMapper _mapper;

		public Projects(ApplicationDbContext context, IMapper mapper)
		{
			_repository = new RepositoryService<Project>(context);
            _ProjectAndEmployeeRepository = new RepositoryService<ProjectsAndEmployee>(context);
			_mapper = mapper;

        }
		// GET: Projects
		public ActionResult Index(string sortOrder, int searchString, string typeFilter, int managerFilter, string statusFilter)
		{
			ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
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

			if (!String.IsNullOrEmpty(typeFilter))
			{
				projects = projects.Where(s => s.ProjectType.Equals(typeFilter));
			}

			if (managerFilter!=0)
			{
				projects = projects.Where(s => s.ProjectManager.Equals(managerFilter));
			}

			if (!String.IsNullOrEmpty(statusFilter))
			{
				projects = projects.Where(s => s.Status.Equals(statusFilter));
			}

			switch (sortOrder)
			{
				case "id_desc":
					projects = projects.OrderByDescending(e => e.Id);
					break;
				case "Type":
					projects = projects.OrderBy(e => e.ProjectType);
					break;
				case "type_desc":
					projects = projects.OrderByDescending(e => e.ProjectType);
					break;
				case "Manager":
					projects = projects.OrderBy(e => e.ProjectManager);
					break;
				case "manager_desc":
					projects = projects.OrderByDescending(e => e.ProjectManager);
					break;
				case "StartDate":
					projects = projects.OrderBy(e => e.StartDate);
					break;
				case "startdate_desc":
					projects = projects.OrderByDescending(e => e.StartDate);
					break;
				case "EndDate":
					projects = projects.OrderBy(e => e.EndDate);
					break;
				case "enddate_desc":
					projects = projects.OrderByDescending(e => e.EndDate);
					break;
				case "Status":
					projects = projects.OrderBy(e => e.Status);
					break;
				case "status_desc":
					projects = projects.OrderByDescending(e => e.Status);
					break;
				default:
					projects = projects.OrderBy(e => e.Id);
					break;
			}
			List<ProjectViewModel> projectsViewModels = new();

			foreach (var project in projects)
			{
				projectsViewModels.Add(_mapper.Map<ProjectViewModel>(project));
			}
			return View(projectsViewModels);
		}

		// GET: Projects/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Projects/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Projects/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: Projects/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Projects/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}


		// GET: Projects/AssignEmployee/5
		public ActionResult AssignEmployee(int id)
		{
			List<int> projects = _repository.GetAllRecords().Where(e => !e.ProjectsAndEmployees.Any(p => p.EmployeeId == id)).Select(e=>e.Id).ToList();
			return View(projects);
		}

		// POST: Projects/AssignEmployee/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AssignEmployee(int id, int projectId)
		{
			try
			{
				_ProjectAndEmployeeRepository.Add(new ProjectsAndEmployee() { EmployeeId = id, ProjectId = projectId });
                return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
