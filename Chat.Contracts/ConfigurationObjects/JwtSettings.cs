using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Contracts.ConfigurationObjects
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public double LifeTime { get; set; }
    }
}
