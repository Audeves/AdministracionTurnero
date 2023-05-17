using Datos.Acceso.General;
using Entidades.Entidades.Configuracion;
using Entidades.Entidades.Principales;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Principales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Principales
{
    public class TurnoDAO : BaseDAO<Turno>
    {

        public TurnoDAO(ContextoDataBase db) : base(db)
        {

        }

        public List<Turno> ObtenerTurnosVentanilla(ModuloAtencion ventanilla)
        {
            IQueryable<Turno> query = CrearQuery();
            query = query.Where(q => q.IdAreaAtencion == ventanilla.IdAreaAtencion
                && ventanilla.Procesos.Select(vp => vp.Id).Contains(q.IdProceso));
            //AQUI ME QUEDE!!!
            return null;
        }
        //Coneccion a la tabla turnos por atender
        public PagingResult<TurnoGRIDDTO> ObtenerGridTurnoPorAtender(PagingConfig config)
        {
            Expression<Func<Turno, TurnoGRIDDTO>> select = a => new TurnoGRIDDTO()
            {
                Id = a.Id,
                NombreCliente = a.NombreCliente,
                NumeroTurno = a.NumeroTurno,
                NombreTramite = a.Tramite.Nombre,
        };
            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

        //Coneccion a la tabla turno en espera
        public PagingResult<TurnoGRIDDTO> ObtenerGridTurnosEnEspera(PagingConfig config)
        {
            Expression<Func<Turno, TurnoGRIDDTO>> select = a => new TurnoGRIDDTO()
            {
                Id = a.Id,
                NombreCliente = a.NombreCliente,
                NumeroTurno = a.NumeroTurno,
                NombreTramite = a.Tramite.Nombre,
            };
            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

    }
}
