using SpikeWebAPI.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SpikeWebAPI.DTOs
{
    public class StudentUpdateRequestDataTransferObject : IValidatableObject
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new StudentUpdateRequestDTOValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));

        }
    }
}
