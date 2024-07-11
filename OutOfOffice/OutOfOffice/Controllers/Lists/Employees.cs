using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
/*		public ActionResult Index()
		{
			IQueryable<Employee> employees = _repository.GetAllRecords();
			return View(employees);
		}*/

		public async Task<IActionResult> Index(string sortOrder)
		{
			ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewData["SubdivisionSortParm"] = sortOrder == "Subdivision" ? "subdivision_desc" : "Subdivision";
			ViewData["PositionSortParm"] = sortOrder == "Position" ? "position_desc" : "Position";
			ViewData["PartnerSortParm"] = sortOrder == "Partner" ? "partner_desc" : "Partner";
			ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "balance_desc" : "Balance";
			ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";

			IQueryable<Employee> employees = _repository.GetAllRecords();

			switch (sortOrder)
			{
				case "name_desc":
					employees = employees.OrderByDescending(e => e.FullName);
					break;
				case "Subdivision":
					employees = employees.OrderBy(e => e.Subdivision);
					break;
				case "subdivision_desc":
					employees = employees.OrderByDescending(e => e.Subdivision);
					break;
				case "Position":
					employees = employees.OrderBy(e => e.Position);
					break;
				case "position_desc":
					employees = employees.OrderByDescending(e => e.Position);
					break;
				case "Partner":
					employees = employees.OrderBy(e => e.PeoplePartner);
					break;
				case "partner_desc":
					employees = employees.OrderByDescending(e => e.PeoplePartner);
					break;
				case "Balance":
					employees = employees.OrderBy(e => e.OutOfOfficeBalance);
					break;
				case "balance_desc":
					employees = employees.OrderByDescending(e => e.OutOfOfficeBalance);
					break;
				case "Status":
					employees = employees.OrderBy(e => e.Status);
					break;
				case "status_desc":
					employees = employees.OrderByDescending(e => e.Status);
					break;
				default:
					employees = employees.OrderBy(e => e.FullName);
					break;
			}
			return View(await employees.AsNoTracking().ToListAsync());
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
