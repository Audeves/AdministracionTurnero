using Entidades.Entidades.Citas;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Citas
{
    public class TramiteCitaCONFIG : EntityTypeConfiguration<TramiteCita>
    {

        public TramiteCitaCONFIG()
        {
            this.ToTable("tbl_tramiteparacitas");

            this.HasKey(tc => tc.Id);

            this.Property(tc => tc.Id).HasColumnName("id");
            this.Property(tc => tc.Tramite).HasColumnName("tramite");
            this.Property(tc => tc.Activo).HasColumnName("activo");

            this.Property(tc => tc.ComentarioAceptar).HasColumnName("comentarioAceptar");
            this.Property(tc => tc.ComentarioRechazar).HasColumnName("comentarioRechazar");
        }

    }
}
