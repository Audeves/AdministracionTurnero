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
    public class RegistroPantallasController : BaseController
    {
        [HttpGet]
        public JsonResult ObtenerGridConfiguracionesPantallas([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<ConfiguracionPantallaGRIDDTO> result = fabrica.ConfiguracionPantallaBO.ObtenerGridConfiguracionesPantallas(config, ObtenerAreasAtencionTotalesUsuarioActivo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(ConfiguracionPantalla pantalla)
        {
            RespuestaDTO respuesta;

            try
            {
                ConfiguracionPantalla pantallaGuardada = fabrica.ConfiguracionPantallaBO.Guardar(pantalla);
                if (pantallaGuardada != null && pantallaGuardada.Id != 0)
                {
                    fabrica.AreaAtencionBO.ActualizarPantallaAreaAtencion(pantallaGuardada, pantalla);
                }
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Configuracion de pantalla guardada correctamente.");
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
            ConfiguracionPantalla registro = fabrica.ConfiguracionPantallaBO.Consultar(id, "AreasAtencion");
            var registroDTO = new
            {
                Id = registro.Id,
                IP = registro.IP,
                CampusPS = registro.CampusPS,
                CampusDescr = registro.CampusDescr,
                Activo = registro.Activo,
                IdsAreasAtencion = registro.AreasAtencion.Select(r => r.Id)
            };

            return Json(registroDTO, JsonRequestBehavior.AllowGet);
        }
    }
}