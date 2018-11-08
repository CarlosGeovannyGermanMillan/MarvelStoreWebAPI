
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMarvel.Models;

namespace TiendaMarvel.Services
{
    public interface IPedidosRepository{
        Task<bool> AgregarPartida(PartidaEnCarrito nuevaPartida, string carritoid);
        Task<List<PartidaEnCarrito>> PartidasPorCarrito(string carritoid);
        Task<bool> QuitarPartida(PartidaEnCarrito sale, string carritoid);
        Task<bool> VaciarPartida(string carritoid); 
        Task<List<PedidoModel>> TodosLosPedidos();
        Task<List<PedidoModel>> LeerPedidosPorFecha(DateTime fecha);
        Task<List<PedidoModel>> LeerPedidosCliente(string Cliente);
        Task<List<PedidoModel>> LeerPedidosPagados();
        Task<string> IniciarPedido(PedidoModel nuevoPedido);
        Task<bool> AvanzarPedido(PedidoModel pedido, string nuevoEstado);
        Task<bool> Actualizar(string id , string nuevoEstado);
        Task<bool> ActualizarReferencia(string id , string nuevoEstado,string referencia);
        Task<bool> CancelarPedido(PedidoModel pedido);
        Task<bool> CompletarPedido(PedidoModel pedido);
        Task<PedidoModel> LeerPedido(string id);
    }
    public class AzPedidosRepository : IPedidosRepository
    {
        public Task<bool> Actualizar(string id, string nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActualizarReferencia(string id, string nuevoEstado, string referencia)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AgregarPartida(PartidaEnCarrito en, string carritoid)
        {
            return Task.FromResult(true);
        }

        public Task<bool> AvanzarPedido(PedidoModel pedido, string nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelarPedido(PedidoModel pedido)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CompletarPedido(PedidoModel pedido)
        {
            throw new NotImplementedException();
        }

        public Task<string> IniciarPedido(PedidoModel nuevoPedido)
        {
            throw new NotImplementedException();
        }

        public Task<PedidoModel> LeerPedido(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<PedidoModel>> LeerPedidosCliente(string Cliente)
        {
            throw new NotImplementedException();
        }

        public Task<List<PedidoModel>> LeerPedidosPagados()
        {
            throw new NotImplementedException();
        }

        public Task<List<PedidoModel>> LeerPedidosPorFecha(DateTime fecha)
        {
            throw new NotImplementedException();
        }

        

        public Task<List<PartidaEnCarrito>> PartidasPorCarrito(string carritoid)
        {
            return Task.FromResult(new List<PartidaEnCarrito>());
        }

        public Task<bool> QuitarPartida(PartidaEnCarrito sale, string carritoid)
        {
            return Task.FromResult(true);
        }

        public Task<List<PedidoModel>> TodosLosPedidos()
        {
            throw new NotImplementedException();
        }

        public Task<bool> VaciarPartida(string carritoid)
        {
            return Task.FromResult(true);
        }
    }
}