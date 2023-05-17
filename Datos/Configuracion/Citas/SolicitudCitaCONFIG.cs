using Entidades.Entidades.Citas;
using Entidades.Entidades.Configuracion;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Configuracion.Citas
{
    public class SolicitudCitaCONFIG : EntityTypeConfiguration<SolicitudCita>
    {

        public SolicitudCitaCONFIG()
        {
            this.ToTable("tbl_citasTramite");

            this.HasKey(sc => sc.Id);

            this.Property(sc => sc.Id).HasColumnName("id");
            this.Property(sc => sc.IdCita).HasColumnName("idcita");

            this.Property(sc => sc.Estatus).HasColumnName("estatus");
            this.Property(sc => sc.Asistio).HasColumnName("asistio");

            this.Property(sc => sc.FechaCita).HasColumnName("fechacita");
            this.Property(sc => sc.HoraCita).HasColumnName("horacita");
            this.Property(sc => sc.FechaCaptura).HasColumnName("fechaCaptura");
            this.Property(sc => sc.IdTramite).HasColumnName("idTramite");
            this.Property(sc => sc.NombreSolicitante).HasColumnName("nombre");
            this.Property(sc => sc.Email).HasColumnName("email");
            this.Property(sc => sc.IdAreaAtencion).HasColumnName("idAreaAtencion");
            this.Property(sc => sc.TramiteStr).HasColumnName("tramite");
            this.Property(sc => sc.ComentariosAdicionales).HasColumnName("comentario");
            this.Property(sc => sc.EmplId).HasColumnName("emplid");
            this.Property(sc => sc.Telefono).HasColumnName("telefonocelular");

            this.Property(sc => sc.PersonaAutorizada).HasColumnName("nombreautoriza");
            this.Property(sc => sc.Parentezco).HasColumnName("parentesco");
            this.Property(sc => sc.Folio).HasColumnName("folio");

            this.Property(sc => sc.ComentariosAdministrador).HasColumnName("comentariosAdministrador");

            this.HasRequired<TramiteCita>(sc => sc.Tramite).WithMany().HasForeignKey(sc => sc.IdTramite);
            this.HasRequired<AreaAtencion>(sc => sc.AreaAtencion).WithMany().HasForeignKey(sc => sc.IdAreaAtencion);

            this.HasMany<BitacoraMovimientoCita>(sc => sc.BitacoraMovimientos).WithRequired(b => b.Cita).HasForeignKey(b => b.IdCita);
        }

    }
}
