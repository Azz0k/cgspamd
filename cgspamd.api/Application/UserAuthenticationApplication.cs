using cgspamd.api.Models;
using cgspamd.core.Context;
using cgspamd.core.Models;
using cgspamd.core.Models.APIModels;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace cgspamd.api.Application
{
    public class UserAuthenticationApplication
    {
        private StoreDbContext db;
        private IOptions<APISettings> settings;
        public UserAuthenticationApplication(IOptions<APISettings> settings, StoreDbContext storeDbContext) 
        {
            this.settings = settings;
            db = storeDbContext;
        }
        public async Task<string?> Authenticate(UserLoginRequest request)
        {
            User? user = await db.Set<User>().FirstOrDefaultAsync(user => user.UserName == request.Login);
            if (user == null || !user.Enabled)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Hash))
            {
                return null;
            }
            return GenerateJwt(user);
        }
        internal string GenerateJwt(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("UserName", user.UserName));
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim("FullName", user.FullName));
            claims.Add(new Claim("IsAdmin", user.IsAdmin.ToString()));
            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.JwtSecretCode)), SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
