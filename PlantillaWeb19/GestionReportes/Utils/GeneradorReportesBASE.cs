using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PlantillaWeb19.Utils;

namespace PlantillaWeb19.GestionReportes.Utils
{
    public class GeneradorReportesBASE
    {
        protected HttpServerUtilityBase Server { get; set; }

        public GeneradorReportesBASE(HttpServerUtilityBase server)
        {
            Server = server;
        }

        /// <summary>
        /// Metodo que permite generar un reporte utilizando como plantilla el archivo especificado y el formato
        /// </summary>
        /// <param name="rutaPlantilla">Ruta donde se encuentra la plantilla '.rdlc'</param>
        /// <param name="tipoReporte">Tipo de reporte a generar</param>
        /// <param name="parametros">Lista de parametros que utiliza el reporte</param>
        /// <returns>Objeto con la informacion del reporte generado</returns>
        protected Reporte GenerarReporte(string rutaPlantilla, TipoReporte tipoReporte, IEnumerable<ReportParameter> parametros)
        {
            return GenerarReporte(rutaPlantilla, tipoReporte, parametros, new List<ReportDataSource>());
        }

        /// <summary>
        /// Metodo que permite generar un reporte utilizando como plantilla el archivo especificado y el formato
        /// </summary>
        /// <param name="rutaPlantilla">Ruta donde se encuentra la plantilla '.rdlc'</param>
        /// <param name="tipoReporte">Tipo de reporte a generar</param>
        /// <param name="parametros">Lista de parametros que utiliza el reporte</param>
        /// <param name="dataSources">Data sources</param>
        /// <returns>Objeto con la informacion del reporte generado</returns>
        protected Reporte GenerarReporte(string rutaPlantilla, TipoReporte tipoReporte, IEnumerable<ReportParameter> parametros, IEnumerable<ReportDataSource> dataSources)
        {
            if (!File.Exists(rutaPlantilla))
            {
                throw new FileNotFoundException("No se ha podido encontrar el archivo: " + rutaPlantilla);
            }

            LocalReport archivo = new LocalReport();
            archivo.ReportPath = rutaPlantilla;
            archivo.EnableExternalImages = true;
            archivo.SetParameters(parametros);
            string mimeType = "";
            string encoding = "";
            string fileNameExtension = "";
            string[] streamIds = null;
            Warning[] warnings = null;

            foreach (var dataSource in dataSources)
            {

                archivo.DataSources.Add(dataSource);
            }

            byte[] fileBytes = archivo.Render(tipoReporte.GetDescription(), null, out mimeType, out encoding, out fileNameExtension, out streamIds, out warnings);


            Reporte reporte = new Reporte();
            reporte.MimeType = mimeType;
            reporte.Encoding = encoding;
            reporte.FileNameExtension = fileNameExtension;
            reporte.StreamIds = streamIds;
            reporte.Warnings = warnings;
            reporte.FileBytes = fileBytes;
            return reporte;
        }
    }
}