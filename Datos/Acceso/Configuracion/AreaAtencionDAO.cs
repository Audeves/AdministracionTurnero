using Datos.Acceso.General;
using Entidades.Entidades.Administracion;
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
    public class AreaAtencionDAO : BaseDAO<AreaAtencion>
    {

        public AreaAtencionDAO(ContextoDataBase db) : base(db)
        {

        }


        public new void Guardar(AreaAtencion areaAtencion)
        {
            AreaAtencion areaAtencionGuardada = this.ConsultarPorId(areaAtencion.Id, "Responsables");
            if (areaAtencionGuardada == null)
            {
                areaAtencionGuardada = new AreaAtencion();
            }

            areaAtencionGuardada.Nombre = areaAtencion.Nombre;
            areaAtencionGuardada.CampusPS = areaAtencion.CampusPS;
            areaAtencionGuardada.CampusDescr = areaAtencion.CampusDescr;
            areaAtencionGuardada.Activo = areaAtencion.Activo;

            areaAtencionGuardada.DisponibleCitas = areaAtencion.DisponibleCitas;
            areaAtencionGuardada.ResponsablePermisoCitas = areaAtencion.ResponsablePermisoCitas;
            areaAtencionGuardada.LugarCitas = areaAtencion.LugarCitas;
            areaAtencionGuardada.CorreoAgendada = areaAtencion.CorreoAgendada;
            areaAtencionGuardada.CorreoCancelada = areaAtencion.CorreoCancelada;
            areaAtencionGuardada.CorreoRemitenteCitas = areaAtencion.CorreoRemitenteCitas;

            var idsResponsablesSeleccionados = areaAtencion.Responsables.Select(p => p.Id).ToList();
            var idsResponsablesGuardados = areaAtencionGuardada.Responsables.Select(p => p.Id).ToList();
            areaAtencionGuardada.Responsables.RemoveAll(p => !idsResponsablesSeleccionados.Contains(p.Id));
            areaAtencionGuardada.Responsables.AddRange(areaAtencion.Responsables.Where(p => !idsResponsablesGuardados.Contains(p.Id)));

            foreach (Usuario item in areaAtencionGuardada.Responsables)
            {
                db.Usuario.Attach(item);
            }

            base.Guardar(areaAtencionGuardada);
        }

        public void ActualizarTurneroAreaAtencion(ConfiguracionTurnero turneroGuardado, ConfiguracionTurnero turnero)
        {
            List<long> idsAreasAtencionGuardadas = turneroGuardado.AreasAtencion.Select(ag => ag.Id).ToList();
            List<long> idsAreasAtencionSeleccionadas = turnero.AreasAtencion.Select(a => a.Id).ToList();

            List<long> quitar = idsAreasAtencionGuardadas.Where(ag => !idsAreasAtencionSeleccionadas.Contains(ag)).ToList();
            List<long> agregar = idsAreasAtencionSeleccionadas.Where(a => !idsAreasAtencionGuardadas.Contains(a)).ToList();

            foreach (long item in quitar)
            {
                AreaAtencion areaAtencionGuardada = this.ConsultarPorId(item);
                areaAtencionGuardada.IdConfiguracionTurnero = null;
                base.Guardar(areaAtencionGuardada);
            }

            foreach (long item in agregar)
            {
                AreaAtencion areaAtencionGuardada = this.ConsultarPorId(item);
                areaAtencionGuardada.IdConfiguracionTurnero = turneroGuardado.Id;
                base.Guardar(areaAtencionGuardada);
            }
        }

        public void ActualizarPantallaAreaAtencion(ConfiguracionPantalla pantallaGuardada, ConfiguracionPantalla pantalla)
        {
            List<long> idsAreasAtencionGuardadas = pantallaGuardada.AreasAtencion.Select(ag => ag.Id).ToList();
            List<long> idsAreasAtencionSeleccionadas = pantalla.AreasAtencion.Select(a => a.Id).ToList();

            List<long> quitar = idsAreasAtencionGuardadas.Where(ag => !idsAreasAtencionSeleccionadas.Contains(ag)).ToList();
            List<long> agregar = idsAreasAtencionSeleccionadas.Where(a => !idsAreasAtencionGuardadas.Contains(a)).ToList();

            foreach (long item in quitar)
            {
                AreaAtencion areaAtencionGuardada = this.ConsultarPorId(item);
                areaAtencionGuardada.IdConfiguracionPantalla = null;
                base.Guardar(areaAtencionGuardada);
            }

            foreach (long item in agregar)
            {
                AreaAtencion areaAtencionGuardada = this.ConsultarPorId(item);
                areaAtencionGuardada.IdConfiguracionPantalla = pantallaGuardada.Id;
                base.Guardar(areaAtencionGuardada);
            }
        }


        public PagingResult<AreaAtencionGRIDDTO> ObtenerGridAreasAtencion(PagingConfig config)
        {
            Expression<Func<AreaAtencion, AreaAtencionGRIDDTO>> select = a => new AreaAtencionGRIDDTO()
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Campus = a.CampusDescr,
                Activo = a.Activo,
                Responsables = a.Responsables,
                DisponibleCitas = a.DisponibleCitas != null ? a.DisponibleCitas.Value : false ,
            };

            var filtros = CrearListaFiltrosVacia();

            return ExecConsultaPaginada(config, select, filtros);
        }

        public List<AreaAtencion> ObtenerAreasAtencionPorCampus(string campus_ps)
        {
            IQueryable<AreaAtencion> areas = CrearQuery();
            return areas.Where(a => a.CampusPS == campus_ps).ToList();
        }

    }
}
