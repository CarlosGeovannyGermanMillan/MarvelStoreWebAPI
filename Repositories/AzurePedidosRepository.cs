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

    public class AzurePedidosRepository : IPedidosRepository
    {
        private readonly string azureConStr;

        public AzurePedidosRepository()
        {
            //Todo pass this int configuration
            azureConStr = @"DefaultEndpointsProtocol=https;AccountName=s100ne2g3;AccountKey=/bfg7+JQpytaARoZfD5ERTGTkDhGiRKRl0K6NQfLGs2XMeQC4aNE7aEIxNOklTmRH8lAaw5aBMuCEdp51m43PQ==;EndpointSuffix=core.windows.net";
        } 

        public async Task<bool> AvanzarPedido(PedidoModel pedido, string nuevoEstado)
        {
            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<PedidoAzEntity>(
                                PedidoAzEntity.PartitionFromRowId(pedido.PedidoId),
                                pedido.PedidoId
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as PedidoAzEntity;
                p.Estado = nuevoEstado;

                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }else{
                return false;
            }

        }

        public async Task<bool> CancelarPedido(PedidoModel pedido)
        {
            return await AvanzarPedido (pedido,"Cancelado");
        }

        public async Task<bool> CompletarPedido(PedidoModel pedido)
        {
            return await AvanzarPedido (pedido,"Completado");
        }

        public async Task<string> IniciarPedido(PedidoModel nuevoPedido)
        {
            
            var table = TablaAzure();
                // Create the table if it doesn't exist.
                var creada = await table.CreateIfNotExistsAsync();
                var azEn= new PedidoAzEntity(nuevoPedido.FechaPedido,nuevoPedido.CorreoElectronico);
                //azEn.PedidoId=nuevoPedido.PedidoId;
                azEn.Cliente = nuevoPedido.Cliente;
                azEn.Productos =  nuevoPedido.Productos;
                azEn.FechaPedido=nuevoPedido.FechaPedido;
                azEn.Estado = "No Pagado";
                azEn.Referencia = "Vacio";
                azEn.CorreoElectronico = nuevoPedido.CorreoElectronico;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(azEn);

                // Execute the insert operation.
                var x = await table.ExecuteAsync(insertOperation);

                return azEn.RowKey;
        }

        public async Task<PedidoModel> LeerPedido(string id)
        {
            var table = TablaAzure();

            TableOperation retrieveOperation = TableOperation.Retrieve<PedidoAzEntity>(
                                                PedidoAzEntity.PartitionFromRowId(id),
                                                id
                                                );

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                var az = retrievedResult.Result as PedidoAzEntity;
                return new PedidoModel(){
                    PedidoId = az.PedidoId,
                    Cliente = az.Cliente,
                    FechaPedido = az.FechaPedido,
                    Estado = az.Estado,
                    Productos =  az.Productos,
                    Referencia = az.Referencia
                };
            }
            return null;
        }
        public async Task<List<PedidoModel>> LeerPedidosPorFecha(DateTime fecha)
        {
            var tabla = TablaAzure();
            
            var particion = PedidoAzEntity.PartitionKeyFromFecha(fecha);
            TableQuery<PedidoAzEntity> query = new TableQuery<PedidoAzEntity>()
                                        .Where(
                                            TableQuery.GenerateFilterCondition("PartitionKey",
                                            QueryComparisons.Equal,particion));

            // Print the phone number of the result.
            var token = new TableContinuationToken();
            var list = new List <PedidoModel>();

            foreach(PedidoAzEntity az in await tabla.ExecuteQuerySegmentedAsync(query,token))
            {
                list.Add(new PedidoModel(){
                    PedidoId = az.PedidoId,
                    Cliente = az.Cliente,
                    FechaPedido = az.FechaPedido,
                    CorreoElectronico = az.CorreoElectronico,
                    Estado = az.Estado,
                    Productos = az.Productos,
                    Referencia = az.Referencia
                });
            }
            return list;
        }

        public async Task<List<PedidoModel>> TodosLosPedidos()
        {
            var table = TablaAzure();
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<PedidoAzEntity> query = new TableQuery<PedidoAzEntity>();
            //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            var token = new TableContinuationToken();
            var list = new List<PedidoModel>();
            // Print the fields for each customer.
            foreach (PedidoAzEntity entity in await table.ExecuteQuerySegmentedAsync(query,token))
            {
                list.Add(new PedidoModel(){
                    PedidoId = entity.PedidoId,
                    Cliente =entity.Cliente,
                    Productos = entity.Productos,
                    FechaPedido = entity.FechaPedido,
                    CorreoElectronico = entity.CorreoElectronico,
                    Estado = entity.Estado,
                    Referencia = entity.Referencia
                });
            }
            return list;
        }
        public Task<List<PartidaEnCarrito>> PartidasPorCarrito(string carritoid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> QuitarPartida(PartidaEnCarrito sale, string carritoid)
        {
            throw new NotImplementedException();
        }
        public Task<bool> VaciarPartida(string carritoid)
        {
            throw new NotImplementedException();
        }
        public Task<bool> AgregarPartida(PartidaEnCarrito nuevaPartida, string carritoid)
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
            CloudTable table = tableClient.GetTableReference("PedidosMarvel");

            return table;
        }

        public async Task<List<PedidoModel>> LeerPedidosCliente(string Cliente)
        {
            var tabla = TablaAzure();
            
            //var particion = PedidoAzEntity.PartitionKeyFromFecha(fecha);
            TableQuery<PedidoAzEntity> query = new TableQuery<PedidoAzEntity>();
                                       

            // Print the phone number of the result.
            var token = new TableContinuationToken();
            var list = new List <PedidoModel>();

            foreach(PedidoAzEntity az in await tabla.ExecuteQuerySegmentedAsync(query,token))
            {
                if(az.CorreoElectronico == Cliente){
                    list.Add(new PedidoModel(){
                    PedidoId = az.PedidoId,
                    Cliente = az.Cliente,
                    FechaPedido = az.FechaPedido,
                    CorreoElectronico = az.CorreoElectronico,
                    Estado = az.Estado,
                    Productos = az.Productos,
                    Referencia = az.Referencia
                });
                }          
            }
            return list;
        }

        public async Task<List<PedidoModel>> LeerPedidosPagados()
        {
            var table = TablaAzure();
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<PedidoAzEntity> query = new TableQuery<PedidoAzEntity>();
            //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            var token = new TableContinuationToken();
            var list = new List<PedidoModel>();
            // Print the fields for each customer.
            foreach (PedidoAzEntity entity in await table.ExecuteQuerySegmentedAsync(query,token))
            {
               // if(entity.Referencia!="Vacio" && entity.Estado!="Cancelado"){
                    list.Add(new PedidoModel(){
                        PedidoId = entity.PedidoId,
                        Cliente =entity.Cliente,
                        Productos = entity.Productos,
                        FechaPedido = entity.FechaPedido,
                        CorreoElectronico = entity.CorreoElectronico,
                        Estado = entity.Estado,
                        Referencia = entity.Referencia
                    });
                //}
            }
            return list;
        }
        public async Task<bool> Actualizar(string id, string nuevoEstado)
        {
            PedidoModel pedido = await LeerPedido(id);
            return await AvanzarPedido(pedido,nuevoEstado);           
        }

        public async Task<bool> ActualizarReferencia(string id, string nuevoEstado, string referencia)
        {
            PedidoModel pedido = await LeerPedido(id);
            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<PedidoAzEntity>(
                                PedidoAzEntity.PartitionFromRowId(pedido.PedidoId),
                                pedido.PedidoId
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as PedidoAzEntity;
                p.Estado = nuevoEstado;
                p.Referencia = referencia;

                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }else{
                return false;
            }
        }
    }


    public class PedidoAzEntity : TableEntity {

        public static string PartitionFromRowId(string id){
            if(string.IsNullOrWhiteSpace(id) || id.Length < 9){
                return string.Empty;
            }
            return id.Substring(0,9);
        }
         public static string RowKeyFromFechayCorreo(DateTime fecha, string correo) 
            =>PartitionKeyFromFecha(fecha) + "_" +fecha.ToString("HH_mm_ss") + "_" + correo;

        public static string PartitionKeyFromFecha(DateTime fecha)
            => "p" + fecha.ToString("yyyyMMdd");
        public PedidoAzEntity()
        {
        }
        public PedidoAzEntity(DateTime fechaPedido, string correo){
            PartitionKey = PartitionKeyFromFecha(fechaPedido);
            RowKey = RowKeyFromFechayCorreo(fechaPedido,correo);
            FechaPedido = fechaPedido;
        }

        public string PedidoId{get{return RowKey;}}
        public string CorreoElectronico{get;set;}
        public string Cliente {get;set;}
        public string Productos {get;set;}
        public DateTime FechaPedido{get;set;}
        public string Estado {get;set;}
        public string Referencia{get;set;}

    }
}