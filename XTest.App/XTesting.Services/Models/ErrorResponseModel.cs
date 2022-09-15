using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTesting.Services.Models
{
    public class ErrorResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        // other fields

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
