using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
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
    public class UsuarioController : BaseController
    {

        [HttpGet]
        public JsonResult ObtenerGridUsuarios([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            Usuario usuarioActivo = ObtenerUsuarioActivo();
            List<UsuarioGRIDDTO> result = fabrica.UsuarioBO.ObtenerGridUsuarios(usuarioActivo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerGridPerfilesElegibles()
        {
            Usuario usuarioActivo = ObtenerUsuarioActivo();
            IEnumerable<PerfilGRIDDTO> perfiles = fabrica.PerfilBO.ConsultarPerfilesElegibles(usuarioActivo);
            return Json(perfiles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Usuario usuario)
        {
            RespuestaDTO respuesta;

            try
            {
                Usuario usuarioActivo = ObtenerUsuarioActivo();
                fabrica.UsuarioBO.Guardar(usuario, usuarioActivo);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Usuario guardado correctamente.");
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
            Usuario registro = fabrica.UsuarioBO.Consultar(id, "Perfiles");
            var usuarioDTO = new
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                IdsPerfiles = registro.Perfiles.Select(p => p.Id),
                Activo = registro.Activo
            };

            return Json(usuarioDTO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerUsuariosResponsablesActivos()
        {
            List<Usuario> usuarios = fabrica.UsuarioBO.ConsultarUsuariosResponsablesActivos();
            return Json(usuarios, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ConsultarUsuariosAtencionVentanillaActivos(long idAreaAtencion)
        {
            List<UsuarioGRIDDTO> result = fabrica.UsuarioBO.ConsultarUsuariosAtencionVentanillaActivos(idAreaAtencion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}