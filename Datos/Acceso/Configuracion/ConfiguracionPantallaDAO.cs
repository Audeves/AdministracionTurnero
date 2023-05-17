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
    public class ConfiguracionPantallaDAO : BaseDAO<ConfiguracionPantalla>
    {

        public ConfiguracionPantallaDAO(ContextoDataBase db) : base(db)
        {

        }


        public PagingResult<ConfiguracionPantallaGRIDDTO> ObtenerGridConfiguracionesPantallas(PagingConfig config, List<AreaAtencion> areasAtencionUsuarioActual)
        {
            Expression<Func<ConfiguracionPantalla, ConfiguracionPantallaGRIDDTO>> select = cp => new ConfiguracionPantallaGRIDDTO()
            {
                Id = cp.Id,
                IP = cp.IP,
                Campus = cp.CampusDescr,
                AreasAtencion = cp.AreasAtencion
            };

            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

        public new ConfiguracionPantalla Guardar(ConfiguracionPantalla pantalla)
        {
            ConfiguracionPantalla pantallaGuardada = this.ConsultarPorId(pantalla.Id, "AreasAtencion");
            if (pantallaGuardada == null)
            {
                pantallaGuardada = new ConfiguracionPantalla();
            }
            
            pantallaGuardada.IP = pantalla.IP;
            pantallaGuardada.CampusPS = pantalla.CampusPS;
            pantallaGuardada.CampusDescr = pantalla.CampusDescr;
            pantallaGuardada.Activo = pantalla.Activo;

            base.Guardar(pantallaGuardada);
            return pantallaGuardada;
        }

    }
}
