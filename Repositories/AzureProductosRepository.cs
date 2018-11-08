using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using TiendaMarvel.Repositories;

namespace TiendaMarvel.Models{
   
    public class AzureProductosRepository : IProductosRepository
    {
        private string azureConStr;
        public AzureProductosRepository()
        {
            azureConStr = @"DefaultEndpointsProtocol=https;AccountName=s100ne2g3;AccountKey=/bfg7+JQpytaARoZfD5ERTGTkDhGiRKRl0K6NQfLGs2XMeQC4aNE7aEIxNOklTmRH8lAaw5aBMuCEdp51m43PQ==;EndpointSuffix=core.windows.net";
        }

        public async Task<bool> ActualizarDatos(ProductoEntity producto)
        {
            //throw new NotImplementedException();
            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<AzProductoEntity>(
                                producto.Codigo.Substring(0,3),
                                producto.Codigo
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as AzProductoEntity;
                p.Precio = producto.Precio.ToString();
                p.Descripcion = producto.Descripcion;
                p.Categoria = producto.Categoria;
                p.PrecioImpuesto = Convert.ToString((Convert.ToDouble(producto.Precio)*0.16)+Convert.ToDouble(producto.Precio));
                p.Nombre = producto.Nombre;

                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }else{
                return false;
            }

        }

        public async Task<bool>  ActualizarImagen(ProductoEntity producto, object imagen)
        {
            //throw new NotImplementedException();
            var file = imagen as IFormFile;
            if(file==null){
                throw new ArgumentException(nameof(imagen));
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureConStr);
            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "my-new-container."
            CloudBlobContainer container = blobClient.GetContainerReference("pizzaimagen");

            // Get a reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{producto.Codigo}.jpeg");

            using (var fileStream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            var url = blockBlob.Uri.AbsoluteUri;

            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<AzProductoEntity>(
                                producto.Codigo.Substring(0,3),
                                producto.Codigo
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as AzProductoEntity;

                p.Imagen = url;


                var upOp = TableOperation.Replace(p);
                await table.ExecuteAsync(upOp);
                return true;
            }else{
                return false;
            }
        }

        public  async Task<bool>  BorrarProducto(string codigo)
        {
            //throw new NotImplementedException();
            var table = TablaAzure();

            var retriveOP = TableOperation 
                            .Retrieve<AzProductoEntity>(
                                codigo.Substring(0,3),
                                codigo
                            );
            var resultado = await table.ExecuteAsync(retriveOP);
            if (resultado != null){
                var p= resultado.Result as AzProductoEntity;
                var upOp = TableOperation.Delete(p);
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
            CloudTable table = tableClient.GetTableReference("ArticulosMarvel");

            return table;
        }
        public async Task<bool>  CrearProducto(ProductoEntity nuevo)
        {
                var table = TablaAzure();
                // Create the table if it doesn't exist.
                var creada = await table.CreateIfNotExistsAsync();

                var azEn= new AzProductoEntity(nuevo.Codigo);
                azEn.Descripcion = nuevo.Descripcion;
                azEn.Nombre= nuevo.Nombre;
                azEn.Categoria = nuevo.Categoria;
                azEn.PrecioImpuesto = Convert.ToString((Convert.ToDouble(nuevo.Precio)*0.16)+Convert.ToDouble(nuevo.Precio));
                azEn.Precio=nuevo.Precio.ToString();
                azEn.Imagen = "";

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(azEn);

                // Execute the insert operation.
                var x = await table.ExecuteAsync(insertOperation);

                return true;
        }

        
        public async Task<ProductoEntity> LeerProducto(string codigo)
        {
            var table = TablaAzure();

            TableOperation retrieveOperation = TableOperation.Retrieve<AzProductoEntity>(
                                                codigo.Substring(0,3),
                                                codigo);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                var az = retrievedResult.Result as AzProductoEntity;
                return new ProductoEntity(){
                    Codigo = az.id,
                    Nombre = az.Nombre,
                    Precio = decimal.Parse( az.Precio),
                    PrecioImpuesto = decimal.Parse(az.PrecioImpuesto),
                    Descripcion = az.Descripcion,
                    Categoria = az.Categoria,
                    ImagenURL = az.Imagen
                };
            }
            return null;
        }

        public async Task<List<ProductoEntity>> ProductosPorCategoria(string categoria)
        {
            var table = TablaAzure();

            TableQuery<AzProductoEntity> query = new TableQuery<AzProductoEntity>();

            var token = new TableContinuationToken();
            var list = new List<ProductoEntity>();

            foreach (AzProductoEntity entity in await table.ExecuteQuerySegmentedAsync(query,token))
            {
                if(entity.Categoria == categoria){
                    list.Add(new ProductoEntity(){
                        Nombre = entity.Nombre,
                        Descripcion = entity.Descripcion,
                        Codigo = entity.id,
                        Categoria = entity.Categoria,
                        Precio = decimal.Parse(entity.Precio),
                        PrecioImpuesto = decimal.Parse(entity.PrecioImpuesto),
                        ImagenURL = entity.Imagen
                    });
                }            
            }
            return list;
        }

        public async Task<List<ProductoEntity>> TodosLosProductos()
        {
            var table = TablaAzure();

            TableQuery<AzProductoEntity> query = new TableQuery<AzProductoEntity>();

            var token = new TableContinuationToken();
            var list = new List<ProductoEntity>();

            foreach (AzProductoEntity entity in await table.ExecuteQuerySegmentedAsync(query,token))
            {
                list.Add(new ProductoEntity(){
                    Nombre = entity.Nombre,
                    Descripcion = entity.Descripcion,
                    Codigo = entity.id,
                    Categoria = entity.Categoria,
                    Precio = decimal.Parse(entity.Precio),
                    PrecioImpuesto = decimal.Parse(entity.PrecioImpuesto),
                    ImagenURL = entity.Imagen
                });
            }
            return list;
        }
        public async Task<List<ProductoEntity>> NueveProductos()
        {
            var table = TablaAzure();

            TableQuery<AzProductoEntity> query = new TableQuery<AzProductoEntity>();

            var token = new TableContinuationToken();
            var list = new List<ProductoEntity>();
            var count = 1;
            foreach (AzProductoEntity entity in await table.ExecuteQuerySegmentedAsync(query,token))
            {
                if(count==10)
                    return list;
                    
                list.Add(new ProductoEntity(){
                    Nombre = entity.Nombre,
                    Descripcion = entity.Descripcion,
                    Codigo = entity.id,
                    Categoria = entity.Categoria,
                    Precio = decimal.Parse(entity.Precio),
                    PrecioImpuesto = decimal.Parse(entity.PrecioImpuesto),
                    ImagenURL = entity.Imagen
                });
                count+=1;
            }
            return list;
        }

        public Task<List<ProductoEntity>> LeerProductosMasPopulares()
        {
            throw new NotImplementedException();
        }
    }
    public class AzProductoEntity : TableEntity{
        public AzProductoEntity()
        {
            
        }

        public AzProductoEntity(string codigo){
            this.PartitionKey = codigo.Substring(0,3);
            this.RowKey = codigo;
            this.id=codigo;
        }

        public String id{get;set;}
        public String Nombre{get;set;}
        public String Precio{get;set;}   
        public String PrecioImpuesto{get;set;}
        public String Descripcion{get;set;}
        public String Categoria{get;set;}
        public String Imagen{get;set;}
    }
}