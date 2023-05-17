using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Estatus
namespace Entidades.Enumeradores
{
    public enum EstatusSolicitudCita
    {
        [Description("Pendiente")]
        Pendiente = 1,
        [Description("Agendada")]
        Agendada = 2,
        [Description("Cancelada")]
        Cancelada = 40

    }
}
