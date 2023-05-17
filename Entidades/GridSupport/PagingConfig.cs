using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades.GridSupport
{
    /// <summary>
    /// Objeto configuracion del paginado
    /// </summary>
    public class PagingConfig
    {
        /// <summary>
        /// Numero de pagina
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// Numero de registros por pagina
        /// </summary>
        public int PageSize { get; set; }
    }
}
