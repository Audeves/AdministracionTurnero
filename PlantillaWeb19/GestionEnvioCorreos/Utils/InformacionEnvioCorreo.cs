using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PlantillaWeb19.GestionEnvioCorreos.Utils
{
    public class InformacionEnvioCorreo
    {

        public InformacionEnvioCorreo()
        {
            EsCuerpoHTML = true;
            ArchivosAdjuntos = new List<Attachment>();
            CorreosCC = new List<string>();
        }

        public string CorreoRemitente { get; set; }
        public string CorreoDestinatario { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public bool EsCuerpoHTML { get; set; }
        public List<Attachment> ArchivosAdjuntos { get; private set; }
        public List<string> CorreosCC { get; private set; }

        public void AgregarArchivoAdjunto(string nombre, byte[] bytesArhivo, string mimeType)
        {
            Stream stream = new MemoryStream(bytesArhivo);
            AgregarArchivoAdjunto(nombre, stream, mimeType);
        }

        public void AgregarArchivoAdjunto(string nombre, Stream stream, string mimeType)
        {
            Attachment archivo = new Attachment(stream, nombre, mimeType);
            ArchivosAdjuntos.Add(archivo);
        }

        public void AgregarCorreoCC(string correoCC)
        {
            CorreosCC.Add(correoCC);
        }

    }
}