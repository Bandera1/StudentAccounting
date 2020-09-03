﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.IdentityModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccountingProject.Services
{
    public interface IJwtTokenService
    {
        string CreateToken(DbUser user);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly EFDbContext _context;
        private readonly IConfiguration _configuration;
        public JwtTokenService(UserManager<DbUser> userManager, EFDbContext context,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _context = context;
        }
        public string CreateToken(DbUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            roles = roles.OrderBy(x => x).ToList();
            var query = _context.Users.AsQueryable();
            var image = user.BaseProfile.Photo;
            var u = user;
            if (image == null)
            {
                image = _configuration.GetValue<string>("DefaultImage");
            }


            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.Id),
                new Claim("name",user.UserName),
                //new Claim("image",image)
            };
            foreach (var el in roles)
            {
                claims.Add(new Claim("roles", el));
            }

            //var now = DateTime.UtcNow;
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Ya-xs-scho-napysat"));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredentials,
                expires: DateTime.Now.AddDays(1),
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
