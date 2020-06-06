using Chat.Contracts.ConfigurationObjects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Chat.BusinessLogic.Helpers
{
   public static class AuthenticationHelper
   {
        public static TokenValidationParameters GetTokenValidationParameters(JwtSettings options, bool validateLifetime = true)
        {
            /* return new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidIssuer = options.Issuer,

                 ValidateAudience = true,
                 ValidAudience = options.Audience,

                 ValidateLifetime = validateLifetime,

                 IssuerSigningKey = GetSymmetricSecurityKey(
                     options.Key),
                 ValidateIssuerSigningKey = true
             };*/
            var key = Encoding.ASCII.GetBytes(options.SecretKey);
            return new TokenValidationParameters //It is how we need validate our token that we take from our client
            {
                ValidateIssuerSigningKey = true, //for validating our token with secret key
                IssuerSigningKey = new SymmetricSecurityKey(key),// that provide encription of signature part by sekret key
                ValidateIssuer = false,
                ValidateAudience = false,//it is like who generate this token and we compare it when we get that(read in documentation)
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        }

        internal static SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }
    }
}
