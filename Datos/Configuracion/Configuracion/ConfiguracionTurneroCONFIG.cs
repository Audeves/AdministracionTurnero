using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Configuracion
{
    public class ConfiguracionTurneroCONFIG : EntityTypeConfiguration<ConfiguracionTurnero>
    {

        public ConfiguracionTurneroCONFIG()
        {
            this.ToTable("tblConfiguracionesTurneros");

            this.HasKey(ct => ct.Id);

            this.Property(ct => ct.Id).HasColumnName("id");
            this.Property(ct => ct.Nombre).HasColumnName("nombre");
            this.Property(ct => ct.IP).HasColumnName("ip");
            this.Property(ct => ct.CampusPS).HasColumnName("campus_ps");
            this.Property(ct => ct.CampusDescr).HasColumnName("campusDescr");
            this.Property(ct => ct.SolicitarId).HasColumnName("solicitarId");
            this.Property(ct => ct.SolicitarNombre).HasColumnName("solicitarNombre");
            this.Property(ct => ct.Activo).HasColumnName("activo");

            this.HasMany<AreaAtencion>(ct => ct.AreasAtencion).WithOptional(a => a.ConfiguracionTurnero).HasForeignKey(a => a.IdConfiguracionTurnero);
        }

    }
}
