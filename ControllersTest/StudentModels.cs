using System.Collections.Generic;

namespace ControllersTest
{
    public class StudentTestCreateRequest
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
    }

    public class StudentTestUpdateRequest
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
    }

    public class StudentTestResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseTestResponse Course { get; set; }
    }

    public class CourseTestResponse
    {
        public string Name { get; set; }
    }

    public class TestPagedResult<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalNumberOfPages { get; set; }

        public int TotalNumberOfRecords { get; set; }

        public string FirstPageUrl { get; set; }

        public string PreviousPageUrl { get; set; }

        public string NextPageUrl { get; set; }

        public string LastPageUrl { get; set; }

        public IList<T> Results { get; set; }
    }
}
