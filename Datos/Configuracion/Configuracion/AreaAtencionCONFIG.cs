using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Configuracion
{
    public class AreaAtencionCONFIG : EntityTypeConfiguration<AreaAtencion>
    {

        public AreaAtencionCONFIG()
        {
            this.ToTable("tblAreasAtencion");

            this.HasKey(a => a.Id);

            this.Property(a => a.Id).HasColumnName("id");
            this.Property(a => a.Nombre).HasColumnName("nombre");
            this.Property(a => a.CampusPS).HasColumnName("campus_ps");
            this.Property(a => a.CampusDescr).HasColumnName("campusDescr");
            this.Property(a => a.Ticket).HasColumnName("ticket");
            this.Property(a => a.Ayuda).HasColumnName("ayuda");

            this.HasMany<Usuario>(a => a.Responsables).WithMany(u => u.AreasAtencionResponsable).Map(m =>
            {
                m.ToTable("tblRelAreasAtencionUsuariosResponsables");
                m.MapLeftKey("idAreaAtencion");
                m.MapRightKey("idUsuarioResponsable");
            });

            this.Property(a => a.IdConfiguracionTurnero).HasColumnName("idConfiguracionTurnero");
            this.Property(a => a.IdConfiguracionPantalla).HasColumnName("idConfiguracionPantallaSalaEspera");

            this.Property(a => a.DisponibleCitas).HasColumnName("disponibleCitas");
            this.Property(a => a.ResponsablePermisoCitas).HasColumnName("responsablePermisoCitas");
            this.Property(a => a.LugarCitas).HasColumnName("lugarCitas");

            this.Property(a => a.CorreoAgendada).HasColumnName("correoAgendada");
            this.Property(a => a.CorreoCancelada).HasColumnName("correoCancelada");
            this.Property(a => a.CorreoRemitenteCitas).HasColumnName("correoRemitenteCitas");

            this.HasMany<ModuloAtencion>(a => a.ModulosAtencion).WithRequired(ma => ma.AreaAtencion).HasForeignKey(ma => ma.IdAreaAtencion);
        }

    }
}
