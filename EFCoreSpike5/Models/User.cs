﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSpike5.Models
{
    public class User
    {
        public User()
        {
            UserClaims = new HashSet<UserClaim>();
        }

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
        public ICollection<UserClaim> UserClaims { get; set; }
    }
}