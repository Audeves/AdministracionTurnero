using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Administracion
{
    public class PantallaAccionDAO : BaseDAO<PantallaAccion>
    {

        public PantallaAccionDAO(ContextoDataBase db) : base(db)
        {

        }


        public List<PantallaAccion> ConsultarAccionesPublicasSinPantalla()
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(pa => pa.Privado == false);
            filtros.Add(pa => pa.Pantalla == null);
            filtros.Add(pa => pa.Activo == true);

            return FindByFilters(filtros, "Pantalla").ToList();
        }

        /// <summary>
        /// Obtengo la lista de las acciones que pueden aplicar para todo el sistema (sin pantalla asignada) y que son privadas (requiren sesion iniciada)
        /// </summary>
        /// <returns></returns>
        public List<PantallaAccion> ConsultarAccionesPrivadasSinPantalla()
        {
            var filtros = CrearListaFiltrosVacia();
            filtros.Add(pa => pa.Privado == true);
            filtros.Add(pa => pa.Pantalla == null);
            filtros.Add(pa => pa.Activo == true);

            return FindByFilters(filtros, "Pantalla").ToList();
        }

    }
}
