using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantillaWeb19.Controllers.General
{
    //Separe todas las llamadas a vistas en este controlador para simplificar la especificacion de rutas
    //Todas las vistas no compartidas quedaron en ~/Views/Vistas/ asi que basta con poner la carpeta en que se encuentran y el nombre que tiene
    //en lugar de poner toda la ruta "~/Views/Vistas/General/Inicio.cshtml" pasa a ser "General/Inicio"
    //En caso de llamar a una vista compartida si es necesario poner toda la ruta.
    public class VistasController : BaseController //: BaseController
    {
        // GET: Vistas
        public ViewResult Inicio()
        {
            return View("General/Inicio");
        }

        public ViewResult Login()
        {
            ReiniciaConstantesSession();
            return View("General/Login");
        }

        public ViewResult Aviso()
        {
            ReiniciaConstantesSession();
            return View("General/Aviso");
        }

        public ViewResult SesionExpirada()
        {
            ReiniciaConstantesSession();
            return View("General/SesionExpirada");
        }







        public ViewResult Modulo()
        {
            return View("Administracion/Modulo");
        }

        public ViewResult Perfil()
        {
            return View("Administracion/Perfil");
        }

        public ViewResult Usuario()
        {
            return View("Administracion/Usuario");
        }

        public ViewResult Pantallas()
        {
            return View("Administracion/Pantallas");
        }




        public ViewResult AreasAtencion()
        {
            return View("Configuracion/AreasAtencion");
        }

        public ViewResult Procesos()
        {
            return View("Configuracion/Procesos");
        }

        public ViewResult Tramites()
        {
            return View("Configuracion/Tramites");
        }

        public ViewResult Turneros()
        {
            return View("Configuracion/Turneros");
        }

        public ViewResult RegistroPantallas()
        {
            return View("Configuracion/RegistroPantallas");
        }

        public ViewResult ModulosAtencion()
        {
            return View("Configuracion/ModulosAtencion");
        }




        public ViewResult AtencionModulo()
        {
            return View("Principales/AtencionModulo");
        }




        public ViewResult AdministracionSolicitudesCita()
        {
            return View("Citas/AdministracionSolicitudesCita");
        }

        public ViewResult AtencionCitasHoy()
        {
            return View("Citas/AtencionCitasHoy");
        }

    }
}