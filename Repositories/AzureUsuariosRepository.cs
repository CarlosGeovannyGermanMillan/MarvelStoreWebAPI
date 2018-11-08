using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using TiendaMarvel.Repositories;

namespace TiendaMarvel.Models{
   
    public class AzureUsuariosRepository : IUsuariosRepository
    {
        private string azureConStr;
        public AzureUsuariosRepository()
        {
            azureConStr = @"DefaultEndpointsProtocol=https;AccountName=s100ne2g3;AccountKey=/bfg7+JQpytaARoZfD5ERTGTkDhGiRKRl0K6NQfLGs2XMeQC4aNE7aEIxNOklTmRH8lAaw5aBMuCEdp51m43PQ==;EndpointSuffix=core.windows.net";
        }

        public async Task<string> CrearUsuario(UsuarioModel nuevo)
        {
                var table = TablaAzure();
                // Create the table if it doesn't exist.
                var creada = await table.CreateIfNotExistsAsync();

                var azEn= new AzUsuarioEntity(nuevo.Id);
                azEn.Id = nuevo.Id;
                azEn.CorreoElectronico = nuevo.CorreoElectronico;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(azEn);

                // Execute the insert operation.
                var x = await table.ExecuteAsync(insertOperation);

                return azEn.RowKey;
        }

        public async Task<UsuarioModel> LeerUsuario(string Id)
        {
            var table = TablaAzure();

            TableOperation retrieveOperation = TableOperation.Retrieve<AzUsuarioEntity>(
                                                Id.Substring(0,3),
                                                Id);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                var az = retrievedResult.Result as AzUsuarioEntity;
                
                return new UsuarioModel(){
                    Id = az.Id,
                    CorreoElectronico = az.CorreoElectronico,

                };
            }
            return null;
        }

        public async Task<UsuarioModel> LeerUsuarioCorreo(string Correo)
        {
            var table = TablaAzure();
            TableQuery<AzUsuarioEntity> query = new TableQuery<AzUsuarioEntity>();

            var token = new TableContinuationToken();
            var list = new List<ProductoEntity>();

            foreach (AzUsuarioEntity az in await table.ExecuteQuerySegmentedAsync(query, token))
            {
                if (az.CorreoElectronico == Correo)
                {
                    
                    return new UsuarioModel()
                    {
                        Id = az.Id,
                        CorreoElectronico = az.CorreoElectronico,

                    };
                }
            }

            return null;
        }

        public Task<List<UsuarioModel>> TodosLosUsuarios()
        {
            throw new NotImplementedException();
        
        }

        private CloudTable TablaAzure()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("UsuariosMarvel");

            return table;
        }
    }
    public class AzUsuarioEntity : TableEntity{
        public AzUsuarioEntity()
        {
            
        }

        public AzUsuarioEntity(string codigo){
            this.PartitionKey = codigo.Substring(0,3);
            this.RowKey = codigo;
            this.Id=codigo;
        }

        public String Id{get;set;}
        public String CorreoElectronico{get;set;}
    
    }
}