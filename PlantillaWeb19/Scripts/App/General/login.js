$(document).ready(function () {

    if (!window.ITSON) {
        window.ITSON = {};
    }

    window.ITSON.Login = function () {

        var utils = ITSON.Utils,
            mensajes = ITSON.Mensajes,
            frmAcceder = $("#frmAcceder"),
            txtUsuario = $("#txtUsuario"),
            txtContraseña = $("#txtContraseña"),
            btnIniciar = $("#btnIniciar");

        //Funcion que se ejecutara al inicio
        function inicio() {
            Eventos();
            Validacion();
        }

        //Funcion que contiene los eventos como clicks, teclas, etc.
        function Eventos() {
            btnIniciar.click(function (e) {
                validarForm();
            });

            txtUsuario.keyup(function (e) {
                if (e.keyCode == 13) {
                    validarForm();
                }
            });

            txtContraseña.keyup(function (e) {
                if (e.keyCode == 13) {
                    validarForm();
                }
            });
        }

        function validarForm() {
            frmAcceder.valid() ? IniciarSesion() : null;
        }

        //Funcion para llevar el control de las validaciones de inicio de sesion
        function Validacion() {
            frmAcceder.validate({
                rules: {
                    txtUsuario: {
                        required: true,
                        maxlength: 50
                    },
                    txtContraseña: {
                        required: true,
                        maxlength: 50
                    }
                },
                messages: {
                    txtUsuario: {
                        required: "Ingrese el usuario",
                        maxlength: "Tu usuario debe de ser como máximo de 50 caracteres"
                    },
                    txtContraseña: {
                        required: "Ingrese la contraseña",
                        maxlength: "Tu contraseña debe de ser como máximo de 50 caracteres"
                    }
                }
            });
        }

        //Funcion para llamar al metodo ObtenerInicioSesion del controlador Inicio
        function IniciarSesion() {
            var datos = getLoginInfo();
            var dialogo = mensajes.Cargando("Cargando su información...");

            utils.ajaxRequest("GET", null, "Inicio", "IniciarSesion", datos, dialogo, function (response) {
                if (response.Estatus == utils.respuestaEstatus.ok) {
                    utils.redirect("Vistas", "Inicio");
                } else if (true) {
                    mensajes.Mensaje("Iniciar sesión", response.Mensaje);
                }
            });
        }

        function getLoginInfo() {
            return {
                cuenta: txtUsuario.val(),
                password: txtContraseña.val()
            };
        }

        inicio();

    }();

});