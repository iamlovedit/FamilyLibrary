using AutoMapper;
using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;
using GalaFamilyLibrary.Domain.Models.Identity;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.Infrastructure.Security.Encyption;

namespace GalaFamilyLibrary.IdentityService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles(IAESEncryptionService aESEncryptionService)
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<UserCreationDTO, ApplicationUser>()
                .ForMember(u => u.CreateTime, options => { options.MapFrom((ud, u) => DateTime.Now); })
                .ForMember(u => u.UpdateTime, options => { options.MapFrom((ud, u) => DateTime.Now); })
                .ForMember(u => u.LastLoginTime, options => { options.MapFrom((ud, u) => DateTime.Now); })
                .ForMember(u => u.Salt,
                    options => { options.MapFrom((ud, u) => { return u.Salt = Guid.NewGuid().ToString("N"); }); })
                .ForMember(u => u.Password,
                    options => { options.MapFrom((ud, u) => ud.Password.MD5Encrypt32(u.Salt)); });
        }
    }
}