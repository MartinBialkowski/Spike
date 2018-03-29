using Spike.WebApi.Types.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Spike.WebApi.Types.DTOs
{
    public class PagingDto: IValidatableObject
    {
        public int? PageNumber { get; set; }
        public int? PageLimit { get; set; }

        public PagingDto()
        {
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PagingValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));

        }
    }
}
