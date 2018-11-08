using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using TiendaMarvel.Models;
using TiendaMarvel.Repositories;
using TiendaMarvel.Services;

namespace TiendaMarvel.Controllers
{
    public class SolicitarController : Controller
    {
        // GET: Catalogo
        IPedidosRepository pedidos;
        private IApplicationBuilder app;

        public SolicitarController()
        {
            pedidos =  new AzurePedidosRepository();
        } 

        public ActionResult Referencia(string id)
        {
           return View();
        }
        public async Task<ActionResult> Index()
        {
            var model = (await pedidos.LeerPedidosPagados())
                    .Select( p => new PedidoModel(){
                        PedidoId = p.PedidoId,
                        CorreoElectronico = p.CorreoElectronico,
                        FechaPedido = p.FechaPedido,
                        Referencia = p.Referencia,
                        Estado = p.Estado,
                        Productos = p.Productos
                    });

            return View(model);
        }
        public async Task<ActionResult> IndexCliente(string cliente)
        {
            var userID = User.Identity.Name;
            //string sessionID =  HttpContext.Session.ToString();
            var model = (await pedidos.LeerPedidosCliente(userID))
                    .Select( p => new PedidoModel(){
                        PedidoId = p.PedidoId,
                        CorreoElectronico = p.CorreoElectronico,
                        FechaPedido = p.FechaPedido,
                        Referencia = p.Referencia,
                        Estado = p.Estado,
                        Productos = p.Productos
                    });

            return View(model);
        }

        public async Task<ActionResult> Actualizar(string id,string estado)
        {
            if(await pedidos.Actualizar(id, estado))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> ActualizarCliente(string id,string estado)
        {
            if(await pedidos.Actualizar(id, estado))
            {
                return RedirectToAction("IndexCliente");
            }
            return RedirectToAction("IndexCliente");
        }
         public async Task<ActionResult> ActualizarReferencia(string id,string estado, string referencia)
        {
            //var userID = User.Identity.Name;
            if(await pedidos.ActualizarReferencia(id, "Sin Verificar", referencia))
            {
                return RedirectToAction("Referencia");
            }
            return RedirectToAction("Referencia");
        }

        public async Task<ActionResult> DamePedido(string id)
        {
            var model = await pedidos.LeerPedido(id);
            return View(new PedidoModel(){
                PedidoId = model.PedidoId,
                CorreoElectronico = model.CorreoElectronico,
                FechaPedido = model.FechaPedido,
                Referencia = model.Referencia,
                Estado = model.Estado,
                Productos = model.Productos
            });

        }

        public async Task<List<ProductoModel>> RegresaDetalle(string id)
        {
            var producto = await pedidos.LeerPedido(id);
            List<ProductoModel> list = JsonConvert.DeserializeObject<List<ProductoModel>>(producto.Productos);
            return list;
        }
        public async Task<ActionResult> Detalle(string x)
        {
            var y = new List<ProductoModel>();
            y = await RegresaDetalle(x);
            var model = (y)
            .Select(p => new ProductoModel(){
                id = p.id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Descripcion = p.Descripcion,
                Categoria = p.Categoria,
                Imagen = p.Imagen
            });
            return View(model);
        }
    }
}