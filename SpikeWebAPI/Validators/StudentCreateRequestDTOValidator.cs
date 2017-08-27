using FluentValidation;
using SpikeWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Validators
{
    public class StudentCreateRequestDTOValidator: AbstractValidator<StudentCreateRequestDataTransferObject>
    {
        public StudentCreateRequestDTOValidator()
        {
            RuleFor(student => student.Name).NotEmpty().WithMessage("Name cannot be empty");
        }
    }
}
