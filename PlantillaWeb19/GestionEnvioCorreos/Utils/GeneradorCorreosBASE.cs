using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace PlantillaWeb19.GestionEnvioCorreos.Utils
{
    public class GeneradorCorreosBASE
    {

        protected HttpServerUtilityBase Server { get; set; }

        public GeneradorCorreosBASE(HttpServerUtilityBase server)
        {
            Server = server;
        }

        public void EnviarCorreo(InformacionEnvioCorreo infoEnvio)
        {
            MailMessage correo = CrearCorreo(infoEnvio);
            SmtpClient smtp = CrearSmtp();
            smtp.Send(correo);
        }

        private MailMessage CrearCorreo(InformacionEnvioCorreo infoEnvio)
        {
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress(infoEnvio.CorreoRemitente);
            correo.To.Add(infoEnvio.CorreoDestinatario);
            correo.Subject = infoEnvio.Asunto;
            correo.Body = infoEnvio.Cuerpo;
            correo.IsBodyHtml = infoEnvio.EsCuerpoHTML;
            correo.Priority = MailPriority.Normal;

            foreach (Attachment archivoAdjunto in infoEnvio.ArchivosAdjuntos)
            {
                correo.Attachments.Add(archivoAdjunto);
            }

            foreach (string correoCC in infoEnvio.CorreosCC)
            {
                correo.CC.Add(correoCC);
            }

            return correo;
        }

        private SmtpClient CrearSmtp()
        {
            string host = ConfigurationManager.AppSettings["hostCorreo"].ToString();
            SmtpClient smtp = new SmtpClient()
            {
                Host = host,
                EnableSsl = false,
                UseDefaultCredentials = true
            };
            return smtp;
        }

        protected String GenerarCuerpoCorreo(string rutaPlantilla, Dictionary<string, string> parametros)
        {
            StringBuilder correoBuilder = ObtenerPlantilla(rutaPlantilla);
            foreach (string key in parametros.Keys)
            {
                correoBuilder.Replace("{" + key + "}", parametros[key]);
            }
            return correoBuilder.ToString();
        }

        private StringBuilder ObtenerPlantilla(string rutaPlantilla)
        {
            if (!System.IO.File.Exists(rutaPlantilla))
            {
                throw new FileNotFoundException("No se ha podido encontrar el archivo: " + rutaPlantilla);
            }
            return new StringBuilder(System.IO.File.ReadAllText(rutaPlantilla));
        }

    }
}