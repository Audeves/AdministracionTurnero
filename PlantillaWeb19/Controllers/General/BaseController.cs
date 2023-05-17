using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.Excepciones;
using Entidades.Transporte;
using Negocio.General;
using PlantillaWeb19.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PlantillaWeb19.Controllers.General
{
    public class BaseController : Controller
    {
        
        //Fabrica de objetos de negocio para no tener que estar creando instancias individuales en este nivel.
        protected FabricaBO fabrica { get; set; }

        public BaseController()
        {

        }

        protected bool SesionActiva()
        {
            return Session[ConstantesSession.Usuario] == null ? false : true;
        }

        protected Usuario ObtenerUsuarioActivo()
        {
            return (Usuario)Session[ConstantesSession.Usuario];
        }

        protected string ObtenerEmplidUsuarioActivo()
        {
            return ((Usuario)Session[ConstantesSession.Usuario]).Emplid;
        }

        protected long ObtenerIdUsuarioActivo()
        {
            return ((Usuario)Session[ConstantesSession.Usuario]).Id;
        }

        protected List<Perfil> ObtenerPerfilesUsuarioActivo()
        {
            return (List<Perfil>)((Usuario)Session[ConstantesSession.Usuario]).Perfiles;
        }

        protected string ObtenerCuentaDominioUsuarioActivo()
        {
            return ((Usuario)Session[ConstantesSession.Usuario]).CuentaDominio;
        }

        [HttpGet]
        public JsonResult ObtenerNombreUsuarioActivo()
        {
            return Json(((Usuario)Session[ConstantesSession.Usuario]).Nombre, JsonRequestBehavior.AllowGet);
        }

        protected List<ExcepcionControladorAccion> ObtenerPermisosUsuarioActivo()
        {
            return (List<ExcepcionControladorAccion>)Session[ConstantesSession.Permisos];
        }

        protected List<AreaAtencion> ObtenerAreasAtencionTotalesUsuarioActivo()
        {
            List<AreaAtencion> areasAtencionResponsable = ObtenerUsuarioActivo().AreasAtencionResponsable;
            List<AreaAtencion> areasAtencionUsuario = ObtenerUsuarioActivo().Perfiles.SelectMany(p => p.AreasAtencion).ToList();

            List<AreaAtencion> areasAtencionTotales = new List<AreaAtencion>();
            areasAtencionTotales.AddRange(areasAtencionResponsable);
            areasAtencionTotales.AddRange(areasAtencionUsuario);
            areasAtencionTotales = areasAtencionTotales.Distinct().ToList();

            return areasAtencionTotales;
        }

        protected List<ModuloAtencion> ObtenerModulosAtencionUsuarioActivo()
        {
            return (List<ModuloAtencion>)ObtenerUsuarioActivo().ModulosAtencion;
        }

        protected void ReiniciaConstantesSession()
        {
            Session[ConstantesSession.Usuario] = null;
            Session[ConstantesSession.Permisos] = null;
        }

        //Se ejecuta en cada llamada a alguna accion para verificar que aun exista una sesion activa y que el usuario tenga permisos
        //a la accion que se llama.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controlador = this.ControllerContext.RouteData.Values["controller"].ToString().Trim();
            string accion = this.ControllerContext.RouteData.Values["action"].ToString().Trim();
            fabrica = new FabricaBO();
            List<ExcepcionControladorAccion> excepciones = fabrica.PantallaBO.ConsultarPantallasyAccionesPublicas();
            if (SesionActiva())
            {
                //Esto debe de ser el equivalente a mis pantallas y acciones publicas.
                if (!excepciones.Exists(e => e.Controlador == controlador && e.Accion == accion))
                {
                    //Si es un usuario activo (registrado al sistema) verificale con permisos,
                    //si no simplemente sacalo, ya que uno publico no tiene permisos asignados,
                    //solo valida con las excepciones.
                    if (ObtenerUsuarioActivo().Activo)
                    {
                        //Si no tiene permiso le manda aviso
                        if (!ValidarPermiso(controlador, accion))
                        {
                            if (Request.AcceptTypes.Contains("application/json"))
                            {
                                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                            }
                            else
                            {
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                {
                                    { "Controller", "Vistas" },
                                    { "Action", "Aviso"}
                                });
                            }

                        }
                    }
                    else
                    {
                        if (Request.AcceptTypes.Contains("application/json"))
                        {
                            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                {
                                    { "Controller", "Vistas" },
                                    { "Action", "Aviso"}
                                });
                        }
                    }
                }
            }
            else
            {
                if (!excepciones.Exists(e => e.Controlador == controlador && e.Accion == accion && e.PublicaLibre == true))
                {
                    if (Request.AcceptTypes.Contains("application/json"))
                    {
                        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "Controller", "Vistas" },
                            { "Action", "SesionExpirada"}
                        });
                    }
                }
            }
        }

        //Verifica que se tenga permiso a un controlador y accion segun los perfiles del usuario.
        //protected bool ValidarPermiso(string controlador, string accion)
        //{
        //    List<Perfil> perfiles = ObtenerPerfilesUsuarioActivo();

        //    if (perfiles == null || perfiles.Count == 0)
        //    {
        //        throw new ExcepcionNegocio("No se pudieron obtener los permisos del usuario");
        //    }

        //    //Revisa si coincide el controlador y accion con el de las pantallas del perfil o con las acciones de pantalla
        //    List<Pantalla> pantallas = perfiles.SelectMany(pu => pu.Pantallas).GroupBy(p => p.Id).Select(p => p.First()).ToList();
        //    Pantalla permiso = pantallas.Where(p => ((p.Controlador == controlador && p.Accion == accion)
        //        || p.PantallasAcciones.Any(pa => pa.Controlador == controlador && pa.Accion == accion))).SingleOrDefault();

        //    return permiso != null;
        //}

        protected bool ValidarPermiso(string controlador, string accion)
        {
            List<ExcepcionControladorAccion> permisos = ObtenerPermisosUsuarioActivo();

            return permisos.Exists(p => (p.Controlador == controlador && p.Accion == accion));
        }

        //Obtiene el menu del usuario segun su perfil o perfiles asignados.
        [HttpGet]
        public JsonResult ObtenerMenu()
        {
            RespuestaDTO respuesta;
            string menu = "";
            List<Perfil> perfilesUsuario = ObtenerPerfilesUsuarioActivo();

            //Todas las pantallas que tienen los perfiles del usuario.
            //Primero obtiene todas las pantallas que estan en todos los elementos de la lista de perfiles dentro de una sola lista de pantallas
            //Evitando hacer una lista de listas, despues se agrupan por Id de pantalla y se seleccionan los primeros elementos encontrados
            //para quitar los elementos repetidos.
            List<Pantalla> pantallas = perfilesUsuario.SelectMany(pu => pu.Pantallas).Where(p => (p.Modulo != null || p.SubModulo != null)).GroupBy(p => p.Id).Select(p => p.First()).ToList(); //perfilesUsuario.Select(pu => pu.Pantallas).Distinct()..ToList();

            //Similar a la parte de perfilBO
            /////////////241019
            pantallas = pantallas.Where(p => p.Activo == true).ToList();
            //dejo pasar aquellas pantallas directas a modulo activo
            List<Pantalla> pantallasEnModulo = pantallas.Where(p => (p.Modulo != null && p.Modulo.Activo == true)).ToList();
            //y luego las que van dentro de submodulo activo
            List<Pantalla> pantallasEnSubmodulo = pantallas.Where(p => (p.SubModulo != null && p.SubModulo.Activo == true && p.SubModulo.Modulo.Activo == true)).ToList();
            List<Pantalla> pantallasActivas = new List<Pantalla>();
            pantallasActivas.AddRange(pantallasEnModulo);
            pantallasActivas.AddRange(pantallasEnSubmodulo);
            pantallas = pantallasActivas;


            //Todos los modulos que se encontraron en esas pantallas
            List<Modulo> modulos = pantallas.Where(p => p.Modulo != null).Select(p => p.Modulo).Distinct().OrderBy(m => m.Nombre).ToList();
            List<Modulo> modulosEnSubmodulos = pantallas.Where(p => p.SubModulo != null).Select(p => p.SubModulo.Modulo).Distinct().OrderBy(m => m.Nombre).ToList();
            modulosEnSubmodulos.ForEach(msm =>
            {
                if (!modulos.Contains(msm))
                {
                    modulos.Add(msm);
                }
            });
            //Todos los submodulos de esas pantallas...
            List<SubModulo> submodulos = pantallas.Where(p => p.SubModulo != null).Select(p => p.SubModulo).Distinct().OrderBy(sm => sm.Nombre).ToList();

            //Submenus que ya se agregaron completos
            List<SubModulo> submodulosAgregados = new List<SubModulo>();
            try
            {
                foreach (Modulo modulo in modulos)
                {
                    menu = menu + "<div class='sidebar-heading'>" + modulo.Nombre + "</div>";

                    List<Pantalla> pantallasModulo = pantallas.Where(p => (p.Modulo == modulo || (p.SubModulo != null && p.SubModulo.Modulo == modulo))).ToList();
                    foreach (Pantalla pantalla in pantallasModulo)
                    {
                        //Se agrega el submenu para el submodulo con las pantallas que se encuentren en la lista
                        //y se agrega el submodulo a la lista de agregados para omitir que se agreguen de nuevo las pantallas pertenecientes al mismo
                        if (pantalla.SubModulo != null && !submodulosAgregados.Contains(pantalla.SubModulo))
                        {
                            SubModulo submodulo = pantalla.SubModulo;
                            submodulosAgregados.Add(submodulo);
                            submodulo.ClaseIcono = string.IsNullOrWhiteSpace(submodulo.ClaseIcono) ? "fas fa-fw fa-folder" : submodulo.ClaseIcono;
                            menu = menu + @"<li class='nav-item'>
                                            <a class='nav-link collapsed' href='#' data-toggle='collapse' data-target='#" + Regex.Replace(submodulo.Nombre, @"\s+", "") + "' aria-expanded='true' aria-controls='collapsePages'> " +
                                                                                              "<i class='" + submodulo.ClaseIcono + "'></i>" +
                                                                                              "<span>" + submodulo.Nombre + "</span>" +
                                                                                          "</a>" +
                                                                                          "<div id='" + Regex.Replace(submodulo.Nombre, @"\s+", "") + "' class='collapse' aria-labelledby='headingPages' data-parent='#accordionSidebar'> " +
                                                                                              "<div class='bg-white py-2 collapse-inner rounded'>" +
                                                                                                  "<h6 class='collapse-header'>Pantallas:</h6>";
                            //Busco todas las pantallas que pertenecen a este submodulo y las agrego.
                            List<Pantalla> pantallasSubModulo = pantallas.Where(p => p.SubModulo == submodulo).ToList();
                            foreach (Pantalla pantallaSubmodulo in pantallasSubModulo)
                            {
                                menu = menu + "<a class='collapse-item' href='" + pantallaSubmodulo.Accion + "'> "
                                    + pantallaSubmodulo.Nombre + " </a>";
                            }

                            menu = menu + " </div> </div> </li>";
                        }
                        //Si no pertenece a un submodulo se agrega la pantalla a la seccion del modulo.
                        else if (pantalla.Modulo != null)
                        {
                            pantalla.ClaseIcono = string.IsNullOrWhiteSpace(pantalla.ClaseIcono) ? "fas fa-user-circle" : pantalla.ClaseIcono;

                            menu = menu + "<li class='nav-item'> " +
                                                "<a class='nav-link' href='" + pantalla.Accion + "'>" +
                                                    "<i class='" + pantalla.ClaseIcono + "'></i>" +
                                                    "<span> " + pantalla.Nombre + " </span>" +
                                                "</a>" +
                                            "</li>";
                        }
                    }

                    menu = menu + "<hr class='sidebar-divider'>";
                }
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Correcto", menu);
            }
            catch (ExcepcionNegocio ex)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, ex.Message);
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

    }
}