using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Enumeradores
{
    public enum TipoMovimientoCita
    {

        [Description("Agendada")]
        Agendar = 1,
        [Description("Cancelacion")]
        Cancelar = 2,
        [Description("Cancelacion masiva")]
        CancelacionMasiva = 3,
        [Description("Registro asistencia")]
        RegistroAsistencia = 4

    }
}
