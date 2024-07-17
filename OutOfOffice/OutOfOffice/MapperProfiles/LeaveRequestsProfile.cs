using AutoMapper;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Models;

namespace OutOfOffice.MapperProfiles
{
	public class LeaveRequestsProfile : Profile
	{
		public LeaveRequestsProfile()
		{
			CreateMap<LeaveRequest, LeaveRequestViewModel>().ForMember(dest => dest.Employee, opt => opt.MapFrom(e => e.EmployeeNavigation.FullName));
			CreateMap<LeaveRequestViewModel, LeaveRequest>();
		}
	}
}
