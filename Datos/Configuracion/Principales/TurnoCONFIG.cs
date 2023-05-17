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
    public class TurnoCONFIG : EntityTypeConfiguration<Turno>
    {

        public TurnoCONFIG()
        {
            this.ToTable("tblTurnosTramitesEmitidos");

            this.HasKey(t => t.Id);

            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.IdAreaAtencion).HasColumnName("idAreaAtencion");
            this.Property(t => t.IdProceso).HasColumnName("idProceso");
            this.Property(t => t.IdTramite).HasColumnName("idTramite");
            this.Property(t => t.FechaEmisionTurno).HasColumnName("fechaEmisionTurno");
            this.Property(t => t.NumeroTurno).HasColumnName("numeroTurno");
            this.Property(t => t.IdConfiguracionTurnero).HasColumnName("idConfiguracionTurnero");
            this.Property(t => t.IdAlumno).HasColumnName("idAlumno");
            this.Property(t => t.NombreCliente).HasColumnName("nombreCliente");

            this.Property(t => t.Estatus).HasColumnName("estatus");

            this.HasRequired<AreaAtencion>(t => t.AreaAtencion).WithMany().HasForeignKey(t => t.IdAreaAtencion);
            this.HasRequired<Proceso>(t => t.Proceso).WithMany().HasForeignKey(t => t.IdProceso);
            this.HasRequired<Tramite>(t => t.Tramite).WithMany().HasForeignKey(t => t.IdTramite);
            this.HasRequired<ConfiguracionTurnero>(t => t.ConfiguracionTurnero).WithMany().HasForeignKey(t => t.IdConfiguracionTurnero);

            this.HasMany<TurnoEtapa>(t => t.Etapas).WithRequired(te => te.Turno).HasForeignKey(te => te.IdTurno);
        }

    }
}
