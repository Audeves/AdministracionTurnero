using Entidades.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Transporte
{
    public class RespuestaDTO
    {

        public RespuestaEstatus Estatus { get; set; }
        public string Mensaje { get; set; }
        public object Datos { get; set; }

        public RespuestaDTO(RespuestaEstatus estatus, string mensaje, object datos = null)
        {
            this.Estatus = estatus;
            this.Mensaje = mensaje;
            this.Datos = datos;
        }

    }
}
