using Entidades.Entidades.Administracion;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.Transporte;
using Entidades.TransporteGrid.Administracion;
using PlantillaWeb19.Binders;
using PlantillaWeb19.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.Administracion
{
    public class PerfilController : BaseController
    {

        [HttpGet]
        public JsonResult ObtenerGridPerfiles([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            Usuario usuarioActivo = ObtenerUsuarioActivo();
            List<PerfilGRIDDTO> result = fabrica.PerfilBO.ObtenerGridPerfiles(usuarioActivo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult ObtenerGridPermisosPantallasElegibles()
        {
            Usuario usuarioActivo = ObtenerUsuarioActivo();
            IEnumerable<PantallaGRIDDTO> result = fabrica.PantallaBO.ConsultarPermisosPantallasElegibles(usuarioActivo.Desarrollador)
                .Select(p => new PantallaGRIDDTO()
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Controlador = p.Controlador,
                Accion = p.Accion,
                Modulo = p.Modulo != null ? p.Modulo.Nombre : p.SubModulo.Modulo.Nombre,
                SubModulo = p.SubModulo != null ? p.SubModulo.Nombre : ""
            }).OrderBy(g => g.Modulo).ThenBy(g => g.Nombre).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Perfil perfil)
        {
            RespuestaDTO respuesta;
            perfil.FechaRegistro = DateTime.Now;
            perfil.IdUsuarioRegistro = ObtenerIdUsuarioActivo();
            try
            {
                Usuario usuarioActivo = ObtenerUsuarioActivo();
                fabrica.PerfilBO.Guardar(perfil, usuarioActivo);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Perfil guardado correctamente.");
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Ocurrió un error al guardar.", e.Message);
            }

            return Json(respuesta, JsonRequestBehavior.DenyGet);
        }


        [HttpGet]
        public JsonResult Consultar(long id)
        {
            Perfil perfil = fabrica.PerfilBO.Consultar(id, "Pantallas", "AreasAtencion");
            var perfilDTO = new
            {
                Id = perfil.Id,
                Nombre = perfil.Nombre,
                IdsPantallas = perfil.Pantallas.Select(p => p.Id),
                Activo = perfil.Activo,
                IdsAreasAtencion = perfil.AreasAtencion.Select(a => a.Id)
            };

            return Json(perfilDTO, JsonRequestBehavior.AllowGet);
        }

    }
}