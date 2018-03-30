using FluentValidation;
using Spike.WebApi.Types.DTOs;

namespace Spike.WebApi.Types.Validators
{
    public class StudentUpdateRequestDtoValidator : AbstractValidator<StudentUpdateRequestDataTransferObject>
    {
        public StudentUpdateRequestDtoValidator()
        {
            RuleFor(student => student.Id).NotNull().WithMessage("Student Id cannot be null");
            RuleFor(student => student.Id).GreaterThan(0).WithMessage("Student Id value must be greater than 0");
            RuleFor(student => student.Name).NotEmpty().WithMessage("Student Id cannot be null or empty");
            RuleFor(student => student.CourseId).NotNull().WithMessage("Course Id cannot be null");
            RuleFor(student => student.CourseId).GreaterThan(0).WithMessage("Course Id value must be greater than 0");
        }
    }
}
