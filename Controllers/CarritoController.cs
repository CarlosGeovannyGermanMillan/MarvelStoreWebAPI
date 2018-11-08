using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaMarvel.Models;
using TiendaMarvel.Services;

namespace TiendaMarvel.Controllers
{
    public class CarritoController : Controller
    {
        private readonly CarritoService carrito;
        IPedidosRepository pedidos;
        public CarritoController(CarritoService carrito){
            this.carrito = carrito;
            pedidos =  new AzurePedidosRepository();
        }
        // GET: Carrito
        public ActionResult Index()
        {
           var model = carrito.Contenido;
            return View(model);
        }
        public ActionResult Resumen()
        {
           var model = carrito.Contenido;
            return View(model);
        }
        public ActionResult ProcesoPago()
        {
           var model = carrito.Contenido;
            return View(model);
        }
        public ActionResult Ticket()
        {
           var model = carrito.Contenido;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(PartidaEnCarrito partida){
            //Add something
            await carrito.AgregarPartida(partida);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Quitar(PartidaEnCarrito partida){
            await carrito.QuitarPartida(partida);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Aumentar(PartidaEnCarrito partida){
            await carrito.QuitarPartida(partida);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Vaciar(){
            await carrito.VaciarCarrito();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> VaciarPago(){
            await carrito.VaciarCarrito();
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> Pagar(PedidoModel pedido){
            
            await carrito.Pagar(pedido);
            return RedirectToAction(nameof(Index));
        }
    }
}