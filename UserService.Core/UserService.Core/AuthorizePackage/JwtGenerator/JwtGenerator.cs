using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace UserService.Core.JwtGenerator
{
    public class JwtGenerator : IJwtGenerator
    {
        public const string ClientIp = "ClientIp";

        public const string UserId = JwtRegisteredClaimNames.NameId;
        public const string UserEmail = JwtRegisteredClaimNames.Email;
        public const string UserName = JwtRegisteredClaimNames.UniqueName;
        public const string UserKey = JwtRegisteredClaimNames.Iss;

        public const string ValidTo = JwtRegisteredClaimNames.Exp;

        public const string Role = JwtRegisteredClaimNames.Actort;

        public const string GeneratePassword = JwtRegisteredClaimNames.Jti;
        
        public const string AcceptRanges = nameof(HeaderNames.AcceptRanges);

        private readonly ProjectOptions _projectOptions;

        public JwtGenerator(ProjectOptions options)
        {
            _projectOptions = options;
        }

        public string CreateToken(Guid userId, string userName, string userEmail, string userKey, IEnumerable<string> roles, IEnumerable<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            List<Claim> tokenClaims = new();

            tokenClaims.AddRange(claims);

            if (_projectOptions.AcceptRanges)
            {
                tokenClaims.Add(new Claim(AcceptRanges, true.ToString()));
            }

            tokenClaims.Add(new Claim(UserEmail, userEmail ?? string.Empty));
            tokenClaims.Add(new Claim(UserName, userName));
            tokenClaims.Add(new Claim(UserId, userId.ToString()));
            tokenClaims.Add(new Claim(UserKey, userKey));

            foreach (string role in roles)
            {
                tokenClaims.Add(new Claim(Role, role));
            }

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_projectOptions.IdentitySecret));

            return tokenHandler.WriteToken(tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(tokenClaims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            }));
        }

        public TokenMeta GetTokenData(string token)
        {
            token = token
                .Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty)
                .Trim();

            JwtSecurityToken jwt = new JwtSecurityTokenHandler()
                .ReadToken(token)
                as JwtSecurityToken;

            string userName = jwt.Claims.Single(c => c.Type == UserName).Value;
            string userKey = jwt.Claims.Single(c => c.Type == UserKey).Value;
            Guid userId = Guid.Parse(jwt.Claims.Single(c => c.Type == UserId).Value);

            string jti = jwt.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value ?? false.ToString();

            DateTime validTo = jwt.ValidTo;

            return new(userName, userId, userKey, validTo, bool.Parse(jti));
        }
    }
}
