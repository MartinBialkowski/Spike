using EFCoreSpike5.Models;

namespace SpikeWebAPI.DTOs
{
    public class StudentResponseDataTransferObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Course Course { get; set; }
    }
}
