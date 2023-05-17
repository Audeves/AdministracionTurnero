using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.Transporte;
using Entidades.TransporteGrid.Configuracion;
using PlantillaWeb19.Binders;
using PlantillaWeb19.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.Configuracion
{
    public class ModulosAtencionController : BaseController
    {
        [HttpGet]
        public JsonResult ObtenerGridModulosAtencion([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<ModuloAtencionGRIDDTO> result = fabrica.ModuloAtencionBO.ObtenerGridModulosAtencion(config, ObtenerAreasAtencionTotalesUsuarioActivo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(ModuloAtencion moduloAtencion)
        {
            RespuestaDTO respuesta;

            try
            {
                fabrica.ModuloAtencionBO.Guardar(moduloAtencion);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Modulo de atencion guardado correctamente.");
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
            ModuloAtencion registro = fabrica.ModuloAtencionBO.Consultar(id, "AreaAtencion", "Procesos", "UsuariosAtencion");
            var registroDTO = new
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                Activo = registro.Activo,
                IdAreaAtencion = registro.IdAreaAtencion,
                IdsProcesos = registro.Procesos.Select(r => r.Id),
                IdsUsuarios = registro.UsuariosAtencion.Select(r => r.Id)
            };

            return Json(registroDTO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public new JsonResult ObtenerModulosAtencionUsuarioActivo()
        {
            List<ModuloAtencion> registros = base.ObtenerModulosAtencionUsuarioActivo();
            var comboDTO = registros.Select(r => new
            {
                Value = r.Id,
                Label = r.Nombre
            });
            return Json(comboDTO, JsonRequestBehavior.AllowGet);
        }
    }
}