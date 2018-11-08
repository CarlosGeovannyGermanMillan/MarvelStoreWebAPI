using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMarvel.Models;

namespace TiendaMarvel.Repositories{


    public interface IProductosRepository
    {
        Task<List<ProductoEntity>> LeerProductosMasPopulares(); 
        Task<List<ProductoEntity>> NueveProductos();
        Task<List<ProductoEntity>> TodosLosProductos();
        Task<List<ProductoEntity>> ProductosPorCategoria(String categoria);

        Task<ProductoEntity> LeerProducto(String codigo);
        Task<bool> CrearProducto(ProductoEntity nuevo);
        Task<bool> ActualizarDatos(ProductoEntity producto);

        Task<bool> ActualizarImagen(ProductoEntity producto, object imagen);

        Task<bool> BorrarProducto(String codigo);
    }

    public class MemoryProductosRepository : IProductosRepository
    {
        public Task<bool> ActualizarDatos(ProductoEntity producto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActualizarImagen(ProductoEntity producto, object imagen)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BorrarProducto(string codigo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CrearProducto(ProductoEntity nuevo)
        {
            throw new NotImplementedException();
        }

        public Task<ProductoEntity> LeerProducto(string codigo)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductoEntity>> LeerProductosMasPopulares()
        {
            return Task.FromResult(
                new List<ProductoEntity>(){
                    new ProductoEntity(){
                        Codigo = "ABCD",
                        Descripcion = "Una Descripcion",
                        ImagenURL = "https://vignette.wikia.nocookie.net/dragonball/images/7/71/Goku_super_saiyan_blue_full_power-episode_124.png/revision/latest?cb=20180105160303",
                        Precio = 100
                    },
                    new ProductoEntity(){
                        Codigo = "ABCDE",
                        Descripcion = "Dos Descripcion",
                        ImagenURL = "https://vignette.wikia.nocookie.net/dragonball/images/7/71/Goku_super_saiyan_blue_full_power-episode_124.png/revision/latest?cb=20180105160303",
                        Precio = 106
                    }
                }
            );
        }

        public Task<List<ProductoEntity>> NueveProductos()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductoEntity>> ProductosPorCategoria(string categoria)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductoEntity>> TodosLosProductos()
        {
            throw new NotImplementedException();
        }
    }
}