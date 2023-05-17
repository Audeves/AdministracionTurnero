using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Configuracion
{
    public class TramiteCONFIG : EntityTypeConfiguration<Tramite>
    {

        public TramiteCONFIG()
        {
            this.ToTable("tblTramites");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.Nombre).HasColumnName("nombre");
            this.Property(t => t.IdProceso).HasColumnName("idProceso");
            this.Property(t => t.Activo).HasColumnName("estatus");
            this.Property(t => t.RequiereExpediente).HasColumnName("requiereExpediente");
            this.Property(t => t.CorreoExpediente).HasColumnName("correoExpediente");
            this.Property(t => t.EstatusRegistro).HasColumnName("estatusRegistro");
            this.Property(t => t.FechaRegistro).HasColumnName("fechaRegistro");
            this.Property(t => t.IdUsuarioRegistro).HasColumnName("idUsuarioRegistro");

            this.Property(t => t.IdAreaAtencionProceso).HasColumnName("idAreaAtencionProceso");

            this.HasRequired(p => p.Proceso).WithMany().HasForeignKey(p => p.IdProceso);
            this.HasRequired(p => p.AreaAtencionProceso).WithMany().HasForeignKey(p => p.IdAreaAtencionProceso);

        }

    }
}
