using Datos.Acceso.General;
using Entidades.Entidades.Configuracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Configuracion
{
    public class TramiteDAO : BaseDAO<Tramite>
    {

        public TramiteDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<TramiteGRIDDTO> ObtenerGridTramites(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<Tramite, TramiteGRIDDTO>> select = t => new TramiteGRIDDTO()
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Proceso = t.Proceso.Nombre,
                Activo = t.Activo
            };

            var filtros = CrearListaFiltrosVacia();
            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencionProceso));

            return ExecConsultaPaginada(config, select, filtros);
        }

        public new void Guardar(Tramite tramite)
        {
            Tramite tramiteGuardado = this.ConsultarPorId(tramite.Id);
            if (tramiteGuardado == null)
            {
                tramiteGuardado = new Tramite();

            }

            tramiteGuardado.Nombre = tramite.Nombre;
            tramiteGuardado.Activo = tramite.Activo;
            tramiteGuardado.IdProceso = tramite.IdProceso;
            tramiteGuardado.RequiereExpediente = tramite.RequiereExpediente;
            tramiteGuardado.CorreoExpediente = tramite.CorreoExpediente;
            tramiteGuardado.IdAreaAtencionProceso = tramite.IdAreaAtencionProceso;
            tramiteGuardado.EstatusRegistro = tramite.EstatusRegistro;
            tramiteGuardado.FechaRegistro = tramite.FechaRegistro;
            tramiteGuardado.IdUsuarioRegistro = tramite.IdUsuarioRegistro;

            base.Guardar(tramiteGuardado);
        }

    }
}
