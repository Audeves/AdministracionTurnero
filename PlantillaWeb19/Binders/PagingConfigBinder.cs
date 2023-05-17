using Entidades.GridSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Binders
{
    /// <summary>
    /// Clase enlazadora para PagingConfig
    /// </summary>
    public class PagingConfigBinder : IModelBinder
    {
        /// <summary>
        /// Realiza el enlace de un objeto PagingConfig con los valores enviados en el formulario
        /// </summary>
        /// <param name="controllerContext">Contexto del controlador</param>
        /// <param name="bindingContext">Contexto del enlace</param>
        /// <returns>Objeto PagingConfig enlazado</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // se extra la informacion de la peticion
            HttpRequestBase request = controllerContext.HttpContext.Request;

            string pageNum = request.QueryString["pagenum"];
            string pageSize = request.QueryString["pagesize"];

            // se evaluan los parametros
            if (string.IsNullOrEmpty(pageNum))
            {
                throw new ArgumentException("Falta el parámetro 'pagenum', revise la configuración del jqxGrid");
            }

            if (string.IsNullOrEmpty(pageSize))
            {
                throw new ArgumentException("Falta el parámetro 'pagesize', revise la configuración del jqxGrid");
            }

            // creacion del objeto
            return new PagingConfig()
            {
                PageNum = int.Parse(pageNum),
                PageSize = int.Parse(pageSize)
            };
        }
    }
}