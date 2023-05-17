using Entidades.Transporte.Reportes;
using Microsoft.Reporting.WebForms;
using PlantillaWeb19.GestionReportes.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantillaWeb19.GestionReportes.Generadores
{
    public class GeneradorReportesCitas : GeneradorReportesBASE
    {

        public GeneradorReportesCitas(HttpServerUtilityBase server)
            : base(server)
        {

        }


        public Reporte GenerarReporteSolicitudesCitas(List<SolicitudCitaReporteDTO> registros)
        {
            IList<ReportDataSource> dataSources = new List<ReportDataSource>();
            dataSources.Add(new ReportDataSource("SolicitudCitaDataSet", registros));

            string rutaPlantilla = Server.MapPath("~/Reportes/Citas/AdministracionSolicitudesCita.rdlc");

            return GenerarReporte(rutaPlantilla, TipoReporte.EXCEL, new List<ReportParameter>(), dataSources);
        }

    }
}