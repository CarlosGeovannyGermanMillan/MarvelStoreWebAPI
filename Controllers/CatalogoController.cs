using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TiendaMarvel.Models;
using TiendaMarvel.Repositories;

namespace TiendaMarvel.Controllers
{
    public class CatalogoController : Controller
    {
        // GET: Catalogo
        IProductosRepository productos;
        private IApplicationBuilder app;

        public CatalogoController()
        {
            productos =  new AzureProductosRepository();
        } 
        public async Task<ActionResult> Index()
        {
            var model = (await productos.TodosLosProductos())
                    .Select( p => new ProductoModel(){
                        id = p.Codigo,
                        Descripcion = p.Descripcion,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        PrecioImpuesto = p.PrecioImpuesto,
                        Categoria = p.Categoria,
                        Imagen = p.ImagenURL
                    });

            return View(model);
        }

        [HttpGet("/products/{id}", Name = "Products_List")]
        public async Task<ActionResult> ProductosPorCategoria(string id)
        {
            

            var model = (await productos.ProductosPorCategoria(id))
                    .Select( p => new ProductoEntity(){
                        Codigo = p.Codigo,
                        Descripcion = p.Descripcion,
                        Nombre = p.Nombre,
                        Precio = p.Precio,
                        PrecioImpuesto = p.PrecioImpuesto,
                        Categoria = p.Categoria,
                        ImagenURL = p.ImagenURL
                    });

            return View(model);
        }

        public async Task<IEnumerable<ProductoModel>> GetAllCategoria(string id)
        {
            var model = (await productos.ProductosPorCategoria(id))
                    .Select( p => new ProductoModel(){
                        id = p.Codigo,
                        Nombre = p.Nombre,
                        Descripcion = p.Descripcion,
                        Categoria = p.Categoria,
                        Precio = p.Precio,
                        PrecioImpuesto = p.PrecioImpuesto,
                        Imagen = p.ImagenURL
                    });
            return model;
        }


        public async Task<ActionResult> Details(string id)
        {
            var model = await productos.LeerProducto(id);
            return View(new ProductoDetalleModel(){
                id = model.Codigo,
                Nombre = model.Nombre,
                Precio = model.Precio,
                PrecioImpuesto = model.PrecioImpuesto,
                Descripcion = model.Descripcion,
                Categoria = model.Categoria,
                Imagen=model.ImagenURL
            });

        }
        

        // GET: Catalogo/Create
        public ActionResult Create()
        {
            var model =  new ProductoCrearModel();
            return View(model);
        }

        // POST: Catalogo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductoCrearModel model)
        {
            if(ModelState.IsValid){
                try
                {
                    // TODO: Add insert logic here

                    await productos.CrearProducto(new ProductoEntity(){
                            Codigo = model.id,
                            Nombre = model.Nombre,
                            Descripcion = model.Descripcion,
                            Categoria=model.Categoria,
                            Precio = model.Precio            
                        });
                        return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Catalogo/Details/5
        
        // GET: Catalogo/Edit/5
        public async Task <ActionResult> Edit(string id)
        {
            var producto = await productos.LeerProducto(id);


            return View(new ProductoEditarModel(){
                id = producto.Codigo,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Descripcion = producto.Descripcion,
                Categoria = producto.Categoria
            });
        }

        // POST: Catalogo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductoEditarModel model)
        {
            if(ModelState.IsValid){
                try
                {
                    // TODO: Add update logic here
                    await productos.ActualizarDatos(new ProductoEntity(){
                        Codigo = model.id,
                        Nombre = model.Nombre,
                        Precio = model.Precio,
                        Descripcion = model.Descripcion,
                        Categoria = model.Categoria
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        public async Task<ActionResult> CambiarImagen(string id){
            var producto =  await productos.LeerProducto(id);

            return View(new ProductoCambiarImagenModel(){
                id = producto.Codigo,
                Nombre = producto.Nombre,
                Imagen =  producto.ImagenURL
        
            });
        }
        [HttpPost]
        public async Task<ActionResult> CambiarImagen(ProductoCambiarImagenModel model){
           
            var p=await productos.LeerProducto(model.id);

            if(p ==null){
                return NotFound();
            }

            if(!ModelState.IsValid){
                return View(model);
            }
            if(model.NuevaImagen.ContentType == "imagen/jpeg"){
                ModelState.AddModelError("NuevaImagen","Solo se aceptan archivos jpeg");
            }
            if(model.NuevaImagen.Length > 10 * 1024 * 1024){
                ModelState.AddModelError("NuevaImagen","Tamaño de la imagen muy grande");
            }
            if(!ModelState.IsValid){
                return View(model);
            }
            
            var res= await productos.ActualizarImagen(p, model.NuevaImagen);
            return RedirectToAction("index");
            
        }
        // GET: Catalogo/Delete/5
        public async Task< ActionResult> Delete(string id)
        {
            var producto = await productos.LeerProducto(id);


            return View(new ProductoBorrarModel(){
                id = producto.Codigo,
                Nombre = producto.Nombre
            });
        }

        // POST: Catalogo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                await productos.BorrarProducto(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}