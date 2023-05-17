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
    public class TramitesController : BaseController
    {

        [HttpGet]
        public JsonResult ObtenerGridTramites([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<TramiteGRIDDTO> result = fabrica.TramiteBO.ObtenerGridTramites(config, ObtenerAreasAtencionTotalesUsuarioActivo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(Tramite tramite)
        {
            RespuestaDTO respuesta;

            tramite.FechaRegistro = DateTime.Now;
            tramite.IdUsuarioRegistro = ObtenerIdUsuarioActivo();

            try
            {
                fabrica.TramiteBO.Guardar(tramite);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Tramite guardado correctamente.");
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
            Tramite registro = fabrica.TramiteBO.Consultar(id, "Proceso", "AreaAtencionProceso");
            var procesoDTO = new
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                IdProceso = registro.IdProceso,
                Activo = registro.Activo,
                RequiereExpediente = registro.RequiereExpediente,
                CorreoExpediente = registro.CorreoExpediente
            };

            return Json(procesoDTO, JsonRequestBehavior.AllowGet);
        }

    }
}