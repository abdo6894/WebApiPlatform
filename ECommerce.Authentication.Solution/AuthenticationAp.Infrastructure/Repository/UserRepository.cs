using AuthenticationAp.Application.DTOs;
using AuthenticationAp.Application.Interfaces;
using AuthenticationAp.Infrastructure.Data;
using AuthenticationApi.Domain.Entites;
using ecommrece.sharedliberary.Logs;
using ecommrece.sharedliberary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAp.Infrastructure.Repository
{
    public class UserRepository(AuthenticationDBContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
           var getuser= await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return getuser is null ? null! : getuser!;
        }
        public async Task<GetUserDTO> GetUser(int UserId)
        {
           var getuser= await context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            return getuser is null ? null! : new GetUserDTO(
                getuser.Id,
                getuser.Name,
                getuser.TelephoneNumber,
                getuser.Address,
                getuser.Email,
                getuser.Role

                );

        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            try
            {
                var getuser = await GetUserByEmail(loginDTO.Email);
                if (getuser is null)
                {
                    return new Response(false, "Invalid email or password");
                }
                bool verfiypassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getuser.Password);
                if (!verfiypassword)
                {
                    return new Response(false, "Invalid email or password");
                }
                var token = GenerateToken(getuser, config);
                return new Response(true, token);
            }
            catch (Exception ex)
            {
                LogException.LogToConsole(ex.Message);
                return new Response(false, ex.Message);
            }

        }
        private static string GenerateToken(AppUser user, IConfiguration config)
        {
            var keyString = config["Authentication:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new Exception("JWT Key is missing in configuration.");

            var key = Encoding.UTF8.GetBytes(keyString);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

            // شرطك هنا كان غلط: كنت بتضيف Role لو كانت فاضية أو = "string"
            if (!string.IsNullOrEmpty(user.Role) && user.Role != "string")
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
            }

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var getuser = await GetUserByEmail(appUserDTO.Email);
             if (getuser is not  null)
            {
                return new Response(false," YOU CAN NOT REGISTER THIS USER, EMAIL ALREADY EXISTS");
            }
             var user = new AppUser
            {
                Name = appUserDTO.Name,
                TelephoneNumber = appUserDTO.TelephoneNumber,
                Address = appUserDTO.Address,
                Email = appUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                 Role = appUserDTO.Role
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.Id> 0
                ? new Response(true, "User registered successfully")
                : new Response(false, "User registration failed, please try again later");
        }
    }


}
