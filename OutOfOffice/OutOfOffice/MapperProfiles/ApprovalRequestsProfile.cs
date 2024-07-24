using AutoMapper;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;

namespace OutOfOffice.MapperProfiles
{
    public class ApprovalRequestsProfile : Profile
    {
        public ApprovalRequestsProfile()
        {
            _ = CreateMap<ApprovalRequest, ApprovalRequestViewModel>().ForMember(dest => dest.Approver, opt => opt.MapFrom(e => e.ApproverNavigation.FullName));
            //CreateMap<ApprovalRequestViewModel, ApprovalRequest>();
        }
    }
}
