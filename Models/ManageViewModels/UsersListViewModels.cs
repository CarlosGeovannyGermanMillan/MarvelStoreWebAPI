using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaMarvel.Models.ManageViewModels
{

    public class UserInSystemViewModel{
        public string Email {get;set;}
        public string Roles{get;set;}
        public string UserName{get;set;}
    }

    public class CambiarRolViewModel{
        
        public CambiarRolViewModel(){
            RolesDisponibles = new List<string>();
        }
        public string UserName {get;set;}
        public string Email {get;set;}
        public string Roles{get;set;}
        public List<string> RolesDisponibles{get; set;} 
    }
}