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
    public class ProcesosController : BaseController
    {

        [HttpGet]
        public JsonResult ObtenerGridProcesos([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<ProcesoGRIDDTO> result = fabrica.ProcesoBO.ObtenerGridProcesos(config, ObtenerAreasAtencionTotalesUsuarioActivo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Proceso proceso)
        {
            RespuestaDTO respuesta;

            proceso.FechaRegistro = DateTime.Now;
            proceso.IdUsuarioRegistro = ObtenerIdUsuarioActivo();

            try
            {
                fabrica.ProcesoBO.Guardar(proceso);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Proceso guardado correctamente.");
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
            Proceso registro = fabrica.ProcesoBO.Consultar(id, "AreaAtencion");
            var procesoDTO = new
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                IdAreaAtencion = registro.IdAreaAtencion,
                Activo = registro.Activo
            };

            return Json(procesoDTO, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerComboProcesos()
        {
            IList<Proceso> registros = fabrica.ProcesoBO.ObtenerTodos("AreaAtencion");
            List<long> idsAreasAtencionUsuarioActual = ObtenerAreasAtencionTotalesUsuarioActivo().Select(a => a.Id).ToList();
            registros = registros.Where(r => idsAreasAtencionUsuarioActual.Any(id => id == r.IdAreaAtencion)).ToList();

            var comboDTO = registros.Select(r => new
            {
                Value = r.Id,
                Label = r.Nombre,
                IdAreaAtencionProceso = r.IdAreaAtencion
            });

            return Json(comboDTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ObtenerProcesosPorAreaAtencion(long idAreaAtencion)
        {
            List<ProcesoGRIDDTO> result = fabrica.ProcesoBO.ObtenerProcesosPorAreaAtencion(idAreaAtencion);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}