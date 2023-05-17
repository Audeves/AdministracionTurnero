$(function () {

    window.ITSON.AtencionCitasHoy = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            tabOpciones = $("#tabOpciones"),

            gridCitasHoy = $("#gridCitasHoy"),

            frmDetalle = $("#frmDetalle"),
            hIdRegistro = $("#hIdRegistro"),
            txtFechaHoraCita = $("#txtFechaHoraCita"),
            txtEstatus = $("#txtEstatus"),
            txtFolio = $("#txtFolio"),
            txtNombreSolicitante = $("#txtNombreSolicitante"),
            txtEmplId = $("#txtEmplId"),
            txtTramite = $("#txtTramite"),
            txtEmail = $("#txtEmail"),
            txtTelefono = $("#txtTelefono"),

            txtNombrePersonaAutorizada = $("#txtNombrePersonaAutorizada"),
            txtParentezco = $("#txtParentezco"),
            txtComentariosAdicionales = $("#txtComentariosAdicionales"),

            txtComentariosAdministrador = $("#txtComentariosAdministrador"),
            btnRegistrarAsistencia = $("#btnRegistrarAsistencia"),
            btnRegresar = $("#btnRegresar")

            ;

        function init() {
            initControles();
            initEventos();
            initGridCitasHoy();
        }

        function initControles() {
            tabOpciones.jqxTabs({ width: "100%", position: "top", theme: tema, keyboardNavigation: false });

            tabOpciones.jqxTabs("disableAt", 1);

            txtFechaHoraCita.attr("readonly", true);
            txtNombreSolicitante.attr("readonly", true);
            txtEmplId.attr("readonly", true);
            txtTramite.attr("readonly", true);
            txtEmail.attr("readonly", true);
            txtTelefono.attr("readonly", true);
            txtNombrePersonaAutorizada.attr("readonly", true);
            txtParentezco.attr("readonly", true);
            txtComentariosAdicionales.attr("readonly", true);
            txtComentariosAdministrador.attr("readonly", true);
        }


        function initEventos() {
            tabOpciones.on("selected", function (event) {
                var selectedTab = event.args.item;
                if (selectedTab == 0) {
                    updateGridCitasHoy();
                    limpiar();
                    limpiarCancelacion();
                    tabOpciones.jqxTabs("disableAt", 1);
                }
            });

            btnRegresar.click(function (e) {
                limpiar();
                tabOpciones.jqxTabs("disableAt", 1);
                tabOpciones.jqxTabs("select", 0);
            });

            btnRegistrarAsistencia.click(function (e) {
                registrarAsistencia(hIdRegistro.val(), txtFolio.val());
            });
        }

        function limpiar() {
            hIdRegistro.val("");
            txtFechaHoraCita.val("");
            txtEstatus.val("");
            txtNombreSolicitante.val("");
            txtEmplId.val("");
            txtTramite.val("");
            txtEmail.val("");
            txtTelefono.val("");
            txtNombrePersonaAutorizada.val("");
            txtParentezco.val("");
            txtComentariosAdicionales.val("");
            txtComentariosAdministrador.val("");
        }

        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", null, "AdministracionSolicitudesCita", "Consultar", { id: id }, dialogo, function (response) {
                setRegistro(response);
                tabOpciones.jqxTabs("enableAt", 1);
                tabOpciones.jqxTabs("select", 1);
            });
        }

        function setRegistro(registro) {
            hIdRegistro.val(registro.Id);
            txtFechaHoraCita.val(registro.FechaCitaStr);
            txtEstatus.val(registro.EstatusStr);
            txtFolio.val(registro.Folio);
            txtNombreSolicitante.val(registro.NombreSolicitante);
            txtEmplId.val(registro.EmplId);
            txtTramite.val(registro.TramiteActual);
            txtEmail.val(registro.Email);
            txtTelefono.val(registro.Telefono);
            txtNombrePersonaAutorizada.val(registro.PersonaAutorizada);
            txtParentezco.val(registro.Parentezco);
            txtComentariosAdicionales.val(registro.ComentariosAdicionales);
            txtComentariosAdministrador.val(registro.ComentariosAdministrador);
            
            if (registro.Estatus == utils.estatusCitas.agendada && registro.Asistio == false && registro.TieneCitaHoy == true) {
                btnRegistrarAsistencia.show();
            } else {
                btnRegistrarAsistencia.hide();
            }
        }

        function initGridCitasHoy() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "AsistioStr", type: "string" },
                { name: "FechaCitaStr", type: "string" },
                { name: "HoraCita", type: "string" },
                { name: "TramiteActual", type: "string" },
                { name: "NombreSolicitante", type: "string" },
                { name: "EmplId", type: "string" },
                { name: "PersonaAutorizada", type: "string" },
                { name: "Parentezco", type: "string" },
                { name: "Folio", type: "string" },
            ],
                dataAdapter = utils.generateGridAdapter(gridCitasHoy, "AdministracionSolicitudesCita", "ObtenerListaGridCitasHoy", fields);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnRegistrarAsistencia" class="btnEditar btn btn-success">Registrar asistencia</button>';
            };

            gridCitasHoy.jqxGrid({
                width: "99%",
                height: 400,
                theme: tema,
                columnsresize: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                virtualmode: false,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                pagesize: 10,
                pagesizeoptions: ['5', '10', '15', '20'],
                autoshowloadelement: true,
                filterable: true,
                showfilterrow: true,
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Id cita", dataField: "IdCita", hidden: true },
                    { text: "ID", dataField: "EmplId", width: "10%", filtertype: "textbox" },
                    { text: "Nombre", dataField: "NombreSolicitante", width: "20%", filtertype: "textbox" },
                    { text: "Fecha y hora cita", dataField: "FechaCitaStr", width: "10%", filterable: false },
                    { text: "Persona autorizada", dataField: "PersonaAutorizada", width: "20%", filtertype: "textbox" },
                    { text: "Trámite", dataField: "TramiteActual", width: "10%", filtertype: "textbox" },
                    { text: "Folio", dataField: "Folio", width: "14%", filtertype: "textbox" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "15%", filterable: false }
                ]
            });


            gridCitasHoy.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridCitasHoy);
                        consultar(row.Id);
                    } else if (objetivo.id == "btnRegistrarAsistencia") {
                        var row = utils.getRowSelected(gridCitasHoy);
                        registrarAsistencia(row.Id, row.Folio)
                    }
                }
            });
        }

        function updateGridCitasHoy() {
            gridCitasHoy.jqxGrid("updatebounddata");
            gridCitasHoy.jqxGrid("clearselection");
        }

        function registrarAsistencia(id, folio) {
            mensajes.MensajeConfirmacion("Asistencia", "Se registrará la asistencia del folio " + folio + ", ¿desea continuar?", function () {
                var dialogo = mensajes.Cargando("Guardando cambios...");
                utils.ajaxRequest("POST", null, "AdministracionSolicitudesCita", "RegistrarAsistencia", { id: id }, dialogo, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        mensajes.Mensaje("Asistencia", response.Mensaje);
                        updateGridCitasHoy();
                        limpiar();
                        tabOpciones.jqxTabs("disableAt", 1);
                        tabOpciones.jqxTabs("select", 0);
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Guardar", response.Mensaje + " " + response.Datos);
                    }
                });
            });
        }



        init();
    }();

});