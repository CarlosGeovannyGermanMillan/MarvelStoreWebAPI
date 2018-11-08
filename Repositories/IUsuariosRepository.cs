using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMarvel.Models;

namespace TiendaMarvel.Repositories{


    public interface IUsuariosRepository
    {
        Task<List<UsuarioModel>> TodosLosUsuarios();
        Task<UsuarioModel> LeerUsuario(String Id);
        Task<UsuarioModel> LeerUsuarioCorreo(String Correo);
        Task<string> CrearUsuario(UsuarioModel nuevoUsuario);
    }
}