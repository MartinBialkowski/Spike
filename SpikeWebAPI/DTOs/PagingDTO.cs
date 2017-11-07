using SpikeWebAPI.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SpikeWebAPI.DTOs
{
    public class PagingDTO: IValidatableObject
    {
        public int? PageNumber { get; set; }
        public int? PageLimit { get; set; }

        public PagingDTO()
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
