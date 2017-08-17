using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
    }
}
