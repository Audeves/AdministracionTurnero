using Datos.Configuracion.Administracion;
using Datos.Configuracion.Citas;
using Datos.Configuracion.Configuracion;
using Datos.Configuracion.General;
using Datos.Configuracion.Principales;
using Entidades.Entidades.Administracion;
using Entidades.Entidades.Citas;
using Entidades.Entidades.Configuracion;
using Entidades.Entidades.General;
using Entidades.Entidades.Principales;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.General
{
    public class ContextoDataBase : DbContext
    {

        public DbSet<Modulo> Modulo { get; set; }
        public DbSet<SubModulo> SubModulo { get; set; }
        public DbSet<Pantalla> Pantalla { get; set; }
        public DbSet<PantallaAccion> PantallaAccion { get; set; }
        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<AreaAtencion> AreaAtencion { get; set; }
        public DbSet<Proceso> Proceso { get; set; }
        public DbSet<Tramite> Tramite { get; set; }
        public DbSet<ConfiguracionTurnero> ConfiguracionTurnero { get; set; }
        public DbSet<ConfiguracionPantalla> ConfiguracionPantalla { get; set; }
        public DbSet<ModuloAtencion> ModuloAtencion { get; set; }

        public DbSet<ModuloAtencionBitacora> ModuloAtencionBitacora { get; set; }
        //
        public DbSet<Turno> Turno { get; set; }
        //
        public DbSet<TurnoEtapa> TurnoEtapa { get; set; }


        //Citas
        public DbSet<TramiteCita> TramiteCita { get; set; }
        public DbSet<SolicitudCita> SolicitudCita { get; set; }
        public DbSet<HistorialCancelacionMasiva> HistorialCancelacionMasiva { get; set; }
        public DbSet<BitacoraMovimientoCita> BitacoraMovimientoCita { get; set; }

        public ContextoDataBase() : base(InicioDataBase.Conexion)
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 600;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ModuloCONFIG());
            modelBuilder.Configurations.Add(new SubModuloCONFIG());
            modelBuilder.Configurations.Add(new PantallaCONFIG());
            modelBuilder.Configurations.Add(new PantallaAccionCONFIG());
            modelBuilder.Configurations.Add(new PerfilCONFIG());
            modelBuilder.Configurations.Add(new UsuarioCONFIG());

            modelBuilder.Configurations.Add(new AreaAtencionCONFIG());
            modelBuilder.Configurations.Add(new ProcesoCONFIG());
            modelBuilder.Configurations.Add(new TramiteCONFIG());
            modelBuilder.Configurations.Add(new ConfiguracionTurneroCONFIG());
            modelBuilder.Configurations.Add(new ConfiguracionPantallaCONFIG());
            modelBuilder.Configurations.Add(new ModuloAtencionCONFIG());

            modelBuilder.Configurations.Add(new ModuloAtencionBitacoraCONFIG());
            //
            modelBuilder.Configurations.Add(new TurnoCONFIG());
            //
            modelBuilder.Configurations.Add(new TurnoEtapaCONFIG());



            //Citas
            modelBuilder.Configurations.Add(new TramiteCitaCONFIG());
            modelBuilder.Configurations.Add(new SolicitudCitaCONFIG());
            modelBuilder.Configurations.Add(new HistorialCancelacionMasivaCONFIG());
            modelBuilder.Configurations.Add(new BitacoraMovimientoCitaCONFIG());


            base.OnModelCreating(modelBuilder);
        }

    }
}
