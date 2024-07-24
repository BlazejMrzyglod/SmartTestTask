using AutoMapper;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;

namespace OutOfOffice.MapperProfiles
{
    public class LeaveRequestsProfile : Profile
    {
        public LeaveRequestsProfile()
        {
            _ = CreateMap<LeaveRequest, LeaveRequestViewModel>().ForMember(dest => dest.Employee, opt => opt.MapFrom(e => e.EmployeeNavigation.FullName));
            _ = CreateMap<LeaveRequestViewModel, LeaveRequest>();
        }
    }
}
