using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.SharedLayer.ResponseModel
{
    public class ValidationError
    {
        public string Property { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
