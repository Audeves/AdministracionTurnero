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
    public class ModuloDAO : BaseDAO<Modulo>
    {

        public ModuloDAO(ContextoDataBase db) : base(db)
        {

        }


        public new Modulo Guardar(Modulo modulo)
        {
            Modulo moduloGuardado = this.ConsultarPorId(modulo.Id);

            if (moduloGuardado == null)
            {
                moduloGuardado = new Modulo();
            }

            moduloGuardado.Nombre = modulo.Nombre;
            moduloGuardado.Activo = modulo.Activo;

            base.Guardar(moduloGuardado);
            return moduloGuardado;
        }

        public PagingResult<ModuloGRIDDTO> ObtenerGridModulos(PagingConfig config)
        {
            Expression<Func<Modulo, ModuloGRIDDTO>> select = (m) => new ModuloGRIDDTO()
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Activo = m.Activo
            };

            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

        public List<Modulo> ConsultarActivos()
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => f.Activo == true);

            return FindByFilters(filtros).ToList();
        }
    }
}
