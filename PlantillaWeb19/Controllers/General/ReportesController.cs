using Entidades.Entidades.Citas;
using Entidades.Enumeradores;
using Entidades.Transporte.Reportes;
using PlantillaWeb19.GestionReportes.Generadores;
using PlantillaWeb19.GestionReportes.Utils;
using PlantillaWeb19.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.General
{
    public class ReportesController : BaseController
    {

        [DateTimeAttribute(Field = "fechaDesde")]
        [DateTimeAttribute(Field = "fechaHasta")]
        public ActionResult SolicitudesCitas(int estatus, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            List<SolicitudCita> registros = fabrica.SolicitudCitaBO.ConsultarRegistrosEXCEL(estatus, fechaDesde, fechaHasta, ObtenerAreasAtencionTotalesUsuarioActivo());

            List<SolicitudCitaReporteDTO> registrosDTO = registros.Select(r => new SolicitudCitaReporteDTO()
            {
                Id = r.Id,
                IdCita = r.IdCita,
                Estatus = r.Estatus,
                Asistio = r.Asistio,
                FechaCita = r.FechaCita,
                HoraCita = r.HoraCita,
                FechaCaptura = r.FechaCaptura,
                TramiteActual = r.Tramite.Tramite,
                AreaAtencion = r.AreaAtencion.Nombre + ", " + r.AreaAtencion.CampusDescr,
                NombreSolicitante = r.NombreSolicitante,
                TramiteCapturado = r.TramiteStr,
                EmplId = r.EmplId,
                Telefono = r.Telefono,
                Email = r.Email,
                ComentariosAdministrador = r.ComentariosAdministrador,

                PersonaAutorizada = r.PersonaAutorizada,
                Parentezco = r.Parentezco,
                Folio = r.Folio,
                ComentariosAdicionales = r.ComentariosAdicionales
            }).ToList();

            GeneradorReportesCitas generador = new GeneradorReportesCitas(Server);
            Reporte reporte = generador.GenerarReporteSolicitudesCitas(registrosDTO);

            string estatusParaDescr;
            if (estatus != 100)
            {
                EstatusSolicitudCita estatusReal = (EstatusSolicitudCita)estatus;
                estatusParaDescr = estatusReal.GetDescription();
            }
            else
            {
                estatusParaDescr = "Toda";
            }

            string nombre = $"Citas {estatusParaDescr}s.xls";
            return File(reporte.FileBytes, reporte.MimeType, nombre);
        }

    }
}