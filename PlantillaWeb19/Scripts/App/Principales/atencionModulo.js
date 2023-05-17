$(function () {

    window.ITSON.AtencionModulo = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            divIniciarAtencion = $("#divIniciarAtencion"),
            frmIniciarAtencion = $("#frmIniciarAtencion"),
            cboModulo = $("#cboModulo"),
            btnIniciarAtencion = $("#btnIniciarAtencion"),


            divAtendiendo = $("#divAtendiendo"),
            gridTurnosPorAtender = $("#gridTurnosPorAtender"),
            gridTurnosEnEspera = $("#gridTurnosEnEspera"),

            btnTerminarAtencionVentanilla = $("#btnTerminarAtencionVentanilla")

            ;

        

        function init() {
            validarUsuarioNoAtendiendo();
            initControlesIniciarAtencion();
        }

        function validarUsuarioNoAtendiendo() {
            var dialogo = mensajes.Cargando("Preparando...");
            utils.ajaxRequest("GET", null, "AtencionModulo", "ValidarUsuarioNoAtendiendo", null, dialogo, function (response) {
                if (response == true) {
                    mostrarIniciarAtencion();
                } else {
                    initControlesAtendiendo();
                }
            });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        function initControlesIniciarAtencion() {
            cboModulo.jqxDropDownList({
                source: utils.generateComboAdapter("ModulosAtencion", "ObtenerModulosAtencionUsuarioActivo"),
                width: "99%",
                height: 42,
                theme: tema,
                autoDropDownHeight: true,
                placeHolder: utils.mensajes.seleccione,
                displayMember: "Label",
                valueMember: "Value"
            });

            frmIniciarAtencion.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#cboModulo", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList }
                ]
            });

            btnIniciarAtencion.click(function (e) {
                frmIniciarAtencion.jqxValidator("validate");
            });

            frmIniciarAtencion.on("validationSuccess", function (e) {
                var modulo = cboModulo.val();
                var dialogo = mensajes.Cargando("Iniciando...");

                utils.ajaxRequest("POST", null, "AtencionModulo", "IniciarAtencion", { idModulo: modulo }, dialogo, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        divIniciarAtencion.hide();
                        initControlesAtendiendo();
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Iniciar", response.Mensaje + ' ' + response.Datos);
                    }
                });
                
            });

            
        }

        function mostrarIniciarAtencion() {
            divIniciarAtencion.show();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        function initControlesAtendiendo() {
            debugger
            initGridTurnosPorAtender();
            initGridTurnosEnEspera();
            initEventosAtendiendo();
            divAtendiendo.show();

        }
        //Turnos por atender
        function initGridTurnosPorAtender() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "NombreCliente", type: "string" },
                { name: "NumeroTurno", type: "int" },
                { name: "NombreTramite", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridTurnosPorAtender, "AtencionModulo", "ObtenerGridTurnoPorAtender", fields);

            gridTurnosPorAtender.jqxGrid({
                width: "45%",
                height: 200,
                theme: tema,
                columnsresize: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                virtualmode: false,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                autoshowloadelement: false,
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Nombre", dataField: "NombreCliente", width: "60%" },
                    { text: "Turno", dataField: "NumeroTurno", width: "15%" },
                    { text: "Tramite", dataField: "NombreTramite", width: "35%" }
                ]
            });
            gridTurnosPorAtender.prepend('<h3 style="font-size: 1em;">Turnos por atender</h3>');
        }
        //Turnos en espera
        function initGridTurnosEnEspera() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "NombreCliente", type: "string" },
                { name: "NumeroTurno", type: "int" },
                { name: "NombreTramite", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridTurnosEnEspera, "AtencionModulo", "ObtenerGridTurnosEnEspera", fields);
            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnRetomarAtencion" class="btnRetomarAtencion btn btn-info">Retomar atención</button>';
            };

            gridTurnosEnEspera.jqxGrid({
                width: "50%",
                height: 200,
                theme: tema,
                columnsresize: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                virtualmode: false,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                autoshowloadelement: false,
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Nombre", dataField: "NombreCliente", width: "55%" },
                    { text: "Turno", dataField: "NumeroTurno", width: "15%" },
                    { text: "Tramite", dataField: "NombreTramite", width: "35%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "45%" }
                ]
            });
            gridTurnosEnEspera.prepend('<h3 style="font-size: 1em;">Turnos en espera</h3>');
        }

        function initEventosAtendiendo() {
            btnTerminarAtencionVentanilla.click(function (e) {
                var dialogo = mensajes.Cargando("Terminando...");

                utils.ajaxRequest("POST", null, "AtencionModulo", "FinalizarAtencion", null, dialogo, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        divAtendiendo.hide();
                        divIniciarAtencion.show();
                        mensajes.Mensaje("Terminar", response.Mensaje);
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Terminar", response.Mensaje);
                    }
                });
            });
        }



        init();

    }();


});