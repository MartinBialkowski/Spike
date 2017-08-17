using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.Models
{
    public class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
