using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TestTaskMVCAuth.Models
{
    public class AuthContext : IdentityDbContext<AuthUser>
    {
        public AuthContext() : base("AuthDb")
        {            
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }

    public class AuthDbInit : DropCreateDatabaseIfModelChanges<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {
            var userManager = new AuthUserManager(new UserStore<AuthUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var role1 = new IdentityRole("admin");
            var role2 = new IdentityRole("user1");
            var role3 = new IdentityRole("user2");

            
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            var admin = new AuthUser{ Email = "simplemail@gmail.com", UserName = "simplemail@gmail.com" };
            string password = "zKut87_y5";
            var result = userManager.Create(admin, password);

            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);
                userManager.AddToRole(admin.Id, role3.Name);
            }

            var user1 = new AuthUser { Email = "diffmail@gmail.com", UserName = "diffmail@gmail.com" };
            password = "zKut87_y6";
            result = userManager.Create(user1, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(user1.Id, role2.Name);
            }

            var user2 = new AuthUser { Email = "closedmail@gmail.com", UserName = "closedmail@gmail.com" };
            password = "zKut87_y7";
            result = userManager.Create(user2, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(user2.Id, role3.Name);
            }

            base.Seed(context);
        }        
    }
}