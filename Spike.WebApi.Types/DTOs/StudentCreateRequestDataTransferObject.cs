using Spike.WebApi.Types.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Spike.WebApi.Types.DTOs
{
    public class StudentCreateRequestDataTransferObject : IValidatableObject
    {
        public int CourseId { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new StudentCreateRequestDTOValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
