using FluentValidation;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Types.Validators
{
    public class StudentCreateRequestDtoValidator: AbstractValidator<StudentCreateRequestDataTransferObject>
    {
        public StudentCreateRequestDtoValidator()
        {
            RuleFor(student => student.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(student => student.CourseId).NotNull().WithMessage("CourseId cannot be null");
            RuleFor(student => student.CourseId).GreaterThan(0).WithMessage("Id value must be greater than 0");
        }
    }
}
