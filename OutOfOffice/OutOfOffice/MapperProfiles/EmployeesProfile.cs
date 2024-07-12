using AutoMapper;
using OutOfOffice.Models;
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
            CreateMap<Employee, EmployeeViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName));
            CreateMap<Employee, EmployeeCreateViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName)).ForMember(dest => dest.Photo, opt => opt.Ignore());
            CreateMap<EmployeeCreateViewModel, Employee>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom<PartnerResolver>()).ForMember(dest => dest.Photo, opt => opt.MapFrom<PhotoResolver>());
            CreateMap<EmployeeEditViewModel, Employee>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom<EditPartnerResolver>()).ForMember(dest => dest.Photo, opt => opt.MapFrom<EditPhotoResolver>());
            CreateMap<Employee, EmployeeEditViewModel>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom(x => x.PeoplePartnerNavigation.FullName)).ForMember(dest => dest.CurrentPhoto, opt => opt.MapFrom(x => x.Photo)).ForMember(dest => dest.PhotoToChange, opt => opt.Ignore());
        }
    }

    internal class PartnerResolver : IValueResolver<EmployeeCreateViewModel, Employee, int>
    {
        private readonly IRepositoryService<Employee> _repository;
        public PartnerResolver(ApplicationDbContext context) { _repository = new RepositoryService<Employee>(context);}

        public int Resolve(EmployeeCreateViewModel source, Employee destination, int destMember, ResolutionContext context)
        {
            return _repository.GetAllRecords().Where(e => e.FullName == source.PeoplePartner).Select(e => e.Id).FirstOrDefault();
        }
    }
    internal class PhotoResolver : IValueResolver<EmployeeCreateViewModel, Employee, byte[]>
    {
        private readonly IRepositoryService<Employee> _repository;
        public PhotoResolver(ApplicationDbContext context) { _repository = new RepositoryService<Employee>(context); }
        public byte[] Resolve(EmployeeCreateViewModel source, Employee destination, byte[] destMember, ResolutionContext context)
        {
            using (var target = new MemoryStream())
            {
                if(source.Photo==null)
                {
                    return null;
                }
                source.Photo!.CopyTo(target);
                return target.ToArray();
            }
        }
    } internal class EditPartnerResolver : IValueResolver<EmployeeEditViewModel, Employee, int>
    {
        private readonly IRepositoryService<Employee> _repository;
        public EditPartnerResolver(ApplicationDbContext context) { _repository = new RepositoryService<Employee>(context);}

        public int Resolve(EmployeeEditViewModel source, Employee destination, int destMember, ResolutionContext context)
        {
            return _repository.GetAllRecords().Where(e => e.FullName == source.PeoplePartner).Select(e => e.Id).FirstOrDefault();
        }
    }
    internal class EditPhotoResolver : IValueResolver<EmployeeEditViewModel, Employee, byte[]>
    {
        private readonly IRepositoryService<Employee> _repository;
        public EditPhotoResolver(ApplicationDbContext context) { _repository = new RepositoryService<Employee>(context); }
        public byte[] Resolve(EmployeeEditViewModel source, Employee destination, byte[] destMember, ResolutionContext context)
        {
            using (var target = new MemoryStream())
            {
                if(source.PhotoToChange==null)
                {
                    return source.CurrentPhoto;
                }
                source.PhotoToChange!.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
