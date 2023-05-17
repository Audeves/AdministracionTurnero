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
    public class PantallasController : BaseController
    {
        
        [HttpGet]
        public JsonResult ObtenerGridPantallas([ModelBinder(typeof(PagingConfigBinder))]PagingConfig config)
        {
            Usuario usuarioActivo = ObtenerUsuarioActivo();
            PagingResult<PantallaGRIDDTO> result = fabrica.PantallaBO.ObtenerGridPantallas(config, usuarioActivo.Desarrollador);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Guardar(Pantalla pantalla)
        {
            RespuestaDTO respuesta;
            try
            {
                fabrica.PantallaBO.Guardar(pantalla);
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Pantalla guardada correctamente.");
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
            Pantalla pantalla = fabrica.PantallaBO.Consultar(id, "PantallasAcciones", "Modulo", "SubModulo");
            var pantallaDTO = new
            {
                Id = pantalla.Id,
                Nombre = pantalla.Nombre,
                Controlador = pantalla.Controlador,
                Accion = pantalla.Accion,
                ClaseIcono = pantalla.ClaseIcono,
                IdModulo = pantalla.IdModulo,
                IdSubModulo = pantalla.IdSubModulo,
                Privado = pantalla.Privado,
                Activo = pantalla.Activo,
                Acciones = pantalla.PantallasAcciones.Select(pa => new
                {
                    Id = pa.Id,
                    Controlador = pa.Controlador,
                    Accion = pa.Accion,
                    Privado = pa.Privado,
                    Activo = pa.Activo
                }),
                IdsAcciones = pantalla.PantallasAcciones.Select(pa => pa.Id)
            };
            return Json(pantallaDTO, JsonRequestBehavior.AllowGet);
        }

    }
}