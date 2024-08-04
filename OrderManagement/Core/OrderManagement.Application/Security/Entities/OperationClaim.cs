using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Security.Entities
{
    public class OperationClaim
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public OperationClaim()
        {
        }

        public OperationClaim(int id, string name)
        {
            Name = name;
        }
    }
}