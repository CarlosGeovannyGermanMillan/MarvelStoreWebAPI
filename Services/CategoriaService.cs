using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace TiendaMarvel.Services
{

    public class Categoria{
        private readonly ISession session;
        private readonly ICategoriasRepository categorias;
        private readonly string usuario;
        private List<Categoria> _categorias;

    }
}