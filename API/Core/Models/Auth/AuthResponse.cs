﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }

    }
}
