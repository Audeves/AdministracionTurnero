using Entidades.Entidades.Principales;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Principales
{
    public class ModuloAtencionBitacoraCONFIG : EntityTypeConfiguration<ModuloAtencionBitacora>
    {

        public ModuloAtencionBitacoraCONFIG()
        {
            this.ToTable("tblModulosAtencionBitacora");

            this.HasKey(mab => mab.Id);

            this.Property(mab => mab.Id).HasColumnName("id");
            this.Property(mab => mab.IdModuloAtencion).HasColumnName("idModuloAtencion");
            this.Property(mab => mab.IdUsuario).HasColumnName("idUsuario");
            this.Property(mab => mab.FechaInicio).HasColumnName("fechaInicio");
            this.Property(mab => mab.FechaTermino).HasColumnName("fechaTermino");
            this.Property(mab => mab.Activo).HasColumnName("activo");

            this.HasRequired(mab => mab.ModuloAtencion).WithMany().HasForeignKey(mab => mab.IdModuloAtencion);
            this.HasRequired(mab => mab.Usuario).WithMany().HasForeignKey(mab => mab.IdUsuario);
        }

    }
}
