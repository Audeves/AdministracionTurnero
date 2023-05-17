$(function () {

    window.ITSON.Turneros = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridTurneros = $("#gridTurneros"),

            modalTurneros = $("#modalTurneros"),
            tituloModal = $("#tituloModal"),
            frmTurneros = $("#frmTurneros"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            txtIP = $("#txtIP"),
            cboCampus = $("#cboCampus"),
            gridAreasAtencion = $("#gridAreasAtencion"),
            chkActivo = $("#chkActivo"),
            chkId = $("#chkId"),
            chkNombre = $("#chkNombre"),

            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),

            readyGridAreasAtencion = false,
            accionModalActual = null

            ;


        function init() {
            initControles();
            initGrid();
            initGridAreasAtencion();
            initEventos();
            initValidaciones();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });
            chkId.jqxCheckBox({ theme: tema });
            chkNombre.jqxCheckBox({ theme: tema });

            cboCampus.jqxDropDownList({
                source: utils.generateComboAdapter("CombosComunes", "Campus"),
                width: "99%",
                height: 42,
                theme: tema,
                autoDropDownHeight: true,
                placeHolder: utils.mensajes.seleccione,
                displayMember: "Label",
                valueMember: "Value"
            });
        }

        function initValidaciones() {
            frmTurneros.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#txtIP", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboCampus", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList },
                    { input: "#gridAreasAtencion", action: "select", message: "Debes seleccionar al menos un área de atención", rule: utils.ruleJqxGridSelectedRows(gridAreasAtencion) }
                ]
            });
        }


        function initGrid() {
            var campos = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Campus", type: "string" },
                { name: "AreasAtencionStr", type: "string" },
                { name: "Activo", type: "bool" },
                { name: "ActivoStr", type: "string" },
                { name: "SolicitarId", type: "bool" },
                { name: "SolicitarIdStr", type: "string" },
                { name: "SolicitarNombre", type: "bool" },
                { name: "SolicitarNombreStr", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridTurneros, "Turneros", "ObtenerGridConfiguracionesTurneros", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };


            gridTurneros.jqxGrid({
                width: "99%",
                height: 200,
                theme: tema,
                columnsresize: true,
                pageable: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                virtualmode: true,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                pagesize: 4,
                pagesizeoptions: ['4'],
                autoshowloadelement: false,
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Nombre", dataField: "Nombre", width: "20%" },
                    { text: "Campus", dataField: "Campus", width: "15%" },
                    { text: "Areas de atencion", dataField: "AreasAtencionStr", width: "35%" },
                    { text: "Id", dataField: "SolicitarIdStr", width: "5%" },
                    { text: "Nombre", dataField: "SolicitarNombreStr", width: "5%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridTurneros.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridTurneros);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridTurneros);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
            });
        }


        function getGridFilters(data) {
            return $.extend(data, {
            });
        }

        function initGridAreasAtencion() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapterSync(gridAreasAtencion, "AreasAtencion", "ObtenerAreasAtencionPorCampus", fields, getGridFiltersAreasAtencion);


            gridAreasAtencion.jqxGrid({
                width: "99%",
                height: 300,
                theme: tema,
                pageable: false,
                virtualmode: false,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                selectionmode: "checkbox",
                //autoshowloadelement: false,
                ready: function (e) {
                    readyGridAreasAtencion = true;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Todos", dataField: "Nombre", width: "90%" }
                ]
            });
        }

        function getGridFiltersAreasAtencion(data) {
            return $.extend(data, {
                campus_ps: cboCampus.val()
            });
        }


        function initEventos() {

            btnNuevo.click(function () {
                abrirModal(utils.accionModal.nuevo);
            });

            frmTurneros.on("validationSuccess", function (e) {
                var registro = getConfiguracionTurnero();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "Turneros", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            modalTurneros.modal("hide");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGrid();
                            limpiar();
                        } else if (response.Estatus == utils.respuestaEstatus.error) {
                            mensajes.Mensaje("Guardar", response.Mensaje);
                        }
                    });
                }
            });

            cboCampus.on("change", function (e) {
                var args = e.args.item.originalItem;
                if (args) {
                    actualizarGridAreasAtencion();
                }
            });
        }

        function abrirModal(accion, row) {
            limpiar();
            accionModalActual = accion;
            //var tituloModal = accion + " modulo";
            //var controlesAccionesEtiquetas = [ btnAccion, tituloModal ];
            var controlesValores = [txtNombre, txtIP, chkActivo, chkId, chkNombre];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");

            gridAreasAtencion.jqxGrid({ disabled: false });
            chkActivo.jqxCheckBox({ disabled: false });
            cboCampus.jqxDropDownList({ disabled: false });
            chkId.jqxCheckBox({ disabled: false });
            chkNombre.jqxCheckBox({ disabled: false });

            utils.onModalCreating(accion, row, btnAccion, tituloModal, controlesValores);

            if (accion != utils.accionModal.detalle) {
                btnAccion.unbind();
                btnAccion.click(function (e) {
                    guardar();
                });
            }

            if (accion == utils.accionModal.detalle) {
                //gridResponsables.jqxGrid({ disabled: true });
                chkActivo.jqxCheckBox({ disabled: true });
                //cboCampus.jqxDropDownList({ disabled: true });
                chkId.jqxCheckBox({ disabled: true });
                chkNombre.jqxCheckBox({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }


            modalTurneros.modal("show");
        }


        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Turneros", "Consultar", { id: id }, dialogo, function (response) {
                setConfiguracionTurnero(response);
            });
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            txtIP.val("");
            chkActivo.val(0);
            cboCampus.jqxDropDownList("clearSelection");
            frmTurneros.jqxValidator("hide");
            chkId.val(0);
            chkNombre.val(0);
            if (readyGridAreasAtencion) {
                gridAreasAtencion.jqxGrid("clearselection");
            }
        }

        function guardar() {
            frmTurneros.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridTurneros.jqxGrid("updatebounddata");
        }

        function actualizarGridAreasAtencion() {
            gridAreasAtencion.jqxGrid("updatebounddata");
        }

        function setConfiguracionTurnero(turnero) {
            hidId.val(turnero.Id);
            txtNombre.val(turnero.Nombre);
            txtIP.val(turnero.IP);
            utils.setDropDownListValue(cboCampus, turnero.CampusPS);
            utils.selectRows(gridAreasAtencion, turnero.IdsAreasAtencion);
            chkActivo.val(turnero.Activo);
            chkId.val(turnero.SolicitarId);
            chkNombre.val(turnero.SolicitarNombre);
        }

        function getConfiguracionTurnero() {
            var areasSeleccionadas = utils.getRowsSelected(gridAreasAtencion),
                areas = [];

            for (var i = 0; i < areasSeleccionadas.length; i++) {
                areas.push({ Id: areasSeleccionadas[i].uid });
            }

            return {
                turnero: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    IP: txtIP.val(),
                    CampusPS: cboCampus.jqxDropDownList("getSelectedItem").value,
                    CampusDescr: cboCampus.jqxDropDownList("getSelectedItem").label,
                    Activo: chkActivo.val(),
                    SolicitarId: chkId.val(),
                    SolicitarNombre: chkNombre.val(),
                    AreasAtencion: areas
                }
            }
        }



        init();
    }();


});