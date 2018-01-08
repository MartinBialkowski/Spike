using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Spike.Core.Entity
{
    public class User: IEntityBase
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public int UnsuccessfulLoginAttempts { get; set; }
        [DataType(DataType.Time)]
        public DateTime LockEnd { get; set; }
        public ICollection<UserClaim> UserClaims { get; set; } = new HashSet<UserClaim>();
    }
}
