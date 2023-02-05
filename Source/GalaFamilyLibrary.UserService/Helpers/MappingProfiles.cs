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
        }
    }
}
