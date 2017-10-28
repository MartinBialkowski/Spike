using FluentValidation;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Validators
{
    public class PagingValidator : AbstractValidator<PagingDTO>
    {
        public PagingValidator()
        {
            RuleFor(paging => paging.PageLimit).GreaterThan(0).WithMessage("page size must be greater than 0");
            RuleFor(paging => paging.PageNumber).GreaterThan(0).WithMessage("page number must be greater than 0");
        }
    }
}
