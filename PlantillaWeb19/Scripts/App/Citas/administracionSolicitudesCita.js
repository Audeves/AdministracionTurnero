$(function () {

    window.ITSON.AdministracionSolicitudesCita = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            tabOpciones = $("#tabOpciones"),

            cboEstatusSolicitudCita = $("#cboEstatusSolicitudCita"),
            dpkFechaDesde = $("#dpkFechaDesde"),
            dpkFechaHasta = $("#dpkFechaHasta"),
            btnConsultar = $("#btnConsultar"),
            btnExportar = $("#btnExportar"),

            //lblResultado = $("#lblResultado"),
            gridResultados = $("#gridResultados"),

            //Formulario
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
            btnTextoDefaultAgendar = $("#btnTextoDefaultAgendar"),
            btnTextoDefaultRechazar = $("#btnTextoDefaultRechazar"),

            btnAgendar = $("#btnAgendar"),
            btnRechazar = $("#btnRechazar"),
            btnRegistrarAsistencia = $("#btnRegistrarAsistencia"),
            btnRegresar = $("#btnRegresar"),

            //Citas hoy
            gridCitasHoy = $("#gridCitasHoy"),


            objetivoConsulta = null,
            opcionesObjetivoConsulta = {
                resultados: 1,
                citasHoy: 2
            },
            textosMensajes = {
                agregarComentarioCorreo: "Es necesario agregar un comentario",
                seleccionarEstatus: "Es necesario seleccionar al menos un estatus"
            },

            textoDefaultAgendar = "",
            textoDefaultRechazar = "",

            ultimoEstatusAlumno = null,


            //Bitacora
            cboTipoMovimientoCita = $("#cboTipoMovimientoCita"),
            dpkFechaDesdeBitacora = $("#dpkFechaDesdeBitacora"),
            dpkFechaHastaBitacora = $("#dpkFechaHastaBitacora"),
            btnConsultarBitacora = $("#btnConsultarBitacora"),
            btnExportarBitacora = $("#btnExportarBitacora"),
            gridResultadosBitacora = $("#gridResultadosBitacora"),


            //Cancelacion masiva
            dpkFechaDesdeCancelacion = $("#dpkFechaDesdeCancelacion"),
            dpkFechaHastaCancelacion = $("#dpkFechaHastaCancelacion"),

            chkPendientes = $("#chkPendientes"),
            chkAgendadas = $("#chkAgendadas"),

            rbtnSi = $("#rbtnSi"),
            rbtnNo = $("#rbtnNo"),

            txtComentariosCancelacion = $("#txtComentariosCancelacion"),

            btnCancelacionMasiva = $("#btnCancelacionMasiva"),
            btnRegresarCancelacionMasiva = $("#btnRegresarCancelacionMasiva")

            ;


        function init() {
            initControles();
            initCombos();
            initEventos();
            initGridResultados();
            initGridCitasHoy();
            initGridBitacora();
        }


        function initControles() {
            tabOpciones.jqxTabs({ width: "100%", position: "top", theme: tema, keyboardNavigation: false });
            //$(".text-input").jqxInput({ width: 250, height: 21, theme: tema });
            //$(".text-input-multiline").jqxInput({ width: 620, height: 60, theme: tema });
            dpkFechaDesde.jqxDateTimeInput({ width: 200, height: 21, theme: tema, culture: "es-MX", value: null });
            dpkFechaHasta.jqxDateTimeInput({ width: 200, height: 21, theme: tema, culture: "es-MX", value: null });

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


            //Bitacora
            dpkFechaDesdeBitacora.jqxDateTimeInput({ width: 200, height: 21, theme: tema, culture: "es-MX", value: null });
            dpkFechaHastaBitacora.jqxDateTimeInput({ width: 200, height: 21, theme: tema, culture: "es-MX", value: null });


            //Cancelacion masiva
            dpkFechaDesdeCancelacion.jqxDateTimeInput({ width: 200, height: 21, theme: tema, culture: "es-MX", value: null });
            dpkFechaHastaCancelacion.jqxDateTimeInput({ width: 200, height: 21, theme: tema, culture: "es-MX", value: null });

            chkPendientes.jqxCheckBox({ theme: tema, checked: true });
            chkAgendadas.jqxCheckBox({ theme: tema, checked: true });

            rbtnSi.jqxRadioButton({ theme: tema, groupName: "enviarCorreo", checked: true });
            rbtnNo.jqxRadioButton({ theme: tema, groupName: "enviarCorreo" });
        }

        function initCombos() {
            cboEstatusSolicitudCita.jqxDropDownList({
                source: utils.generateComboAdapter("CombosComunes", "EstatusSolicitudCita"),
                width: 200,
                height: 21,
                theme: tema,
                autoDropDownHeight: true,
                placeHolder: utils.mensajes.seleccione,
                displayMember: "Label",
                valueMember: "Value"
            });

            cboTipoMovimientoCita.jqxDropDownList({
                source: utils.generateComboAdapter("CombosComunes", "TipoMovimientoCita"),
                width: 200,
                height: 21,
                theme: tema,
                autoDropDownHeight: true,
                placeHolder: utils.mensajes.seleccione,
                displayMember: "Label",
                valueMember: "Value"
            });
        }

        function initEventos() {
            tabOpciones.on("selected", function (event) {
                var selectedTab = event.args.item;
                if (selectedTab == 0) {
                    objetivoConsulta = null;
                    updateGridResultados();
                    limpiar();
                    limpiarCancelacion();
                    //tabOpciones.jqxTabs("select", 0);
                    tabOpciones.jqxTabs("disableAt", 1);
                }
                if (selectedTab == 2) {
                    objetivoConsulta = null;
                    updateGridCitasHoy();
                    limpiar();
                    limpiarCancelacion();
                    tabOpciones.jqxTabs("disableAt", 1);
                }
                if (selectedTab == 2) {
                    limpiar();
                    limpiarCancelacion();
                    tabOpciones.jqxTabs("disableAt", 1);
                }
                if (selectedTab == 4) {
                    objetivoConsulta = null;
                    limpiar();
                    limpiarCancelacion();
                    tabOpciones.jqxTabs("disableAt", 1);
                }
            });

            btnConsultar.click(function (e) {
                gridResultados.jqxGrid("gotopage", 0);
                updateGridResultados();
            });

            btnExportar.click(function (e) {
                exportarExcel();
            });


            btnRegresar.click(function (e) {
                if (objetivoConsulta == opcionesObjetivoConsulta.resultados) {
                    //updateGridResultados();
                    limpiar();
                    tabOpciones.jqxTabs("disableAt", 1);
                    tabOpciones.jqxTabs("select", 0);
                } else if (objetivoConsulta == opcionesObjetivoConsulta.citasHoy) {
                    //updateGridCitasHoy();
                    limpiar();
                    tabOpciones.jqxTabs("disableAt", 1);
                    tabOpciones.jqxTabs("select", 2);
                }

            });

            btnRegresarCancelacionMasiva.click(function (e) {
                limpiarCancelacion();
                tabOpciones.jqxTabs("select", 0);
            });


            btnAgendar.click(function (e) {
                var comentarioCorreo = txtComentariosAdministrador.val();
                if (comentarioCorreo == "") {
                    mensajes.Mensaje("Agendar", textosMensajes.agregarComentarioCorreo);
                    return;
                }
                mensajes.MensajeConfirmacion("Agendar", "Se agendará la cita ¿desea continuar?", function () {
                    guardarCambios(utils.estatusCitas.agendada);
                });
            });

            btnRechazar.click(function (e) {
                var comentarioCorreo = txtComentariosAdministrador.val();
                if (comentarioCorreo == "") {
                    mensajes.Mensaje("Rechazar", textosMensajes.agregarComentarioCorreo);
                    return;
                }
                mensajes.MensajeConfirmacion("Rechazar", "Se rechazará la cita ¿desea continuar?", function () {
                    guardarCambios(utils.estatusCitas.cancelada);
                });
            });

            btnRegistrarAsistencia.click(function (e) {
                registrarAsistencia(hIdRegistro.val(), txtFolio.val());
            });

            btnTextoDefaultAgendar.click(function (e) {
                txtComentariosAdministrador.val(textoDefaultAgendar);
            });

            btnTextoDefaultRechazar.click(function (e) {
                txtComentariosAdministrador.val(textoDefaultRechazar);
            });



            //Bitacora
            btnConsultarBitacora.click(function (e) {
                gridResultadosBitacora.jqxGrid("gotopage", 0);
                updateGridBitacora();
            });


            //Cancelacion masiva
            rbtnSi.on("change", function (e) {
                txtComentariosCancelacion.val("");
                var checked = e.args.checked;
                if (checked == true) {
                    txtComentariosCancelacion.attr("readonly", false);
                } else {
                    txtComentariosCancelacion.attr("readonly", true);
                }
            });

            rbtnNo.on("change", function (e) {
                txtComentariosCancelacion.val("");
                var checked = e.args.checked;
                if (checked == true) {
                    txtComentariosCancelacion.attr("readonly", true);
                } else {
                    txtComentariosCancelacion.attr("readonly", false);
                }
            });

            btnCancelacionMasiva.click(function (e) {
                if (chkAgendadas.val() == false && chkPendientes.val() == false) {
                    mensajes.Mensaje("Error", textosMensajes.seleccionarEstatus);
                    return;
                }
                if (rbtnSi.val() == true && txtComentariosCancelacion.val() == "") {
                    mensajes.Mensaje("Error", textosMensajes.agregarComentarioCorreo);
                    return;
                }
                cancelacionMasiva();
            })
        }

        function initGridResultados() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "IdCita", type: "long" },
                { name: "EstatusStr", type: "string" },
                { name: "AsistioStr", type: "string" },
                { name: "FechaCitaStr", type: "string" },
                { name: "HoraCita", type: "string" },
                { name: "FechaCapturaStr", type: "string" },
                { name: "TramiteActual", type: "string" },
                { name: "NombreSolicitante", type: "string" },
                { name: "FechaCapturaStr", type: "string" },
                { name: "EmplId", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridResultados, "AdministracionSolicitudesCita", "ObtenerGridSolicitudCita", fields, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>';
            };

            gridResultados.jqxGrid({
                width: "99%",
                height: 400,
                theme: tema,
                columnsresize: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                pageable: true,
                virtualmode: true,
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
                    { text: "Nombre", dataField: "NombreSolicitante", width: "25%", filtertype: "textbox" },
                    { text: "Fecha y hora cita", dataField: "FechaCitaStr", width: "10%", filterable: false },
                    { text: "Asistió", dataField: "AsistioStr", width: "5%", filtertype: "textbox" },
                    { text: "Trámite", dataField: "TramiteActual", width: "10%", filtertype: "textbox" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "5%", filterable: false }
                ]
            });

            gridResultados.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridResultados);
                        consultar(row.Id, opcionesObjetivoConsulta.resultados);
                    }
                }
            });

            gridResultados.on("bindingcomplete", function (e) {
                var rows = gridResultados.jqxGrid('getrows').length;
                var item = cboEstatusSolicitudCita.jqxDropDownList("getSelectedItem");
                if (item != null) {
                    //var txt = "Se encontraron " + rows + " resultados con el estatus " + cboEstatusSolicitudCita.jqxDropDownList("getSelectedItem").label;
                    //lblResultado.text(txt);
                    btnExportar.show();
                }
            });

            gridResultados.on("filter", function (event) {
                // el parametro 'filter' evita que el txt de la fila de filtros pierda el foco cuando se esten filtrando los datos
                gridResultados.jqxGrid("updatebounddata", "filter");
                gridResultados.jqxGrid("clearselection");
            });
        }

        function getGridFilters(data) {
            return $.extend(data, {
                estatus: cboEstatusSolicitudCita.val(),
                fechaDesde: dpkFechaDesde.val(),
                fechaHasta: dpkFechaHasta.val()
            });
        }


        function updateGridResultados() {
            gridResultados.jqxGrid("updatebounddata");
            gridResultados.jqxGrid("clearselection");
            if (cboEstatusSolicitudCita.val() == utils.estatusCitas.pendiente) {
                gridResultados.jqxGrid("hidecolumn", "AsistioStr");
            } else {
                gridResultados.jqxGrid("showcolumn", "AsistioStr");
            }
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

            textoDefaultAgendar = "";
            textoDefaultRechazar = "";

            ultimoEstatusAlumno = null;
        }

        function consultar(id, objetivo) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", null, "AdministracionSolicitudesCita", "Consultar", { id: id }, dialogo, function (response) {
                setRegistro(response);
                tabOpciones.jqxTabs("enableAt", 1);
                tabOpciones.jqxTabs("select", 1);
                objetivoConsulta = objetivo;
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

            ultimoEstatusAlumno = registro.Estatus;

            if (registro.Estatus == utils.estatusCitas.pendiente) {
                btnAgendar.show();
                btnRechazar.show();
                btnTextoDefaultAgendar.show();
                btnTextoDefaultRechazar.show();
                txtComentariosAdministrador.attr("readonly", false);
            } else if (registro.Estatus == utils.estatusCitas.agendada && registro.TieneCitaProxima == true) {
                btnAgendar.hide();
                btnRechazar.show();
                btnTextoDefaultAgendar.hide();
                btnTextoDefaultRechazar.show();
                txtComentariosAdministrador.attr("readonly", false);
            } else {
                btnAgendar.hide();
                btnRechazar.hide();
                btnTextoDefaultAgendar.hide();
                btnTextoDefaultRechazar.hide();
                txtComentariosAdministrador.attr("readonly", true);
            }

            if (registro.Estatus == utils.estatusCitas.agendada && registro.Asistio == false && registro.TieneCitaHoy == true) {
                btnRegistrarAsistencia.show();
            } else {
                btnRegistrarAsistencia.hide();
            }

            textoDefaultAgendar = registro.ComentarioAceptar;
            textoDefaultRechazar = registro.ComentarioRechazar;
        }

        function exportarExcel() {
            utils.generateReport("Reportes", "SolicitudesCitas",
                {
                    estatus: cboEstatusSolicitudCita.val(),
                    fechaDesde: dpkFechaDesde.val(),
                    fechaHasta: dpkFechaHasta.val()
                });
        }


        function guardarCambios(estatus) {
            var dialogo = mensajes.Cargando("Guardando cambios...");
            var registro = { id: hIdRegistro.val(), estatus: estatus, comentariosAdministrador: txtComentariosAdministrador.val(), estatusAnterior: ultimoEstatusAlumno };
            utils.ajaxRequest("POST", null, "AdministracionSolicitudesCita", "GuardarRevisionAdministrador", registro, dialogo, function (response) {
                if (response.Estatus == utils.respuestaEstatus.ok) {
                    mensajes.Mensaje("Guardar", response.Mensaje);
                    //updateGridResultados();
                    limpiar();
                    tabOpciones.jqxTabs("disableAt", 1);
                    tabOpciones.jqxTabs("select", 0);
                } else if (response.Estatus == utils.respuestaEstatus.error) {
                    mensajes.Mensaje("Guardar", response.Mensaje + " " + response.Datos);
                }
            });
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
                        consultar(row.Id, opcionesObjetivoConsulta.citasHoy);
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
                        tabOpciones.jqxTabs("select", 2);
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Guardar", response.Mensaje + " " + response.Datos);
                    }
                });
            });
        }









        //Bitacora 

        function initGridBitacora() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "FechaEmisionStr", type: "string" },
                { name: "TipoMovimientoStr", type: "string" },
                { name: "Usuario", type: "string" },
                { name: "NombreSolicitante", type: "string" },
                { name: "FechaCitaStr", type: "string" },
                { name: "Tramite", type: "string" },
                { name: "EmplId", type: "string" },
                { name: "Folio", type: "string" },
            ],
                dataAdapter = utils.generateGridAdapter(gridResultadosBitacora, "AdministracionSolicitudesCita", "ObtenerGridBitacoraMovimientos", fields, getGridFiltersBitacora);
            

            gridResultadosBitacora.jqxGrid({
                width: "99%",
                height: 400,
                theme: tema,
                columnsresize: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                pageable: true,
                virtualmode: true,
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
                    { text: "Fecha movimiento", dataField: "FechaEmisionStr", width: "15%", filterable: false },
                    { text: "Movimiento", dataField: "TipoMovimientoStr", width: "15%", filtertype: "textbox" },
                    { text: "Usuario", dataField: "Usuario", width: "15%", filtertype: "textbox" },
                    { text: "Nombre solicitante", dataField: "NombreSolicitante", width: "20%", filtertype: "textbox" },
                    { text: "Fecha y hora cita", dataField: "FechaCitaStr", width: "15%", filterable: false },
                    { text: "Tramite", dataField: "Tramite", width: "20%", filtertype: "textbox" },
                ]
            });

            gridResultadosBitacora.on("filter", function (event) {
                // el parametro 'filter' evita que el txt de la fila de filtros pierda el foco cuando se esten filtrando los datos
                gridResultadosBitacora.jqxGrid("updatebounddata", "filter");
                gridResultadosBitacora.jqxGrid("clearselection");
            });
        }

        function getGridFiltersBitacora(data) {
            return $.extend(data, {
                tipoMovimiento: cboTipoMovimientoCita.val(),
                fechaDesde: dpkFechaDesdeBitacora.val(),
                fechaHasta: dpkFechaHastaBitacora.val()
            });
        }

        function updateGridBitacora() {
            gridResultadosBitacora.jqxGrid("updatebounddata");
            gridResultadosBitacora.jqxGrid("clearselection");
        }






        //Cancelacion masiva

        function limpiarCancelacion() {
            dpkFechaDesdeCancelacion.val(null);
            dpkFechaHastaCancelacion.val(null);
            chkPendientes.val(true);
            chkAgendadas.val(true);
            rbtnSi.jqxRadioButton({ checked: true });
            txtComentariosCancelacion.val("");
        }

        function getCancelacionMasiva() {
            return {
                fechaDesde: dpkFechaDesdeCancelacion.val(),
                fechaHasta: dpkFechaHastaCancelacion.val(),
                pendientes: chkPendientes.val(),
                agendadas: chkAgendadas.val(),
                enviarCorreo: rbtnSi.val(),
                comentarioCancelacion: txtComentariosCancelacion.val()
            }
        }

        function cancelacionMasiva() {
            mensajes.MensajeConfirmacion("Cancelación", "Se cancelarán todas las citas que se encuentren según la configuración llenada, ¿desea continuar?", function () {
                var dialogo = mensajes.Cargando("Cancelando citas...");
                var configuracion = getCancelacionMasiva();
                utils.ajaxRequest("POST", null, "AdministracionSolicitudesCita", "CancelacionMasiva", configuracion, dialogo, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        limpiarCancelacion();
                        mensajes.Mensaje("Cancelación", response.Mensaje);
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Error", response.Mensaje + " " + response.Datos);
                    }
                });
            });
        }

        init();
    }();

});