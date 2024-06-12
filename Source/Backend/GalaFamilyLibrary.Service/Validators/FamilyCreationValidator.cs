using FluentValidation;
using GalaFamilyLibrary.DataTransferObject.FamilyLibrary;

namespace GalaFamilyLibrary.Service.Validators
{
    public class FamilyCreationValidator : AbstractValidator<FamilyCreationDTO>
    {
        public FamilyCreationValidator()
        {
            RuleFor(fc => fc.Name).NotNull().WithMessage("");
        }
    }
}