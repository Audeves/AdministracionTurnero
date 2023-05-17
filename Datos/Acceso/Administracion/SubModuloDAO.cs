using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Administracion
{
    public class SubModuloDAO : BaseDAO<SubModulo>
    {

        public SubModuloDAO(ContextoDataBase db) : base(db)
        {

        }


        public new SubModulo Guardar(SubModulo subModulo)
        {
            SubModulo subModuloGuardado = this.ConsultarPorId(subModulo.Id);
            if (subModuloGuardado == null)
            {
                subModuloGuardado = new SubModulo();
            }

            subModuloGuardado.Nombre = subModulo.Nombre;
            subModuloGuardado.Activo = subModulo.Activo;
            subModuloGuardado.ClaseIcono = subModulo.ClaseIcono;
            subModuloGuardado.IdModulo = subModulo.IdModulo;

            base.Guardar(subModuloGuardado);
            return subModuloGuardado;
        }

        public PagingResult<SubModuloGRIDDTO> ObtenerGridSubModulos(PagingConfig config)
        {
            Expression<Func<SubModulo, SubModuloGRIDDTO>> select = (m) => new SubModuloGRIDDTO()
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Modulo = m.Modulo.Nombre,
                ClaseIcono = m.ClaseIcono,
                Activo = m.Activo
            };

            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

        public List<SubModulo> ConsultarActivos()
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => f.Activo == true);

            return FindByFilters(filtros, "Modulo").ToList();
        }

    }
}
