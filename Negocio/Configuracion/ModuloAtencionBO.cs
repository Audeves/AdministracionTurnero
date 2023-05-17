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
    public class ModuloAtencionBO : BaseBO<ModuloAtencion>
    {

        public ModuloAtencionBO(ContextoDataBase db) : base(db)
        {

        }

        public PagingResult<ModuloAtencionGRIDDTO> ObtenerGridModulosAtencion(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return ModuloAtencionDAO.ObtenerGridModulosAtencion(config, areasAtencionUsuarioActual);
        }

        public override void Guardar(ModuloAtencion moduloAtencion)
        {
            ModuloAtencionDAO.Guardar(moduloAtencion);
        }

    }
}
