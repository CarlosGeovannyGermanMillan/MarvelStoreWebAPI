using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaMarvel.Models;

namespace TiendaMarvel.Controllers
{
    public class CategoriasController : Controller
    {
        ICategoriasRepository categorias;

        public CategoriasController(){
            categorias =  new AzureCategoriasRepository();
        }
        // GET: Categorias
        public async Task<ActionResult> Index()
        {

            var model = (await categorias.LeerTodas())
                    .Select( p => new CategoriaModel(){
                        CategoriaCodigo = p.CategoriaCodigo,
                        Descripcion = p.Descripcion
                    });

            return View(model);
        }

        // GET: Categorias/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var model = await categorias.LeerPorId(id);
            return View(new CategoriaModel(){
                CategoriaCodigo = model.CategoriaCodigo,
                Descripcion = model.Descripcion,
            });
            //var cat = await categorias.LeerPorId(id);
            //return View(cat);
        }

        // GET: Categorias/Create

        public ActionResult Create()
        {
            var model =  new CategoriaModel();
            return View(model);
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoriaModel model)
        {
            if(ModelState.IsValid){
                var cat = categorias.LeerPorId(model.CategoriaCodigo);
                if(cat==null){
                    return NotFound();
                }
                try
                {
                    // TODO: Add insert logic here
                    await categorias.CrearCategoria(new CategoriaModel(){
                        CategoriaCodigo = model.CategoriaCodigo,
                        Descripcion = model.Descripcion          
                    });

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else{
                return View(model);
            }
        }

        // GET: Categorias/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var c = await categorias.LeerPorId(id);

            if(c == null){
                return NotFound();
            }
            return View(new CategoriaModel(){
                CategoriaCodigo = c.CategoriaCodigo,
                Descripcion = c.Descripcion
            });


            //return View(c);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoriaModel model)
        {
            if(ModelState.IsValid){
                try
                {
                    // TODO: Add update logic here
                    await categorias.UpdateCategoria(new CategoriaModel(){
                            CategoriaCodigo = model.CategoriaCodigo,
                            Descripcion = model.Descripcion
                    });


                    //c.Descripcion = model.Descripcion;
                    //await categorias.ActualizarCategoria();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        // GET: Categorias/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var c = await categorias.LeerPorId(id);
            return View(new CategoriaModel(){
                CategoriaCodigo = c.CategoriaCodigo,
                Descripcion = c.Descripcion
            });
        }

        // POST: Categorias/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {
            var c = await categorias.LeerPorId(id);

            if(c == null){
                return NotFound();
            }
            try
            {
                // TODO: Add update logic here
                
                await categorias.BorrarCategoria(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}