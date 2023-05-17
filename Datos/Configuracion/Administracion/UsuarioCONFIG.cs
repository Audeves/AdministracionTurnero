using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Administracion
{
    public class UsuarioCONFIG : EntityTypeConfiguration<Usuario>
    {

        public UsuarioCONFIG()
        {
            this.ToTable("tblUsuario");
            this.HasKey(u => u.Id);

            this.Property(u => u.Id).HasColumnName("id");
            this.Property(u => u.Emplid).HasColumnName("emplid");
            this.Property(u => u.Nombre).HasColumnName("nombre");
            this.Property(u => u.CuentaDominio).HasColumnName("cuentaDominio");
            this.Property(u => u.DptoAdscripcion).HasColumnName("dptoAdscripcion");
            this.Property(u => u.ClaveDptoAdscripcion).HasColumnName("claveDptoAdscripcion");
            this.Property(u => u.DireccionAcademica).HasColumnName("direccionAcademica");
            this.Property(u => u.ClaveDireccionAcademica).HasColumnName("claveDireccionAcademica");
            this.Property(u => u.Activo).HasColumnName("activo");

            this.HasMany(u => u.Perfiles).WithMany().Map(m =>
            {
                m.ToTable("tblRelUsuariosPerfiles");
                m.MapLeftKey("idUsuario");
                m.MapRightKey("idPerfil");
            });
            


            this.Property(u => u.Desarrollador).HasColumnName("desarrollador");
            this.Property(u => u.Correo).HasColumnName("correo");

        }

    }
}
