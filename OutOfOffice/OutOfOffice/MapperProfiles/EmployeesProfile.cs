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
            CreateMap<EmployeeCreateViewModel, Employee>().ForMember(dest => dest.PeoplePartner, opt => opt.MapFrom<PartnerResolver>()).ForMember(dest => dest.Photo, opt => opt.MapFrom<PhotoResolver>());
        }
    }

    internal class PartnerResolver : IValueResolver<EmployeeCreateViewModel, Employee, int>
    {
        private readonly IRepositoryService<Employee> _repository;
        public PartnerResolver(ApplicationDbContext context) { _repository = new RepositoryService<Employee>(context); }

        public int Resolve(EmployeeCreateViewModel source, Employee destination, int destMember, ResolutionContext context)
        {
            return _repository.GetAllRecords().Where(e => e.FullName == source.PeoplePartner).Select(e => e.Id).FirstOrDefault();
        }
    }
    internal class PhotoResolver : IValueResolver<EmployeeCreateViewModel, Employee, byte[]>
    {
        public byte[] Resolve(EmployeeCreateViewModel source, Employee destination, byte[] destMember, ResolutionContext context)
        {
            using (var target = new MemoryStream())
            {
                source.Photo!.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
