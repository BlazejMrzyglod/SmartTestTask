using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;
using System.Security.Claims;

namespace OutOfOffice.Controllers.Lists
{
	public class LeaveRequests : Controller
	{
		private readonly IRepositoryService<LeaveRequest> _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

		public LeaveRequests(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
		{
			_repository = new RepositoryService<LeaveRequest>(context);
			_mapper = mapper;
			_userManager = userManager;
		}

		// GET: LeaveRequests
		public ActionResult Index(string sortOrder, int searchString, string employeeFilter, string reasonFilter, string statusFilter)
		{
			ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
			ViewData["EmployeeSortParm"] = sortOrder == "Employee" ? "employee_desc" : "Employee";
			ViewData["ReasonSortParm"] = sortOrder == "Reason" ? "reason_desc" : "Reason";
			ViewData["StartDateSortParm"] = sortOrder == "StartDate" ? "startdate_desc" : "StartDate";
			ViewData["EndDateSortParm"] = sortOrder == "EndDate" ? "enddate_desc" : "EndDate";
			ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
			ViewData["NumberFilter"] = searchString;
			ViewData["EmployeeFilter"] = employeeFilter;
			ViewData["ReasonFilter"] = reasonFilter;
			ViewData["StatusFilter"] = statusFilter;


			IQueryable<LeaveRequest> leaveRequests = _repository.GetAllRecords().Include(e => e.EmployeeNavigation);

			if (searchString != 0)
			{
				leaveRequests = leaveRequests.Where(s => s.Id.Equals(searchString));
			}

			if (!String.IsNullOrEmpty(employeeFilter))
			{
				leaveRequests = leaveRequests.Where(s => s.EmployeeNavigation.FullName.Contains(employeeFilter));
			}

			if (!String.IsNullOrEmpty(reasonFilter))
			{
				leaveRequests = leaveRequests.Where(s => s.AbscenceReason.Equals(reasonFilter));
			}

			if (!String.IsNullOrEmpty(statusFilter))
			{
				leaveRequests = leaveRequests.Where(s => s.Status.Equals(statusFilter));
			}

			List<LeaveRequestViewModel> viewModels = new();

			switch (sortOrder)
			{
				case "id_desc":
					leaveRequests = leaveRequests.OrderByDescending(e => e.Id);
					break;
				case "Employee":
					leaveRequests = leaveRequests.OrderBy(e => e.EmployeeNavigation.FullName);
					break;
				case "employee_desc":
					leaveRequests = leaveRequests.OrderByDescending(e => e.EmployeeNavigation.FullName);
					break;
				case "Reason":
					leaveRequests = leaveRequests.OrderBy(e => e.AbscenceReason);
					break;
				case "reason_desc":
					leaveRequests = leaveRequests.OrderByDescending(e => e.AbscenceReason);
					break;
				case "StartDate":
					leaveRequests = leaveRequests.OrderBy(e => e.StartDate);
					break;
				case "startdate_desc":
					leaveRequests = leaveRequests.OrderByDescending(e => e.StartDate);
					break;
				case "EndDate":
					leaveRequests = leaveRequests.OrderBy(e => e.EndDate);
					break;
				case "enddate_desc":
					leaveRequests = leaveRequests.OrderByDescending(e => e.EndDate);
					break;
				case "Status":
					leaveRequests = leaveRequests.OrderBy(e => e.Status);
					break;
				case "status_desc":
					leaveRequests = leaveRequests.OrderByDescending(e => e.Status);
					break;
				default:
					leaveRequests = leaveRequests.OrderBy(e => e.Id);
					break;
			}
			foreach (var request in leaveRequests)
			{
				viewModels.Add(_mapper.Map<LeaveRequestViewModel>(request));
			}

			return View(viewModels);
		}

		// GET: LeaveRequests/Details/5
		public ActionResult Details(int id)
		{
			LeaveRequest leaveRequest = _repository.GetAllRecords()
														 .Include(e => e.EmployeeNavigation).Where(e => e.Id == id).Single();
			return View(_mapper.Map<LeaveRequestViewModel>(leaveRequest));
		}

		// GET: LeaveRequests/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: LeaveRequests/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("AbscenceReason,StartDate,EndDate,Comment,Status")] LeaveRequestViewModel _leaveRequest)
		{
			try
			{
				LeaveRequest leaveRequest = _mapper.Map<LeaveRequest>(_leaveRequest);
				ApplicationUser? _user = await _userManager.GetUserAsync(User);
				leaveRequest.Employee = _user!.EmployeeId;
				_repository.Add(leaveRequest);
				_repository.Save();
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
