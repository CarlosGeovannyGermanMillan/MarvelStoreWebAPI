using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiendaMarvel.Models;
using TiendaMarvel.Repositories;

namespace TiendaMarvel.Controllers
{
    public class HomeController : Controller
    {
        //Verificar los constructores 
        //Uno des para la administracion de usuarios y el otro para construir los productos

       /* private UserManager<ApplicationUser> userManager; 
        public HomeController(UserManager<ApplicationUser> userM){
            userManager=userM;
        }
        [Authorize(Roles="Ventas")]
        public async Task<IActionResult> Contact()
        {
            var user = await userManager.GetUserAsync(this.User); //Buscar datos de la BD del usuario Logg
            ViewData["Message"] = $"Your contact page. {user.Carrera} - {user.DeporteFavorito}";

            return View();
        }*/

        //---------------------------
        private readonly IProductosRepository productos;
        private readonly ICategoriasRepository categorias;

        public HomeController(IProductosRepository prodsReporitory)
        {
            productos= prodsReporitory;
        }

        /*public async Task<List<List<ProductoEntity>>> Index()
        {
            var ProdCat = new List<List<ProductoEntity>>();
            var catego = await categorias.LeerTodas();
            
            foreach(var c in catego){
                var produc = await productos.ProductosPorCategoria(c.Descripcion);
                var model = produc.Select(p => new ProductoEnIndice(){
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    ImagenURL = p.ImagenURL,
                    Precio = p.Precio
                });
                ProdCat.Add(model);
            }
            return ProdCat;
        }*/

        
        public async Task<IActionResult> Index()
        {
            var prods = await productos.NueveProductos();
            var model = prods.Select(p => new ProductoEntity(){
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                ImagenURL = p.ImagenURL,
                Precio = p.Precio,
                PrecioImpuesto = p.PrecioImpuesto,
                Categoria = p.Categoria
            });
            return View(model);
        }
        

        public IActionResult About()
        {
            string nombre;
            nombre = HttpContext.Session.GetString("Nombre");
            ViewData["Message"] = "Your contact page." +nombre;
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
