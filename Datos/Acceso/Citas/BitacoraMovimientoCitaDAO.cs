using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Citas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Citas
{
    public class BitacoraMovimientoCitaDAO : BaseDAO<BitacoraMovimientoCita>
    {

        public BitacoraMovimientoCitaDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<BitacoraMovimientoCitaGRIDDTO> ObtenerGrid(PagingConfig config, string Usuario, string NombreSolicitante,
            string TipoMovimientoStr, string Tramite,
            int tipoMovimiento, DateTime? fechaDesde, DateTime? fechaHasta, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<BitacoraMovimientoCita, BitacoraMovimientoCitaGRIDDTO>> select = bm => new BitacoraMovimientoCitaGRIDDTO()
            {
                Id = bm.Id,
                FechaEmision = bm.FechaEmision,
                Usuario = bm.Usuario,
                Estatus = bm.Estatus,
                EstatusStr = bm.EstatusStr,
                TipoMovimiento = bm.TipoMovimiento,
                TipoMovimientoStr = bm.TipoMovimientoStr,
                ComentarioAdministrador = bm.ComentarioAdministrador,

                FechaCita = bm.Cita.FechaCita,
                HoraCita = bm.Cita.HoraCita,
                Tramite = bm.Cita.TramiteStr,
                NombreSolicitante = bm.Cita.NombreSolicitante,
                FechaCaptura = bm.Cita.FechaCaptura,
                EmplId = bm.Cita.EmplId,
                IdCita = bm.IdCita,
                Folio = bm.Cita.Folio
            };

            Expression<Func<BitacoraMovimientoCita, DateTime?>> order = o => DbFunctions.TruncateTime(o.FechaEmision);

            var filtros = CrearListaFiltrosVacia();
            if (tipoMovimiento != 100)
            {
                filtros.Add(f => f.TipoMovimiento == (TipoMovimientoCita)tipoMovimiento);
            }

            if (fechaDesde != null)
            {
                filtros.Add(f => f.FechaEmision >= fechaDesde);
            }
            if (fechaHasta != null)
            {
                fechaHasta = fechaHasta.Value.AddHours(23).AddMinutes(59);
                filtros.Add(f => f.FechaEmision <= fechaHasta);
            }
            if (!string.IsNullOrWhiteSpace(Usuario))
            {
                filtros.Add(f => f.Usuario.Contains(Usuario));
            }
            if (!string.IsNullOrWhiteSpace(NombreSolicitante))
            {
                filtros.Add(f => f.Cita.NombreSolicitante.Contains(NombreSolicitante));
            }
            if (!string.IsNullOrWhiteSpace(TipoMovimientoStr))
            {
                filtros.Add(f => f.TipoMovimientoStr.Contains(TipoMovimientoStr));
            }
            if (!string.IsNullOrWhiteSpace(Tramite))
            {
                filtros.Add(f => f.Cita.TramiteStr.Contains(Tramite));
            }

            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.Cita.IdAreaAtencion));

            return ExecConsultaPaginadaOrdenada(config, select, filtros, order, false);
        }

    }
}
