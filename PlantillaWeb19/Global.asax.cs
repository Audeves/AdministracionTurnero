using Entidades.GridSupport;
using Negocio.General;
using PlantillaWeb19.App_Start;
using PlantillaWeb19.Binders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PlantillaWeb19
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ColeccionConfig.RegistrarColecciones(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(PagingConfig), new PagingConfigBinder());

            string conexion = ConfigurationManager.ConnectionStrings["bdConexion2"].ConnectionString;
            InicioNegocio inicioNegocio = new InicioNegocio(conexion);
        }
    }
}
