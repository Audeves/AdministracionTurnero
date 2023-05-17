using Entidades.Entidades.Citas;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Citas
{
    public class BitacoraMovimientoCitaCONFIG : EntityTypeConfiguration<BitacoraMovimientoCita>
    {

        public BitacoraMovimientoCitaCONFIG()
        {
            this.ToTable("tblBitacoraMovimientoCita");

            this.HasKey(b => b.Id);

            this.Property(b => b.Id).HasColumnName("id");
            this.Property(b => b.IdCita).HasColumnName("idCita");
            this.Property(b => b.Usuario).HasColumnName("usuario");
            this.Property(b => b.Estatus).HasColumnName("estatus");
            this.Property(b => b.EstatusStr).HasColumnName("estatusStr");
            this.Property(b => b.Asistio).HasColumnName("asistio");
            this.Property(b => b.TipoMovimiento).HasColumnName("tipoMovimiento");
            this.Property(b => b.TipoMovimientoStr).HasColumnName("tipoMovimientoStr");
            this.Property(b => b.ComentarioAdministrador).HasColumnName("comentarioAdministrador");

        }

    }
}
