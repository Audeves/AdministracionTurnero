using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.Excepciones;
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
    public class PantallaDAO : BaseDAO<Pantalla>
    {

        public PantallaDAO(ContextoDataBase db) : base(db)
        {

        }


        public new void Guardar(Pantalla pantalla)
        {
            Pantalla pantallaGuardada = this.ConsultarPorId(pantalla.Id, "PantallasAcciones", "Modulo", "SubModulo");
            if (pantallaGuardada == null)
            {
                pantallaGuardada = new Pantalla();
                pantallaGuardada.PantallasAcciones = pantalla.PantallasAcciones;
            }
            else
            {
                EliminarEntities(pantallaGuardada.PantallasAcciones.Where(pag =>
                !pantalla.PantallasAcciones.Select(pa => pa.Id).Contains(pag.Id)).ToList());
                pantallaGuardada.PantallasAcciones.AddRange(pantalla.PantallasAcciones.Where(pa => pa.Id == 0));
                pantallaGuardada.PantallasAcciones.ForEach(pa =>
                {
                    var accion = pantalla.PantallasAcciones.Where(a => a.Id != 0).SingleOrDefault(a => a.Id == pa.Id);
                    if (accion != null)
                    {
                        pa.Accion = accion.Accion;
                        pa.Privado = accion.Privado;
                        pa.Activo = accion.Activo;
                    }
                    
                });
            }

            pantallaGuardada.Nombre = pantalla.Nombre;
            pantallaGuardada.Controlador = pantalla.Controlador;
            pantallaGuardada.Accion = pantalla.Accion;
            pantallaGuardada.ClaseIcono = pantalla.ClaseIcono;
            pantallaGuardada.IdModulo = pantalla.IdModulo;
            pantallaGuardada.IdSubModulo = pantalla.IdSubModulo;
            pantallaGuardada.Privado = pantalla.Privado;
            pantallaGuardada.Activo = pantalla.Activo;

            

            base.Guardar(pantallaGuardada);
        }


        public PagingResult<PantallaGRIDDTO> ObtenerGridPantallas(PagingConfig config, bool desarrolladorUsuarioActivo)
        {
            Expression<Func<Pantalla, PantallaGRIDDTO>> select = p => new PantallaGRIDDTO()
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Controlador = p.Controlador,
                Accion = p.Accion,
                Activo = p.Activo,
                Privado = p.Privado,
                Modulo = p.Modulo != null ? p.Modulo.Nombre : null,
                SubModulo = p.SubModulo != null ? p.SubModulo.Nombre : null
            };

            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => (f.Modulo != null || f.SubModulo != null));

            if (!desarrolladorUsuarioActivo)
            {
                filtros.Add(f => f.Desarrollador == false);
            }
            

            return ExecConsultaPaginada(config, select, filtros);
        }


        /// <summary>
        /// Obtener las pantallas que no sean privadas y esten activas. Utilizado en la llamada a cada accion a ejecutar.
        /// </summary>
        /// <returns></returns>
        public List<Pantalla> ConsultarPantallasyAccionesPublicas()
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(p => p.Privado == false);
            filtros.Add(p => p.Activo == true);

            return FindByFilters(filtros, "PantallasAcciones").ToList();
        }

        public Pantalla ConsultarPantallaPorAccion(string accion)
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(p => p.Accion == accion);

            return FindUniqueByFilters(filtros, "PantallasAcciones");
        }

        public List<Pantalla> ConsultarPermisosPantallasElegibles(bool desarrolladorUsuarioActivo)
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => (f.Modulo != null || f.SubModulo != null));
            filtros.Add(f => f.Activo == true);
            filtros.Add(f => f.Privado == true);
            if (!desarrolladorUsuarioActivo)
            {
                filtros.Add(f => f.Desarrollador == false);
            }
            

            return FindByFilters(filtros, "Modulo", "SubModulo.Modulo").ToList();
        }

        //Utilizado para agregar manualmente a un perfil de usuario las pantallas publicas que si pertenecen a un modulo o submodulo
        public List<Pantalla> ConsultarPantallasPublicasConModuloSubModulo()
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(f => f.Privado == false);
            filtros.Add(f => f.Activo == true);
            filtros.Add(f => (f.Modulo != null || f.SubModulo != null));
            filtros.Add(f => f.PantallasAcciones.Any(pa => (pa.Privado == false && pa.Activo == true)));

            return FindByFilters(filtros, "Modulo", "SubModulo.Modulo", "PantallasAcciones").ToList();
        }

    }
}
