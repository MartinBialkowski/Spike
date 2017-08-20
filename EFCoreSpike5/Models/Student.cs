using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSpike5.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public Course Course { get; set; }
    }
}
