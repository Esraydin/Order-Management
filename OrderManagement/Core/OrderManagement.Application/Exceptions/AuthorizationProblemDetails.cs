using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OrderManagement.Application.Exceptions
{
    public class AuthorizationProblemDetails:ProblemDetails
    {
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
