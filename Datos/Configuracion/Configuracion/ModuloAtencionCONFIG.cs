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
    public class ModuloAtencionCONFIG : EntityTypeConfiguration<ModuloAtencion>
    {

        public ModuloAtencionCONFIG()
        {
            this.ToTable("tblModulosAtencion");

            this.HasKey(ma => ma.Id);

            this.Property(ma => ma.Id).HasColumnName("id");
            this.Property(ma => ma.Nombre).HasColumnName("nombre");
            this.Property(ma => ma.Activo).HasColumnName("activo");
            this.Property(ma => ma.IdAreaAtencion).HasColumnName("idAreaAtencion");

            this.HasMany<Proceso>(ma => ma.Procesos).WithMany().Map(m =>
            {
                m.ToTable("tblRelModulosAtencionProcesos");
                m.MapLeftKey("idModuloAtencion");
                m.MapRightKey("idProceso");
            });

            this.HasMany<Usuario>(ma => ma.UsuariosAtencion).WithMany(u => u.ModulosAtencion).Map(m =>
            {
                m.ToTable("tblRelModulosAtencionUsuarios");
                m.MapLeftKey("idModuloAtencion");
                m.MapRightKey("idUsuario");
            });

        }

    }
}
