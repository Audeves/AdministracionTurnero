using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Configuracion
{
    public class ConfiguracionPantallaCONFIG : EntityTypeConfiguration<ConfiguracionPantalla>
    {

        public ConfiguracionPantallaCONFIG()
        {
            this.ToTable("tblConfiguracionesPantallas");

            this.HasKey(cp => cp.Id);

            this.Property(cp => cp.Id).HasColumnName("id");
            this.Property(cp => cp.IP).HasColumnName("ip");
            this.Property(cp => cp.CampusPS).HasColumnName("campus_ps");
            this.Property(cp => cp.CampusDescr).HasColumnName("campusDescr");
            this.Property(cp => cp.Activo).HasColumnName("activo");

            this.HasMany<AreaAtencion>(ct => ct.AreasAtencion).WithOptional(a => a.ConfiguracionPantalla).HasForeignKey(a => a.IdConfiguracionPantalla);
        }

    }
}
