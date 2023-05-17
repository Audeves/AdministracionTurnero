using Datos.Acceso.Administracion;
using Datos.Acceso.Citas;
using Datos.Acceso.Configuracion;
using Datos.Acceso.General;
using Datos.Acceso.Principales;
using Entidades.Entidades.Administracion;
using Entidades.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.General
{
    public class BaseBO<TEntity> where TEntity : EntityBase
    {

        protected BaseDAO<TEntity> BaseDAO { get; private set; }
        protected StoredProceduresDAO StoredProceduresDAO { get; private set; }
         
        protected ModuloDAO ModuloDAO { get; private set; }
        protected SubModuloDAO SubModuloDAO { get; private set; }
        protected PantallaDAO PantallaDAO { get; private set; }
        protected PantallaAccionDAO PantallaAccionDAO { get; private set; }

        protected PerfilDAO PerfilDAO { get; private set; }


        protected UsuarioDAO UsuarioDAO { get; private set; }
        


        protected AreaAtencionDAO AreaAtencionDAO { get; private set; }
        protected ProcesoDAO ProcesoDAO { get; private set; }
        protected TramiteDAO TramiteDAO { get; private set; }
        protected ConfiguracionTurneroDAO ConfiguracionTurneroDAO { get; private set; }
        protected ConfiguracionPantallaDAO ConfiguracionPantallaDAO { get; private set; }
        protected ModuloAtencionDAO ModuloAtencionDAO { get; private set; }

        protected ModuloAtencionBitacoraDAO ModuloAtencionBitacoraDAO { get; private set; }

        protected TurnoDAO TurnoDAO { get; private set; }




        protected SolicitudCitaDAO SolicitudCitaDAO { get; private set; }
        protected HistorialCancelacionMasivaDAO HistorialCancelacionMasivaDAO { get; private set; }
        protected BitacoraMovimientoCitaDAO BitacoraMovimientoCitaDAO { get; private set; }

        public BaseBO(ContextoDataBase db)
        {
            BaseDAO = new BaseDAO<TEntity>(db);
            StoredProceduresDAO = new StoredProceduresDAO(db);

            ModuloDAO = new ModuloDAO(db);
            SubModuloDAO = new SubModuloDAO(db);
            PantallaDAO = new PantallaDAO(db);
            PantallaAccionDAO = new PantallaAccionDAO(db);
            PerfilDAO = new PerfilDAO(db);
            UsuarioDAO = new UsuarioDAO(db);
            


            AreaAtencionDAO = new AreaAtencionDAO(db);
            ProcesoDAO = new ProcesoDAO(db);
            TramiteDAO = new TramiteDAO(db);
            ConfiguracionTurneroDAO = new ConfiguracionTurneroDAO(db);
            ConfiguracionPantallaDAO = new ConfiguracionPantallaDAO(db);
            ModuloAtencionDAO = new ModuloAtencionDAO(db);

            ModuloAtencionBitacoraDAO = new ModuloAtencionBitacoraDAO(db);
            TurnoDAO = new TurnoDAO(db);




            SolicitudCitaDAO = new SolicitudCitaDAO(db);
            HistorialCancelacionMasivaDAO = new HistorialCancelacionMasivaDAO(db);
            BitacoraMovimientoCitaDAO = new BitacoraMovimientoCitaDAO(db);
        }

        public virtual void Guardar(TEntity entity)
        {
            throw new NotImplementedException("Debes implementar el metodo Guardar en tu objeto BO");
        }

        public virtual void Eliminar(long id)
        {
            BaseDAO.Eliminar(id);
        }

        public virtual TEntity Consultar(long id)
        {
            return BaseDAO.ConsultarPorId(id);
        }

        public virtual TEntity Consultar(long id, params string[] relaciones)
        {
            return BaseDAO.ConsultarPorId(id, relaciones);
        }

        public virtual IList<TEntity> ObtenerTodos(params string[] relaciones)
        {
            return BaseDAO.ObtenerTodos(relaciones);
        }

    }
}
