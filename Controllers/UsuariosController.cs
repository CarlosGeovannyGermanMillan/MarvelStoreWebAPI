using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaMarvel.Models;
using TiendaMarvel.Repositories;


namespace TiendaMarvel.Controllers
{
    [Produces("application/json")]
    [Route("api/Usuarios")]
    public class UsuariosController : Controller
    {
         IUsuariosRepository usuarios;
        public UsuariosController()
        {
            //implementar uno en azure
            usuarios =  new AzureUsuariosRepository();
        } 

        [HttpGet("{id}", Name = "GetUsuariosPorId")]
        public async Task<IActionResult> GetById(string id)
        {
            var usuario = await usuarios.LeerUsuario(id);
            return Ok(usuario);
            
        }

        [HttpGet("Correo/{id}")]
        public async Task<IActionResult> GetByCorreo(string id)
        {
            var usuario = await usuarios.LeerUsuarioCorreo(id);
            return Ok(usuario);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioModel nuevoUsuario){
            
            if(string.IsNullOrWhiteSpace(nuevoUsuario.Id)){
                return this.BadRequest(StatusCodes.Status400BadRequest);
            }
            
            var id = await usuarios.CrearUsuario(nuevoUsuario);
            nuevoUsuario.Id = id;
            
            return Ok(nuevoUsuario);
        }
    }
}