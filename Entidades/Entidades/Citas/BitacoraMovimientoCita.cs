using Entidades.Enumeradores;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades.Citas
{
    public class BitacoraMovimientoCita : EntityBase
    {

        public SolicitudCita Cita { get; set; }
        public long IdCita { get; set; } //columna id de tbl_citastramite

        public DateTime FechaEmision { get; set; }
        public string Usuario { get; set; } //cuenta de dominio

        public EstatusSolicitudCita Estatus { get; set; }
        public string EstatusStr { get; set; }
        public string ComentarioAdministrador { get; set; }

        public bool Asistio { get; set; }

        public TipoMovimientoCita TipoMovimiento { get; set; }
        public string TipoMovimientoStr { get; set; }

    }
}
