using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Excepciones
{
    public class ExcepcionNegocio : Exception
    {
        public List<string> lstMensajes { get; set; }
        public ExcepcionNegocio(List<string> pMensajes) { lstMensajes = pMensajes; }
        public ExcepcionNegocio() : base() { }
        public ExcepcionNegocio(string mensaje) : base(mensaje) { }
        public ExcepcionNegocio(string mensaje, Exception inner) : base(mensaje, inner) { }
        protected ExcepcionNegocio(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
