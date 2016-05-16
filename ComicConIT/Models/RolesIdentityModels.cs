using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ComicConIT.Models
{
    public class AppDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);

            // создаем пользователей
            var admin = new ApplicationUser { Email = "ITRAUSER@mail.ru", UserName = "ITRAUSER@mail.ru", Id="1234" };
            string password = "12345678";
            var AdminResult = userManager.Create(admin, password);
         
            var user = new ApplicationUser { Email = "user@user.ru", UserName = "user@user.ru", Id="5678" };
            var UserResult = userManager.Create(user, password);

            // если создание пользователя прошло успешно
            if (AdminResult.Succeeded && UserResult.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
                userManager.AddToRole(admin.Id, role2.Name);

                userManager.AddToRole(user.Id, role2.Name);
            }

            base.Seed(context);
        }
    }
}
