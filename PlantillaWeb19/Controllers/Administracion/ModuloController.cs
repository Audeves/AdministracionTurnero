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
    public class ModuloController : BaseController
    {

        [HttpGet]
        public JsonResult ObtenerGridModulos([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<ModuloGRIDDTO> result = fabrica.ModuloBO.ObtenerGridModulos(config);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Modulo modulo)
        {
            RespuestaDTO respuesta;
            try
            {
                fabrica.ModuloBO.Guardar(modulo);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Módulo guardado correctamente.");
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
            Modulo modulo = fabrica.ModuloBO.Consultar(id);
            var moduloDTO = new
            {
                Id = modulo.Id,
                Nombre = modulo.Nombre,
                Activo = modulo.Activo
            };

            return Json(moduloDTO, JsonRequestBehavior.AllowGet);
        }


        /// /////////////////////////////////////////////////////////////////////
        /// 


        [HttpGet]
        public JsonResult ObtenerGridSubModulos([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<SubModuloGRIDDTO> result = fabrica.SubModuloBO.ObtenerGridSubModulos(config);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult GuardarSubModulo(SubModulo subModulo)
        {
            RespuestaDTO respuesta;

            try
            {
                fabrica.SubModuloBO.Guardar(subModulo);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Submódulo guardado correctamente.");
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Ocurrió un error al guardar.", e);
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ConsultarSubModulo(long id)
        {
            SubModulo subModulo = fabrica.SubModuloBO.Consultar(id, "Modulo");
            var subModuloDTO = new
            {
                Id = subModulo.Id,
                Nombre = subModulo.Nombre,
                IdModulo = subModulo.Modulo.Id,
                ClaseIcono = subModulo.ClaseIcono,
                Activo = subModulo.Activo
            };

            return Json(subModuloDTO, JsonRequestBehavior.AllowGet);
        }
    }
}