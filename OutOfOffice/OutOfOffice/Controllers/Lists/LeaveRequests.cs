using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;

namespace OutOfOffice.Controllers.Lists
{
    [Authorize]
    public class LeaveRequests(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly RepositoryService<LeaveRequest> _repository = new(context);
        private readonly RepositoryService<ApprovalRequest> _approvalRequestsRepository = new(context);
        private readonly RepositoryService<Employee> _employeesRepository = new(context);
        private readonly RepositoryService<Project> _projectsRepository = new(context);
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        // GET: LeaveRequests
        public ActionResult Index(string sortOrder, int searchString, string employeeFilter, string reasonFilter, string statusFilter)
        {
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
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

            if (!string.IsNullOrEmpty(employeeFilter))
            {
                leaveRequests = leaveRequests.Where(s => s.EmployeeNavigation.FullName.Contains(employeeFilter));
            }

            if (!string.IsNullOrEmpty(reasonFilter))
            {
                leaveRequests = leaveRequests.Where(s => s.AbscenceReason.Equals(reasonFilter));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                leaveRequests = leaveRequests.Where(s => s.Status.Equals(statusFilter));
            }

            List<LeaveRequestViewModel> viewModels = [];

            leaveRequests = sortOrder switch
            {
                "id_desc" => leaveRequests.OrderByDescending(e => e.Id),
                "Employee" => leaveRequests.OrderBy(e => e.EmployeeNavigation.FullName),
                "employee_desc" => leaveRequests.OrderByDescending(e => e.EmployeeNavigation.FullName),
                "Reason" => leaveRequests.OrderBy(e => e.AbscenceReason),
                "reason_desc" => leaveRequests.OrderByDescending(e => e.AbscenceReason),
                "StartDate" => leaveRequests.OrderBy(e => e.StartDate),
                "startdate_desc" => leaveRequests.OrderByDescending(e => e.StartDate),
                "EndDate" => leaveRequests.OrderBy(e => e.EndDate),
                "enddate_desc" => leaveRequests.OrderByDescending(e => e.EndDate),
                "Status" => leaveRequests.OrderBy(e => e.Status),
                "status_desc" => leaveRequests.OrderByDescending(e => e.Status),
                _ => leaveRequests.OrderBy(e => e.Id),
            };
            foreach (LeaveRequest request in leaveRequests)
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
        [Authorize(Roles = "Administrator, Employee")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<IActionResult> Create([Bind("AbscenceReason,StartDate,EndDate,Comment")] LeaveRequestViewModel _leaveRequest)
        {
            try
            {
                LeaveRequest leaveRequest = _mapper.Map<LeaveRequest>(_leaveRequest);
                ApplicationUser? _user = await _userManager.GetUserAsync(User);
                leaveRequest.Employee = _user!.EmployeeId;
                _ = _repository.Add(leaveRequest);
                _ = _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequests/Edit/5
        [Authorize(Roles = "Administrator, Employee")]
        public ActionResult Edit(int id)
        {
            LeaveRequest leaveRequest = _repository.GetAllRecords().Where(x => x.Id == id).Include(e => e.EmployeeNavigation).Single(); ;

            return View(_mapper.Map<LeaveRequestViewModel>(leaveRequest));
        }

        // POST: LeaveRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Employee")]
        public ActionResult Edit(int id, [Bind("AbscenceReason,StartDate,EndDate,Comment")] LeaveRequestViewModel _leaveRequest)
        {
            try
            {
                LeaveRequest? leaveRequest = _repository.GetSingle(id);
                if (leaveRequest != null)
                {
                    leaveRequest.AbscenceReason = _leaveRequest.AbscenceReason;
                    leaveRequest.StartDate = _leaveRequest.StartDate;
                    leaveRequest.EndDate = _leaveRequest.EndDate;
                    leaveRequest.Comment = _leaveRequest.Comment;
                    _ = _repository.Edit(leaveRequest);
                    _ = _repository.Save();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequests/Submit/5
        [Authorize(Roles = "Administrator, Employee")]
        public async Task<IActionResult> Submit(int id)
        {
            try
            {
                LeaveRequest? leaveRequest = _repository.GetSingle(id);

                if (leaveRequest != null)
                {
                    if (leaveRequest.Status != "Submitted")
                    {
                        leaveRequest.Status = "Submitted";
                        _ = _repository.Edit(leaveRequest);
                        _ = _repository.Save();

                        ApplicationUser? _user = await _userManager.GetUserAsync(User);

                        if (_user == null)
                            return View();
                        Employee employee = _employeesRepository.GetAllRecords().Where(e => e.Id == _user.EmployeeId).Include(e => e.ProjectsAndEmployees).Single();

                        ApprovalRequest approvalRequest = new()
                        {
                            Approver = employee.PeoplePartner,
                            LeaveRequest = leaveRequest.Id
                        };
                        _ = _approvalRequestsRepository.Add(approvalRequest);
                        _ = _approvalRequestsRepository.Save();

                        List<int> projectManagersIds = [];

                        foreach (ProjectsAndEmployee projectId in employee.ProjectsAndEmployees)
                        {
                            Project? project = _projectsRepository.GetSingle(projectId.ProjectId);

                            if (project == null)
                                return View();

                            if (!projectManagersIds.Contains(project.ProjectManager))
                            {
                                projectManagersIds.Add(project.ProjectManager);
                            }
                        }

                        foreach (int projectManagerId in projectManagersIds)
                        {
                            approvalRequest = new ApprovalRequest()
                            {
                                Approver = projectManagerId,
                                LeaveRequest = leaveRequest.Id
                            };
                            _ = _approvalRequestsRepository.Add(approvalRequest);
                            _ = _approvalRequestsRepository.Save();
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

        // GET: LeaveRequests/Submit/5
        [Authorize(Roles = "Administrator, Employee")]
        public ActionResult Cancel(int id)
        {
            try
            {
                LeaveRequest leaveRequest = _repository.GetAllRecords().Where(e => e.Id == id).Include(e => e.ApprovalRequests).Single();

                if (leaveRequest.Status != "Canceled")
                {
                    leaveRequest.Status = "Canceled";
                    _ = _repository.Edit(leaveRequest);
                    _ = _repository.Save();


                    foreach (ApprovalRequest request in leaveRequest.ApprovalRequests)
                    {
                        request.Status = "Canceled";
                        _ = _approvalRequestsRepository.Edit(request);
                        _ = _approvalRequestsRepository.Save();
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
