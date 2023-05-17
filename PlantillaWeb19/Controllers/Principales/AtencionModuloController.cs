using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.Transporte;
using Entidades.TransporteGrid.Configuracion;
using Entidades.TransporteGrid.Principales;
using PlantillaWeb19.Binders;
using PlantillaWeb19.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.Principales
{
    public class AtencionModuloController : BaseController
    {

        [HttpGet]
        public JsonResult ValidarModuloLibre(long idModulo)
        {
            RespuestaDTO respuesta;

            try
            {
                bool libre = fabrica.ModuloAtencionBitacoraBO.ValidarModuloLibre(idModulo);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Libre", libre);
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Error. ", e.Message);
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ValidarUsuarioNoAtendiendo()
        {
            bool noAtendiendo = fabrica.ModuloAtencionBitacoraBO.ValidarUsuarioNoAtendiendo(ObtenerIdUsuarioActivo());
            return Json(noAtendiendo, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult IniciarAtencion(long idModulo)
        {
            RespuestaDTO respuesta;

            try
            {
                fabrica.ModuloAtencionBitacoraBO.IniciarAtencion(idModulo, ObtenerIdUsuarioActivo());
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Iniciado correctamente.");
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Error. ", e.Message);
            }

            return Json(respuesta, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult FinalizarAtencion()
        {
            RespuestaDTO respuesta;

            try
            {
                fabrica.ModuloAtencionBitacoraBO.FinalizarAtencion(ObtenerIdUsuarioActivo());
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Ha finalizado la atención, el módulo ha sido liberado.");
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Error. ", e.Message);
            }

            return Json(respuesta, JsonRequestBehavior.DenyGet);
        }

        //[HttpGet]
        //public JsonResult ObtenerGridTurnosPorAtender2([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        //{
        //    PagingResult<AreaAtencionGRIDDTO> result = fabrica.AreaAtencionBO.ObtenerGridAreasAtencion(config);
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult ObtenerGridTurnoPorAtender([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<TurnoGRIDDTO> result = fabrica.ModuloAtencionBitacoraBO.ObtenerGridTurnoPorAtender(config);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerGridTurnosEnEspera([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<TurnoGRIDDTO> result = fabrica.ModuloAtencionBitacoraBO.ObtenerGridTurnosEnEspera(config);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
