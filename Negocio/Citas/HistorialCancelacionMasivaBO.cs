using Datos.Acceso.General;
using Entidades.Entidades.Citas;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Citas
{
    public class HistorialCancelacionMasivaBO : BaseBO<HistorialCancelacionMasiva>
    {

        public HistorialCancelacionMasivaBO(ContextoDataBase db) : base(db)
        {

        }


        public void Guardar(string usuarioCuentaDominio, DateTime? fechaDesde, DateTime? fechaHasta, bool pendientes, bool agendadas,
            bool enviarCorreo, string comentarioCancelacion, int cantidadCitas)
        {
            HistorialCancelacionMasiva historialCancelacionMasiva = new HistorialCancelacionMasiva()
            {
                Usuario = usuarioCuentaDominio,
                FechaEmision = DateTime.Now,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Pendientes = pendientes,
                Agendadas = agendadas,
                EnviarCorreo = enviarCorreo,
                ComentarioCancelacion = comentarioCancelacion,
                CantidadCitas = cantidadCitas
            };

            HistorialCancelacionMasivaDAO.Guardar(historialCancelacionMasiva);
        }

    }
}
