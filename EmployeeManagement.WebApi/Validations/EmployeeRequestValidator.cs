using EmployeeManagement.WebApi.DataTransferObjects.RequestDtos;
using FluentValidation;

namespace EmployeeManagement.WebApi.Validations
{
    public class EmployeeRequestValidator : AbstractValidator<EmployeeRequestDto>
    {
        public EmployeeRequestValidator()
        {
            RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .Length(2, 100)
                    .WithMessage("Name must be minimum 2 and maximum 50 charachters.");
            RuleFor(x => x.Salary)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Salary is required");
            RuleFor(x => x.DepartmentId)
                    .NotEmpty()
                    .WithMessage("DepartmentId is required");
        }
    }
}
