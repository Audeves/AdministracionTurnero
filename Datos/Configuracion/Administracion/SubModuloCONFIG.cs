using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Administracion
{
    public class SubModuloCONFIG : EntityTypeConfiguration<SubModulo>
    {

        public SubModuloCONFIG()
        {
            this.ToTable("tblSubModulo");

            this.HasKey(s => s.Id);

            this.Property(s => s.Id).HasColumnName("id");
            this.Property(s => s.Nombre).HasColumnName("nombre");
            this.Property(s => s.Activo).HasColumnName("activo");
            this.Property(s => s.ClaseIcono).HasColumnName("claseIcono");
            this.Property(s => s.IdModulo).HasColumnName("idModulo");
        }

    }
}
