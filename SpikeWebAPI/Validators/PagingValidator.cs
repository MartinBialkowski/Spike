using EFCoreSpike5.ConstraintsModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.Validators
{
    public class PagingValidator : AbstractValidator<Paging>
    {
        public PagingValidator()
        {
            RuleFor(paging => paging.PageLimit).NotNull().WithMessage("page size cannot be null");
            RuleFor(paging => paging.PageLimit).GreaterThan(0).WithMessage("page size must be greater than 0");
        }
    }
}
