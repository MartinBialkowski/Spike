namespace SpikeWebAPI.DTOs
{
    public class StudentUpdateRequestDataTransferObject
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
    }
}
