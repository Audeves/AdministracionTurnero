using Datos.Acceso.General;
using Datos.Acceso.Principales;
using Entidades.Entidades.Principales;
using Entidades.GridSupport;
using Entidades.TransporteGrid.Principales;
using Entidades.Excepciones;
using Negocio.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Principales
{
    public class ModuloAtencionBitacoraBO : BaseBO<ModuloAtencionBitacora>
    {

        public ModuloAtencionBitacoraBO(ContextoDataBase db) : base(db)
        {

        }


        public bool ValidarModuloLibre(long idModulo)
        {
            if (!ModuloAtencionBitacoraDAO.ValidarModuloLibre(idModulo))
            {
                throw new ExcepcionNegocio("El modulo seleccionado esta siendo atendido por otro usuario");
            }
            return true;
        }

        public bool ValidarUsuarioNoAtendiendo(long idUsuario)
        {
            return ModuloAtencionBitacoraDAO.ValidarUsuarioNoAtendiendo(idUsuario);
        }

        public void IniciarAtencion(long idModulo, long idUsuario)
        {
            if (!ModuloAtencionBitacoraDAO.ValidarModuloLibre(idModulo))
            {
                throw new ExcepcionNegocio("El modulo seleccionado esta siendo atendido por otro usuario");
            }
            else
            {
                ModuloAtencionBitacoraDAO.IniciarAtencion(idModulo, idUsuario);
            }
        }

        public void FinalizarAtencion(long idUsuario)
        {
            ModuloAtencionBitacoraDAO.FinalizarAtencion(idUsuario);
        }

        public ModuloAtencionBitacora ObtenerRegistroBitacoraActivo(long idUsuario)
        {
            return ModuloAtencionBitacoraDAO.ObtenerRegistroBitacoraActivo(idUsuario);
        }


        /// <summary>
        /// EJEMPLO
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public PagingResult<TurnoGRIDDTO> ObtenerGridTurnoPorAtender(PagingConfig config)
        {
            return TurnoDAO.ObtenerGridTurnoPorAtender(config);
        }
        public PagingResult<TurnoGRIDDTO> ObtenerGridTurnosEnEspera(PagingConfig config)
        {
            return TurnoDAO.ObtenerGridTurnosEnEspera(config);
        }


    }
}
