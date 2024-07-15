using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models;
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
        public Projects(ApplicationDbContext context)
		{
			_repository = new RepositoryService<Project>(context);
            _ProjectAndEmployeeRepository = new RepositoryService<ProjectsAndEmployee>(context);

        }
		// GET: Projects
		public ActionResult Index()
		{
			return View();
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
