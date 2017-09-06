using EFCoreSpike5.ConstraintsModels;
using SpikeWebAPI.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SpikeWebAPI.ConstraintsModels
{
    public class Paging : IPaging
    {
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }

        public int Offset
        {
            get
            {
                return (PageNumber - 1) * PageLimit;
            }
        }

        public Paging()
        {
            PageNumber = 1;
            PageLimit = 50;
        }
        public Paging(int pageNumber, int pageLimit)
        {
            PageNumber = pageNumber;
            PageLimit = pageLimit;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new PagingValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));

        }
    }
}
