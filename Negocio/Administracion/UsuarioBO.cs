using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using Entidades.Enumeradores;
using Entidades.Excepciones;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Administracion;
using Entidades.Utils;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Administracion
{
    public class UsuarioBO : BaseBO<Usuario>
    {

        public UsuarioBO(ContextoDataBase db) : base(db)
        {

        }

        public Usuario ConsultarPorCuentaDominio(string cuentaDominio)
        {
            Usuario usuario = UsuarioDAO.ConsultarPorCuentaDominio(cuentaDominio);
            return usuario;
        }

        //Metodo para agregar la pantalla de inicio a los permisos del usuario
        // al ser una pantalla privada tiene que estar asignada a un perfil, pero para evitar crear ese registro por bd ya que aplicara para todos los perfiles
        // se lo agrego por codigo.
        public Usuario AgregarPermisoPantallaInicioUsuarioValido(Usuario usuario)
        {
            usuario.Perfiles[0].Pantallas.Add(PantallaDAO.ConsultarPantallaPorAccion("Inicio"));
            return usuario;
        }

        public List<UsuarioGRIDDTO> ObtenerGridUsuarios(Usuario usuarioActivo)
        {
            List<Usuario> result = UsuarioDAO.ObtenerGridUsuarios();
            if (!usuarioActivo.Desarrollador)
            {
                result = result.Where(r => r.Desarrollador == false && r.Perfiles.Any(p => p.AreasAtencion.Any(a =>
                    usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(a.Id)))).ToList();
            }

            return result.Select(u => new UsuarioGRIDDTO
            {
                Id = u.Id,
                Emplid = u.Emplid,
                Nombre = u.Nombre,
                DptoAdscripcion = u.DptoAdscripcion,
                CuentaDominio = u.CuentaDominio,
                Activo = u.Activo
            }).ToList();
        }

        public void Guardar(Usuario usuario, Usuario usuarioActivo)
        {
            //revisar si existe el empleado en tabla usuarios...
            if (UsuarioDAO.ConsultarUsuarioExistenteEnSistema(usuario.Emplid))
            {
                //throw new ExcepcionNegocio("El empleado ya esta registrado en el sistema.");
                UsuarioDAO.ActualizarUsuarioExistenteEnSistemaNoVisible(usuario, usuarioActivo);
            }
            else
            {
                UsuarioDAO.Guardar(usuario, usuarioActivo);
            }
        }

        public List<Usuario> ConsultarUsuariosResponsablesActivos()
        {
            return UsuarioDAO.ConsultarUsuariosResponsablesActivos();
        }

        public List<UsuarioGRIDDTO> ConsultarUsuariosAtencionVentanillaActivos(long idAreaAtencion)
        {
            List<Usuario> result = UsuarioDAO.ConsultarUsuariosAtencionVentanillaActivos(idAreaAtencion);
            //List<long> idsAreasAtencionUsuarioActual = areasAtencionUsuarioActual.Select(a => a.Id).ToList();

            //result = result.Where(r => r.Perfiles.Any(p => p.Nombre.Contains(Constantes.AtencionVentanilla) && p.AreasAtencion.Any(a =>
            //        idsAreasAtencionUsuarioActual.Contains(a.Id)))).ToList();

            return result.Select(u => new UsuarioGRIDDTO
            {
                Id = u.Id,
                Emplid = u.Emplid,
                Nombre = u.Nombre,
                DptoAdscripcion = u.DptoAdscripcion,
                CuentaDominio = u.CuentaDominio,
                Activo = u.Activo
            }).ToList();
        }

    }
}
