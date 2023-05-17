using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.Transporte.Reportes;
using Entidades.TransporteGrid.Citas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entidades.Utils;

namespace Datos.Acceso.Citas
{
    public class SolicitudCitaDAO : BaseDAO<SolicitudCita>
    {

        public SolicitudCitaDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<SolicitudCitaGRIDDTO> ObtenerGrid(PagingConfig config, string EmplId, string NombreSolicitante,
            string AsistioStr, string TramiteActual,
            int estatus, DateTime? fechaDesde, DateTime? fechaHasta, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<SolicitudCita, SolicitudCitaGRIDDTO>> select = sc => new SolicitudCitaGRIDDTO()
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
                Telefono = sc.Telefono
            };

            Expression<Func<SolicitudCita, DateTime?>> order = o => DbFunctions.TruncateTime(o.FechaCita);
            Expression<Func<SolicitudCita, string>> thenBy = o => o.HoraCita;

            var filtros = CrearListaFiltrosVacia();
            //no mms... piden todos y era enum...
            if (estatus != 100)
            {
                filtros.Add(f => f.Estatus == (EstatusSolicitudCita)estatus);
            }
            
            if (fechaDesde != null)
            {
                filtros.Add(f => f.FechaCita >= fechaDesde);
            }
            if (fechaHasta != null)
            {
                filtros.Add(f => f.FechaCita <= fechaHasta);
            }
            if (!string.IsNullOrWhiteSpace(EmplId))
            {
                filtros.Add(f => f.EmplId.Contains(EmplId));
            }
            if (!string.IsNullOrWhiteSpace(NombreSolicitante))
            {
                filtros.Add(f => f.NombreSolicitante.Contains(NombreSolicitante));
            }
            if (!string.IsNullOrWhiteSpace(AsistioStr))
            {
                if (AsistioStr.ToLower().Contains("si"))
                {
                    filtros.Add(f => f.Asistio == true);
                }
                else if (AsistioStr.ToLower().Contains("no"))
                {
                    filtros.Add(f => f.Asistio == false);
                }
            }
            if (!string.IsNullOrWhiteSpace(TramiteActual))
            {
                filtros.Add(f => f.Tramite.Tramite.Contains(TramiteActual));
            }

            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencion));

            return ExecConsultaPaginadaOrdenadaDoble(config, select, filtros, order, thenBy);
        }


        public List<SolicitudCita> ConsultarRegistrosEXCEL(int estatus, DateTime? fechaDesde, DateTime? fechaHasta,
            List<AreaAtencion> areasAtencionUsuarioActual)
        {
            var filtros = CrearListaFiltrosVacia();

            //no mms... piden todos y era enum...
            if (estatus != 100)
            {
                filtros.Add(f => f.Estatus == (EstatusSolicitudCita)estatus);
            }
            
            if (fechaDesde != null)
            {
                filtros.Add(f => f.FechaCita >= fechaDesde);
            }
            if (fechaHasta != null)
            {
                filtros.Add(f => f.FechaCita <= fechaHasta);
            }

            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencion));

            return FindByFilters(filtros, "AreaAtencion", "Tramite").ToList();
        }

        public SolicitudCita GuardarRevisionAdministrador(long id, EstatusSolicitudCita estatus, string comentariosAdministrador,
            string usuarioCuentaDominio)
        {
            SolicitudCita citaGuardada = this.ConsultarPorId(id, "AreaAtencion", "Tramite", "BitacoraMovimientos");
            citaGuardada.Estatus = estatus;
            citaGuardada.ComentariosAdministrador = comentariosAdministrador;


            /********************* Preparo y agrego el movimiento *********************/
            TipoMovimientoCita tipoMovimiento = citaGuardada.Estatus == EstatusSolicitudCita.Agendada ? TipoMovimientoCita.Agendar : TipoMovimientoCita.Cancelar;

            citaGuardada.BitacoraMovimientos.Add(new BitacoraMovimientoCita()
            {
                FechaEmision = DateTime.Now,
                Usuario = usuarioCuentaDominio,
                Estatus = citaGuardada.Estatus,
                EstatusStr = citaGuardada.Estatus.GetDescriptionEnum(),
                Asistio = citaGuardada.Asistio,
                TipoMovimiento = tipoMovimiento,
                TipoMovimientoStr = tipoMovimiento.GetDescriptionEnum(),
                ComentarioAdministrador = citaGuardada.ComentariosAdministrador
            });
            /******************************************/

            base.Guardar(citaGuardada);

            return citaGuardada;
        }


        public List<SolicitudCita> ObtenerListaGridCitasHoy(List<AreaAtencion> areasAtencionUsuarioActual)
        {
            var hoy = DateTime.Today;

            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => f.Estatus == EstatusSolicitudCita.Agendada);
            filtros.Add(f => DbFunctions.TruncateTime(f.FechaCita) == hoy);
            filtros.Add(f => f.Asistio == false);
            

            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencion));

            return FindByFilters(filtros, "AreaAtencion", "Tramite").ToList();
        }


        public void RegistrarAsistencia(long id, string usuarioCuentaDominio)
        {
            SolicitudCita citaGuardada = this.ConsultarPorId(id, "BitacoraMovimientos");
            citaGuardada.Asistio = true;

            /********************* Preparo y agrego el movimiento *********************/
            TipoMovimientoCita tipoMovimiento = TipoMovimientoCita.RegistroAsistencia;

            citaGuardada.BitacoraMovimientos.Add(new BitacoraMovimientoCita()
            {
                FechaEmision = DateTime.Now,
                Usuario = usuarioCuentaDominio,
                Estatus = citaGuardada.Estatus,
                EstatusStr = citaGuardada.Estatus.GetDescriptionEnum(),
                Asistio = citaGuardada.Asistio,
                TipoMovimiento = tipoMovimiento,
                TipoMovimientoStr = tipoMovimiento.GetDescriptionEnum()
            });
            /******************************************/

            base.Guardar(citaGuardada);
        }


        public List<SolicitudCita> ObtenerCitasCancelacionMasiva(DateTime fechaDesde, DateTime? fechaHasta, bool pendientes, bool agendadas,
            List<AreaAtencion> areasAtencionUsuarioActual)
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => f.FechaCita >= fechaDesde);
            filtros.Add(f => f.Asistio == false);
            if (fechaHasta != null)
            {
                filtros.Add(f => f.FechaCita <= fechaHasta);
            }
            if (pendientes && agendadas)
            {
                filtros.Add(f => f.Estatus == EstatusSolicitudCita.Pendiente || f.Estatus == EstatusSolicitudCita.Agendada);
            }
            else
            {
                if (pendientes)
                {
                    filtros.Add(f => f.Estatus == EstatusSolicitudCita.Pendiente);
                }
                if (agendadas)
                {
                    filtros.Add(f => f.Estatus == EstatusSolicitudCita.Agendada);
                }
            }

            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencion));

            return FindByFilters(filtros, "AreaAtencion", "Tramite", "BitacoraMovimientos").ToList();
        }



        public void CancelacionMasiva(List<SolicitudCita> citas, string comentarioCancelacion, string usuarioCuentaDominio)
        {
            citas.ForEach(c => {
                c.Estatus = EstatusSolicitudCita.Cancelada; c.ComentariosAdministrador = comentarioCancelacion;

                /********************* Preparo y agrego el movimiento *********************/
                TipoMovimientoCita tipoMovimiento = TipoMovimientoCita.CancelacionMasiva;

                c.BitacoraMovimientos.Add(new BitacoraMovimientoCita()
                {
                    FechaEmision = DateTime.Now,
                    Usuario = usuarioCuentaDominio,
                    Estatus = c.Estatus,
                    EstatusStr = c.Estatus.GetDescriptionEnum(),
                    Asistio = c.Asistio,
                    TipoMovimiento = tipoMovimiento,
                    TipoMovimientoStr = tipoMovimiento.GetDescriptionEnum(),
                    ComentarioAdministrador = c.ComentariosAdministrador
                });
                /******************************************/
            });

            base.Guardar(citas);
        }

    }
}
