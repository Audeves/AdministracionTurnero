using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Utils
{
    /// <summary>
    /// Clase atributo que permite obtener correctamente parametros de tipo fecha en el controlador si estos se encuentran
    /// con un formato de fecha valido
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DateTimeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Nombre del parametro del controlador que se evaluara
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Aplicacion del filtro que basicamente comprueba si el parametro fue enviado, de ser asi realiza la conversion
        /// a un tipo fecha valido, sino el parametro se vuelve 'null'
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // se obtiene el valor del parametro
            string paramValue = filterContext.RequestContext.HttpContext.Request[Field];

            // si se especifico un valor, se realiza la conversion
            if (!string.IsNullOrWhiteSpace(paramValue))
            {
                filterContext.ActionParameters[Field] = DateTime.Parse(paramValue);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}