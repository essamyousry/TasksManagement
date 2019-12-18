using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;


namespace TasksAuthWebApi.AuthenticationService
{
    public class JWTAuthenticationIdentity : GenericIdentity 
    {
        public string Username { get; set; }
        public int UserID { get; set; }
        public JWTAuthenticationIdentity(string username) : base(username)
        {
            Username = username;
        }
    }

    public class AuthenticationModule
    {
        private const string communicationKey = "GQDstc21ewfffffffffffFiwDffVvVBrk";
        SecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(communicationKey));

        public string GenerateTokenForUser(string Username, int UserID)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(communicationKey));
            var now = DateTime.UtcNow;
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, Username),
                new Claim(ClaimTypes.NameIdentifier, UserID.ToString())
            }, "Custom");

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = DateTime.Now.AddHours(24),
                Issuer = "self",
                Audience = "http://www.example.com",
                SigningCredentials = signingCredentials,
                Subject = claimsIdentity
            };

            var tokenHanlder = new JwtSecurityTokenHandler();
            var plainToken = tokenHanlder.CreateToken(securityTokenDescriptor);
            var signedAndEncodedToken = tokenHanlder.WriteToken(plainToken);

            return signedAndEncodedToken;
        }

        // Decrypts string token and returns JWTSecurityToken (actual Token) not string form.
        public JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        {
            var tokenValidationParamters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    "http://www.example.com"
                },
                ValidIssuers = new string[]
                {
                    "self"
                },
                IssuerSigningKey = signingKey
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken, tokenValidationParamters, out validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
            return validatedToken as JwtSecurityToken;
        }

        public JWTAuthenticationIdentity PopulateUserIdentity(JwtSecurityToken userPayLoadToken)
        {
            string name = (userPayLoadToken).Claims.FirstOrDefault(m => m.Type == "unique_name").Value;
            string userid = (userPayLoadToken).Claims.FirstOrDefault(m => m.Type == "nameid").Value;

            return new JWTAuthenticationIdentity(name) { UserID = Convert.ToInt32(userid), Username = name };
        }

    }
}