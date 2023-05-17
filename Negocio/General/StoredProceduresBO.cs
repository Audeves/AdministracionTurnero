using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Entidades.Entidades.General;
using Entidades.GridSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.General
{
    public class StoredProceduresBO : SQLBaseBO
    {

        public StoredProceduresBO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<Empleado> ConsultarEmpleados(PagingConfig config, string emplId, string nombre, string cuentaDominio, string dptoAdscripcion)
        {
            return StoredProceduresDAO.ConsultarEmpleados(config, emplId, nombre, cuentaDominio, dptoAdscripcion);
        }

        public IList<Campus> ConsultarCampus()
        {
            return StoredProceduresDAO.ConsultarCampus();
        }

        public void RegistrarCitaSERVICIOSGENERALES(SolicitudCita cita)
        {
            StoredProceduresDAO.RegistrarCitaSERVICIOSGENERALES(cita);
        }

        public void EliminarCitasSERVICIOSGENERALES(List<SolicitudCita> citas)
        {
            StoredProceduresDAO.EliminarCitasSERVICIOSGENERALES(citas);
        }

        public string ConsultarCorreoEmpleado(string emplId)
        {
            return StoredProceduresDAO.ConsultarCorreoEmpleado(emplId);
        }

    }
}
