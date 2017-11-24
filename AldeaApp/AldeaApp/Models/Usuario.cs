using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AldeaApp.Models
{
    public class Usuario
    {  public int CodUsuario { get; set; }
       public string Tipoid { get; set; }
       public string NumId { get; set; }
       public  string NomUsuario { get; set; }
       public string ApellidosUsuario { get; set; }
       public DateTime FechaNacimiento { get; set; }
       public string CiudadNacimiento { get; set; }
        public string DepartamentoNacimiento { get; set; }
        public string PaisNacimiento { get; set; }
        public string CiudadResidencia { get; set; }
        public string DepartamentoResidencia { get; set; }
        public  string PaisResidencia { get; set; }
        public string DireccionResidencia { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string CorreoElectronico { get; set; }
        public string InstitucionEgreso { get; set; }
        public string AnioEgreso { get; set; }
        public string TipoAfiliacion { get; set; }
        public string TituloPregrado { get; set; }
        public string InstitucionPregrado { get; set; }
        public string AnioGraduacionPregrado { get; set; }
        public string TituloPosgrado { get; set; }
        public string InstitucionPosgrado { get; set; }
        public string AnioGraduacionPosgrado { get; set; }
        public string NomEmpresaTrabajo { get; set; }
        public string Cargo { get; set; }
        public string DirEmpresa { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string Contrasenia { get; set; }
        
    }
}