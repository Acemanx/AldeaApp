using AldeaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data;

namespace AldeaApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Modificar()
        {
            return View();
        }
        public ActionResult GenerarComprobante()
        {
            return View();
        }
        public ActionResult Informacion()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }
        public ActionResult AgregarPago()
        {
            return View();
        }
        public ActionResult Registro()
        {
            return View();
        }
        public ActionResult ModificarInformacion()
        {
            return View();
        }

        public JsonResult CrearUsuario(string Tipoid, string NumId, string NomUsuario, string ApellidosUsuario, DateTime FechaNacimiento,
         string CiudadNacimiento, string DepartamentoNacimiento, string PaisNacimiento, string CiudadResidencia, string DepartamentoResidencia,
         string PaisResidencia, string DireccionResidencia, string TelefonoFijo, string TelefonoCelular, string CorreoElectronico, string InstitucionEgreso,
         string AnioEgreso, string TipoAfiliacion, string TituloPregrado, string InstitucionPregrado, string AnioGraduacionPregrado, string TituloPosgrado,
         string InstitucionPosgrado, string AnioGraduacionPosgrado, string NomEmpresaTrabajo, string Cargo, string DirEmpresa, string TelefonoEmpresa, string Contrasenia)
        {
            string mensaje;
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            dt = conex.ExecuteDataSet("Usp_BuscarUsuario", Tipoid, NumId).Tables[0];
            if (dt.Rows.Count == 0)
            {

                string newpass = PasswordStorage.CreateHash(Contrasenia);

                conex.ExecuteDataSet("Usp_CrearUsuario", Tipoid, NumId, NomUsuario, ApellidosUsuario,
                FechaNacimiento, CiudadNacimiento, DepartamentoNacimiento,
              PaisNacimiento, CiudadResidencia, DepartamentoResidencia, PaisResidencia, DireccionResidencia, TelefonoFijo,
              TelefonoCelular, CorreoElectronico, InstitucionEgreso, AnioEgreso,
              TipoAfiliacion, TituloPregrado, InstitucionPregrado, AnioGraduacionPregrado, TituloPosgrado,
              InstitucionPosgrado, AnioGraduacionPosgrado, NomEmpresaTrabajo, Cargo, DirEmpresa,
              TelefonoEmpresa, newpass);
                mensaje = "Usuario creado éxitosamente";
            }
            else
            {
                mensaje = "El usuario ya existe";
            }

            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarUsuario(string Tipoid, string NumId)
        {

            Database conex = Conexion.getInstancia();
            Respuesta r = new Respuesta();
            DataTable dt = new DataTable();
            List<Usuario> l = new List<Usuario>();
            dt = conex.ExecuteDataSet("Usp_BuscarUsuario", Tipoid, NumId).Tables[0];
            if (dt.Rows.Count > 0)
            {

                Usuario u = (new Usuario
                {
                    CodUsuario = Convert.ToInt32(dt.Rows[0]["IdUsuario"]),
                    Tipoid = dt.Rows[0]["TipoIdentificacion"].ToString(),
                    NumId = dt.Rows[0]["NumIdentificacion"].ToString(),
                    NomUsuario = dt.Rows[0]["NombresUsuario"].ToString(),
                    ApellidosUsuario = dt.Rows[0]["ApellidosUsuario"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(dt.Rows[0]["FechaNacimiento"]),
                    CiudadNacimiento = dt.Rows[0]["CiudadNacimiento"].ToString(),
                    DepartamentoNacimiento = dt.Rows[0]["DepartamentoNacimiento"].ToString()
                    ,
                    PaisNacimiento = dt.Rows[0]["PaisNacimiento"].ToString(),
                    CiudadResidencia = dt.Rows[0]["CiudadResidencia"].ToString(),
                    DepartamentoResidencia = dt.Rows[0]["DepartamentoResidencia"].ToString(),
                    PaisResidencia = dt.Rows[0]["PaisResidencia"].ToString(),
                    DireccionResidencia = dt.Rows[0]["DireccionResidencia"].ToString(),
                    TelefonoFijo = dt.Rows[0]["TelefonoFijo"].ToString(),
                    TelefonoCelular = dt.Rows[0]["TelefonoCelular"].ToString(),
                    CorreoElectronico = dt.Rows[0]["CorreoElectronico"].ToString(),
                    InstitucionEgreso = dt.Rows[0]["InstitucionEgreso"].ToString(),
                    AnioEgreso = dt.Rows[0]["AnioEgreso"].ToString(),
                    TipoAfiliacion = dt.Rows[0]["TipoAfiliciacion"].ToString(),
                    TituloPregrado = dt.Rows[0]["TituloPregrado"].ToString(),
                    InstitucionPregrado = dt.Rows[0]["InstitucionPregrado"].ToString(),
                    AnioGraduacionPregrado = dt.Rows[0]["AnioGraduacionPregrado"].ToString(),
                    TituloPosgrado = dt.Rows[0]["TituloPosgrado"].ToString(),
                    InstitucionPosgrado = dt.Rows[0]["InstitucionPosgrado"].ToString(),
                    AnioGraduacionPosgrado = dt.Rows[0]["AnioGraduacionPosgrado"].ToString(),
                    NomEmpresaTrabajo = dt.Rows[0]["NomEmpresaTrabajo"].ToString(),
                    Cargo = dt.Rows[0]["Cargo"].ToString(),
                    DirEmpresa = dt.Rows[0]["DireccionEmpresa"].ToString(),
                    TelefonoEmpresa = dt.Rows[0]["TelefonoEmpresa"].ToString(),
                    Contrasenia = dt.Rows[0]["Contrasenia"].ToString()
                });
                r.miusuario = u;
                r.Mensaje = string.Empty;
            }
            else
            {

                r.Mensaje = "El Usuario no existe";
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ModificarUsuario(string CodUsuario, string Tipoid, string NumId, string NomUsuario, string ApellidosUsuario, DateTime FechaNacimiento,
        string CiudadNacimiento, string DepartamentoNacimiento, string PaisNacimiento, string CiudadResidencia, string DepartamentoResidencia,
        string PaisResidencia, string DireccionResidencia, string TelefonoFijo, string TelefonoCelular, string CorreoElectronico, string InstitucionEgreso,
        string AnioEgreso, string TipoAfiliacion, string TituloPregrado, string InstitucionPregrado, string AnioGraduacionPregrado, string TituloPosgrado,
        string InstitucionPosgrado, string AnioGraduacionPosgrado, string NomEmpresaTrabajo, string Cargo, string DirEmpresa, string TelefonoEmpresa,string Contrasenia)
        {

            int CodiUsuario = Convert.ToInt32(CodUsuario);
            Database conex = Conexion.getInstancia();
            string newpass = PasswordStorage.CreateHash(Contrasenia);
            conex.ExecuteDataSet("Usp_ActualizarUsuario", CodiUsuario, Tipoid, NumId, NomUsuario, ApellidosUsuario, FechaNacimiento, CiudadNacimiento, DepartamentoNacimiento,
          PaisNacimiento, CiudadResidencia, DepartamentoResidencia, PaisResidencia, DireccionResidencia, TelefonoFijo, TelefonoCelular, CorreoElectronico, InstitucionEgreso, AnioEgreso,
          TipoAfiliacion, TituloPregrado, InstitucionPregrado, AnioGraduacionPregrado, TituloPosgrado, InstitucionPosgrado, AnioGraduacionPosgrado, NomEmpresaTrabajo, Cargo, DirEmpresa,
          TelefonoEmpresa,newpass);


            return Json("Usuario Modificado éxitosamente", JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerificarUsuario(string Tipoid, string NumId, string contrasenia)
        {
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            string mensaje;
            dt = conex.ExecuteDataSet("Usp_BuscarUsuario", Tipoid, NumId).Tables[0];
            if (dt.Rows.Count == 0)
            {
                mensaje = "El usuario no existe.";

            }
            else
            {
                string password = dt.Rows[0]["Contrasenia"].ToString();
                bool pass = PasswordStorage.VerifyPassword(contrasenia, password);
                if (pass == true)
                {
                    mensaje = "Bienvenido a ALDEA.";
                }
                else
                {
                    mensaje = "Contraseña incorrecta";
                }

            }



            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TraerUsuarios()
        {
            DB_AldeaEntities db = new DB_AldeaEntities();
            List<Tb_Usuarios> lista = new List<Tb_Usuarios>();
            lista = db.Tb_Usuarios.ToList();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TraerInformacion()
        {
            DB_AldeaEntities db = new DB_AldeaEntities();
            List<Tb_ParametrosInformativos> informacion = new List<Tb_ParametrosInformativos>();
            informacion = db.Tb_ParametrosInformativos.ToList();
            return Json(informacion, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TraerInformacionAdmin()
        {
            DB_AldeaEntities db = new DB_AldeaEntities();
            List<Tb_ParametrosInformativos> informacion = new List<Tb_ParametrosInformativos>();
            informacion = db.Tb_ParametrosInformativos.ToList();
            return Json(informacion, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ModificarParametros(string Descripcion, int Valor)
        {
            Database conex = Conexion.getInstancia();

            conex.ExecuteDataSet("Usp_ModificarValor", Descripcion, Valor);

            return Json("Información modificada éxitosamente", JsonRequestBehavior.AllowGet);
        }


    }

}