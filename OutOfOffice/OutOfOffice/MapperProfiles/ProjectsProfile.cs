using AutoMapper;
using OutOfOffice.Models;
using OutOfOffice.Models.ViewModels;

namespace OutOfOffice.MapperProfiles
{
	public class ProjectsProfile : Profile
	{
		public ProjectsProfile() 
		{
			CreateMap<Project, ProjectViewModel>();
			CreateMap<ProjectViewModel, Project>();
		}
	}
}
