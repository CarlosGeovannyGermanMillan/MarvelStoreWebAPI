using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TiendaMarvel.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        public string Carrera {get;set;}
        public string Token {get;set;}
        public string DeporteFavorito{get;set;}
    }
}
