$(function () {

    window.ITSON.Pantallas = function () {

        var utils = ITSON.Utils,
            mensajes = ITSON.Mensajes,
            tema = utils.getTema(),
            btnNuevo = $("#btnNuevo"),
            gridPantallas = $("#gridPantallas"),
            modalPantallas = $("#modalPantallas"),
            tituloModal = $("#tituloModal"),
            frmPantallas = $("#frmPantallas"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            txtControlador = $("#txtControlador"),
            txtAccion = $("#txtAccion"),
            txtClaseIcono = $("#txtClaseIcono"),
            cboModulo = $("#cboModulo"),
            cboSubmodulo = $("#cboSubmodulo"),
            chkActivo = $("#chkActivo"),
            chkPrivado = $("#chkPrivado"),
            gridAcciones = $("#gridAcciones"),

            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),

            readyGridAcciones = false;

        function init() {
            initControles();
            initGrid();
            initGridAcciones();
            initEventos();
            initValidaciones();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });
            chkPrivado.jqxCheckBox({ theme: tema });

            cboModulo.jqxDropDownList({
                source: utils.generateComboAdapter("CombosComunes", "Modulos"),
                width: "99%",
                height: 42,
                theme: tema,
                autoDropDownHeight: true,
                placeHolder: utils.mensajes.seleccione,
                displayMember: "Label",
                valueMember: "Value"
            });

            cboSubmodulo.jqxDropDownList({
                source: utils.generateComboAdapter("CombosComunes", "Submodulos"),
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
            frmPantallas.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#txtControlador", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#txtAccion", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#gridAcciones", action: "select", message: "Debes agregar al menos una acción", rule: utils.ruleJqxGridHasRows(gridAcciones) }
                ]
            });
        }

        function initEventos() {
            btnNuevo.click(function (e) {
                abrirModal(utils.accionModal.nuevo);
            });

            btnAccion.click(function (e) {
                guardar();
            });

            cboModulo.on("select", function (e) {
                var args = e.args;
                if (args) {
                    cboSubmodulo.jqxDropDownList("clearSelection");
                }
            });

            cboSubmodulo.on("select", function (e) {
                var args = e.args;
                if (args) {
                    cboModulo.jqxDropDownList("clearSelection");
                }
            });

            frmPantallas.on("validationSuccess", function (e) {
                var modulo = cboModulo.jqxDropDownList('selectedIndex'),
                submodulo = cboSubmodulo.jqxDropDownList('selectedIndex');

                if (!(modulo >= 0 || submodulo >= 0)) {
                    mensajes.Mensaje("Error", "Es necesario seleccionar un módulo o un submódulo");
                    return;
                }

                var registro = getPantalla();
                if (registro) {
                    var dialogo = mensajes.Cargando("Guardando registro");
                    utils.ajaxRequest("POST", null, "Pantallas", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            modalPantallas.modal("hide");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGrid();
                            limpiar();
                        } else if (response.Estatus == utils.respuestaEstatus.error) {
                            mensajes.Mensaje("Guardar", response.Mensaje);
                        }
                    });
                }
            });
        }

        function initGrid() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Controlador", type: "string" },
                { name: "Accion", type: "string" },
                { name: "PrivadoStr", type: "string" },
                { name: "ActivoStr", type: "string" },
                { name: "Modulo", type: "string" },
                { name: "SubModulo", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridPantallas, "Pantallas", "ObtenerGridPantallas", fields);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };

            gridPantallas.jqxGrid({
                width: "99%",
                height: 300,
                theme: tema,
                columnsresize: true,
                pageable: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                virtualmode: true,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                pagesize: 10,
                pagesizeoptions: ['5', '10', '15', '20'],
                autoshowloadelement: false,
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Nombre", dataField: "Nombre", width: "20%" },
                    { text: "Alcance", dataField: "PrivadoStr", width: "15%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "15%" },
                    { text: "Módulo", dataField: "Modulo", width: "15%" },
                    { text: "Submódulo", dataField: "SubModulo", width: "15%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridPantallas.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridPantallas);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridPantallas);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
                //$('.btnDetalle').unbind();
                //$('.btnEditar').unbind();
                //$('.btnDetalle').click(function () {
                //    var row = utils.getRowSelected(gridPantallas);
                //    abrirModal(utils.accionModal.detalle, row);
                //});
                //$('.btnEditar').click(function () {
                //    var row = utils.getRowSelected(gridPantallas);
                //    abrirModal(utils.accionModal.editar, row);
                //});
            });
        }


        function abrirModal(accion, row) {
            limpiar();
            //var controlesAccionesEtiquetas = { btnAccion, tituloModal };
            var controlesValores = [txtNombre, txtControlador, txtAccion, txtClaseIcono];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");
            gridAcciones.jqxGrid({ disabled: false });
            chkActivo.jqxCheckBox({ disabled: false });
            chkPrivado.jqxCheckBox({ disabled: false });
            cboModulo.jqxDropDownList({ disabled: false });
            cboSubmodulo.jqxDropDownList({ disabled: false });

            utils.onModalCreating(accion, row, btnAccion, tituloModal, controlesValores);


            if (accion == utils.accionModal.detalle) {
                gridAcciones.jqxGrid({ disabled: true });
                chkActivo.jqxCheckBox({ disabled: true });
                chkPrivado.jqxCheckBox({ disabled: true });
                cboModulo.jqxDropDownList({ disabled: true });
                cboSubmodulo.jqxDropDownList({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                consultar(row.Id);
            }


            modalPantallas.modal("show");

        }

        function initGridAcciones() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Controlador", type: "string" },
                { name: "Accion", type: "string" },
                { name: "Privado", type: "boolean" },
                { name: "Activo", type: "boolean" }
            ],
            datos = new Array(),
            dataAdapter = utils.generateGridLocalAdapter(fields, datos);


            gridAcciones.jqxGrid({
                width: "99%",
                height: 200,
                theme: tema,
                showemptyrow: false,
                editable: true,
                localization: utils.getIdiomaGrid(),
                autoshowloadelement: false,
                source: dataAdapter,
                showstatusbar: true,
                renderstatusbar: function (statusbar) {
                    var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
                    var addButton = $("<div style='float: left; margin-left: 5px;'><span style='margin: 4px; position: relative; top: -0.5px;'>Agregar</span></div>");
                    statusbar.append(container);
                    container.append(addButton);
                    addButton.jqxButton({ width: 80, height: 17, theme: tema });
                    addButton.click(function (event) {
                        var row = {
                            Id: 0,
                            Controlador: txtAccion.val() != "" ? txtAccion.val() : "¿Controlador?",
                            Accion: "¿Accion?",
                            Activo: true,
                            Privado: true
                        };
                        gridAcciones.jqxGrid('addrow', row.Id, row);
                    });
                },
                columns: [
                    {
                        text: "Eliminar", dataField: "Eliminar", columntype: "button", width: "15%",
                        cellsrenderer: function () {
                            return "Eliminar";
                        },
                        buttonclick: function (clickRow) {
                            var row = gridAcciones.jqxGrid("getrowdata", clickRow);
                            eliminarAccion(row);
                        }
                    },
                    { text: "Id", dataField: "Id", hidden: true, editable: false },
                    {
                        text: "Controlador", dataField: "Controlador", width: "30%", editable: true,
                        initeditor: function (row, column, editor) {
                            editor.attr('maxlength', 50);
                        }
                    },
                    {
                        text: "Acción", dataField: "Accion", width: "30%", editable: true,
                        initeditor: function (row, column, editor) {
                            editor.attr('maxlength', 50);
                        }
                    },
                    { text: "Activo", dataField: "Activo", width: "12%", editable: true, columntype: "checkbox" },
                    { text: "Privado", dataField: "Privado", width: "12%", editable: true, columntype: "checkbox" }
                ],
                ready: function (e) {
                    readyGridAcciones = true;
                }
            });
        }

        function eliminarAccion(row) {
            if (row) {
                mensajes.MensajeConfirmacion("Eliminar", "¿Esta seguro de eliminar esta acción?", function () {
                    gridAcciones.jqxGrid("deleterow", row.Id);
                });
            }
        }



        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Pantallas", "Consultar", { id: id }, dialogo, function (response) {
                setPantalla(response);
            });
        }

        function setPantalla(pantalla) {
            hidId.val(pantalla.Id);
            txtNombre.val(pantalla.Nombre);
            txtControlador.val(pantalla.Controlador);
            txtAccion.val(pantalla.Accion);
            txtClaseIcono.val(pantalla.ClaseIcono);
            cboModulo.val(pantalla.IdModulo);
            cboSubmodulo.val(pantalla.IdSubModulo);
            chkActivo.val(pantalla.Activo);
            chkPrivado.val(pantalla.Privado);
            gridAcciones.jqxGrid("addrow", pantalla.IdsAcciones, pantalla.Acciones);
        }

        function getPantalla() {
            return {
                pantalla: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    Controlador: txtControlador.val(),
                    Accion: txtAccion.val(),
                    ClaseIcono: txtClaseIcono.val(),
                    IdModulo: cboModulo.val(),
                    IdSubModulo: cboSubmodulo.val(),
                    Privado: chkPrivado.val(),
                    Activo: chkActivo.val(),
                    PantallasAcciones: gridAcciones.jqxGrid("getrows")
                }
            }
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            txtControlador.val("Vistas");
            txtAccion.val("");
            txtClaseIcono.val("");
            cboModulo.jqxDropDownList("clearSelection");
            cboSubmodulo.jqxDropDownList("clearSelection");
            chkActivo.val(0);
            chkPrivado.val(0);
            gridAcciones.jqxGrid("clear");
            frmPantallas.jqxValidator("hide");
        }

        function guardar() {
            frmPantallas.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridPantallas.jqxGrid("updatebounddata");
        }


        init();

    }();

});