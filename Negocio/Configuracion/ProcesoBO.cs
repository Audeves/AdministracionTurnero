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
    public class ProcesoBO : BaseBO<Proceso>
    {

        public ProcesoBO(ContextoDataBase db) : base(db)
        {

        }

        public PagingResult<ProcesoGRIDDTO> ObtenerGridProcesos(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return ProcesoDAO.ObtenerGridProcesos(config, areasAtencionUsuarioActual);
        }


        public void Guardar(Proceso proceso)
        {
            ProcesoDAO.Guardar(proceso);
        }

        public List<ProcesoGRIDDTO> ObtenerProcesosPorAreaAtencion(long idAreaAtencion)
        {
            List<Proceso> procesos = ProcesoDAO.ObtenerProcesosPorAreaAtencion(idAreaAtencion);
            return procesos.Select(p => new ProcesoGRIDDTO()
            {
                Id = p.Id,
                Nombre = p.Nombre
            }).ToList();
        }

    }
}
