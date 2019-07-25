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

                var defaultRole = new ApplicationUserRole
                {
                    Name = "Administrators",
                    NormalizedName = "Administrators"
                };
                await _roleManager.CreateAsync(defaultRole);
                var defaultUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@163.com",
                    NormalizedUserName = "admin",
                    Avatar = "img.jpg",
                    SecurityStamp=DateTime.Now.Ticks.ToString()
                };

                var result = await _userManager.CreateAsync(defaultUser, "admin");
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败");
                }

                await _userManager.AddToRoleAsync(defaultUser, "Administrators");
                
            }
        }
    }
}
