using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OutOfOffice.Controllers.Lists
{
	public class LeaveRequests : Controller
	{
		// GET: LeaveRequests
		public ActionResult Index()
		{
			return View();
		}

		// GET: LeaveRequests/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: LeaveRequests/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: LeaveRequests/Create
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

		// GET: LeaveRequests/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: LeaveRequests/Edit/5
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

		// GET: LeaveRequests/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: LeaveRequests/Delete/5
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
