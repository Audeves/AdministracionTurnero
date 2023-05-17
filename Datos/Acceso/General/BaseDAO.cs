using Entidades.General;
using Entidades.GridSupport;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.General
{
    public class BaseDAO<TEntity> where TEntity : EntityBase
    {

        public ContextoDataBase db { get; private set; }

        public BaseDAO(ContextoDataBase db)
        {
            this.db = db;
        }

        //Guardar una entidad
        public virtual void Guardar(TEntity entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    db.Set<TEntity>().Add(entity);
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new EntityException("Ocurrió un error al intentar guardar.", e);
            }
        }

        //Guardar una lista de entidades
        public virtual void Guardar(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (TEntity entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        db.Set<TEntity>().Add(entity);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new EntityException("Ocurrió un error al intentar guardar.", e);
            }
        }

        //Consultar entidad por id
        public virtual TEntity ConsultarPorId(long id)
        {
            return db.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        //Consultar entidad con relaciones por id
        public virtual TEntity ConsultarPorId(long id, params string[] relaciones)
        {
            DbQuery<TEntity> query = db.Set<TEntity>();
            foreach (string relacion in relaciones)
            {
                query = query.Include(relacion);
            }
            return query.FirstOrDefault(e => e.Id == id);
        }

        //Obtener toda la lista de entidades
        public virtual IList<TEntity> ObtenerTodos(params string[] relaciones)
        {
            DbQuery<TEntity> query = db.Set<TEntity>();

            foreach (string relacion in relaciones)
            {
                query = query.Include(relacion);
            }

            return query.ToList();
        }

        //Eliminar por id
        public virtual void Eliminar(long id)
        {
            TEntity entity = ConsultarPorId(id);
            if (entity != null)
            {
                db.Set<TEntity>().Remove(entity);
                db.SaveChanges();
            }
            else
            {
                throw new EntityException("El objeto a eliminar no existe.");
            }
        }

        //Eliminar por entidad cargada
        public virtual void Eliminar(TEntity entity)
        {
            if (entity != null)
            {
                db.Set<TEntity>().Remove(entity);
                db.SaveChanges();
            }
            else
            {
                throw new EntityException("El objeto a eliminar no existe.");
            }
        }

        //Eliminar lista de entidades
        public virtual void EliminarEntities<TEntity>(IList<TEntity> entities) where TEntity : EntityBase
        {
            foreach (TEntity entity in entities)
            {
                if (entities != null)
                {
                    db.Set<TEntity>().Remove(entity);
                }
            }
            db.SaveChanges();
        }

        //Crear objeto consulta
        protected virtual IQueryable<TEntity> CrearQuery(params string[] relationships)
        {
            IQueryable<TEntity> query = db.Set<TEntity>().AsQueryable();
            foreach (string relationship in relationships)
            {
                query = query.Include(relationship);
            }
            return query;
        }

        //Crear una consulta multiple con filtros
        public IEnumerable<TEntity> FindByFilters(IList<Expression<Func<TEntity, bool>>> filters, params string[] relationships)
        {
            IQueryable<TEntity> query = CrearQuery();
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
            foreach (string relation in relationships)
            {
                query = query.Include(relation);
            }
            return query.AsEnumerable();
        }

        //Crear consulta individual con filtros
        public TEntity FindUniqueByFilters(IList<Expression<Func<TEntity, bool>>> filters, params string[] relationships)
        {
            IQueryable<TEntity> query = CrearQuery();
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
            foreach (string relation in relationships)
            {
                query = query.Include(relation);
            }
            return query.AsEnumerable().SingleOrDefault();
        }

        //Crear una consulta individual con los filtros enviados, el objeto vuelve desenlazado por lo que puede clonarse facilmente
        public TEntity FindUniqueByFiltersDetached(IList<Expression<Func<TEntity, bool>>> filters, params string[] relationships)
        {
            IQueryable<TEntity> query = CrearQuery();
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
            foreach (string relation in relationships)
            {
                query = query.Include(relation);
            }
            return query.AsNoTracking().AsEnumerable().SingleOrDefault();
        }

        //Crear una lista de filtros vacia
        public virtual IList<Expression<Func<TEntity, bool>>> CrearListaFiltrosVacia()
        {
            return new List<Expression<Func<TEntity, bool>>>();
        }

        //Actualizar una o varias propiedades de una lista de elementos
        public void UpdateListProperty<TEntity>(List<TEntity> list, Action<TEntity> action) where TEntity : EntityBase
        {
            list.Where(p => p.Id == 0).ToList().ForEach(action);
        }

        /// <summary>
        /// Metodo para ejecutar una consulta paginada
        /// </summary>
        /// <typeparam name="TSelect">Tipo de objetos respuesta, debe ser un objeto GRIDDTO</typeparam>
        /// <param name="config">Configuracion del paginado</param>
        /// <param name="select">Seleccion de campos</param>
        /// <param name="filters">Lista de filtros a aplicar en la consulta</param>
        /// <returns>Objeto con lista paginada</returns>
        protected virtual PagingResult<TSelect> ExecConsultaPaginada<TSelect>(PagingConfig config, Expression<Func<TEntity, TSelect>> select, IList<Expression<Func<TEntity, bool>>> filters)
        {
            try
            {
                // resultado
                PagingResult<TSelect> result = new PagingResult<TSelect>();

                // query
                IQueryable<TEntity> query = CrearQuery();

                // aplicacion de filtros         

                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }

                // conteo total de registros
                result.TotalRows = query.LongCount();

                // ordenamiento default
                query = query.OrderByDescending(e => e.Id);


                // paginado
                query = query.Skip(config.PageNum * config.PageSize);
                query = query.Take(config.PageSize);

                // llenado del objeto resultado
                result.Rows = query.Select(select).ToList();

                // seleccion de campos            
                return result;
            }
            catch (Exception e)
            {
                throw new EntityException("Ocurrió un error al intentar realizar la consulta paginada", e);
            }
        }

        /// <summary>
        /// Metodo para ejecutar una consulta paginada
        /// </summary>
        /// <typeparam name="TSelect">Tipo de objetos respuesta, debe ser un objeto GRIDDTO</typeparam>
        /// <param name="config">Configuracion del paginado</param>
        /// <param name="select">Seleccion de campos</param>
        /// <param name="filters">Lista de filtros a aplicar en la consulta</param>
        /// <param name="order">Expresion para ordernar los registros</param>
        /// <param name="asc">Tipo de ordenamiento ascendente o descendente</param>
        /// <returns>Objeto con lista paginada</returns>
        protected virtual PagingResult<TSelect> ExecConsultaPaginadaOrdenada<TSelect, TOrderKey>(PagingConfig config, Expression<Func<TEntity, TSelect>> select,
            IList<Expression<Func<TEntity, bool>>> filters, Expression<Func<TEntity, TOrderKey>> order, bool asc = true)
        {
            try
            {
                // resultado
                PagingResult<TSelect> result = new PagingResult<TSelect>();

                // query
                IQueryable<TEntity> query = CrearQuery();

                // aplicacion de filtros            
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }

                // conteo total de registros
                result.TotalRows = query.LongCount();

                // ordenamiento personalizado
                if (asc)
                {
                    query = query.OrderBy(order);
                }
                else
                {
                    query = query.OrderByDescending(order);
                }

                // paginado
                query = query.Skip(config.PageNum * config.PageSize);
                query = query.Take(config.PageSize);

                // llenado del objeto resultado
                result.Rows = query.Select(select).ToList();

                // seleccion de campos            
                return result;
            }
            catch (Exception e)
            {
                throw new EntityException("Ocurrió un error al intentar realizar la consulta paginada", e);
            }
        }

        /// <summary>
        /// Metodo para ejecutar una consulta paginada
        /// </summary>
        /// <typeparam name="TSelect">Tipo de objetos respuesta, debe ser un objeto GRIDDTO</typeparam>
        /// <param name="config">Configuracion del paginado</param>
        /// <param name="select">Seleccion de campos</param>
        /// <param name="filters">Lista de filtros a aplicar en la consulta</param>
        /// <param name="order">Expresion para ordernar los registros</param>
        /// <param name="asc">Tipo de ordenamiento ascendente o descendente</param>
        /// <returns>Objeto con lista paginada</returns>
        protected virtual PagingResult<TSelect> ExecConsultaPaginadaOrdenadaDoble<TSelect, TOrderKey, TOrderThenKey>(PagingConfig config, Expression<Func<TEntity, TSelect>> select,
            IList<Expression<Func<TEntity, bool>>> filters, Expression<Func<TEntity, TOrderKey>> order, Expression<Func<TEntity, TOrderThenKey>> thenBy, bool asc = true)
        {
            try
            {
                // resultado
                PagingResult<TSelect> result = new PagingResult<TSelect>();

                // query
                IQueryable<TEntity> query = CrearQuery();

                // aplicacion de filtros            
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }

                // conteo total de registros
                result.TotalRows = query.LongCount();

                // ordenamiento personalizado
                if (asc)
                {
                    query = query.OrderBy(order).ThenBy(thenBy);
                }
                else
                {
                    query = query.OrderByDescending(order).ThenByDescending(thenBy);
                }

                // paginado
                query = query.Skip(config.PageNum * config.PageSize);
                query = query.Take(config.PageSize);

                // llenado del objeto resultado
                result.Rows = query.Select(select).ToList();

                // seleccion de campos            
                return result;
            }
            catch (Exception e)
            {
                throw new EntityException("Ocurrió un error al intentar realizar la consulta paginada", e);
            }
        }

    }
}
