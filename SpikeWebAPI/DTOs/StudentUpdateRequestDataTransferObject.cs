using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpikeWebAPI.DTOs
{
    public class StudentUpdateRequestDataTransferObject: IValidatableObject
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
