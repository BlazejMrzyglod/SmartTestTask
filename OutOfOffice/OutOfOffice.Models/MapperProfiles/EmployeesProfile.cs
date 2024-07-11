using AutoMapper;
using OutOfOffice.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.Models.MapperProfiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName));
            //CreateMap<EmployeeViewModel, Employee>();
        }
    }
}
