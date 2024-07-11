using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice.Models;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;
using System.Linq;

namespace OutOfOffice.Controllers.Lists
{
	public class Employees : Controller
	{
		private readonly IRepositoryService<Employee> _repository;

		public Employees(ApplicationDbContext context)
		{
			_repository = new RepositoryService<Employee>(context);
		}

		// GET: Employees
		public ActionResult Index()
		{
			IQueryable<Employee> employees = _repository.GetAllRecords();
			return View(employees);
		}

		// GET: Employees/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: Employees/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Employees/Create
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

		// GET: Employees/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: Employees/Edit/5
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

		// GET: Employees/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Employees/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
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
	}
}
