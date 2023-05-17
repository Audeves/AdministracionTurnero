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
    public class PantallaBO : BaseBO<Pantalla>
    {

        public PantallaBO(ContextoDataBase db) : base(db)
        {

        }

        public override void Guardar(Pantalla pantalla)
        {
            PantallaDAO.Guardar(pantalla);
        }

        public PagingResult<PantallaGRIDDTO> ObtenerGridPantallas(PagingConfig config, bool desarrolladorUsuarioActivo)
        {
            return PantallaDAO.ObtenerGridPantallas(config, desarrolladorUsuarioActivo);
        }

        public List<ExcepcionControladorAccion> ConsultarPantallasyAccionesPublicas()
        {
            List<Pantalla> pantallas = PantallaDAO.ConsultarPantallasyAccionesPublicas();
            List<ExcepcionControladorAccion> excepciones = pantallas.Select(p => new ExcepcionControladorAccion()
            {
                Controlador = p.Controlador,
                Accion = p.Accion,
                PublicaLibre = p.PublicaLibre
            }).ToList();
            excepciones.AddRange(pantallas.SelectMany(p => p.PantallasAcciones.Where(pa => (pa.Privado == false && pa.Activo == true)).Select(pa => new ExcepcionControladorAccion()
            {
                Controlador = pa.Controlador,
                Accion = pa.Accion,
                PublicaLibre = pa.PublicaLibre
            }).ToList()).ToList());

            //Obtengo las acciones de pantalla publicas que no dependen de una pantalla
            List<PantallaAccion> acciones = PantallaAccionDAO.ConsultarAccionesPublicasSinPantalla();
            excepciones.AddRange(acciones.Select(pa => new ExcepcionControladorAccion()
            {
                Controlador = pa.Controlador,
                Accion = pa.Accion,
                PublicaLibre = pa.PublicaLibre
            }).ToList());

            return excepciones;
        }

        public List<Pantalla> ConsultarPermisosPantallasElegibles(bool desarrolladorUsuarioActivo)
        {
            return PantallaDAO.ConsultarPermisosPantallasElegibles(desarrolladorUsuarioActivo);
        }

        //Hasta ahorita solo en inicio para agregar manualmente a perfil de usuario
        public List<Pantalla> ConsultarPantallasPublicasConModuloSubModulo()
        {
            List<Pantalla> pantallas = PantallaDAO.ConsultarPantallasPublicasConModuloSubModulo();
            pantallas.ForEach(p =>
            {
                List<PantallaAccion> accionesEliminar = new List<PantallaAccion>();
                p.PantallasAcciones.ForEach(pa =>
                {
                    if (!pa.Activo || pa.Privado) {
                        accionesEliminar.Add(pa);
                    }
                });
                //una vez que termine el ciclo quito del arbol del objeto las acciones que no cumplieron con la validacion.
                p.PantallasAcciones.RemoveAll(pa => accionesEliminar.Contains(pa));
            });
            return pantallas;
        }

    }
}
