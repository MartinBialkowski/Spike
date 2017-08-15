using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}
