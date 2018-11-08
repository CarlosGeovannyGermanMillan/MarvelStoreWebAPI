using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaMarvel.Models{

    public class PedidoModel {
        public string PedidoId{get;set;}
        public string Cliente {get;set;}
        public string CorreoElectronico{get;set;}
        public DateTime FechaPedido{get;set;}
        public string Estado {get;set;}
        public string Productos {get;set;}
        public string Referencia{get;set;}
    }


    public class PedidoEditarModel
        {   
             public string PedidoId{get;set;}
            public string Cliente {get;set;}
            public string CorreoElectronico{get;set;}
            public DateTime FechaPedido{get;set;}
            public string Estado {get;set;}
            public string Productos {get;set;}
        }
}