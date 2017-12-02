using AldeaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Data;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Web.Security;
using iTextSharp;

using iTextSharp.text.pdf;
using iTextSharp.text;

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
        [Authorize]
        public ActionResult Modificar()
        {
            return View();
        }
        [Authorize]
        public ActionResult GenerarComprobante()
        {
            return View();
        }
        [Authorize]
        public ActionResult TraerComprobantes()
        {
            return View();
        }
        [Authorize]
        public ActionResult Informacion()
        {
            return View();
        }
        [Authorize]
        public ActionResult Usuarios()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }
        [Authorize]
        public ActionResult ModificarInformacion()
        {
            return View();
        }
        [Authorize]
        public ActionResult ModificarmiUsuario()
        {
            return View();
        }
        [Authorize]
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
        [Authorize]
        public ActionResult GenerarLibro()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                // Create Excel Worksheet
                package.Workbook.Worksheets.Add("UsuariosAldea");
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                ws.Name = "UsuariosAldea"; //Setting Sheet's name
                ws.Cells.Style.Font.Size = 12; //Default font size for whole sheet
                ws.Cells.Style.Font.Name = "Arial";

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1

                DataTable dt = new DataTable();
                Database conex = Conexion.getInstancia();
                dt = conex.ExecuteDataSet("Usp_MostrarUsuarios").Tables[0];
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                //Formateo la columna de Fechas
                //int cantidadFias = dt.Rows.Count;
                //for (int i = 1; i <= cantidadFias + 1; i++)
                //{
                //    ws.Cells[i, 4].Style.Numberformat.Format = "mm-dd-yy";
                //}



                //Autofit de todas las columnas y encabezado en negrita
                for (int i = 1; i <= dt.Columns.Count; i++)
                {
                    ws.Cells[i, 5].Style.Numberformat.Format = "mm-dd-yy";
                    ws.Column(i).AutoFit();
                }

                //En negrita el encabezado
                ws.Cells["A1:AB1"].Style.Font.Bold = true;
                ws.Cells["A:AB"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var stream = new MemoryStream();
                package.SaveAs(stream);
                DateTime FechaActual = DateTime.Now;
                string fileName = "UsuariosAldea-" + FechaActual.Year.ToString() + "-" + FechaActual.Month.ToString().PadLeft(2, '0') + "-" + FechaActual.Day.ToString().PadLeft(2, '0') + ".xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                stream.Position = 0;
                return File(stream, contentType, fileName);
            }
        }
        public string idpagos { get; set; }
     
        [Authorize]
        public ActionResult PDFGenerator(string id
            )

        {
            using (Document document = new Document())
            {
                
                string idpago = id;
                //while (id != null)
                int identificacionpago = Convert.ToInt32(idpago);
                DataTable dt = new DataTable();
                Database conex = Conexion.getInstancia();
                dt = conex.ExecuteDataSet("Usp_TraerPago", identificacionpago).Tables[0];
                string pago = dt.Rows[0]["ValorPago"].ToString();
                string aniopago = dt.Rows[0]["AnioPago"].ToString();
                DateTime fechapago = Convert.ToDateTime(dt.Rows[0]["FechaPago"]);
                //int pag = Convert.ToInt32(ViewBag.datos);
                int idusuario = Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString());

                DataTable dt1 = new DataTable();
                dt1 = conex.ExecuteDataSet("Usp_UsuarioPago", idusuario).Tables[0];
                string Numid = dt1.Rows[0]["NumIdentificacion"].ToString();
                string Tipoid = dt1.Rows[0]["TipoIdentificacion"].ToString();
                string Nombre = dt1.Rows[0]["NombresUsuario"].ToString();
                string apellidos = dt1.Rows[0]["ApellidosUsuario"].ToString();
                MemoryStream workStream = new MemoryStream();
                // Document document = new Document();
                PdfWriter.GetInstance(document, workStream).CloseStream = false;

                //Image
                string imageURL = @"D:\aldea.jpg";
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageURL);
                
                //Resize image depend upon your need
                logo.ScaleToFit(140f, 120f);
                //Give space before image
                logo.SpacingBefore = 10f;
                //Give some space after the image
                logo.SpacingAfter = 1f;
                logo.Alignment = Element.ALIGN_RIGHT;
                //Tamaño del logo
                //logo.ScaleToFit(50f, 50f);
                //Image

                //Image and text
                PdfPTable table = new PdfPTable(2);
                PdfPCell cell = new PdfPCell();
                PdfPCell cell2 = new PdfPCell();
                Paragraph p = new Paragraph();
                Paragraph p2 = new Paragraph();
                p.Add(new Phrase("Asociación Lasallista de Exalumnos ALDEA", new Font(Font.FontFamily.HELVETICA, 13f, Font.BOLD)));
                
                p2.Add(new Chunk(logo, 100, -30));
                p2.Alignment = Element.ALIGN_CENTER;
                cell.AddElement(p);
                cell2.AddElement(p2);
                cell.BorderWidth = 0;
                cell2.BorderWidth = 0;
                table.AddCell(cell);
                table.AddCell(cell2);

                //Image and text
                document.Open();
                document.Add(new Paragraph("  "));
                document.Add(table);
                //document.Add(logo);
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("Asociación Lasallista de Exalumnos ALDEA", new Font(Font.FontFamily.HELVETICA, 13f, Font.BOLD)));
                document.Add(new Paragraph("Certificado de aporte económico", new Font(Font.FontFamily.HELVETICA, 13f, Font.BOLD)));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));

                //document.Add(new Paragraph("                               por el año " + aniopago));
                // Element.ALIGN_JUSTIFIED;
                Paragraph p4 = new Paragraph("La Asociación Lasallista de Exalumnos ALDEA, certifica que la persona " + Nombre + " " + apellidos + " identificado con el documento " + Tipoid + " " + Numid + " " + " en la fecha " + fechapago.Day + "/" + fechapago.Month + "/" + fechapago.Year + " realizó un aporte por la suma de " + pago + " pesos a la asociación, destinado al año " + aniopago);
                p4.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(p4);
                //document.Add(new Paragraph("La Asociación Lasallista de Exalumnos ALDEA, certifica que la persona " + Nombre + " " + apellidos + " identificado con el documento " + Tipoid + " " + Numid + " " + " en la fecha "+ fechapago.Day+"/"+ fechapago.Month + "/" + fechapago.Year  +  " realizó un aporte por la suma de " + pago + " pesos a la asociación, destinado al año "+ aniopago));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("___________________________ "));
                document.Add(new Paragraph("Firma del Revisor Fiscal   "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                document.Add(new Paragraph("  "));
                Paragraph p3 = new Paragraph("Generado el " + DateTime.Now.ToString());
                p3.Alignment = Element.ALIGN_RIGHT;
                document.Add(p3);

                //document.SaveAs(workStream);
                document.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;
               
                return File(workStream, "application/pdf");
            }
        }
        public JsonResult CrearUsuario(string Tipoid, string NumId, string NomUsuario, string ApellidosUsuario, DateTime FechaNacimiento,
         string CiudadNacimiento, string DepartamentoNacimiento, string PaisNacimiento, string CiudadResidencia, string DepartamentoResidencia,
         string PaisResidencia, string DireccionResidencia, string TelefonoFijo, string TelefonoCelular, string CorreoElectronico, string InstitucionEgreso,
         string AnioEgreso, string TipoAfiliacion, string TituloPregrado, string InstitucionPregrado, string AnioGraduacionPregrado, string TituloPosgrado,
         string InstitucionPosgrado, string AnioGraduacionPosgrado, string NomEmpresaTrabajo, string Cargo, string DirEmpresa, string TelefonoEmpresa, string Contrasenia)
        {
            Respuesta3 r3 = new Respuesta3();
            r3.message = new List<string>();
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            dt = conex.ExecuteDataSet("Usp_BuscarUsuario", Tipoid, NumId).Tables[0];
            if (dt.Rows.Count == 0)
            {
                if (string.IsNullOrEmpty(Tipoid.Trim()) || string.IsNullOrWhiteSpace(Tipoid.Trim()))
                {
                    r3.message.Add("<li>Debe seleccionar el tipo de identificación</li>");
                }
                if (string.IsNullOrEmpty(NumId.Trim()) || string.IsNullOrWhiteSpace(NumId.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el número de identificación</li>");
                }
                if (string.IsNullOrEmpty(NomUsuario.Trim()) || string.IsNullOrWhiteSpace(NomUsuario.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el nombre</li>");
                }
                if (string.IsNullOrEmpty(ApellidosUsuario.Trim()) || string.IsNullOrWhiteSpace(ApellidosUsuario.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar los apellidos</li>");
                }
                if (FechaNacimiento.Year > 1999 )
                {
                    r3.message.Add("<li>Debe ingresar una fecha de nacimiento válida</li>");
                }
                if (string.IsNullOrEmpty(CiudadNacimiento.Trim()) || string.IsNullOrWhiteSpace(CiudadNacimiento.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar la ciudad de nacimiento</li>");
                }
                if (string.IsNullOrEmpty(DepartamentoNacimiento.Trim()) || string.IsNullOrWhiteSpace(DepartamentoNacimiento.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el departamento de nacimiento</li>");
                }
                if (string.IsNullOrEmpty(PaisNacimiento.Trim()) || string.IsNullOrWhiteSpace(PaisNacimiento.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el país de nacimiento</li>");
                }
                if (string.IsNullOrEmpty(CiudadResidencia.Trim()) || string.IsNullOrWhiteSpace(CiudadResidencia.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar la ciudad de residencia</li>");
                }
                if (string.IsNullOrEmpty(DepartamentoResidencia.Trim()) || string.IsNullOrWhiteSpace(DepartamentoResidencia.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el departamento de residencia</li>");
                }
                if (string.IsNullOrEmpty(PaisResidencia.Trim()) || string.IsNullOrWhiteSpace(PaisResidencia.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el pais de residencia</li>");
                }
                if (string.IsNullOrEmpty(DireccionResidencia.Trim()) || string.IsNullOrWhiteSpace(DireccionResidencia.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar la dirección de residencia</li>");
                }
                if (string.IsNullOrEmpty(TelefonoFijo.Trim()) || string.IsNullOrWhiteSpace(TelefonoFijo.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el teléfono fijo</li>");
                }
                if (string.IsNullOrEmpty(TelefonoCelular.Trim()) || string.IsNullOrWhiteSpace(TelefonoCelular.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el teléfono celular</li>");
                }
                if (string.IsNullOrEmpty(CorreoElectronico.Trim()) || string.IsNullOrWhiteSpace(CorreoElectronico.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el correo electrónico</li>");
                }
                if (string.IsNullOrEmpty(InstitucionEgreso.Trim()) || string.IsNullOrWhiteSpace(InstitucionEgreso.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar la institución de egreso</li>");
                }
                if (string.IsNullOrEmpty(AnioEgreso.Trim()) || string.IsNullOrWhiteSpace(AnioEgreso.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el año de egreso</li>");
                }
                if (string.IsNullOrEmpty(TipoAfiliacion.Trim()) || string.IsNullOrWhiteSpace(TipoAfiliacion.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el tipo de afiliación</li>");
                }
                if (string.IsNullOrEmpty(InstitucionPregrado.Trim()) || string.IsNullOrWhiteSpace(InstitucionPregrado.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar la institución de pregrado</li>");
                }
                if (string.IsNullOrEmpty(TituloPregrado.Trim()) || string.IsNullOrWhiteSpace(TituloPregrado.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el titulo de pregrado</li>");
                }
                if (string.IsNullOrEmpty(AnioGraduacionPregrado.Trim()) || string.IsNullOrWhiteSpace(AnioGraduacionPregrado.Trim()))
                {
                    r3.message.Add("<li>Debe ingresar el año de graduación de pregrado</li>");
                }
                if (r3.message.Count == 0)
                {
                    string newpass = PasswordStorage.CreateHash(Contrasenia);

                    conex.ExecuteDataSet("Usp_CrearUsuario", Tipoid, NumId, NomUsuario, ApellidosUsuario,
                    FechaNacimiento, CiudadNacimiento, DepartamentoNacimiento,
                    PaisNacimiento, CiudadResidencia, DepartamentoResidencia, PaisResidencia, DireccionResidencia, TelefonoFijo,
                    TelefonoCelular, CorreoElectronico, InstitucionEgreso, AnioEgreso,
                    TipoAfiliacion, TituloPregrado, InstitucionPregrado, AnioGraduacionPregrado, TituloPosgrado,
                    InstitucionPosgrado, AnioGraduacionPosgrado, NomEmpresaTrabajo, Cargo, DirEmpresa,
                    TelefonoEmpresa, newpass);
                    r3.existe = "";
                    
                }
            }
            else
            {   
                r3.existe= "El usuario ya existe";
            }

            return Json(r3, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarUsuario(string Tipoid, string NumId)
        {

         //   System.Web.HttpContext.Current.User.Identity.Name

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
        public JsonResult BuscarUsuario1()
        {

            string datosiniciosesion = System.Web.HttpContext.Current.User.Identity.Name;
            string Tipoid = datosiniciosesion.Split('&')[1];
            string NumId= datosiniciosesion.Split('&')[0];
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


            return Json("Usuario modificado éxitosamente", JsonRequestBehavior.AllowGet);
        }

        //Obtener datos de inicio sesión
        public JsonResult ObtenerDatosSesion()
        {

            string datosiniciosesion = System.Web.HttpContext.Current.User.Identity.Name;
            string dato;
            if (datosiniciosesion != "")
            {
                dato = datosiniciosesion.Split('&')[2];
            }
            else
            {
                dato = "0";
            }
            return Json(dato,JsonRequestBehavior.AllowGet);
        }
        public JsonResult verificarInicioSesion()
        {
            bool aut = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            return Json(aut, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerificarUsuario(string Tipoid, string NumId, string contrasenia)
        {
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            string mensaje;
            Respuesta2 r = new Respuesta2();
           // int iniciosesion;
            dt = conex.ExecuteDataSet("Usp_BuscarUsuario", Tipoid, NumId).Tables[0];
            if (dt.Rows.Count == 0)
            {
                //  mensaje = "El usuario no existe.";
                r.mensaje = "El usuario no existe.";
            }
            else
            {
                string password = dt.Rows[0]["Contrasenia"].ToString();
                string rol = dt.Rows[0]["Rol"].ToString();
                bool pass = PasswordStorage.VerifyPassword(contrasenia, password);
                if (pass == true)
                {
                    FormsAuthentication.SetAuthCookie(NumId+"&"+Tipoid+"&"+rol, false);
                    //FormsAuthentication.SetAuthCookie(NumId,ROL, false);
                    //mensaje = "Bienvenido a ALDEA.";
                    r.mensaje = "";
                    r.rol = rol;
                }
                else
                {
                    // mensaje = "Contraseña incorrecta";
                   r.mensaje = "Contraseña incorrecta";
                }

            }



            return Json(r, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
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
        [Authorize]
        public JsonResult TraerInformacionAdmin()
        {
            DB_AldeaEntities db = new DB_AldeaEntities();
            List<Tb_ParametrosInformativos> informacion = new List<Tb_ParametrosInformativos>();
            informacion = db.Tb_ParametrosInformativos.ToList();
            return Json(informacion, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult ActualizarParametro(string id,string Descripcion, string Valor)
        {
            Database conex = Conexion.getInstancia();
            int id1 = Convert.ToInt32(id);
            conex.ExecuteDataSet("Usp_ActualizarParametro",id1, Descripcion, Valor);

            return Json("Información modificada éxitosamente", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult AgregarPago(string id, string AnioPagado, DateTime FechaPago, string ValorPagado)
        {
            //Agregar Parametro
            Database conex = Conexion.getInstancia();
            int idUsuario = Convert.ToInt32(id);
            conex.ExecuteDataSet("Usp_AgregarPago", idUsuario, AnioPagado, FechaPago, ValorPagado);
            return Json("Aporte agregado exitosamente", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult EliminarParametro(string id)
        {
            //Agregar Parametro
            Database conex = Conexion.getInstancia();
            int idParametro = Convert.ToInt32(id);
            conex.ExecuteDataSet("Usp_EliminarParametro", idParametro);
            return Json("Parámetro eliminado exitosamente", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult SeleccionarItem(string id)
        {
            int id1 = Convert.ToInt32(id);
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            List<Tb_ParametrosInformativos> informacion = new List<Tb_ParametrosInformativos>();
            Informacion i = new Informacion();
            dt =conex.ExecuteDataSet("Usp_SeleccionarParametro", id1).Tables[0];
            i.id=Convert.ToInt32 (dt.Rows[0]["IdParametros"].ToString());
            i.Descripcion = dt.Rows[0]["Descripcion"].ToString();
            i.Valor = dt.Rows[0]["Valor"].ToString();
            return Json(i, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult SeleccionarPago(string id)
        {
            int idAporte = Convert.ToInt32(id);
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            List<Tb_ParametrosInformativos> informacion = new List<Tb_ParametrosInformativos>();
            Pago p = new Pago();
            dt = conex.ExecuteDataSet("Usp_SeleccionarPago", idAporte).Tables[0];
            p.idPago = Convert.ToInt32(dt.Rows[0]["IdPagos"].ToString());
            p.AnioPago = dt.Rows[0]["AnioPago"].ToString();
            p.FechaPago = Convert.ToDateTime(dt.Rows[0]["FechaPago"].ToString());
            p.ValorPago = dt.Rows[0]["ValorPago"].ToString();
            return Json(p, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult CrearParametro(string Descripcion, string Valor)
        {
            Database conex = Conexion.getInstancia();
            
            conex.ExecuteDataSet("Usp_CrearParametro", Descripcion, Valor);

            return Json("Parámetro agregado éxitosamente", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult TraerPagos()
        {
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            Respuesta4 r4 = new Respuesta4();
            string datosiniciosesion = System.Web.HttpContext.Current.User.Identity.Name;
            string Tipoid = datosiniciosesion.Split('&')[1];
            string NumId = datosiniciosesion.Split('&')[0];
            dt = conex.ExecuteDataSet("Usp_BuscarUsuario", Tipoid, NumId).Tables[0];
           int id= Convert.ToInt32(dt.Rows[0]["IdUsuario"].ToString());
            dt1 = conex.ExecuteDataSet("Usp_TraerPagos",id).Tables[0];
            if (dt1.Rows.Count == 0)
            {
                r4.mensaje = "Usted no tiene pagos registrados";
            }
            else
            {
                List<Pago> pagos = new List<Pago>();
                Pago p;
                for (int i=0; i<dt1.Rows.Count; i++)
                {
                   pagos.Add(p=new Pago {idPago=Convert.ToInt32(dt1.Rows[i]["IdPagos"].ToString()), AnioPago = dt1.Rows[i]["AnioPago"].ToString(), FechaPago = Convert.ToDateTime(dt1.Rows[i]["FechaPago"]), ValorPago = dt1.Rows[i]["ValorPago"].ToString() });
                }
                r4.mensaje = "";
                r4.Pagos = pagos;
            }
            return Json(r4, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult PagosUsuarios()
        {
            Database conex = Conexion.getInstancia();
            DataTable dt = new DataTable();                  
            dt = conex.ExecuteDataSet("usp_TraerPagosUsuarios").Tables[0];
            List<PagosUsuarios> pagosusuarios = new List<PagosUsuarios>();
            PagosUsuarios pu;
            if (dt.Rows.Count == 0)
            {
                dt = null;
            }
            else
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    pagosusuarios.Add(pu = new PagosUsuarios { idPago = Convert.ToInt32(dt.Rows[i]["IdPagos"].ToString()), TipoId = dt.Rows[i]["Tipo"].ToString(), NumeroId = dt.Rows[i]["Identificación"].ToString(), NomUsuario = dt.Rows[i]["Nombres"].ToString(), ApellUsuario = dt.Rows[i]["Apellidos"].ToString(), AnioPago = dt.Rows[i]["Año"].ToString(), FechaPago = Convert.ToDateTime(dt.Rows[i]["Fecha Pago"]), ValorPago = dt.Rows[i]["Valor"].ToString() });
                }

            }
            return Json(pagosusuarios, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult ActualizarPagoUsuario(string id1, string Anio, DateTime Fecha, string ValorPago)
        {
            Database conex = Conexion.getInstancia();
            int id = Convert.ToInt32(id1);
            conex.ExecuteDataSet("usp_ActualizarPagoUsuario", id, Anio, Fecha, ValorPago);
            return Json("Aporte modificado correctamente", JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult EliminarAporte(string id1)
        {
            Database conex = Conexion.getInstancia();
            int id = Convert.ToInt32(id1);
            conex.ExecuteDataSet("Usp_EliminarAporte", id);
            return Json("Aporte eliminado exitosamente", JsonRequestBehavior.AllowGet);
        }
    }

}