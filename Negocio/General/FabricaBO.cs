using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Negocio.Administracion;
using Negocio.Citas;
using Negocio.Configuracion;
using Negocio.Principales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.General
{
    //Creo instancias unicas de los objetos de negocio.
    public class FabricaBO
    {

        //Declarar contexto bd
        private ContextoDataBase db { get; set; }

        public FabricaBO()
        {
            db = new ContextoDataBase();
        }


        private StoredProceduresBO storedProceduresBO;

        private UsuarioBO usuarioBO;
        private ModuloBO moduloBO;
        private SubModuloBO subModuloBO;
        private PantallaBO pantallaBO;
        private PerfilBO perfilBO;
        


        private AreaAtencionBO areaAtencionBO;
        private ProcesoBO procesoBO;
        private TramiteBO tramiteBO;
        private ConfiguracionTurneroBO configuracionTurneroBO;
        private ConfiguracionPantallaBO configuracionPantallaBO;
        private ModuloAtencionBO moduloAtencionBO;

        private ModuloAtencionBitacoraBO moduloAtencionBitacoraBO;



        private SolicitudCitaBO solicitudCitaBO;
        private HistorialCancelacionMasivaBO historialCancelacionMasivaBO;
        private BitacoraMovimientoCitaBO bitacoraMovimientoCitaBO;
         

        public StoredProceduresBO StoredProceduresBO
        {
            get
            {
                if (storedProceduresBO == null)
                {
                    storedProceduresBO = new StoredProceduresBO(db);
                }
                return storedProceduresBO;
            }
        }

        public UsuarioBO UsuarioBO
        {
            get
            {
                if (usuarioBO == null)
                {
                    usuarioBO = new UsuarioBO(db);
                }
                return usuarioBO;
            }
        }

        public ModuloBO ModuloBO
        {
            get
            {
                if (moduloBO == null)
                {
                    moduloBO = new ModuloBO(db);
                }
                return moduloBO;
            }
        }

        public SubModuloBO SubModuloBO
        {
            get
            {
                if (subModuloBO == null)
                {
                    subModuloBO = new SubModuloBO(db);
                }
                return subModuloBO;
            }
        }

        public PantallaBO PantallaBO
        {
            get
            {
                if (pantallaBO == null)
                {
                    pantallaBO = new PantallaBO(db);
                }
                return pantallaBO;
            }
        }

        public PerfilBO PerfilBO
        {
            get
            {
                if (perfilBO == null)
                {
                    perfilBO = new PerfilBO(db);
                }
                return perfilBO;
            }
        }




        



        public AreaAtencionBO AreaAtencionBO
        {
            get
            {
                if (areaAtencionBO == null)
                {
                    areaAtencionBO = new AreaAtencionBO(db);
                }
                return areaAtencionBO;
            }
        }

        public ProcesoBO ProcesoBO
        {
            get
            {
                if (procesoBO == null)
                {
                    procesoBO = new ProcesoBO(db);
                }
                return procesoBO;
            }
        }

        public TramiteBO TramiteBO
        {
            get
            {
                if (tramiteBO == null)
                {
                    tramiteBO = new TramiteBO(db);
                }
                return tramiteBO;
            }
        }

        public ConfiguracionTurneroBO ConfiguracionTurneroBO
        {
            get
            {
                if (configuracionTurneroBO == null)
                {
                    configuracionTurneroBO = new ConfiguracionTurneroBO(db);
                }
                return configuracionTurneroBO;
            }
        }

        public ConfiguracionPantallaBO ConfiguracionPantallaBO
        {
            get
            {
                if (configuracionPantallaBO == null)
                {
                    configuracionPantallaBO = new ConfiguracionPantallaBO(db);
                }
                return configuracionPantallaBO;
            }
        }

        public ModuloAtencionBO ModuloAtencionBO
        {
            get
            {
                if (moduloAtencionBO == null)
                {
                    moduloAtencionBO = new ModuloAtencionBO(db);
                }
                return moduloAtencionBO;
            }
        }






        public ModuloAtencionBitacoraBO ModuloAtencionBitacoraBO
        {
            get
            {
                if (moduloAtencionBitacoraBO == null)
                {
                    moduloAtencionBitacoraBO = new ModuloAtencionBitacoraBO(db);
                }
                return moduloAtencionBitacoraBO;
            }
        }





        public SolicitudCitaBO SolicitudCitaBO
        {
            get
            {
                if (solicitudCitaBO == null)
                {
                    solicitudCitaBO = new SolicitudCitaBO(db);
                }
                return solicitudCitaBO;
            }
        }

        public HistorialCancelacionMasivaBO HistorialCancelacionMasivaBO
        {
            get
            {
                if (historialCancelacionMasivaBO == null)
                {
                    historialCancelacionMasivaBO = new HistorialCancelacionMasivaBO(db);
                }
                return historialCancelacionMasivaBO;
            }
        }

        public BitacoraMovimientoCitaBO BitacoraMovimientoCitaBO
        {
            get
            {
                if (bitacoraMovimientoCitaBO == null)
                {
                    bitacoraMovimientoCitaBO = new BitacoraMovimientoCitaBO(db);
                }
                return bitacoraMovimientoCitaBO;
            }
        }

    }
}
