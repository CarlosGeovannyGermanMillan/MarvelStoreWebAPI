


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TiendaMarvel.Models;

namespace TiendaMarvel.Services
{
    public class CarritoService{
        private readonly ISession session;
        private readonly IPedidosRepository pedidos;
        private readonly string usuario;
        private List<PartidaEnCarrito> _partidas;

        private const string PARTIDASLISTA = "Carrito:Lista";
        private List<PartidaEnCarrito> Partidas{
            get{
                if(_partidas == null){
                    var data = session.GetString(PARTIDASLISTA);
                    if(!string.IsNullOrWhiteSpace(data)){
                        _partidas = JsonConvert.DeserializeObject<List<PartidaEnCarrito>>(data);
                    }else if (!string.IsNullOrWhiteSpace(usuario)){
                        var pars = pedidos.PartidasPorCarrito(usuario).Result;
                        _partidas = pars;
                        Guardar();
                    }
                }

                if(_partidas==null){
                    _partidas = new List<PartidaEnCarrito>();
                }
                return _partidas;
            }
        }
        public IEnumerable<PartidaEnCarrito> Contenido{
            get{
                return Partidas.AsEnumerable();
            }
        }

        public int NumeroDePartidas{
            get{
                return Partidas.Count;
            }
        }
        private void Guardar(){
            if(_partidas!=null){
                var serialized= JsonConvert.SerializeObject(_partidas);
                session.SetString(PARTIDASLISTA,serialized);
            }
            
        }

        public CarritoService(ISession session, 
                            IPedidosRepository pedidos,
                            string usuario){
            this.session = session;
            this.pedidos = pedidos;
            this.usuario = usuario;
        }

        public async Task<bool> AgregarPartida(PartidaEnCarrito nuevaPartida){
            var lista = Partidas;
            bool band =false;
            foreach (var item in lista)
            {
                if(item.Codigo == nuevaPartida.Codigo){
                    item.Cantidad++;
                    band=true;
                }
            }
            if(!band)
                lista.Add(nuevaPartida);

            if(!string.IsNullOrWhiteSpace(usuario)){
                await pedidos.AgregarPartida(nuevaPartida: nuevaPartida, carritoid: usuario);
            }

            Guardar();
            return true;
        }
        public async Task<bool> QuitarPartida(PartidaEnCarrito partida){
            var lista=Partidas;
            var find = lista.FirstOrDefault( p => partida.Codigo == partida.Codigo);
            if(find!=null){
                lista.Remove(find);
                if(!string.IsNullOrWhiteSpace(usuario)){
                    await pedidos.QuitarPartida(sale: partida, carritoid: usuario);
                }
                Guardar();
            }
            return true;
        }
        public async Task<bool> VaciarCarrito(){
           var lista= Partidas;
            if(_partidas!=null){
                _partidas.Clear();
                if(!string.IsNullOrWhiteSpace(usuario)){
                  await pedidos.VaciarPartida(carritoid: usuario);
                }
                //session.SetString(PARTIDASLISTA,string.Empty);
                Guardar();
                
            }
            //lista.Clear();
            return true;
        }


        public async Task<bool> Pagar(PedidoModel pedido){
           var lista= Partidas;
            if(_partidas!=null){
                if(!string.IsNullOrWhiteSpace(usuario)){
                  await pedidos.IniciarPedido(pedido);
                }
                //session.SetString(PARTIDASLISTA,string.Empty);
                Guardar();
                
            }

            //lista.Clear();
            return true;
        }
    }
  

    public class PartidaEnCarrito{
        public string Codigo{get;set;}
        public string Descripcion{get;set;}
        public string Nombre{get;set;}
        public decimal Precio{get;set;}
        public decimal PrecioImpuesto{get;set;}
        public int Cantidad{get;set;}
    }

}