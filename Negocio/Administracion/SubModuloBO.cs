using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Administracion;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Administracion
{
    public class SubModuloBO : BaseBO<SubModulo>
    {

        public SubModuloBO(ContextoDataBase db) : base(db)
        {

        }

        public SubModulo Guardar(SubModulo subModulo)
        {
            return SubModuloDAO.Guardar(subModulo);
        }

        public PagingResult<SubModuloGRIDDTO> ObtenerGridSubModulos(PagingConfig config)
        {
            return SubModuloDAO.ObtenerGridSubModulos(config);
        }

        public List<SubModulo> ConsultarActivos()
        {
            return SubModuloDAO.ConsultarActivos();
        }


    }
}
