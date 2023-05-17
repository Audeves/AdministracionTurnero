using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Administracion
{
    public class PantallaCONFIG : EntityTypeConfiguration<Pantalla>
    {

        public PantallaCONFIG()
        {
            this.ToTable("tblPantalla");
            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnName("id");
            this.Property(p => p.Nombre).HasColumnName("nombre");
            this.Property(p => p.Controlador).HasColumnName("controlador");
            this.Property(p => p.Accion).HasColumnName("accion");
            this.Property(p => p.Activo).HasColumnName("activo");
            this.Property(p => p.ClaseIcono).HasColumnName("claseIcono");
            this.Property(p => p.Privado).HasColumnName("privado");
            this.Property(p => p.Desarrollador).HasColumnName("desarrollador");
            this.Property(p => p.PublicaLibre).HasColumnName("publicaLibre");

            this.Property(p => p.IdModulo).HasColumnName("idModulo");
            this.Property(p => p.IdSubModulo).HasColumnName("idSubModulo");

            this.HasOptional(p => p.Modulo).WithMany().HasForeignKey(p => p.IdModulo);
            this.HasOptional(p => p.SubModulo).WithMany().HasForeignKey(p => p.IdSubModulo);

            this.HasMany(p => p.PantallasAcciones).WithOptional(pa => pa.Pantalla).HasForeignKey(pa => pa.IdPantalla);

        }

    }
}
