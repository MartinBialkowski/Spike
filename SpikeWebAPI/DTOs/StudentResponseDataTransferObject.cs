namespace SpikeWebAPI.DTOs
{
    public class StudentResponseDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CourseResponseDataTransferObject Course { get; set; }
    }
}
