using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
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
    public class ProcesoDAO : BaseDAO<Proceso>
    {

        public ProcesoDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<ProcesoGRIDDTO> ObtenerGridProcesos(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<Proceso, ProcesoGRIDDTO>> select = p => new ProcesoGRIDDTO()
            {
                Id = p.Id,
                Nombre = p.Nombre,
                
                AreaAtencion = p.AreaAtencion.Nombre + ", " + p.AreaAtencion.CampusDescr,
                Activo = p.Activo
            };

            var filtros = CrearListaFiltrosVacia();
            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencion));



            return ExecConsultaPaginada(config, select, filtros);
        }


        public new void Guardar(Proceso proceso)
        {
            Proceso procesoGuardado = this.ConsultarPorId(proceso.Id);
            if (procesoGuardado == null)
            {
                procesoGuardado = new Proceso();

            }

            procesoGuardado.Nombre = proceso.Nombre;
            procesoGuardado.Activo = proceso.Activo;
            procesoGuardado.IdAreaAtencion = proceso.IdAreaAtencion;
            procesoGuardado.EstatusRegistro = proceso.EstatusRegistro;
            procesoGuardado.FechaRegistro = proceso.FechaRegistro;
            procesoGuardado.IdUsuarioRegistro = proceso.IdUsuarioRegistro;

            base.Guardar(procesoGuardado);
        }


        public List<Proceso> ObtenerProcesosPorAreaAtencion(long idAreaAtencion)
        {
            IQueryable<Proceso> query = CrearQuery();
            return query.Where(q => q.IdAreaAtencion == idAreaAtencion).ToList();
        }

    }
}
