using Datos.Acceso.General;
using Entidades.Entidades.Configuracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Configuracion;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Configuracion
{
    public class AreaAtencionBO : BaseBO<AreaAtencion>
    {

        public AreaAtencionBO(ContextoDataBase db) : base(db)
        {

        }


        public void Guardar(AreaAtencion areaAtencion)
        {

            AreaAtencionDAO.Guardar(areaAtencion);
            
        }

        public void ActualizarTurneroAreaAtencion(ConfiguracionTurnero turneroGuardado, ConfiguracionTurnero turnero)
        {
            AreaAtencionDAO.ActualizarTurneroAreaAtencion(turneroGuardado, turnero);
        }

        public void ActualizarPantallaAreaAtencion(ConfiguracionPantalla pantallaGuardada, ConfiguracionPantalla pantalla)
        {
            AreaAtencionDAO.ActualizarPantallaAreaAtencion(pantallaGuardada, pantalla);
        }


        public PagingResult<AreaAtencionGRIDDTO> ObtenerGridAreasAtencion(PagingConfig config)
        {
            return AreaAtencionDAO.ObtenerGridAreasAtencion(config);
        }

        public List<AreaAtencionGRIDDTO> ObtenerAreasAtencionPorCampus(string campus_ps)
        {
            List<AreaAtencion> areas = AreaAtencionDAO.ObtenerAreasAtencionPorCampus(campus_ps);
            List<AreaAtencionGRIDDTO> areasDTO = areas.Select(a => new AreaAtencionGRIDDTO()
            {
                Id = a.Id,
                Nombre = a.Nombre + ", " + a.CampusDescr
            }).ToList();
            return areasDTO;
        }

    }
}
