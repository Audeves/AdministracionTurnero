using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PlantillaWeb19.GestionReportes.Utils
{
    public enum TipoReporte
    {

        [Description("PDF")]
        PDF = 1,
        [Description("Excel")]
        EXCEL = 2

    }
}