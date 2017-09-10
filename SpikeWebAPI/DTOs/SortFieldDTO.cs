using EFCoreSpike5.CommonModels;
using EFCoreSpike5.ConstraintsModels;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SpikeWebAPI.DTOs
{
    public class SortFieldDTO : ISortField, IValidatableObject
    {
        public SortOrder SortOrder { get; set; }
        public string PropertyName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
