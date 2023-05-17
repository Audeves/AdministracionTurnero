using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Configuracion
{
    public class ProcesoCONFIG : EntityTypeConfiguration<Proceso>
    {

        public ProcesoCONFIG()
        {
            this.ToTable("tblProcesos");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnName("id");
            this.Property(p => p.Nombre).HasColumnName("nombre");
            this.Property(p => p.Activo).HasColumnName("estatus");
            this.Property(p => p.IdAreaAtencion).HasColumnName("idAreaAtencion");
            this.Property(p => p.EstatusRegistro).HasColumnName("estatusRegistro");
            this.Property(p => p.FechaRegistro).HasColumnName("fechaRegistro");
            this.Property(p => p.IdUsuarioRegistro).HasColumnName("idUsuarioRegistro");

            this.HasRequired(p => p.AreaAtencion).WithMany().HasForeignKey(p => p.IdAreaAtencion);
        }

    }
}
