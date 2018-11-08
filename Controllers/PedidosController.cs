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
    [Produces("application/json")]
    [Route("api/Pedidos")]
    
    public class PedidosController : Controller
    {
        IPedidosRepository pedidos;
        public PedidosController()
        {
            //implementar uno en azure
            pedidos =  new AzurePedidosRepository();
        } 

        


        [HttpGet("fecha/{fecha}")]
        public async Task<IEnumerable<PedidoModel>>  GetAllFecha(DateTime fecha)
        {
            var resultados = await pedidos.LeerPedidosPorFecha(fecha);
            return resultados;
        }

        [HttpGet("{id}", Name = "GetPorId")]
        public async Task<IActionResult> GetById(string id)
         {
            var pedido = await pedidos.LeerPedido(id);
           return Ok(pedido);
            
        }

        [HttpGet]
        public async Task<IEnumerable<PedidoModel>> GetAll()
        {
            var model = (await pedidos.TodosLosPedidos())
                    .Select( p => new PedidoModel(){
                        PedidoId = p.PedidoId,
                        Cliente = p.Cliente,
                        Productos= p.Productos,
                        FechaPedido = p.FechaPedido,
                        Estado = p.Estado,
                        CorreoElectronico = p.CorreoElectronico,
                        Referencia = p.Referencia

                    });
            return model;
        }

        [HttpGet("Cliente/{id}")]
        public async Task<IEnumerable<PedidoModel>> GetCliente(string id)
        {
            var model = (await pedidos.LeerPedidosCliente(id))
                    .Select( p => new PedidoModel(){
                        PedidoId = p.PedidoId,
                        Cliente = p.Cliente,
                        Productos= p.Productos,
                        FechaPedido = p.FechaPedido,
                        Estado = p.Estado,
                        CorreoElectronico = p.CorreoElectronico,
                        Referencia = p.Referencia

                    });
            return model;
        }
        //Utilizar este para crear o lo que normalmente se haria en create
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoModel nuevoPedido){
            
            if(string.IsNullOrWhiteSpace(nuevoPedido.Cliente)){
                return this.BadRequest(StatusCodes.Status400BadRequest);
            }
            
            var id = await pedidos.IniciarPedido(nuevoPedido);
            nuevoPedido.PedidoId = id;
            
            return Ok(nuevoPedido);
        }

        
        [HttpPut("{id}/Avanzar/{nuevoEstado}", Name = "Avanzar")]
        public async Task<IActionResult> Put(string id,string nuevoEstado){

            var pedido = await pedidos.LeerPedido(id);

        //VALIDAR LOS ESTADOS DEL PROCESO DE PREPARACION


            if(pedido == null){
                return NotFound();
            }
            var update = pedidos.AvanzarPedido(pedido,nuevoEstado);
            return Ok();

        }

        [HttpPut("{id}/Completar", Name = "Completar")]
        public async Task<IActionResult> Put(string id){

            var pedido = await pedidos.LeerPedido(id);

            if(pedido == null){
                return NotFound();
            }
            var update = pedidos.CompletarPedido(pedido);
            return Ok();

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Delete(string id){
            
            var pedido = await pedidos.LeerPedido(id);
            if(pedido ==null){
                return NotFound();
            }
            //VALIDAR QUE NO SE PUEDA CANCELAR SI YA ESTA COMPLETO EL PEDIDO
            var res = await pedidos.CancelarPedido(pedido);
            return Ok();
        }
    }
}