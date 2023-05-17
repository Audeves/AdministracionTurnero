using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Administracion
{
    public class ModuloCONFIG : EntityTypeConfiguration<Modulo>
    {

        public ModuloCONFIG()
        {
            this.ToTable("tblModulo");

            this.HasKey(m => m.Id);

            this.Property(m => m.Id).HasColumnName("id");
            this.Property(m => m.Nombre).HasColumnName("nombre");
            this.Property(m => m.Activo).HasColumnName("activo");

            this.HasMany(m => m.SubModulos).WithRequired(s => s.Modulo).HasForeignKey(s => s.IdModulo);
        }

    }
}
