using Entidades.General;
using Entidades.GridSupport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Acceso.General
{
    public class SQLBaseDAO<TEntityBase>: BaseDAO<TEntityBase> where TEntityBase : EntityBase
    {

        public SQLBaseDAO(ContextoDataBase db) : base(db)
        {
        }

        public void ExecSPOperaciones(string storeProcedure, List<SqlParameter> parameters = null)
        {
            parameters = parameters ?? new List<SqlParameter>();
            string sql = CreateSPString(storeProcedure, parameters);
            db.Database.ExecuteSqlCommand(sql, parameters.ToArray());
        }

        public IList<TSQLEntity> ExecSPConsulta<TSQLEntity>(string storeProcedure, List<SqlParameter> parameters = null)
            where TSQLEntity : SQLEntityBase
        {
            parameters = parameters ?? new List<SqlParameter>();
            string sql = CreateSPString(storeProcedure, parameters);
            return db.Database.SqlQuery<TSQLEntity>(sql, parameters.ToArray()).ToList();
        }

        public TSQLEntity ExecSPConsultaUnico<TSQLEntity>(string storeProcedure, List<SqlParameter> parameters = null)
            where TSQLEntity : SQLEntityBase
        {
            parameters = parameters ?? new List<SqlParameter>();
            string sql = CreateSPString(storeProcedure, parameters);
            return db.Database.SqlQuery<TSQLEntity>(sql, parameters.ToArray()).SingleOrDefault();
        }

        public PagingResult<TSQLEntity> ExecSPConsultaPaginada<TSQLEntity>(PagingConfig config, string storeProcedure, List<SqlParameter> parameters)
            where TSQLEntity : SQLEntityBase
        {
            parameters = parameters ?? new List<SqlParameter>();

            // se agregan automaticamente los parametros de configuracion del paginado
            parameters.Insert(0, new SqlParameter("@pagina", config.PageNum));
            parameters.Insert(1, new SqlParameter("@numRegistros", config.PageSize));

            // se agrega como parametro de salida el numero total de registros para evitar realizar el query dos veces
            SqlParameter tipoParam = new SqlParameter("@totalRows", 0);
            tipoParam.Direction = ParameterDirection.Output;
            parameters.Insert(2, tipoParam);

            // se crea el objeto resultado
            PagingResult<TSQLEntity> result = new PagingResult<TSQLEntity>();

            // cadena de construccion de la consulta
            string sql = CreateSPString(storeProcedure, parameters, "@totalRows");

            result.Rows = db.Database.SqlQuery<TSQLEntity>(sql.ToString(), parameters.ToArray()).ToList();
            result.TotalRows = (long)tipoParam.Value;

            // ejecucion del query
            return result;
        }

        private string CreateSPString(string storeProcedure, List<SqlParameter> parameters, params string[] parametersOut)
        {
            // cadena de construccion de la consulta
            StringBuilder query = new StringBuilder(storeProcedure);

            // si no has listade parametros, se crea un arreglo vacio
            if (parameters == null)
            {
                parameters = new List<SqlParameter>();
            }

            // se itera el arreglo de parametros y se agregan a la consulta
            for (int i = 0; i < parameters.Count; i++)
            {
                SqlParameter param = parameters[i];
                if (i == 0)
                {
                    // si el parametro es de salida, se agrega la palabra out
                    if (parametersOut.Contains(param.ParameterName))
                    {
                        query.Append(" " + param.ParameterName + " out");
                    }
                    else
                    {
                        query.Append(" " + param.ParameterName);
                    }
                }
                else
                {
                    // si el parametro es de salida, se agrega la palabra out
                    if (parametersOut.Contains(param.ParameterName))
                    {
                        query.Append("," + param.ParameterName + " out");
                    }
                    else
                    {
                        query.Append("," + param.ParameterName);
                    }
                }
            }

            return query.ToString();
        }

    }
}
