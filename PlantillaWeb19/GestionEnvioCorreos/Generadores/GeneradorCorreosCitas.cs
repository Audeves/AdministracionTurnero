using Entidades.Entidades.Citas;
using PlantillaWeb19.GestionEnvioCorreos.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PlantillaWeb19.GestionEnvioCorreos.Generadores
{
    public class GeneradorCorreosCitas : GeneradorCorreosBASE
    {

        public GeneradorCorreosCitas(HttpServerUtilityBase server) : base(server)
        {

        }


        public void EnviarCorreoCitaAgendada(SolicitudCita cita)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>()
            {
                { "COMENTARIOADMIN", cita.ComentariosAdministrador },
                { "SOLICITANTE", cita.NombreSolicitante },
                { "FOLIO", cita.Folio },
                { "FECHA", cita.FechaCita.ToString("dd/MMMM/yyyy") },
                { "HORA", cita.HoraCita },
                //{ "LUGAR", "Edificio de Registro Escolar Campus Náinari" },
                { "TRAMITE", cita.Tramite.Tramite },
            };

            if (cita.AreaAtencion.Nombre == "Registro Escolar" && cita.AreaAtencion.CampusPS == "00002")
            {
                if (cita.PersonaAutorizada != null)
                {
                    parametros.Add("MOSTRARAUTORIZADO", "block");
                    parametros.Add("AUTORIZADO", $"{cita.PersonaAutorizada} ({cita.Parentezco})");
                }
                else
                {
                    parametros.Add("MOSTRARAUTORIZADO", "none");
                }
                
            }

            string rutaPlantilla = Server.MapPath($"~/PlantillasCorreos/Citas/{cita.AreaAtencion.CorreoAgendada}.html");
            string cuerpoCorreo = GenerarCuerpoCorreo(rutaPlantilla, parametros);

            InformacionEnvioCorreo info = new InformacionEnvioCorreo()
            {
                CorreoRemitente = cita.AreaAtencion.CorreoRemitenteCitas,
                CorreoDestinatario = ConfigurationManager.AppSettings["correoDestinatario"] == null ? cita.Email : ConfigurationManager.AppSettings["correoDestinatario"].ToString(),
                Asunto = "Solicitud de cita ITSON - Cita aceptada",
                Cuerpo = cuerpoCorreo,
                EsCuerpoHTML = true
            };

            EnviarCorreo(info);
        }



        public void EnviarCorreoCitaRechazada(SolicitudCita cita)
        {
            string fechaCitaPerrona = $"{cita.FechaCita.ToString("dddd, dd \\de MMMM \\de yyyy")} a las {cita.HoraCita}";
            Dictionary<string, string> parametros = new Dictionary<string, string>()
            {
                { "COMENTARIOADMIN", cita.ComentariosAdministrador },
                { "TRAMITE", cita.Tramite.Tramite },
                { "FECHACITA", fechaCitaPerrona }
            };

            string rutaPlantilla = Server.MapPath($"~/PlantillasCorreos/Citas/{cita.AreaAtencion.CorreoCancelada}.html");
            string cuerpoCorreo = GenerarCuerpoCorreo(rutaPlantilla, parametros);

            InformacionEnvioCorreo info = new InformacionEnvioCorreo()
            {
                CorreoRemitente = cita.AreaAtencion.CorreoRemitenteCitas,
                CorreoDestinatario = ConfigurationManager.AppSettings["correoDestinatario"] == null ? cita.Email : ConfigurationManager.AppSettings["correoDestinatario"].ToString(),
                Asunto = "Solicitud de cita ITSON - Cita rechazada",
                Cuerpo = cuerpoCorreo,
                EsCuerpoHTML = true
            };

            EnviarCorreo(info);
        }

    }
}