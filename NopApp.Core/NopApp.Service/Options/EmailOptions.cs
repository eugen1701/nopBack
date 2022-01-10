﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NopApp.Service.Options
{
    public class EmailOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string EmailConfirmationUrl { get; set; }
        public int Timeout { get; set; }
    }
}
