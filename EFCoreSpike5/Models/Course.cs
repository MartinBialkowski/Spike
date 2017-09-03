﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSpike5.Models
{
    public class Course: IEntityBase
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
