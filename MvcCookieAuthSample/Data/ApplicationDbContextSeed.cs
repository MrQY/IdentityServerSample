using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MvcCookieAuthSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCookieAuthSample.Data
{
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUserRole> _roleManager;

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Users.Any())
            {
                _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                _roleManager = services.GetRequiredService<RoleManager<ApplicationUserRole>>();

                var defaultUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@163.com",
                    NormalizedUserName = "admin",
                    Avatar = "img.jpg"
                };

                await _userManager.AddToRoleAsync(defaultUser, "Administrators");
                var result = await _userManager.CreateAsync(defaultUser, "admin");
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败");
                }
            }
        }
    }
}
