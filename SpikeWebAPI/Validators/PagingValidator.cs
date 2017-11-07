using FluentValidation;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Validators
{
    public class PagingValidator : AbstractValidator<PagingDTO>
    {
        public PagingValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(paging => paging.PageLimit).NotNull().GreaterThan(0).WithMessage("page size must be greater than 0");
            RuleFor(paging => paging.PageNumber).NotNull().GreaterThan(0).WithMessage("page number must be greater than 0");
        }
    }
}
