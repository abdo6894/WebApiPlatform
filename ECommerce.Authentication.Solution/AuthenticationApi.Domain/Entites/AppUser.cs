﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Domain.Entites
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TelephoneNumber { get; set; }
        public string Role { get; set; }
        public DateTime DataRegistered { get; set; } = DateTime.UtcNow;
    }
}
