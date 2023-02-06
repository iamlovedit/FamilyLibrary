using AutoMapper;
using GalaFamilyLibrary.UserService.DataTransferObjetcts;
using GalaFamilyLibrary.UserService.Models;

namespace GalaFamilyLibrary.UserService.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<LibraryUser, LibraryUserDto>();
            CreateMap<LibraryUserCreationDto, LibraryUser>().ForMember(u => u.CreateTime, options =>
            {
                options.MapFrom((ud, u) =>
                {
                    return DateTime.Now;
                });
            }).ForMember(u => u.UpdateTime, options =>
            {
                options.MapFrom((ud, u) =>
                {
                    return DateTime.Now;
                });
            })
            .ForMember(u => u.Salt, options =>
            {
                options.MapFrom((ud, u) =>
                {
                    return u.Salt = Guid.NewGuid().ToString("N");
                });
            })
            .ForMember(u => u.Password, options =>
            {
                options.MapFrom((ud, u) =>
                {
                    return ud.Password.MD5Encrypt32(u.Salt);
                });
            });
        }
    }
}
