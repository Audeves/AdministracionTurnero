using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Entidades.Entidades.General;
using Entidades.General;
using Entidades.GridSupport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.General
{
    public class StoredProceduresDAO : SQLBaseDAO<SQLEntityBase>
    {

        public StoredProceduresDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<Empleado> ConsultarEmpleados(PagingConfig config, string emplId, string nombre, string cuentaDominio, string dptoAdscripcion)
        {
            // arreglo de parametros
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@emplId", emplId),
                new SqlParameter("@nombre", nombre),
                new SqlParameter("@cuentaDominio", cuentaDominio),
                new SqlParameter("@dptoAdscripcion", dptoAdscripcion)
            };
            return ExecSPConsultaPaginada<Empleado>(config, "CIAConsultarEmpleados", parametros);
        }

        public IList<Campus> ConsultarCampus()
        {
            return ExecSPConsulta<Campus>("CIAConsultarCampus", null);
        }

        public void RegistrarCitaSERVICIOSGENERALES(SolicitudCita cita)
        {
            //string horaSalida = DateTime.Now.Date.Add(TimeSpan.Parse(cita.HoraCita)).AddMinutes(60).ToString("HH:mm");
            string[] formatos = { "hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt", @"hh:mm tt" };
            var parseo = DateTime.TryParseExact(cita.HoraCita, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var value);
            string horaIngreso = value.ToString("HH:mm");
            string horaSalida = value.AddMinutes(60).ToString("HH:mm");

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter("@responsable", cita.AreaAtencion.ResponsablePermisoCitas),
                new SqlParameter("@dependencia", DBNull.Value),
                new SqlParameter("@nombre", cita.PersonaAutorizada == null ? cita.NombreSolicitante : cita.PersonaAutorizada),
                new SqlParameter("@fecha", cita.FechaCita),
                new SqlParameter("@horaIngreso", horaIngreso),
                new SqlParameter("@horaSalida", horaSalida),
                new SqlParameter("@campus", cita.AreaAtencion.CampusDescr),
                new SqlParameter("@lugarTrabajo", cita.AreaAtencion.LugarCitas),
                //new SqlParameter("@lugarTrabajo", "Registro escolar - Oficinas, cubiculos, cajas, site, ventanillas de atencion, archivo."),
                new SqlParameter("@actividadPresencial", cita.Tramite.Tramite),
                new SqlParameter("@tipoEmpleado", "Visita")
            };

            ExecSPOperaciones("SERVICIOSGENERALESRegistrarCita", parametros);
        }


        public void EliminarCitasSERVICIOSGENERALES(List<SolicitudCita> citas)
        {
            DataTable citaAsTable = new DataTable();
            citaAsTable.Columns.Add(new DataColumn("nombre", typeof(string)));
            citaAsTable.Columns.Add(new DataColumn("fecha", typeof(DateTime)));
            citaAsTable.Columns.Add(new DataColumn("campus", typeof(string)));

            foreach (SolicitudCita item in citas)
            {
                citaAsTable.Rows.Add(
                    item.PersonaAutorizada == null ? item.NombreSolicitante : item.PersonaAutorizada,
                    item.FechaCita,
                    item.AreaAtencion.CampusDescr
                    );

            }

            SqlParameter citasParameter = new SqlParameter("@citas", citaAsTable);
            citasParameter.TypeName = "CitaEliminarSERVICIOSGENERALESType";

            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                citasParameter
            };

            ExecSPOperaciones("SERVICIOSGENERALESEliminarCitas", parametros);
        }


        public string ConsultarCorreoEmpleado(string emplId)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter("@emplId", emplId)
            };

            SingleString correo = ExecSPConsultaUnico<SingleString>("JDEConsultarCorreoEmpleado", parametros);

            return correo != null ? correo.Texto.Trim() : null;
        }

    }
}
