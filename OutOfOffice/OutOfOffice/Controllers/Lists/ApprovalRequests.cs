using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;

namespace OutOfOffice.Controllers.Lists
{
	public class ApprovalRequests : Controller
	{
		private readonly IRepositoryService<ApprovalRequest> _repository;
		private readonly IRepositoryService<LeaveRequest> _leaveRequestsRepository;
		private readonly IRepositoryService<Employee> _employeesRepository;
		private readonly IMapper _mapper;

		public ApprovalRequests(ApplicationDbContext context, IMapper mapper)
		{
			_repository = new RepositoryService<ApprovalRequest>(context);
			_leaveRequestsRepository = new RepositoryService<LeaveRequest>(context);
			_employeesRepository = new RepositoryService<Employee>(context);
			_mapper = mapper;
		}
		// GET: ApprovalRequests
		public async Task<IActionResult> Index(string sortOrder, int searchString, string approverFilter, int requestFilter, string statusFilter)
		{
			ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
			ViewData["ApproverSortParm"] = sortOrder == "Approver" ? "approver_desc" : "Approver";
			ViewData["RequestSortParm"] = sortOrder == "Request" ? "request_desc" : "Request";
			ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
			ViewData["NumberFilter"] = searchString;
			ViewData["ApproverFilter"] = approverFilter;
			ViewData["RequestFilter"] = requestFilter;
			ViewData["StatusFilter"] = statusFilter;


			IQueryable<ApprovalRequest> approvalRequests = _repository.GetAllRecords()
				.Include(e => e.ApproverNavigation);

			if (searchString != 0)
			{
				approvalRequests = approvalRequests.Where(s => s.Id.Equals(searchString));
			}

			if (!String.IsNullOrEmpty(approverFilter))
			{
				approvalRequests = approvalRequests.Where(s => s.ApproverNavigation.FullName.Contains(approverFilter));
			}

			if (requestFilter != 0)
			{
				approvalRequests = approvalRequests.Where(s => s.LeaveRequest.Equals(requestFilter));
			}

			if (!String.IsNullOrEmpty(statusFilter))
			{
				approvalRequests = approvalRequests.Where(s => s.Status.Equals(statusFilter));
			}

			List<ApprovalRequestViewModel> viewModels = new();

			switch (sortOrder)
			{
				case "id_desc":
					approvalRequests = approvalRequests.OrderByDescending(e => e.Id);
					break;
				case "Approver":
					approvalRequests = approvalRequests.OrderBy(e => e.Approver);
					break;
				case "approver_desc":
					approvalRequests = approvalRequests.OrderByDescending(e => e.Approver);
					break;
				case "Request":
					approvalRequests = approvalRequests.OrderBy(e => e.LeaveRequest);
					break;
				case "request_desc":
					approvalRequests = approvalRequests.OrderByDescending(e => e.LeaveRequest);
					break;
				case "Status":
					approvalRequests = approvalRequests.OrderBy(e => e.Status);
					break;
				case "status_desc":
					approvalRequests = approvalRequests.OrderByDescending(e => e.Status);
					break;
				default:
					approvalRequests = approvalRequests.OrderBy(e => e.Id);
					break;
			}
			foreach (var request in approvalRequests)
			{
				viewModels.Add(_mapper.Map<ApprovalRequestViewModel>(request));
			}

			return View(viewModels);
		}

		// GET: ApprovalRequests/Details/5
		public ActionResult Details(int id)
		{
			ApprovalRequest approvalRequest = _repository.GetAllRecords()
														 .Include(e => e.ApproverNavigation).Where(e => e.Id == id).Single();
			return View(_mapper.Map<ApprovalRequestViewModel>(approvalRequest));
		}
		
		// GET: ApprovalRequests/Details/5
		public ActionResult Approve(int id)
		{

			ApprovalRequest approvalRequest = _repository.GetSingle(id);
			if (approvalRequest.Status != "Approved")
			{
				approvalRequest.Status = "Approved";
				_repository.Edit(approvalRequest);
				_repository.Save();

				LeaveRequest leaveRequest = _leaveRequestsRepository.GetSingle(approvalRequest.LeaveRequest);
				leaveRequest.Status = "Approved";
				_leaveRequestsRepository.Edit(leaveRequest);
				_leaveRequestsRepository.Save();

				Employee employee = _employeesRepository.GetSingle(leaveRequest.Employee);
				employee.OutOfOfficeBalance -= (leaveRequest.EndDate.Day - leaveRequest.StartDate.Day);
				_employeesRepository.Edit(employee);
				_employeesRepository.Save();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}