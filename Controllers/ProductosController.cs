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
    [Route("api/Productos")]
    public class ProductosController : Controller
    {
        IProductosRepository productos;
        public ProductosController()
        {
            productos =  new AzureProductosRepository();
        } 


        [HttpGet]
        public async Task<IEnumerable<ProductoModel>> GetAll()
        {
            var model = (await productos.TodosLosProductos())
                    .Select( p => new ProductoModel(){
                        id = p.Codigo,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Categoria = p.Categoria,
                        Precio = p.Precio,
                        PrecioImpuesto = p.PrecioImpuesto,
                        Imagen = p.ImagenURL
                    });
            return model;
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(string id)
        {

            var model = await productos.LeerProducto(id);
            return new ObjectResult(model);
            
        }
        [HttpGet("PorCategoria/{id}")]
        public async Task<IEnumerable<ProductoModel>> PorCategoria(string id)
        {
            var model = (await productos.ProductosPorCategoria(id))
                    .Select(p => new ProductoModel()
                    {
                        id = p.Codigo,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Categoria = p.Categoria,
                        Precio = p.Precio,
                        PrecioImpuesto = p.PrecioImpuesto,
                        Imagen = p.ImagenURL
                    });
            return model;
        }

        [HttpGet("ProductosPorCategoria/{id}", Name = "GetTodoCategoria")]
        public async Task<IEnumerable<ProductoModel>> GetAllCategoria(string id)
        {
            var model = (await productos.ProductosPorCategoria(id))
                    .Select(p => new ProductoModel()
                    {
                        id = p.Codigo,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Categoria = p.Categoria,
                        Precio = p.Precio,
                        PrecioImpuesto = p.PrecioImpuesto,
                        Imagen = p.ImagenURL
                    });
            return model;
        }

    }
}