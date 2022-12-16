using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Azure.Identity;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using Constants;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsForumApi.Database.Repositories;

namespace BusinessLogic.Services.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<LoginResult> SingIn(Login user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            user.Password = PasswordHelper.GenerateSHA256(user.Password);

            if (dbUser == null || dbUser.Password != user.Password)
            {
                throw new AuthenticationException("Wrong username or password");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, dbUser.Role)
            };
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var jwt = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256));

            return new LoginResult
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
                Role = dbUser.Role,
                Token = new JwtSecurityTokenHandler().WriteToken(jwt)
            };
        }

        public async Task CreateAccount(Login user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            if (dbUser != null)
            {
                throw new AuthenticationFailedException("User already exist");
            }

            dbUser = new User
                { Name = user.Name, Password = PasswordHelper.GenerateSHA256(user.Password), Role = "user" };
            await _context.AddAsync(dbUser);
            await _context.SaveChangesAsync();
        }
    }
}
