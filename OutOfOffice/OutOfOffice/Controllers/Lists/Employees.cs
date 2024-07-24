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
    public class Employees(ApplicationDbContext context, IMapper mapper) : Controller
    {
        private readonly RepositoryService<Employee> _repository = new(context);
        private readonly IMapper _mapper = mapper;

        // GET: Employees
        public IActionResult Index(string sortOrder, string searchString, string positionFilter, string subdivisionFilter, string statusFilter)
        {
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["SubdivisionSortParm"] = sortOrder == "Subdivision" ? "subdivision_desc" : "Subdivision";
            ViewData["PositionSortParm"] = sortOrder == "Position" ? "position_desc" : "Position";
            ViewData["PartnerSortParm"] = sortOrder == "Partner" ? "partner_desc" : "Partner";
            ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "balance_desc" : "Balance";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["NameFilter"] = searchString;
            ViewData["PositionFilter"] = positionFilter;
            ViewData["SubdivisionFilter"] = subdivisionFilter;
            ViewData["StatusFilter"] = statusFilter;

            IQueryable<Employee> employees = _repository.GetAllRecords()
                .Include(e => e.PeoplePartnerNavigation);

            if (!string.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.FullName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(positionFilter))
            {
                employees = employees.Where(s => s.Position.Equals(positionFilter));
            }

            if (!string.IsNullOrEmpty(subdivisionFilter))
            {
                employees = employees.Where(s => s.Subdivision.Equals(subdivisionFilter));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                employees = employees.Where(s => s.Status.Equals(statusFilter));
            }

            List<EmployeeViewModel> employeesViewModels = [];

            employees = sortOrder switch
            {
                "name_desc" => employees.OrderByDescending(e => e.FullName),
                "Subdivision" => employees.OrderBy(e => e.Subdivision),
                "subdivision_desc" => employees.OrderByDescending(e => e.Subdivision),
                "Position" => employees.OrderBy(e => e.Position),
                "position_desc" => employees.OrderByDescending(e => e.Position),
                "Partner" => employees.OrderBy(e => e.PeoplePartner),
                "partner_desc" => employees.OrderByDescending(e => e.PeoplePartner),
                "Balance" => employees.OrderBy(e => e.OutOfOfficeBalance),
                "balance_desc" => employees.OrderByDescending(e => e.OutOfOfficeBalance),
                "Status" => employees.OrderBy(e => e.Status),
                "status_desc" => employees.OrderByDescending(e => e.Status),
                _ => employees.OrderBy(e => e.FullName),
            };
            foreach (Employee employee in employees)
            {
                employeesViewModels.Add(_mapper.Map<EmployeeViewModel>(employee));
            }

            return View(employeesViewModels);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,FullName,Subdivision,Position,Status,PeoplePartner,OutOfOfficeBalance,Photo")] EmployeeCreateViewModel employee)
        {
            try
            {
                _ = _repository.Add(_mapper.Map<Employee>(employee));
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
            Employee employee = _repository.GetAllRecords().Where(x => x.Id == id).Include(x => x.PeoplePartnerNavigation).Single();

            return View(_mapper.Map<EmployeeEditViewModel>(employee));
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,FullName,Subdivision,Position,Status,PeoplePartner,OutOfOfficeBalance,PhotoToChange, CurrentPhoto")] EmployeeEditViewModel employee)
        {
            try
            {
                _ = _repository.Edit(_mapper.Map<Employee>(employee));
                _ = _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Details/5
        public ActionResult Details(int id)
        {
            Employee employee = _repository.GetAllRecords().Where(x => x.Id == id).Include(x => x.PeoplePartnerNavigation).Include(x => x.ProjectsAndEmployees).Single();

            return View(_mapper.Map<EmployeeEditViewModel>(employee));
        }

        // GET: Employees/ChangeStatus/5
        public ActionResult ChangeStatus(int id)
        {
            Employee? employee = _repository.GetSingle(id);
            if (employee != null)
            {
                string status = employee.Status;
                employee.Status = status == "Active" ? "Inactive" : "Active";
                _ = _repository.Edit(employee);
                _ = _repository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
