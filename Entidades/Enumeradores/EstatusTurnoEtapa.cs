using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Enumeradores
{
    public enum EstatusTurnoEtapa
    {
        [Description("Por atender")]
        PorAtender = 1,
        [Description("Atendiendo")]
        Atendiendo = 2,
        [Description("En espera")]
        EnEspera = 3,
        [Description("Atención finalizada")]
        AtencionTerminada = 4,

        [Description("Cancelado")]
        Cancelado = 101

    }
}
