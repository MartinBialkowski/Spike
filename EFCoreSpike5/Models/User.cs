using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.Models
{
    public class User
    {
        public User()
        {
            UserClaims = new HashSet<UserClaim>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime CreationDate { get; set; }
        public int UnsuccessfulLoginAttempts { get; set; }
        public DateTime LockEnd { get; set; }
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}
