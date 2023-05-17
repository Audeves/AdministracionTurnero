using Datos.Acceso.General;
using Entidades.Entidades.Principales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Principales
{
    public class ModuloAtencionBitacoraDAO : BaseDAO<ModuloAtencionBitacora>
    {

        public ModuloAtencionBitacoraDAO(ContextoDataBase db) : base(db)
        {
            
        }


        public bool ValidarModuloLibre(long idModulo)
        {
            IQueryable<ModuloAtencionBitacora> query = CrearQuery();
            return query.Where(q => q.IdModuloAtencion == idModulo && q.Activo).SingleOrDefault() == null;
        }

        public bool ValidarUsuarioNoAtendiendo(long idUsuario)
        {
            IQueryable<ModuloAtencionBitacora> query = CrearQuery();
            return query.Where(q => q.IdUsuario == idUsuario && q.Activo).SingleOrDefault() == null;
        }

        public void IniciarAtencion(long idModulo, long idUsuario)
        {
            ModuloAtencionBitacora bitacora = new ModuloAtencionBitacora()
            {
                IdModuloAtencion = idModulo,
                IdUsuario = idUsuario,
                FechaInicio = DateTime.Now,
                Activo = true
            };

            base.Guardar(bitacora);
        }

        public void FinalizarAtencion(long idUsuario)
        {
            ModuloAtencionBitacora bitacoraActiva = ObtenerRegistroBitacoraActivo(idUsuario);

            bitacoraActiva.FechaTermino = DateTime.Now;
            bitacoraActiva.Activo = false;

            base.Guardar(bitacoraActiva);
        }

        public ModuloAtencionBitacora ObtenerRegistroBitacoraActivo(long idUsuario)
        {
            IQueryable<ModuloAtencionBitacora> query = CrearQuery();
            return query.Where(q => q.IdUsuario == idUsuario && q.Activo).SingleOrDefault();
        }

    }
}
