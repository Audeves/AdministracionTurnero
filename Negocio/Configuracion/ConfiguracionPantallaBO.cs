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
    public class ConfiguracionPantallaBO : BaseBO<ConfiguracionPantalla>
    {

        public ConfiguracionPantallaBO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<ConfiguracionPantallaGRIDDTO> ObtenerGridConfiguracionesPantallas(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return ConfiguracionPantallaDAO.ObtenerGridConfiguracionesPantallas(config, areasAtencionUsuarioActual);
        }

        public ConfiguracionPantalla Guardar(ConfiguracionPantalla pantalla)
        {
            return ConfiguracionPantallaDAO.Guardar(pantalla);
        }

    }
}
