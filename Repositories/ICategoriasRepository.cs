using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMarvel.Models;

public interface ICategoriasRepository
{
    Task<List<CategoriaModel>> LeerTodas();
    Task<CategoriaModel> LeerPorId(string id);
    Task<bool> UpdateCategoria(CategoriaModel update);
    Task<bool> CrearCategoria(CategoriaModel nuevo);
    Task<bool> ActualizarCategoria();
    Task<bool> BorrarCategoria(string id);
}

public class MemoryCategoriasRepository : ICategoriasRepository
{
    public Task<bool> ActualizarCategoria()
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> BorrarCategoria(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> CrearCategoria(CategoriaModel nuevo)
    {
        throw new System.NotImplementedException();
    }

    public Task<CategoriaModel> LeerPorId(string id)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<CategoriaModel>> LeerTodas()
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> UpdateCategoria(CategoriaModel update)
    {
        return Task.FromResult(true);
    }
}
//Agregò el MemoryCategoriaRepository 
//Se tiene que hacer la implementacion en Azure