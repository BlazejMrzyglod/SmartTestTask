using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OutOfOffice.Controllers.Lists
{
	public class Projects : Controller
	{
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

		// GET: Projects/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: Projects/Delete/5
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
