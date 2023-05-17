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
    public class AreasAtencionController : BaseController
    {

        [HttpGet]
        public JsonResult ObtenerGridAreasAtencion([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            PagingResult<AreaAtencionGRIDDTO> result = fabrica.AreaAtencionBO.ObtenerGridAreasAtencion(config);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(AreaAtencion areaAtencion)
        {
            RespuestaDTO respuesta;

            try
            {
                fabrica.AreaAtencionBO.Guardar(areaAtencion);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Area de atencion guardada correctamente.");
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
            AreaAtencion registro = fabrica.AreaAtencionBO.Consultar(id, "Responsables");
            var areaAtencionDTO = new
            {
                Id = registro.Id,
                Nombre = registro.Nombre,
                CampusPS = registro.CampusPS,
                CampusDescr = registro.CampusDescr,
                Activo = registro.Activo,
                IdsResponsables = registro.Responsables.Select(r => r.Id),

                DisponibleCitas = registro.DisponibleCitas,
                ResponsablePermisoCitas = registro.ResponsablePermisoCitas,
                LugarCitas = registro.LugarCitas,
                CorreoAgendada = registro.CorreoAgendada,
                CorreoCancelada = registro.CorreoCancelada,
                CorreoRemitenteCitas = registro.CorreoRemitenteCitas,
            };

            return Json(areaAtencionDTO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerAreasAtencionUsuarioResponsable()
        {
            List<AreaAtencion> areasAtencion = ObtenerUsuarioActivo().AreasAtencionResponsable;
            var areasAtencionDTO = areasAtencion.Select(a => new
            {
                Id = a.Id,
                Nombre = a.Nombre
            });
            return Json(areasAtencionDTO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerAreasAtencionUsuarioTotales()
        {
            List<AreaAtencion> areasAtencionResponsable = ObtenerUsuarioActivo().AreasAtencionResponsable;
            List<AreaAtencion> areasAtencionUsuario = ObtenerUsuarioActivo().Perfiles.SelectMany(p => p.AreasAtencion).ToList();

            List<AreaAtencion> areasAtencionTotales = new List<AreaAtencion>();
            areasAtencionTotales.AddRange(areasAtencionResponsable);
            areasAtencionTotales.AddRange(areasAtencionUsuario);
            areasAtencionTotales = areasAtencionTotales.Distinct().ToList();


            IEnumerable<ComboDTO> areasAtencion = areasAtencionTotales.Select(a => new ComboDTO()
            {
                Value = a.Id.ToString(),
                Label = a.Nombre
            });
            return Json(areasAtencion, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerAreasAtencionPorCampus(string campus_ps)
        {
            List<AreaAtencionGRIDDTO> result = fabrica.AreaAtencionBO.ObtenerAreasAtencionPorCampus(campus_ps);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}