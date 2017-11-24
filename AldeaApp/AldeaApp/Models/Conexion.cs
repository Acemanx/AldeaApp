using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
namespace AldeaApp.Models
{
    public class Conexion
    {
        
            static Database Instancia;


            public DateTime Fecha { get; set; }
            private Conexion()
            {

            }
            public static Database getInstancia()
            {
                if (Instancia == null)
                {
                    Instancia = DatabaseFactory.CreateDatabase("ConexionAldea");

                }
                return Instancia;
            }



        }
    }
