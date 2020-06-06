using Chat.BusinessLogic.Helpers;
using Chat.Contracts.ConfigurationObjects;
using Chat.Contracts.Constats.GeneralConstants;
using Chat.Contracts.Entity;
using Chat.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BusinessLogic.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> GetAccessTokenAsync(User user, JwtSettings jwtSettings)
        {
            var claimsIdentity = await GetClaimsIdentityAsync(user);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(jwtSettings.LifeTime)),
                signingCredentials: new SigningCredentials(AuthenticationHelper.GetSymmetricSecurityKey(jwtSettings.SecretKey),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentityAsync(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                   // new Claim(GeneralApiConstants.RefreshToken, user.RefreshToken)
                };

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    var role = await this._roleManager.FindByNameAsync(userRole);
                    if (role == null)
                    {
                        continue;
                    }

                    var roleClaims = await this._roleManager.GetClaimsAsync(role);

                    foreach (var roleClaim in roleClaims)
                    {
                        if (claims.Contains(roleClaim))
                        {
                            continue;
                        }

                        claims.Add(roleClaim);
                    }
                }

                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, ApiConstants.Token, ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
    }
}
