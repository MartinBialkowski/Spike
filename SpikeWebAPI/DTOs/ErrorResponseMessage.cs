using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpikeWebAPI.DTOs
{
    public class ErrorResponseMessage
    {
        public string UserMessage { get; set; }
        public string InternalMessage { get; set; }
        public string MoreInfo { get; set; }

        public void CreateNotFoundError()
        {
            UserMessage = "Sorry, the requested resource does not exist";
        }

        public void CreateBadRequestError()
        {
            UserMessage = "Sorry, the given parameters are invalid";
        }

        public string GetErrorAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
