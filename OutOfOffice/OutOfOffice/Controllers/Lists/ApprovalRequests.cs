using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OutOfOffice.Controllers.Lists
{
	public class ApprovalRequests : Controller
	{
		// GET: ApprovalRequests
		public ActionResult Index()
		{
			return View();
		}

		// GET: ApprovalRequests/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: ApprovalRequests/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: ApprovalRequests/Create
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

		// GET: ApprovalRequests/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ApprovalRequests/Edit/5
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

		// GET: ApprovalRequests/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ApprovalRequests/Delete/5
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
