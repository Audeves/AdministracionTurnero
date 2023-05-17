using Datos.Acceso.General;
using Entidades.Entidades.Configuracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.Configuracion
{
    public class ConfiguracionTurneroDAO : BaseDAO<ConfiguracionTurnero>
    {

        public ConfiguracionTurneroDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<ConfiguracionTurneroGRIDDTO> ObtenerGridConfiguracionesTurneros(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<ConfiguracionTurnero, ConfiguracionTurneroGRIDDTO>> select = ct => new ConfiguracionTurneroGRIDDTO()
            {
                Id = ct.Id,
                Nombre = ct.Nombre,
                IP = ct.IP,
                Campus = ct.CampusDescr,
                AreasAtencion = ct.AreasAtencion,
                SolicitarId = ct.SolicitarId,
                SolicitarNombre = ct.SolicitarNombre
            };

            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

        public new ConfiguracionTurnero Guardar(ConfiguracionTurnero turnero)
        {
            ConfiguracionTurnero turneroGuardado = this.ConsultarPorId(turnero.Id, "AreasAtencion");
            if (turneroGuardado == null) 
            {
                turneroGuardado = new ConfiguracionTurnero();
            }

            turneroGuardado.Nombre = turnero.Nombre;
            turneroGuardado.IP = turnero.IP;
            turneroGuardado.CampusPS = turnero.CampusPS;
            turneroGuardado.CampusDescr = turnero.CampusDescr;
            turneroGuardado.SolicitarId = turnero.SolicitarId;
            turneroGuardado.SolicitarNombre = turnero.SolicitarNombre;
            turneroGuardado.Activo = turnero.Activo;

            base.Guardar(turneroGuardado);
            return turneroGuardado;
        }

    }
}
