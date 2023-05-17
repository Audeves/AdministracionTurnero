using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using Entidades.Entidades.Principales;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Principales
{
    public class TurnoEtapaCONFIG : EntityTypeConfiguration<TurnoEtapa>
    {

        public TurnoEtapaCONFIG()
        {
            this.ToTable("tblTurnosEtapas");

            this.HasKey(te => te.Id);

            this.Property(te => te.Id).HasColumnName("id");
            this.Property(te => te.IdTurno).HasColumnName("idTurnoTramiteEmitido");
            this.Property(te => te.FechaMovimiento).HasColumnName("fechaMovimiento");
            this.Property(te => te.Estatus).HasColumnName("estatus");
            this.Property(te => te.EstatusDescr).HasColumnName("estatusDescr");
            this.Property(te => te.IdModuloAtencion).HasColumnName("idModuloAtencion");
            this.Property(te => te.IdUsuarioAtencion).HasColumnName("idUsuarioAtencion");

            this.HasRequired<ModuloAtencion>(te => te.ModuloAtencion).WithMany().HasForeignKey(te => te.IdModuloAtencion);
            this.HasRequired<Usuario>(te => te.UsuarioAtencion).WithMany().HasForeignKey(te => te.IdUsuarioAtencion);
        }

    }
}
