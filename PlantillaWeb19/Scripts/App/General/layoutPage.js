$(document).ready(function () {

    if (!window.ITSON) {
        window.ITSON = {};
    }

    window.ITSON.LayoutPage = function () {

        //Variable con la cual accederemos a los metodos del archivo Views/General/Mensajes.js
        var metodoMensajes = ITSON.Mensajes,
            utils = ITSON.Utils;

        //Funcion que se ejecutara al inicio
        function Inicio() {

            //Ejecuta la funcion encargada de obtener el menu
            ObtenerDatosUsuario();
            ObtenerMenuPorIdUsuario();
        }

        function ObtenerDatosUsuario() {
            var divCargando = metodoMensajes.Cargando("Cargando menú...");

            utils.ajaxRequest("GET", null, "Vistas", "ObtenerNombreUsuarioActivo", null, divCargando, function (response) {
                
                    $("#nombreUsuario").append(response);
                
            });
        }

        //Funcion para llamar al metodo ObtenerMenuPorIdUsuario del controlador Sitio
        function ObtenerMenuPorIdUsuario() {

            var divCargando = metodoMensajes.Cargando("Cargando menú...");

            utils.ajaxRequest("GET", null, "Vistas", "ObtenerMenu", null, divCargando, function (response) {
                if (response.Estatus == utils.respuestaEstatus.ok) {
                    $("#menu").append(response.Datos);
                } else if (true) {
                    $("#menu").empty();
                    metodoMensajes.Mensaje("Menú", response.Mensaje);
                }
            });
        }

        //Ejecuta la funcion de inicio
        Inicio();

    }();

});