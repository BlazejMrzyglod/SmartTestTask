using AutoMapper;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Models;

namespace OutOfOffice.MapperProfiles
{
    public class ApprovalRequestsProfile : Profile
    {
        public ApprovalRequestsProfile()
        {
            CreateMap<ApprovalRequest, ApprovalRequestViewModel>().ForMember(dest => dest.Approver, opt => opt.MapFrom(e => e.ApproverNavigation.FullName));
            //CreateMap<ApprovalRequestViewModel, ApprovalRequest>();
        }
    }
}
