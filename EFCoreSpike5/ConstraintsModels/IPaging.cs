using System.ComponentModel.DataAnnotations;

namespace EFCoreSpike5.ConstraintsModels
{
    public interface IPaging : IValidatableObject
    {
        int PageNumber { get; set; }
        int PageLimit { get; set; }
        int Offset { get; }
    }
}
