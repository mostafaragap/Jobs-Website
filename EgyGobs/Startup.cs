using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication1.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }

        public void CreateDefaultRolesAndUsers ()
        {
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole(); 
            if(!rolemanager.RoleExists("Admin"))
            {
                role.Name = "Admin";
                rolemanager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "Admin@gmail.com";
                user.PhoneNumber = "01158688277";
                user.usertype = "Admin";
                user.country = "Egypt";
                var check = usermanager.Create(user, "MOstafa1234_");
                if(check.Succeeded)
                {
                    usermanager.AddToRole(user.Id, "Admin"); 
                }

            }

        }
    }
}
