using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.ConstraintsModels
{
    public interface IPagingModel
    {
        int PageNumber { get; set; }
        int PageLimit { get; set; }
        int Offset { get; }
    }
}
