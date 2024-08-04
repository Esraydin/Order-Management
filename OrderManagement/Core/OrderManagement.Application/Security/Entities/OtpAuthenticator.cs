using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Security.Entities
{
    public class OtpAuthenticator 
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public byte[] SecretKey { get; set; }
        public bool IsVerified { get; set; }

        public virtual User User { get; set; }

        public OtpAuthenticator()
        {
        }

        public OtpAuthenticator(int id, Guid userId, byte[] secretKey, bool isVerified) : this()
        {
            Id = id;
            UserId = userId;
            SecretKey = secretKey;
            IsVerified = isVerified;
        }
    }
}
