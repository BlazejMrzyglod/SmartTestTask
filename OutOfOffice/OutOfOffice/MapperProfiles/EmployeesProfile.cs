using AutoMapper;
using OutOfOffice.Models.Models;
using OutOfOffice.Models.ViewModels;
using OutOfOffice.Services.Data;
using OutOfOffice.Services.Repository;
using OutOfOffice.Services.Repository.EntityFramework;

namespace OutOfOffice.MapperProfiles
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            _ = CreateMap<Employee, EmployeeViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName));
            _ = CreateMap<Employee, EmployeeCreateViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName)).ForMember(dest => dest.Photo, opt => opt.Ignore());
            _ = CreateMap<EmployeeCreateViewModel, Employee>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom<PartnerResolver>()).ForMember(dest => dest.Photo, opt => opt.MapFrom<PhotoResolver>());
            _ = CreateMap<EmployeeEditViewModel, Employee>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom<EditPartnerResolver>()).ForMember(dest => dest.Photo, opt => opt.MapFrom<EditPhotoResolver>());
            _ = CreateMap<Employee, EmployeeEditViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName)).ForMember(dest => dest.CurrentPhoto, opt => opt.MapFrom(x => x.Photo)).ForMember(dest => dest.PhotoToChange, opt => opt.Ignore());
        }
    }

    internal class PartnerResolver(ApplicationDbContext context) : IValueResolver<EmployeeCreateViewModel, Employee, int>
    {
        private readonly RepositoryService<Employee> _repository = new(context);

        public int Resolve(EmployeeCreateViewModel source, Employee destination, int destMember, ResolutionContext context)
        {
            return _repository.GetAllRecords().Where(e => e.FullName == source.PeoplePartner).Select(e => e.Id).FirstOrDefault();
        }
    }
    internal class PhotoResolver : IValueResolver<EmployeeCreateViewModel, Employee, byte[]?>
    {
        public byte[]? Resolve(EmployeeCreateViewModel source, Employee destination, byte[]? destMember, ResolutionContext context)
        {
            using MemoryStream target = new();
            if (source.Photo == null)
            {
                return null;
            }
            source.Photo!.CopyTo(target);
            return target.ToArray();
        }
    }
    internal class EditPartnerResolver(ApplicationDbContext context) : IValueResolver<EmployeeEditViewModel, Employee, int>
    {
        private readonly RepositoryService<Employee> _repository = new(context);

        public int Resolve(EmployeeEditViewModel source, Employee destination, int destMember, ResolutionContext context)
        {
            return _repository.GetAllRecords().Where(e => e.FullName == source.PeoplePartner).Select(e => e.Id).FirstOrDefault();
        }
    }
    internal class EditPhotoResolver : IValueResolver<EmployeeEditViewModel, Employee, byte[]?>
    {
        public byte[]? Resolve(EmployeeEditViewModel source, Employee destination, byte[]? destMember, ResolutionContext context)
        {
            using MemoryStream target = new();
            if (source.PhotoToChange == null)
            {
                return source.CurrentPhoto ?? null;
            }
            source.PhotoToChange!.CopyTo(target);
            return target.ToArray();
        }
    }
}
