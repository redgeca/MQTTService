﻿using Microsoft.IdentityModel.Tokens;

namespace IoTFallServer.Options
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;

    }
}
