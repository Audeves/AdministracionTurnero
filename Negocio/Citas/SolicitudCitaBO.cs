using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Citas;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades.Utils;

namespace Negocio.Citas
{
    public class SolicitudCitaBO : BaseBO<SolicitudCita>
    {

        public SolicitudCitaBO(ContextoDataBase db) : base(db)
        {

        }



        public PagingResult<SolicitudCitaGRIDDTO> ObtenerGrid(PagingConfig config, string EmplId, string NombreSolicitante,
            string AsistioStr, string TramiteActual,
            int estatus, DateTime? fechaDesde, DateTime? fechaHasta, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return SolicitudCitaDAO.ObtenerGrid(config, EmplId, NombreSolicitante, AsistioStr, TramiteActual,
                estatus, fechaDesde, fechaHasta, areasAtencionUsuarioActual);
        }

        public List<SolicitudCita> ConsultarRegistrosEXCEL(int estatus, DateTime? fechaDesde, DateTime? fechaHasta,
            List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return SolicitudCitaDAO.ConsultarRegistrosEXCEL(estatus, fechaDesde, fechaHasta, areasAtencionUsuarioActual);
        }

        public SolicitudCita GuardarRevisionAdministrador(long id, EstatusSolicitudCita estatus, string comentariosAdministrador, 
            string usuarioCuentaDominio)
        {
            return SolicitudCitaDAO.GuardarRevisionAdministrador(id, estatus, comentariosAdministrador, usuarioCuentaDominio);
        }

        public List<SolicitudCitaGRIDDTO> ObtenerListaGridCitasHoy(List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return SolicitudCitaDAO.ObtenerListaGridCitasHoy(areasAtencionUsuarioActual).Select(sc => new SolicitudCitaGRIDDTO()
            {
                Id = sc.Id,
                IdCita = sc.IdCita,
                Estatus = sc.Estatus,
                Asistio = sc.Asistio,
                FechaCita = sc.FechaCita,
                HoraCita = sc.HoraCita,
                FechaCaptura = sc.FechaCaptura,
                TramiteActual = sc.Tramite.Tramite,
                AreaAtencion = sc.AreaAtencion.Nombre + ", " + sc.AreaAtencion.CampusDescr,
                NombreSolicitante = sc.NombreSolicitante,
                TramiteCapturado = sc.TramiteStr,
                EmplId = sc.EmplId,
                Telefono = sc.Telefono,
                PersonaAutorizada = sc.PersonaAutorizada,
                Parentezco = sc.Parentezco,
                Folio = sc.Folio
            }).OrderBy(sc => sc.FechaCitaStr).ToList();
        }

        public void RegistrarAsistencia(long id, string usuarioCuentaDominio)
        {
            SolicitudCitaDAO.RegistrarAsistencia(id, usuarioCuentaDominio);
        }


        public List<SolicitudCita> ObtenerCitasCancelacionMasiva(DateTime? fechaDesde, DateTime? fechaHasta, bool pendientes, bool agendadas,
            List<AreaAtencion> areasAtencionUsuarioActual)
        {
            if (fechaDesde == null)
            {
                fechaDesde = DateTime.Now.Date;
            }

            return SolicitudCitaDAO.ObtenerCitasCancelacionMasiva(fechaDesde.Value, fechaHasta, pendientes, agendadas, areasAtencionUsuarioActual);
        }


        public void CancelacionMasiva(List<SolicitudCita> citas, string comentarioCancelacion, string usuarioCuentaDominio)
        {
            SolicitudCitaDAO.CancelacionMasiva(citas, comentarioCancelacion, usuarioCuentaDominio);
            
        }

    }
}
