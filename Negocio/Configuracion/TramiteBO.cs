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
    public class TramiteBO : BaseBO<Tramite>
    {

        public TramiteBO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<TramiteGRIDDTO> ObtenerGridTramites(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return TramiteDAO.ObtenerGridTramites(config, areasAtencionUsuarioActual);
        }

        public override void Guardar(Tramite tramite)
        {
            TramiteDAO.Guardar(tramite);
        }

    }
}
