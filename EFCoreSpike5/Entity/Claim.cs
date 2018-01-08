using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Spike.Core.Entity
{
    public class Claim: IEntityBase
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Value { get; set; }
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}
