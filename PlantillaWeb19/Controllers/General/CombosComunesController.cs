using Entidades.Entidades.General;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.Transporte;
using PlantillaWeb19.Binders;
using PlantillaWeb19.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.General
{
    public class CombosComunesController : BaseController
    {

        [HttpGet]
        public JsonResult Campus()
        {
            IEnumerable<ComboDTO> combo = fabrica.StoredProceduresBO.ConsultarCampus().Select(c => new ComboDTO()
            {
                Label = c.Nombre,
                Value = c.Clave
            });
            return Json(combo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Modulos()
        {
            IEnumerable<ComboDTO> combo = fabrica.ModuloBO.ConsultarActivos().Select(m => new ComboDTO()
            {
                Label = m.Nombre,
                Value = m.Id.ToString()
            });
            return Json(combo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Submodulos()
        {
            IEnumerable<ComboDTO> combo = fabrica.SubModuloBO.ConsultarActivos().Select(sm => new ComboDTO()
            {
                Label = $"{sm.Nombre} ({sm.Modulo.Nombre})",
                Value = sm.Id.ToString()
            });
            return Json(combo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerEmpleados([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config,
            string EmplId = "", string Nombre = "", string CuentaDominio = "", string DptoAdscripcion = "")
        {
            PagingResult<Empleado> empleados = fabrica.StoredProceduresBO.ConsultarEmpleados(config, EmplId, Nombre, CuentaDominio, DptoAdscripcion);
            return Json(empleados, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult EstatusSolicitudCita()
        {
            IEnumerable<ComboDTO> lista = RequestUtils.ConvertEnumToComboDTO<EstatusSolicitudCita>().OrderBy(c => c.Value).ToList();

            List<ComboDTO> enumeradores = lista.ToList();
            enumeradores.Add(new ComboDTO() { Label = "Todas", Value = "100" });
            lista = enumeradores;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TipoMovimientoCita()
        {
            IEnumerable<ComboDTO> lista = RequestUtils.ConvertEnumToComboDTO<TipoMovimientoCita>().OrderBy(c => c.Value).ToList();

            List<ComboDTO> enumeradores = lista.ToList();
            enumeradores.Add(new ComboDTO() { Label = "Todas", Value = "100" });
            lista = enumeradores;

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

    }
}