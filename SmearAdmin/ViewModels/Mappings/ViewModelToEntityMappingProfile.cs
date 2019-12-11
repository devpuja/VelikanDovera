using AutoMapper;
using SmearAdmin.Models;

namespace SmearAdmin.ViewModels.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<RegistrationViewModel, AppUser>()
                .ForMember(au => au.UserName, map => map.MapFrom(vm => vm.CustomUserName))
                .ForMember(au => au.PasswordRaw, map => map.MapFrom(vm => vm.PasswordRaw)).ReverseMap();

            //CreateMap<MasterKeyValueViewModel, MasterKeyValue>().ReverseMap();
            //CreateMap<ContactResourseViewModel, ContactResourse>().ReverseMap();
            //CreateMap<HQRegionViewModel, HQRegion>().ReverseMap();

        }
    }
}
