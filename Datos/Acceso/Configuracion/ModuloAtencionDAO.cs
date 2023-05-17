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
    public class ModuloAtencionDAO : BaseDAO<ModuloAtencion>
    {

        public ModuloAtencionDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<ModuloAtencionGRIDDTO> ObtenerGridModulosAtencion(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<ModuloAtencion, ModuloAtencionGRIDDTO>> select = ma => new ModuloAtencionGRIDDTO()
            {
                Id = ma.Id,
                Nombre = ma.Nombre,
                AreaAtencion = ma.AreaAtencion.Nombre,
                UsuariosAtencion = ma.UsuariosAtencion,
                Activo = ma.Activo
            };

            var filtros = CrearListaFiltrosVacia();
            List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();
            filtros.Add(f => idsAreasAtencionUsuarioActual.Contains(f.IdAreaAtencion));

            return ExecConsultaPaginada(config, select, filtros);
        }


        public new void Guardar(ModuloAtencion moduloAtencion)
        {
            ModuloAtencion moduloAtencionGuardado = this.ConsultarPorId(moduloAtencion.Id, "Procesos", "UsuariosAtencion");
            if (moduloAtencionGuardado == null)
            {
                moduloAtencionGuardado = new ModuloAtencion();
            }

            moduloAtencionGuardado.Nombre = moduloAtencion.Nombre;
            moduloAtencionGuardado.Activo = moduloAtencion.Activo;
            moduloAtencionGuardado.IdAreaAtencion = moduloAtencion.IdAreaAtencion;

            var idsProcesosSeleccionados = moduloAtencion.Procesos.Select(p => p.Id).ToList();
            var idsProcesosGuardados = moduloAtencionGuardado.Procesos.Select(pg => pg.Id).ToList();
            moduloAtencionGuardado.Procesos.RemoveAll(pg => !idsProcesosSeleccionados.Contains(pg.Id));
            moduloAtencionGuardado.Procesos.AddRange(moduloAtencion.Procesos.Where(p => !idsProcesosGuardados.Contains(p.Id)));

            var idsUsuariosSeleccionados = moduloAtencion.UsuariosAtencion.Select(p => p.Id).ToList();
            var idsUsuariosGuardados = moduloAtencionGuardado.UsuariosAtencion.Select(pg => pg.Id).ToList();
            moduloAtencionGuardado.UsuariosAtencion.RemoveAll(ug => !idsUsuariosSeleccionados.Contains(ug.Id));
            moduloAtencionGuardado.UsuariosAtencion.AddRange(moduloAtencion.UsuariosAtencion.Where(u => !idsUsuariosGuardados.Contains(u.Id)));

            foreach (Proceso item in moduloAtencionGuardado.Procesos)
            {
                db.Proceso.Attach(item);
            }

            foreach (Usuario item in moduloAtencionGuardado.UsuariosAtencion)
            {
                db.Usuario.Attach(item);
            }

            base.Guardar(moduloAtencionGuardado);
        }

    }
}
