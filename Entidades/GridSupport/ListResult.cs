using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades.GridSupport
{
    public class ListResult<TGridDTO>
    {
        public IList<TGridDTO> Rows { get; set; }
        public long TotalRows { get; set; }

        public ListResult()
        {
        }

        public ListResult(IList<TGridDTO> rows)
        {
            Rows = rows;
            TotalRows = rows.Count;
        }

        public static ListResult<TGridDTO> CreateEmpty()
        {
            return new ListResult<TGridDTO>()
            {
                Rows = new List<TGridDTO>(),
                TotalRows = 0
            };
        }
    }
}
