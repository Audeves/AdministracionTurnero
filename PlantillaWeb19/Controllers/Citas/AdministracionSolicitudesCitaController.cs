using Entidades.Entidades.Administracion;
using Entidades.Entidades.Citas;
using Entidades.Enumeradores;
using Entidades.GridSupport;
using Entidades.Transporte;
using Entidades.TransporteGrid.Citas;
using PlantillaWeb19.Controllers.General;
using PlantillaWeb19.GestionEnvioCorreos.Generadores;
using PlantillaWeb19.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.Citas
{
    public class AdministracionSolicitudesCitaController : BaseController
    {
        
        [HttpGet]
        [DateTimeAttribute(Field = "fechaDesde")]
        [DateTimeAttribute(Field = "fechaHasta")]
        public JsonResult ObtenerGridSolicitudCita(PagingConfig config, string EmplId = "", string NombreSolicitante = "",
            string AsistioStr = "", string TramiteActual = "",
            int? estatus = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            PagingResult<SolicitudCitaGRIDDTO> result;
            if (estatus == null)
            {
                result = PagingResult<SolicitudCitaGRIDDTO>.CreateEmpty();
            }
            else
            {
                result = fabrica.SolicitudCitaBO.ObtenerGrid(config, EmplId, NombreSolicitante, AsistioStr, TramiteActual,
                    estatus.Value, fechaDesde, fechaHasta, ObtenerAreasAtencionTotalesUsuarioActivo());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Consultar(long id)
        {
            SolicitudCita registro = fabrica.SolicitudCitaBO.Consultar(id, "Tramite", "AreaAtencion");

            Usuario usuario = ObtenerUsuarioActivo();
            string comentarioAceptar = registro.Tramite.ComentarioAceptar != null ?
                registro.Tramite.ComentarioAceptar.Replace("{NOMBRE}", usuario.Nombre).Replace("{CORREO}", usuario.Correo) :
                "";
            string comentarioRechazar = registro.Tramite.ComentarioRechazar != null ?
                registro.Tramite.ComentarioRechazar.Replace("{NOMBRE}", usuario.Nombre).Replace("{CORREO}", usuario.Correo) :
                "";
            var registroDTO = new
            {
                Id = registro.Id,
                IdCita = registro.IdCita,
                Estatus = registro.Estatus,
                EstatusStr = registro.Estatus.GetDescription(),
                Asistio = registro.Asistio,
                AsistioStr = registro.Asistio ? "Si" : "No",
                FechaCita = registro.FechaCita,
                HoraCita = registro.HoraCita,
                FechaCitaStr = $"{registro.FechaCita.ToString("dd/MM/yyyy")}, {registro.HoraCita}",
                FechaCaptura = registro.FechaCaptura,
                FechaCapturaStr = $"{registro.FechaCaptura.ToString("dd/MM/yyyy, HH:mm")}",
                TramiteActual = registro.Tramite.Tramite,
                AreaAtencion = registro.AreaAtencion.Nombre + ", " + registro.AreaAtencion.CampusDescr,
                NombreSolicitante = registro.NombreSolicitante,
                TramiteCapturado = registro.TramiteStr,
                EmplId = registro.EmplId,
                Telefono = registro.Telefono,
                Email = registro.Email,
                PersonaAutorizada = registro.PersonaAutorizada,
                Parentezco = registro.Parentezco,
                Folio = registro.Folio,
                ComentariosAdicionales = registro.ComentariosAdicionales,
                ComentariosAdministrador = registro.ComentariosAdministrador,

                TieneCitaHoy = registro.FechaCita.Date == DateTime.Today ? true : false,
                TieneCitaProxima = registro.FechaCita.Date >= DateTime.Today ? true : false,

                ComentarioAceptar = comentarioAceptar,
                ComentarioRechazar = comentarioRechazar
            };

            return Json(registroDTO, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarRevisionAdministrador(long id, EstatusSolicitudCita estatus, string comentariosAdministrador, EstatusSolicitudCita estatusAnterior)
        {
            RespuestaDTO respuesta;
            SolicitudCita citaGuardada = null;
            try
            {
                citaGuardada = fabrica.SolicitudCitaBO.GuardarRevisionAdministrador(id, estatus, comentariosAdministrador, ObtenerCuentaDominioUsuarioActivo());
                if (citaGuardada == null || citaGuardada.Id == 0)
                {
                    respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Ocurrió un error al actualizar la cita.");
                    return Json(respuesta, JsonRequestBehavior.DenyGet);
                }

                if (citaGuardada.Estatus == EstatusSolicitudCita.Agendada)
                {
                    fabrica.StoredProceduresBO.RegistrarCitaSERVICIOSGENERALES(citaGuardada);
                }

                if (citaGuardada.Estatus == EstatusSolicitudCita.Cancelada && estatusAnterior == EstatusSolicitudCita.Agendada)
                {
                    fabrica.StoredProceduresBO.EliminarCitasSERVICIOSGENERALES(new List<SolicitudCita>() { citaGuardada });
                }

                string textoEstatus = $"Cita {estatus.GetDescription()} correctamente.";
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, textoEstatus);
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Ocurrió un error al actualizar la cita.", e.Message);
            }

            try
            {
                GeneradorCorreosCitas generadorCorreos = new GeneradorCorreosCitas(Server);
                if (citaGuardada.Estatus == EstatusSolicitudCita.Agendada)
                {
                    generadorCorreos.EnviarCorreoCitaAgendada(citaGuardada);
                }
                else if (citaGuardada.Estatus == EstatusSolicitudCita.Cancelada)
                {
                    generadorCorreos.EnviarCorreoCitaRechazada(citaGuardada);
                }

            }
            catch (Exception e)
            {

            }

            return Json(respuesta, JsonRequestBehavior.DenyGet);
        }


        [HttpGet]
        public JsonResult ObtenerListaGridCitasHoy()
        {
            List<SolicitudCitaGRIDDTO> result = fabrica.SolicitudCitaBO.ObtenerListaGridCitasHoy(ObtenerAreasAtencionTotalesUsuarioActivo());
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarAsistencia(long id)
        {
            RespuestaDTO respuesta;
            try
            {
                fabrica.SolicitudCitaBO.RegistrarAsistencia(id, ObtenerCuentaDominioUsuarioActivo());
                respuesta = new RespuestaDTO(RespuestaEstatus.OK, "Asistencia registrada.");
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Ocurrió un error al actualizar la cita.", e.Message);
            }

            return Json(respuesta, JsonRequestBehavior.DenyGet);
        }





        //Bitacora
        [HttpGet]
        [DateTimeAttribute(Field = "fechaDesde")]
        [DateTimeAttribute(Field = "fechaHasta")]
        public JsonResult ObtenerGridBitacoraMovimientos(PagingConfig config, string Usuario = "", string NombreSolicitante = "",
            string TipoMovimientoStr = "", string Tramite = "",
            int? tipoMovimiento = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            PagingResult<BitacoraMovimientoCitaGRIDDTO> result;
            if (tipoMovimiento == null)
            {
                result = PagingResult<BitacoraMovimientoCitaGRIDDTO>.CreateEmpty();
            }
            else
            {
                result = fabrica.BitacoraMovimientoCitaBO.ObtenerGrid(config, Usuario, NombreSolicitante, TipoMovimientoStr, Tramite,
                    tipoMovimiento.Value, fechaDesde, fechaHasta, ObtenerAreasAtencionTotalesUsuarioActivo());
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }









        //Cancelacion masiva

        [HttpPost]
        [DateTimeAttribute(Field = "fechaDesde")]
        [DateTimeAttribute(Field = "fechaHasta")]
        public JsonResult CancelacionMasiva(DateTime? fechaDesde, DateTime? fechaHasta, bool pendientes, bool agendadas, 
            bool enviarCorreo, string comentarioCancelacion)
        {
            RespuestaDTO respuesta;
            List<SolicitudCita> citas = fabrica.SolicitudCitaBO.ObtenerCitasCancelacionMasiva(fechaDesde, fechaHasta, pendientes, agendadas,
                ObtenerAreasAtencionTotalesUsuarioActivo());


            if (citas.Count == 0)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "No se encontró ninguna cita que cumpla con la configuración seleccionada.", "");
                return Json(respuesta, JsonRequestBehavior.DenyGet);
            }

            try
            {
                fabrica.StoredProceduresBO.EliminarCitasSERVICIOSGENERALES(citas);
                fabrica.SolicitudCitaBO.CancelacionMasiva(citas, comentarioCancelacion, ObtenerCuentaDominioUsuarioActivo());
                
                fabrica.HistorialCancelacionMasivaBO.Guardar(ObtenerCuentaDominioUsuarioActivo(), fechaDesde, fechaHasta, pendientes, agendadas, enviarCorreo, comentarioCancelacion, citas.Count);

                respuesta = new RespuestaDTO(RespuestaEstatus.OK, $"Se cancelaron {citas.Count} citas correctamente.");
            }
            catch (Exception e)
            {
                respuesta = new RespuestaDTO(RespuestaEstatus.ERROR, "Ocurrió un error al cancelar la cita.", e.Message);
                return Json(respuesta, JsonRequestBehavior.DenyGet);
            }

            
            if (enviarCorreo)
            {
                //citas.ForEach(c => {
                //    c.ComentariosAdministrador = comentarioCancelacion;
                //});
                GeneradorCorreosCitas generador = new GeneradorCorreosCitas(Server);
                foreach (SolicitudCita item in citas)
                {
                    try
                    {
                        generador.EnviarCorreoCitaRechazada(item);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            

            return Json(respuesta, JsonRequestBehavior.DenyGet);
        }

    }
}