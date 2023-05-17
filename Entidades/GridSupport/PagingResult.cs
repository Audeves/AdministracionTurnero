using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades.GridSupport
{
    public class PagingResult<TGridDTO>
    {
        public IList<TGridDTO> Rows { get; set; }
        public long TotalRows { get; set; }
        public static PagingResult<TGridDTO> CreateEmpty() 
        {
            return new PagingResult<TGridDTO>() { 
                Rows = new List<TGridDTO>(),
                TotalRows = 0
            };
        }
    }
}
