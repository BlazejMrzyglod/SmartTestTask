using AutoMapper;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;

namespace OutOfOffice.MapperProfiles
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
            _ = CreateMap<Project, ProjectViewModel>();
            _ = CreateMap<ProjectViewModel, Project>();
        }
    }
}
