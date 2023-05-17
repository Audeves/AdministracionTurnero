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
    public class TurnerosController : BaseController
    {
        [HttpGet]
        public JsonResult ObtenerGridConfiguracionesTurneros([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<ConfiguracionTurneroGRIDDTO> result = fabrica.ConfiguracionTurneroBO.ObtenerGridConfiguracionesTurneros(config, ObtenerAreasAtencionTotalesUsuarioActivo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(ConfiguracionTurnero turnero)
        {
            RespuestaDTO respuesta;
            
            try
            {
                ConfiguracionTurnero turneroGuardado = fabrica.ConfiguracionTurneroBO.Guardar(turnero);
                if (turneroGuardado != null && turneroGuardado.Id != 0)
                {
                    fabrica.AreaAtencionBO.ActualizarTurneroAreaAtencion(turneroGuardado, turnero);
                }
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Configuracion de turnero guardada correctamente.");
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
            ConfiguracionTurnero registro = fabrica.ConfiguracionTurneroBO.Consultar(id, "AreasAtencion");
            var registroDTO = new
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                IP = registro.IP,
                CampusPS = registro.CampusPS,
                CampusDescr = registro.CampusDescr,
                Activo = registro.Activo,
                SolicitarId = registro.SolicitarId,
                SolicitarNombre = registro.SolicitarNombre,
                IdsAreasAtencion = registro.AreasAtencion.Select(r => r.Id)
            };

            return Json(registroDTO, JsonRequestBehavior.AllowGet);
        }
    }
}