using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Administracion;
using Entidades.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Administracion
{
    public class UsuarioDAO : BaseDAO<Usuario>
    {

        public List<string> RelacionesCompletas = new List<string>()
        {
            "Perfiles",
            "Perfiles.Pantallas",
            "Perfiles.Pantallas.PantallasAcciones",
            "Perfiles.Pantallas.Modulo",
            "Perfiles.Pantallas.SubModulo.Modulo",
            "AreasAtencionResponsable.Responsables",
            "Perfiles.AreasAtencion",
            "Perfiles.AreasAtencion.Responsables",
            "ModulosAtencion"
        };

        public UsuarioDAO(ContextoDataBase db) : base(db)
        {

        }


        public Usuario ConsultarPorCuentaDominio(string cuentaDominio)
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(u => u.CuentaDominio == cuentaDominio);
            filtros.Add(u => u.Activo == true);

            Usuario usuario = FindUniqueByFilters(filtros, RelacionesCompletas.ToArray());
            return usuario;
        }

        public List<Usuario> ObtenerGridUsuarios()
        {
            IQueryable<Usuario> usuarios = CrearQuery("Perfiles", "Perfiles.AreasAtencion");
            return usuarios.ToList();
            //Expression<Func<Usuario, UsuarioGRIDDTO>> select = u => new UsuarioGRIDDTO()
            //{
            //    Id = u.Id,
            //    Emplid = u.Emplid,
            //    Nombre = u.Nombre,
            //    DptoAdscripcion = u.DptoAdscripcion,
            //    CuentaDominio = u.CuentaDominio,
            //    Activo = u.Activo
            //};

            //var filtros = CrearListaFiltrosVacia();

            //Expression<Func<Usuario, string>> order = o => o.Nombre;

            //return ExecConsultaPaginadaOrdenada(config, select, filtros, order);
        }

        public new void Guardar(Usuario usuario, Usuario usuarioActivo)
        {
            Usuario usuarioGuardado = this.ConsultarPorId(usuario.Id, "Perfiles", "Perfiles.AreasAtencion");
            if (usuarioGuardado == null)
            {
                usuarioGuardado = new Usuario();
                usuarioGuardado.Emplid = usuario.Emplid;
                usuarioGuardado.Nombre = usuario.Nombre;
                usuarioGuardado.CuentaDominio = usuario.CuentaDominio;
                usuarioGuardado.DptoAdscripcion = usuario.DptoAdscripcion;
                usuarioGuardado.ClaveDptoAdscripcion = usuario.ClaveDptoAdscripcion;
                usuarioGuardado.Correo = new StoredProceduresDAO(db).ConsultarCorreoEmpleado(usuario.Emplid);
            }

            usuarioGuardado.Activo = usuario.Activo;
            if (usuarioGuardado.Correo == null && usuarioGuardado != null)
            {
                usuarioGuardado.Correo = new StoredProceduresDAO(db).ConsultarCorreoEmpleado(usuarioGuardado.Emplid);
            }

            var idsPerfilesSeleccionados = usuario.Perfiles.Select(p => p.Id).ToList();
            var idsPerfilesGuardados = usuarioGuardado.Perfiles.Select(p => p.Id).ToList();
            //Uso la misma mañosada que use para guardar los perfiles...

            List<Perfil> usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable = usuarioGuardado.Perfiles.Where(ugp =>
                ugp.AreasAtencion.Any(ugpa => usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(ugpa.Id))).ToList();

            usuarioGuardado.Perfiles.RemoveAll(p => usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable
                .Select(ugpcar => ugpcar.Id).Contains(p.Id));


            usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable.RemoveAll(ugpcar => !idsPerfilesSeleccionados.Contains(ugpcar.Id));
            usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable.AddRange(usuario.Perfiles.Where(p => !idsPerfilesGuardados.Contains(p.Id)));

            usuarioGuardado.Perfiles.AddRange(usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable);

            //usuarioActivo.AreasAtencionResponsable.Select(a => a.Id).Contains(ugp.AreasAtencion.SelectMany<AreaAtencion, long>(ua => ua.Id))

            //usuarioGuardado.Perfiles.RemoveAll(p => !idsPerfilesSeleccionados.Contains(p.Id));
            //usuarioGuardado.Perfiles.AddRange(usuario.Perfiles.Where(p => !idsPerfilesGuardados.Contains(p.Id)));

            foreach (Perfil item in usuarioGuardado.Perfiles)
            {
                db.Perfil.Attach(item);
            }

            base.Guardar(usuarioGuardado);
        }

        public bool ConsultarUsuarioExistenteEnSistema(string emplid)
        {
            IQueryable<Usuario> query = CrearQuery();
            return query.Where(u => u.Emplid == emplid).SingleOrDefault() != null ? true : false;
        }

        public void ActualizarUsuarioExistenteEnSistemaNoVisible(Usuario usuario, Usuario usuarioActivo)
        {
            IQueryable<Usuario> query = CrearQuery("Perfiles", "Perfiles.AreasAtencion");
            Usuario usuarioGuardado = query.Where(u => u.Emplid == usuario.Emplid).SingleOrDefault();

            if (usuarioGuardado.Correo == null)
            {
                usuarioGuardado.Correo = new StoredProceduresDAO(db).ConsultarCorreoEmpleado(usuarioGuardado.Emplid);
            }

            var idsPerfilesSeleccionados = usuario.Perfiles.Select(p => p.Id).ToList();
            var idsPerfilesGuardados = usuarioGuardado.Perfiles.Select(p => p.Id).ToList();
            //Uso la misma mañosada que use para guardar los perfiles...

            List<Perfil> usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable = usuarioGuardado.Perfiles.Where(ugp =>
                ugp.AreasAtencion.Any(ugpa => usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(ugpa.Id))).ToList();

            usuarioGuardado.Perfiles.RemoveAll(p => usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable
                .Select(ugpcar => ugpcar.Id).Contains(p.Id));


            usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable.RemoveAll(ugpcar => !idsPerfilesSeleccionados.Contains(ugpcar.Id));
            usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable.AddRange(usuario.Perfiles.Where(p => !idsPerfilesGuardados.Contains(p.Id)));

            usuarioGuardado.Perfiles.AddRange(usuarioGuardadoPerfilesCorrespondientesAAreasAtencionResponsable);

            foreach (Perfil item in usuarioGuardado.Perfiles)
            {
                db.Perfil.Attach(item);
            }

            base.Guardar(usuarioGuardado);
        }

        public List<Usuario> ConsultarUsuariosResponsablesActivos()
        {
            IQueryable<Usuario> usuarios = CrearQuery("Perfiles");
            return usuarios.Where(u => u.Activo == true && u.Perfiles.Any(p => p.Nombre == Constantes.Responsable)).OrderBy(u => u.Nombre).ToList();
        }

        public List<Usuario> ConsultarUsuariosAtencionVentanillaActivos(long idAreaAtencion)
        {
            IQueryable<Usuario> usuarios = CrearQuery("Perfiles", "Perfiles.AreasAtencion");
            return usuarios.Where(u => u.Activo == true && u.Perfiles.Any(p => 
                p.Nombre.Contains(Constantes.AtencionVentanilla) && p.AreasAtencion.Any(
                    a => a.Id == idAreaAtencion)))
                .OrderBy(u => u.Nombre).ToList();
        }


    }
}
