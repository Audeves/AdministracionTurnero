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
    public class ModuloBO : BaseBO<Modulo>
    {

        public ModuloBO(ContextoDataBase db) : base(db)
        {

        }

        public Modulo Guardar(Modulo modulo)
        {
            return ModuloDAO.Guardar(modulo);
        }

        public PagingResult<ModuloGRIDDTO> ObtenerGridModulos(PagingConfig config)
        {
            return ModuloDAO.ObtenerGridModulos(config);
        }

        public List<Modulo> ConsultarActivos()
        {
            return ModuloDAO.ConsultarActivos();
        }

    }
}
