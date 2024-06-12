using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.Identity;

namespace GalaFamilyLibrary.Service.Validators
{
    public class UserCreationValidator : AbstractValidator<UserCreationDTO>
    {
        public UserCreationValidator()
        {
            
        }
    }
}