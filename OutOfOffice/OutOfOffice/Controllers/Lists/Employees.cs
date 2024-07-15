﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OutOfOffice.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;
using System.Linq;

namespace OutOfOffice.Controllers.Lists
{
    public class Employees : Controller
    {
        private readonly IRepositoryService<Employee> _repository;
        private readonly IMapper _mapper;

        public Employees(ApplicationDbContext context, IMapper mapper)
        {
            _repository = new RepositoryService<Employee>(context);
            _mapper = mapper;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string sortOrder, string searchString, string positionFilter, string subdivisionFilter, string statusFilter)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.FullName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(positionFilter))
            {
                employees = employees.Where(s => s.Position.Equals(positionFilter));
            }

            if (!String.IsNullOrEmpty(subdivisionFilter))
            {
                employees = employees.Where(s => s.Subdivision.Equals(subdivisionFilter));
            }

            if (!String.IsNullOrEmpty(statusFilter))
            {
                employees = employees.Where(s => s.Status.Equals(statusFilter));
            }

            List<EmployeeViewModel> employeesViewModels = new();

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
            foreach (var employee in employees)
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
                _repository.Add(_mapper.Map<Employee>(employee));
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
                _repository.Edit(_mapper.Map<Employee>(employee));
                _repository.Save();
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
            Employee employee = _repository.GetAllRecords().Where(x => x.Id == id).Include(x => x.PeoplePartnerNavigation).Include(x=>x.ProjectsAndEmployees).Single();

            return View(_mapper.Map<EmployeeEditViewModel>(employee));
        }

        // GET: Employees/ChangeStatus/5
        public ActionResult ChangeStatus(int id)
        {
            Employee employee = _repository.GetSingle(id);
            string status = employee.Status;
            if (status == "Active")
                employee.Status = "Inactive";
            else
                employee.Status = "Active";
            _repository.Edit(employee);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
