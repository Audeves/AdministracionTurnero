using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
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
    public class PerfilDAO : BaseDAO<Perfil>
    {

        public PerfilDAO(ContextoDataBase db) : base(db)
        {

        }

        public List<Perfil> ObtenerGridPerfiles()
        {
            IQueryable<Perfil> perfiles = CrearQuery("AreasAtencion");
            return perfiles.ToList();
            //Expression<Func<Perfil, PerfilGRIDDTO>> select = p => new PerfilGRIDDTO()
            //{
            //    Id = p.Id,
            //    Nombre = p.Nombre,
            //    Activo = p.Activo,
            //    AreasAtencion = p.AreasAtencion
            //};

            //var filtros = CrearListaFiltrosVacia();
            //if (!usuarioActivo.Desarrollador)
            //{
            //    filtros.Add(p => p.Desarrollador == false);
            //    //filtros.Add(p => p.AreasAtencion.(usuarioActivo.AreasAtencionResponsable));
            //    ////filtros.Add(p => p.AreasAtencion.Any(a => usuarioActivo.AreasAtencionResponsable.Contains(a)));
            //}


            //PagingResult<PerfilGRIDDTO> result = ExecConsultaPaginada(config, select, filtros);
            //result.Rows = result.Rows.Where(p => p.AreasAtencion.Any(a => 
            //    usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(a.Id))).ToList();
            //return null;
        }

        public new void Guardar(Perfil perfil, Usuario usuarioActivo)
        {
            Perfil perfilGuardado = this.ConsultarPorId(perfil.Id, "Pantallas", "AreasAtencion");
            if (perfilGuardado == null)
            {
                perfilGuardado = new Perfil();
                perfilGuardado.ElegibleAsignar = true;
                perfilGuardado.Desarrollador = usuarioActivo.Desarrollador;
            }

            var idsPantallasSeleccionadas = perfil.Pantallas.Select(p => p.Id).ToList();
            var idsPantallasGuardadas = perfilGuardado.Pantallas.Select(p => p.Id).ToList();
            perfilGuardado.Pantallas.RemoveAll(p => !idsPantallasSeleccionadas.Contains(p.Id));
            perfilGuardado.Pantallas.AddRange(perfil.Pantallas.Where(p => !idsPantallasGuardadas.Contains(p.Id)));

            var idsAreasAtencionSeleccionadas = perfil.AreasAtencion.Select(a => a.Id).ToList();
            var idsAreasAtencionGuardadas = perfilGuardado.AreasAtencion.Select(a => a.Id).ToList();
            //if (!usuarioActivo.Desarrollador)
            //{
            //Extraigo las areas del perfil guardado que corresponden con las del usuario responsable activo
            List<AreaAtencion> perfilGuardadoAreasCorrespondientesAResponsable = perfilGuardado.AreasAtencion.Where(pa =>
                usuarioActivo.AreasAtencionResponsable.Select(ua => ua.Id).Contains(pa.Id)).ToList();
            //del perfil guardado elimino las areas de atencion que filtre arriba
            perfilGuardado.AreasAtencion.RemoveAll(a => perfilGuardadoAreasCorrespondientesAResponsable.Select(pgacr => pgacr.Id).Contains(a.Id));

            //de mis areas de atencion que extraje del perfil ahora elimino las que no estan seleccionadas en la vista
            perfilGuardadoAreasCorrespondientesAResponsable.RemoveAll(pgacr => !idsAreasAtencionSeleccionadas.Contains(pgacr.Id));
            //y agrego las que estan seleccionadas y que no esten guardadas ya en el perfil guardado
            perfilGuardadoAreasCorrespondientesAResponsable.AddRange(perfil.AreasAtencion.Where(a => !idsAreasAtencionGuardadas.Contains(a.Id)));

            //por ultimo vuelvo a unir las areas de atencion del perfil guardado con las areas de atencion del perfil que maneje en este procesito...
            perfilGuardado.AreasAtencion.AddRange(perfilGuardadoAreasCorrespondientesAResponsable);

            //}
            //else
            //{
            //    perfilGuardado.AreasAtencion.RemoveAll(a => !idsAreasAtencionSeleccionadas.Contains(a.Id));
            //    perfilGuardado.AreasAtencion.AddRange(perfil.AreasAtencion.Where(a => !idsAreasAtencionGuardadas.Contains(a.Id)));
            //}
            

            perfilGuardado.Nombre = perfil.Nombre;
            perfilGuardado.Activo = perfil.Activo;
            perfilGuardado.EstatusRegistro = perfil.EstatusRegistro;
            perfilGuardado.FechaRegistro = perfil.FechaRegistro;
            perfilGuardado.IdUsuarioRegistro = perfil.IdUsuarioRegistro;
            foreach (Pantalla item in perfilGuardado.Pantallas)
            {
                db.Pantalla.Attach(item);
            }

            foreach (AreaAtencion item in perfilGuardado.AreasAtencion)
            {
                db.AreaAtencion.Attach(item);
            }

            base.Guardar(perfilGuardado);
        }


        public List<Perfil> ConsultarPerfilesElegibles(bool desarrolladorUsuarioActivo)
        {
            var filtros = CrearListaFiltrosVacia();
            if (desarrolladorUsuarioActivo)
            {
                filtros.Add(p => p.Activo == true && (p.Desarrollador == true || p.ElegibleAsignar == true));
            }
            else
            {
                filtros.Add(p => p.Activo == true && p.ElegibleAsignar == true);
            }
            
            return FindByFilters(filtros, "AreasAtencion").ToList();
        }

    }
}
