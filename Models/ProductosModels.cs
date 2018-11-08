using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TiendaMarvel.Models{
    public class ProductoEnIndice{
        public string Codigo{get;set;}
        public string Nombre{get;set;}
        public string Descripcion{get;set;}
        public string ImagenURL{get;set;}
        public decimal Precio{get;set;}
        public string Categoria{get;set;}
        public decimal PrecioImpuesto{get;set;} 
    }


    public class ProductoEntity{
        public string Codigo{get;set;}
        public string Nombre{get;set;}
        public string Descripcion{get;set;}
        public string ImagenURL{get;set;}
        public string Categoria{get;set;}
        public decimal PrecioImpuesto{get;set;} 
        public decimal Precio{get;set;}
    }
    public class ProductoModel{
         public String id{get;set;}
        public String Nombre{get;set;}
        public Decimal Precio{get;set;}     
        public Decimal PrecioImpuesto{get;set;} 
        public string Descripcion{get;set;}
        public String Categoria{get;set;}
        public String Imagen{get;set;}
    }
    public class ProductoDetalleModel
        {
            public String id{get;set;}
            public String Nombre{get;set;}
            public Decimal Precio{get;set;}
            public Decimal PrecioImpuesto{get;set;}
            public String Categoria{get;set;}
            public String Descripcion{get;set;}
            public String Imagen{get;set;}
        }
        public class ProductoEditarModel
        {   
            public String id{get;set;}
            [Required]
            public Decimal Precio{get;set;}
            [Required]
            public String Nombre{get;set;}
            public String Descripcion{get;set;}
            public string Categoria{get;set;}

        }

        public class ProductoCambiarImagenModel
        {
            public String id {get;set;}
            public String Nombre {get;set;}
            
            public String Imagen {get;set;}
            [Required]
            
            public IFormFile NuevaImagen {get; set;}
        }
        public class ProductoCrearModel
        {
            [Required]
            public String id{get;set;}
            [Required]
            public String Nombre{get;set;}
            [Required]
            public Decimal Precio{get;set;}
            
            public String Descripcion{get;set;}
            
            public String Categoria{get;set;}
            public String Imagen{get;set;}
        }

        public class ProductoBorrarModel
        {
            public String id{get;set;}
            public String Nombre {get;set;}

        }
}