using FluentValidation;
using GalaFamilyLibrary.Domain.DataTransferObjects.FamilyLibrary;

namespace GalaFamilyLibrary.Domain.Validators
{
    public class FamilyCreationValidator : AbstractValidator<FamilyCreationDTO>
    {
        public FamilyCreationValidator()
        {
            RuleFor(fc => fc.Name).NotNull().WithMessage("");
            
        }
    }
}