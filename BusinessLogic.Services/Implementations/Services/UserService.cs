using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Azure.Identity;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using Constants;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsForumApi.Database.Repositories;

namespace BusinessLogic.Services.Implementations.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtOptions _jwtOptions;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext context, IOptions<JwtOptions> jwtOptions, IMapper mapper)
        {
            _context = context;
            _jwtOptions = jwtOptions.Value;
            _mapper = mapper;
        }

        public async Task<LoginResult> SingIn(Login user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            user.Password = PasswordHelper.GenerateSHA256(user.Password);

            if (dbUser == null || dbUser.Password != user.Password)
            {
                throw new AuthenticationException("Wrong username or password");
            }

            if (dbUser.Role == Roles.banRole)
            {
                throw new AuthenticationException("User banned");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, dbUser.Role)
            };
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(20)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
                    SecurityAlgorithms.HmacSha256));

            return new LoginResult
            {
                Id = dbUser.Id,
                Name = dbUser.Name,
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
                { Name = user.Name, Password = PasswordHelper.GenerateSHA256(user.Password), Role = Roles.userRole};
            await _context.AddAsync(dbUser);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeUserRole(NameAndRole user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            if (dbUser == null)
            {
                throw new Exception("User not exist");
            }

            if (user.Role is Roles.userRole or Roles.adminRole or Roles.banRole or Roles.moderatorRole) 
            {
                dbUser.Role = user.Role;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Wrong Role");
            }

        }

        public async Task DeleteAccount(NameAndRole user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            if (dbUser == null)
            {
                throw new Exception("User not exist");
            }

            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
        }

        public async Task BanAccount(NameAndRole user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            if (dbUser == null)
            {
                throw new Exception("User not exist");
            }

            dbUser.Role = Roles.banRole;

            await _context.SaveChangesAsync();
        }

        public async Task UnBanAccount(NameAndRole user)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(a => a.Name == user.Name);
            if (dbUser == null)
            {
                throw new Exception("User not exist");
            }

            if (dbUser.Role != Roles.banRole)
            {
                throw new Exception("User not banned");
            }

            dbUser.Role = Roles.userRole;

            await _context.SaveChangesAsync();
        }

        public async Task<IList<NameAndRole>> GetUsersList()
        {
            return _context.Users.Select(_mapper.Map<NameAndRole>).ToList();
        }
    }
}
