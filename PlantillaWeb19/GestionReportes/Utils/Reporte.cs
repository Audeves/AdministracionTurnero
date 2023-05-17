using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantillaWeb19.GestionReportes.Utils
{
    public class Reporte
    {

        public string FileNameExtension { get; set; }
        public string Encoding { get; set; }
        public string MimeType { get; set; }
        public string[] StreamIds { get; set; }
        public Warning[] Warnings { get; set; }
        public byte[] FileBytes { get; set; }

    }
}