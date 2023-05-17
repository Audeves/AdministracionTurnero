using Entidades.Entidades.Administracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Administracion
{
    public class PantallaAccionCONFIG : EntityTypeConfiguration<PantallaAccion>
    {

        public PantallaAccionCONFIG()
        {
            this.ToTable("tblPantallaAccion");
            this.HasKey(pa => pa.Id);

            this.Property(pa => pa.Id).HasColumnName("id");
            this.Property(pa => pa.Controlador).HasColumnName("controlador");
            this.Property(pa => pa.Accion).HasColumnName("accion");
            this.Property(pa => pa.Activo).HasColumnName("activo");
            this.Property(pa => pa.Privado).HasColumnName("privado");
            this.Property(pa => pa.PublicaLibre).HasColumnName("publicaLibre");
            this.Property(pa => pa.IdPantalla).HasColumnName("idPantalla");
        }

    }
}
