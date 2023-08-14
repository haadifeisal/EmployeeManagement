using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using FluentValidation;

namespace EmployeeManagement.WebApi.Validations
{
    public class DepartmentRequestValidator : AbstractValidator<DepartmentRequestDto>
    {
        public DepartmentRequestValidator()
        {
            RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .Length(2, 100)
                    .WithMessage("Name must be minimum 2 and maximum 50 charachters.");
        }
    }
}
