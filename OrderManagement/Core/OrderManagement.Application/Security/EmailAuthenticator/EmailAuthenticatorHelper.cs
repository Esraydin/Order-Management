﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Security.EmailAuthenticator
{
    public class EmailAuthenticatorHelper:IEmailAuthenticatorHelper
    {
        public Task<string> CreateEmailActivationKey()
        {
            string key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            return Task.FromResult(key);
        }

        public Task<string> CreateEmailActivationCode()
        {
            string code = RandomNumberGenerator.GetInt32(Convert.ToInt32(Math.Pow(10, 6))).ToString().PadLeft(6, '0');
            return Task.FromResult(code);
        }
    }
}
