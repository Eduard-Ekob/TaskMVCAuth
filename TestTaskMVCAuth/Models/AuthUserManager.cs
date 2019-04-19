using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace TestTaskMVCAuth.Models
{
    public class AuthUserManager : UserManager<AuthUser>
    {
        public AuthUserManager(IUserStore<AuthUser> store) : base(store)
        {            
        }

        public static AuthUserManager Create(IdentityFactoryOptions<AuthUserManager> options, IOwinContext context)
        {
            AuthContext db = context.Get<AuthContext>();
            AuthUserManager manager = new AuthUserManager(new UserStore<AuthUser>(db));
            return manager;
        }
    }
}