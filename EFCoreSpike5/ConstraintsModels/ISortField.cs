using EFCoreSpike5.CommonModels;

namespace EFCoreSpike5.ConstraintsModels
{
    public interface ISortField
    {
        SortOrder SortOrder { get; set; }
        string PropertyName { get; set; }
    }
}
