using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSpike5.Models
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
