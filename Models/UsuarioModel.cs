using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TiendaMarvel.Models{
    public class UsuarioModel{
        public string Id{get;set;}
        public string CorreoElectronico{get;set;}
    }
}