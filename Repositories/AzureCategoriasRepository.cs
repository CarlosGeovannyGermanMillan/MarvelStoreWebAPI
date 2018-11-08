using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using TiendaMarvel.Models;
using TiendaMarvel.Services;

namespace TiendaMarvel.Models{

    public class AzureCategoriasRepository : ICategoriasRepository
    {
        private readonly string azureConStr;

        public AzureCategoriasRepository()
        {
            //Todo pass this int configuration
            azureConStr = @"DefaultEndpointsProtocol=https;AccountName=s100ne2g3;AccountKey=/bfg7+JQpytaARoZfD5ERTGTkDhGiRKRl0K6NQfLGs2XMeQC4aNE7aEIxNOklTmRH8lAaw5aBMuCEdp51m43PQ==;EndpointSuffix=core.windows.net";
        }

        public Task<bool> ActualizarCategoria()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> BorrarCategoria(string id)
        {
            //throw new NotImplementedException();
            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<CategoriaAzEntity>(
                                id.Substring(0,3),
                                id
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as CategoriaAzEntity;
                var upOp = TableOperation.Delete(p);
                await table.ExecuteAsync(upOp);
                return true;
            }else{
                return false;
            }
        }

        public async Task<bool> CrearCategoria(CategoriaModel nuevo)
        {
            var table = TablaAzure();
                // Create the table if it doesn't exist.
                var creada = await table.CreateIfNotExistsAsync();

                var azEn= new CategoriaAzEntity(nuevo.CategoriaCodigo);
                azEn.Descripcion = nuevo.Descripcion;
                azEn.CategoriaCodigo= nuevo.CategoriaCodigo;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(azEn);

                // Execute the insert operation.
                var x = await table.ExecuteAsync(insertOperation);

                return true;
        }

        public async Task<CategoriaModel> LeerPorId(string id)
        {
            var table = TablaAzure();

            TableOperation retrieveOperation = TableOperation.Retrieve<CategoriaAzEntity>(
                                                id.Substring(0,3),
                                                id);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                var az = retrievedResult.Result as CategoriaAzEntity;
                return new CategoriaModel(){
                    CategoriaCodigo = az.CategoriaCodigo,
                    Descripcion = az.Descripcion
                };
            }
            return null;
        }

        public async Task<List<CategoriaModel>> LeerTodas()
        {
            var table = TablaAzure();

            TableQuery<CategoriaAzEntity> query = new TableQuery<CategoriaAzEntity>();

            var token = new TableContinuationToken();
            var list = new List<CategoriaModel>();

            foreach (CategoriaAzEntity entity in await table.ExecuteQuerySegmentedAsync(query,token))
            {
                list.Add(new CategoriaModel(){
                    CategoriaCodigo = entity.CategoriaCodigo,
                    Descripcion = entity.Descripcion
                });
            }
            return list;
        }

        public async Task<bool> UpdateCategoria(CategoriaModel update)
        {
            //throw new NotImplementedException();
            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<CategoriaAzEntity>(
                                update.CategoriaCodigo.Substring(0,3),
                                update.CategoriaCodigo
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as CategoriaAzEntity;
                p.Descripcion = update.Descripcion;

                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }else{
                return false;
            }          
        }
        private CloudTable TablaAzure()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("CategoriasMarvel");

            return table;
        }
    }
    
    public class CategoriaAzEntity : TableEntity {

    
        public CategoriaAzEntity()
        {
        }
        public CategoriaAzEntity(string codigo){
            this.PartitionKey = codigo.Substring(0,3);
            this.RowKey = codigo;
            this.CategoriaCodigo=codigo;
        }

        public string CategoriaCodigo{get;set;}
        public string Descripcion {get;set;}

    }
}