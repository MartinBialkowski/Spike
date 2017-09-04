using FluentValidation;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Validators
{
    public class StudentCreateRequestDTOValidator: AbstractValidator<StudentCreateRequestDataTransferObject>
    {
        public StudentCreateRequestDTOValidator()
        {
            RuleFor(student => student.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(student => student.CourseId).NotNull().WithMessage("CourseId cannot be null");
            RuleFor(student => student.CourseId).GreaterThan(0).WithMessage("Id value must be greater than 0");
        }
    }
}
