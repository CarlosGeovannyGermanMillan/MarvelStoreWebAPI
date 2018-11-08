


using Microsoft.AspNetCore.Identity;
using TiendaMarvel.Models;

namespace TiendaMarvel.Data{
    public static class IdentityInitializer{

        public static void SeedRoles(RoleManager<IdentityRole> roleManager){
            if(!roleManager.RoleExistsAsync("Admin").Result){
                var adminRole = new IdentityRole("Admin");
                var res = roleManager.CreateAsync(adminRole).Result;
            }
            if(!roleManager.RoleExistsAsync("Ventas").Result){
                var ventasRole = new IdentityRole("Ventas");
                var res2=roleManager.CreateAsync(ventasRole).Result;
            }
        }


        public static void SeedUsers(UserManager<ApplicationUser> userManager){
            //var admin = userManager.FindByNameAsync("Admin").Result;
            if(userManager.FindByNameAsync("admin@myapp.com").Result==null){
                var adminUser = new ApplicationUser();
                adminUser.UserName = "admin@myapp.com";
                adminUser.Email = "admin@myapp.com";
                adminUser.PhoneNumber = "12345";
                adminUser.Token = "12345678";
                adminUser.Carrera = "Ing. TIC's";
                adminUser.DeporteFavorito = "Soccer";


                var resultado=userManager.CreateAsync(adminUser, "12345678").Result;

                if(resultado.Succeeded){
                    userManager.AddToRoleAsync(adminUser,"Admin");
                }
            }
        }
    }
}
