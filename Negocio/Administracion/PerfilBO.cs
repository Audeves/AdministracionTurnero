using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Administracion;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Administracion
{
    public class PerfilBO : BaseBO<Perfil>
    {

        public PerfilBO(ContextoDataBase db) : base(db)
        {

        }


        /// <summary>
        /// Convierto el relajo de perfiles, pantallas y acciones a objetos mas simples para manejarlo mas facilmente al validar permiso en cada ejecucion
        /// de accion.
        /// Filtro cada cosa activa aqui para evitar problemas de validaciones al hacer la consulta de usuario por cuenta de dominio.
        /// </summary>
        /// <param name="perfiles"></param>
        /// <returns></returns>
        public List<ExcepcionControladorAccion> ConvertirPerfilesEnPermisosSimplificado(List<Perfil> perfiles)
        {         
            List<Pantalla> pantallas = perfiles.SelectMany(pu => pu.Pantallas).GroupBy(p => p.Id).Select(p => p.First()).ToList();
            pantallas = pantallas.Where(p => p.Activo == true).ToList();
            //dejo pasar aquellas pantallas directas a modulo activo
            List<Pantalla> pantallasEnModulo = pantallas.Where(p => (p.Modulo != null && p.Modulo.Activo == true)).ToList();
            //y luego las que van dentro de submodulo activo
            List<Pantalla> pantallasEnSubmodulo = pantallas.Where(p => (p.SubModulo != null && p.SubModulo.Activo == true && p.SubModulo.Modulo.Activo == true)).ToList();
            List<Pantalla> pantallasActivas = new List<Pantalla>();
            pantallasActivas.AddRange(pantallasEnModulo);
            pantallasActivas.AddRange(pantallasEnSubmodulo);
            //Si en sus perfiles no tiene ninguna pantalla activa regreso la lista vacia...
            if (pantallasActivas.Count == 0)
            {
                return new List<ExcepcionControladorAccion>();
            }
            //Invoco al metodo para agregar una pantalla especifica, en este caso la pantalla de inicio a los permisos del usuario
            // al ser una pantalla privada tiene que estar asignada a un perfil, pero para evitar crear ese registro por bd ya que aplicara para todos los perfiles
            // se lo agrego por codigo.
            pantallasActivas.Add(PantallaDAO.ConsultarPantallaPorAccion("Inicio"));
            List<ExcepcionControladorAccion> permisosSimplificados = pantallasActivas.Select(p => new ExcepcionControladorAccion()
            {
                Controlador = p.Controlador,
                Accion = p.Accion,
                EsPantalla = true
            }).ToList();
            permisosSimplificados.AddRange(pantallasActivas.SelectMany(p => p.PantallasAcciones.Where(pa => pa.Activo == true).Select(pa => new ExcepcionControladorAccion()
            {
                Controlador = pa.Controlador,
                Accion = pa.Accion,
                EsAccion = true
            }).ToList()).ToList());


            //Obtengo las acciones de pantalla publicas que no dependen de una pantalla
            //Como esta lista es independiente del usuario, internamente si tiene el filtro de activo asi que ya no lo pongo aqui.
            List<PantallaAccion> acciones = PantallaAccionDAO.ConsultarAccionesPrivadasSinPantalla();
            permisosSimplificados.AddRange(acciones.Select(pa => new ExcepcionControladorAccion()
            {
                Controlador = pa.Controlador,
                Accion = pa.Accion,
                EsAccion = true
            }).ToList());

            return permisosSimplificados;
        }

        public List<PerfilGRIDDTO> ObtenerGridPerfiles(Usuario usuarioActivo)
        {
            List<Perfil> result = PerfilDAO.ObtenerGridPerfiles();
            if (!usuarioActivo.Desarrollador)
            {
                result = result.Where(p => p.Desarrollador == false && 
                    p.AreasAtencion.Any(a => usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(a.Id))).ToList();
            }
            return result.Select(p => new PerfilGRIDDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Activo = p.Activo
            }).ToList();
        }

        public void Guardar(Perfil perfil, Usuario usuarioActivo)
        {
            PerfilDAO.Guardar(perfil, usuarioActivo);
        }

        public List<PerfilGRIDDTO> ConsultarPerfilesElegibles(Usuario usuarioActivo)
        {
            List<Perfil> result = PerfilDAO.ConsultarPerfilesElegibles(usuarioActivo.Desarrollador);
            if (!usuarioActivo.Desarrollador)
            {
                result = result.Where(p => p.AreasAtencion.Any(a => usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(a.Id))).ToList();
            }

            return result.Select(p => new PerfilGRIDDTO()
            {
                Id = p.Id,
                Nombre = p.Nombre
            }).ToList();
        }

    }
}
