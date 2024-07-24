using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;

namespace OutOfOffice.Controllers.Lists
{
    public class ApprovalRequests(ApplicationDbContext context, IMapper mapper) : Controller
    {
        private readonly RepositoryService<ApprovalRequest> _repository = new(context);
        private readonly RepositoryService<LeaveRequest> _leaveRequestsRepository = new(context);
        private readonly RepositoryService<Employee> _employeesRepository = new(context);
        private readonly IMapper _mapper = mapper;

        // GET: ApprovalRequests
        public IActionResult Index(string sortOrder, int searchString, string approverFilter, int requestFilter, string statusFilter)
        {
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
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

            if (!string.IsNullOrEmpty(approverFilter))
            {
                approvalRequests = approvalRequests.Where(s => s.ApproverNavigation.FullName.Contains(approverFilter));
            }

            if (requestFilter != 0)
            {
                approvalRequests = approvalRequests.Where(s => s.LeaveRequest.Equals(requestFilter));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                approvalRequests = approvalRequests.Where(s => s.Status.Equals(statusFilter));
            }

            List<ApprovalRequestViewModel> viewModels = [];

            approvalRequests = sortOrder switch
            {
                "id_desc" => approvalRequests.OrderByDescending(e => e.Id),
                "Approver" => approvalRequests.OrderBy(e => e.Approver),
                "approver_desc" => approvalRequests.OrderByDescending(e => e.Approver),
                "Request" => approvalRequests.OrderBy(e => e.LeaveRequest),
                "request_desc" => approvalRequests.OrderByDescending(e => e.LeaveRequest),
                "Status" => approvalRequests.OrderBy(e => e.Status),
                "status_desc" => approvalRequests.OrderByDescending(e => e.Status),
                _ => approvalRequests.OrderBy(e => e.Id),
            };
            foreach (ApprovalRequest request in approvalRequests)
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

        // GET: ApprovalRequests/Approve/5
        public ActionResult Approve(int id)
        {

            ApprovalRequest? approvalRequest = _repository.GetSingle(id);
            if (approvalRequest != null)
            {
                if (approvalRequest.Status != "Approved")
                {
                    approvalRequest.Status = "Approved";
                    _ = _repository.Edit(approvalRequest);
                    _ = _repository.Save();

                    LeaveRequest leaveRequest = _leaveRequestsRepository.GetAllRecords().Where(e => e.Id == approvalRequest.LeaveRequest).Include(e => e.ApprovalRequests).Single();
                    if (leaveRequest.Status != "Approved")
                    {
                        foreach (ApprovalRequest request in leaveRequest.ApprovalRequests)
                        {
                            if (request.Status != "Approved")
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }

                        leaveRequest.Status = "Approved";
                        _ = _leaveRequestsRepository.Edit(leaveRequest);
                        _ = _leaveRequestsRepository.Save();
                        Employee? employee = _employeesRepository.GetSingle(leaveRequest.Employee);
                        if (employee != null)
                        {
                            employee.OutOfOfficeBalance -= leaveRequest.EndDate.Day - leaveRequest.StartDate.Day;
                            _ = _employeesRepository.Edit(employee);
                            _ = _employeesRepository.Save();
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ApprovalRequests/Reject/5
        public ActionResult Reject()
        {
            return View();
        }

        // POST: ApprovalRequests/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(int id, string comment)
        {
            try
            {
                ApprovalRequest? approvalRequest = _repository.GetSingle(id);
                if (approvalRequest != null)
                {
                    if (approvalRequest.Status != "Rejected")
                    {
                        approvalRequest.Comment = comment;
                        approvalRequest.Status = "Rejected";
                        _ = _repository.Edit(_mapper.Map<ApprovalRequest>(approvalRequest));
                        _ = _repository.Save();

                        LeaveRequest? leaveRequest = _leaveRequestsRepository.GetSingle(approvalRequest.LeaveRequest);
                        if (leaveRequest != null)
                        {
                            leaveRequest.Status = "Rejected";
                            _ = _leaveRequestsRepository.Edit(leaveRequest);
                            _ = _leaveRequestsRepository.Save();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}