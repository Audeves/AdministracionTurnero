using Entidades.Entidades.Administracion;
using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Administracion
{
    public class PerfilCONFIG : EntityTypeConfiguration<Perfil>
    {

        public PerfilCONFIG()
        {
            this.ToTable("tblPerfil");
            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnName("id");
            this.Property(p => p.Nombre).HasColumnName("nombre");
            this.Property(p => p.Activo).HasColumnName("activo");
            this.Property(p => p.Desarrollador).HasColumnName("desarrollador");

            this.Property(p => p.EstatusRegistro).HasColumnName("estatusRegistro");
            this.Property(p => p.FechaRegistro).HasColumnName("fechaRegistro");
            this.Property(p => p.IdUsuarioRegistro).HasColumnName("idUsuarioRegistro");

            this.HasMany(p => p.Pantallas).WithMany().Map(m =>
            {
                m.ToTable("tblRelPerfilesPantallas");
                m.MapLeftKey("idPerfil");
                m.MapRightKey("idPantalla");
            });

            this.HasMany<AreaAtencion>(p => p.AreasAtencion).WithMany(a => a.Perfiles).Map(m =>
            {
                m.ToTable("tblRelAreasAtencionPerfiles");
                m.MapLeftKey("idPerfil");
                m.MapRightKey("idAreaAtencion");
            });


            this.Property(p => p.ElegibleAsignar).HasColumnName("elegibleAsignar");
        }

    }
}
