using Entidades.Entidades.Citas;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Citas
{
    public class HistorialCancelacionMasivaCONFIG : EntityTypeConfiguration<HistorialCancelacionMasiva>
    {

        public HistorialCancelacionMasivaCONFIG()
        {
            this.ToTable("tblHistorialCancelacionMasiva");

            this.HasKey(h => h.Id);

            this.Property(h => h.Id).HasColumnName("id");
            this.Property(h => h.FechaEmision).HasColumnName("fechaEmision");
            this.Property(h => h.Usuario).HasColumnName("usuario");
            this.Property(h => h.FechaDesde).HasColumnName("fechaDesde");
            this.Property(h => h.FechaHasta).HasColumnName("fechaHasta");
            this.Property(h => h.Pendientes).HasColumnName("pendientes");
            this.Property(h => h.Agendadas).HasColumnName("agendadas");
            this.Property(h => h.EnviarCorreo).HasColumnName("enviarCorreo");
            this.Property(h => h.ComentarioCancelacion).HasColumnName("comentarioCancelacion");
            this.Property(h => h.CantidadCitas).HasColumnName("cantidadCitas");
        }

    }
}
