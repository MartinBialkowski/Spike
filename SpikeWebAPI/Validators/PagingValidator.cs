using FluentValidation;
using EFCoreSpike5.ConstraintsModels;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Validators
{
    public class PagingValidator : AbstractValidator<PagingDTO>
    {
        public PagingValidator()
        {
            //RuleFor(paging => paging.PageLimit).NotNull().WithMessage("page size cannot be null");
            RuleFor(paging => paging.PageLimit).GreaterThan(0).WithMessage("page size must be greater than 0");
            //RuleFor(paging => paging.PageNumber).NotNull().WithMessage("page number cannot be null");
            RuleFor(paging => paging.PageNumber).GreaterThan(0).WithMessage("page number must be greater than 0");
        }
    }
}
