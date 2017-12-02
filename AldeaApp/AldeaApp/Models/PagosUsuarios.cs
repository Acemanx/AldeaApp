using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AldeaApp.Models
{
    public class PagosUsuarios
    {
        public int idPago { get; set;}
        public string TipoId { get; set;}
        public string NumeroId { get; set;}
        public string NomUsuario { get; set;}
        public string ApellUsuario { get; set;}
        public string AnioPago { get; set;}
        public DateTime FechaPago { get; set;}
        public string ValorPago { get; set;}
    }
}