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
    public class ConfiguracionTurneroBO : BaseBO<ConfiguracionTurnero>
    {

        public ConfiguracionTurneroBO(ContextoDataBase db) : base(db)
        {

        }

        public PagingResult<ConfiguracionTurneroGRIDDTO> ObtenerGridConfiguracionesTurneros(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return ConfiguracionTurneroDAO.ObtenerGridConfiguracionesTurneros(config, areasAtencionUsuarioActual);
        }

        public ConfiguracionTurnero Guardar(ConfiguracionTurnero turnero)
        {
            return ConfiguracionTurneroDAO.Guardar(turnero);
        }

    }
}
