using FluentValidation;
using SpikeWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Validators
{
    public class StudentUpdateRequestDTOValidator : AbstractValidator<StudentUpdateRequestDataTransferObject>
    {
        public StudentUpdateRequestDTOValidator()
        {
            RuleFor(student => student.Id).NotNull().WithMessage("Student Id cannot be null");
            RuleFor(student => student.Id).GreaterThan(0).WithMessage("Student Id value must be greater than 0");
            RuleFor(student => student.Name).NotEmpty().WithMessage("Student Id cannot be null or empty");
            RuleFor(student => student.CourseId).NotNull().WithMessage("Course Id cannot be null");
            RuleFor(student => student.CourseId).GreaterThan(0).WithMessage("Course Id value must be greater than 0");
        }
    }
}
