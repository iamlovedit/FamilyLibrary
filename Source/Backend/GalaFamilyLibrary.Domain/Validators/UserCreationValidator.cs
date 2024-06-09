using FluentValidation;
using GalaFamilyLibrary.Domain.DataTransferObjects.Identity;

namespace GalaFamilyLibrary.Domain.Validators
{
    public class UserCreationValidator : AbstractValidator<UserCreationDTO>
    {
        public UserCreationValidator()
        {
            
        }
    }
}