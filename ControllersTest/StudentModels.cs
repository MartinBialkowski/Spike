using System;
using System.Collections.Generic;
using System.Text;

namespace ControllersTest
{
    public class StudentCreateRequest
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
    }

    public class StudentUpdateRequest
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
    }
}
