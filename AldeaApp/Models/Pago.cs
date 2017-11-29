using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AldeaApp.Models
{
    public class Pago
    {
        public int idPago { get; set; }
        public string AnioPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string ValorPago { get; set; }
   
    }
}