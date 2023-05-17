using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Entidades.Entidades.Configuracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Citas;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Citas
{
    public class BitacoraMovimientoCitaBO : BaseBO<BitacoraMovimientoCita>
    {

        public BitacoraMovimientoCitaBO(ContextoDataBase db) : base(db)
        {

        }

        public PagingResult<BitacoraMovimientoCitaGRIDDTO> ObtenerGrid(PagingConfig config, string Usuario, string NombreSolicitante,
            string TipoMovimientoStr, string Tramite,
            int tipoMovimiento, DateTime? fechaDesde, DateTime? fechaHasta, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            return BitacoraMovimientoCitaDAO.ObtenerGrid(config, Usuario, NombreSolicitante, TipoMovimientoStr, Tramite,
                tipoMovimiento, fechaDesde,
                fechaHasta, areasAtencionUsuarioActual);
        }

    }
}
