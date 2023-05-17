$(function () {

    window.ITSON.RegistroPantallas = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridPantallas = $("#gridPantallas"),

            modalPantallas = $("#modalPantallas"),
            tituloModal = $("#tituloModal"),
            frmPantallas = $("#frmPantallas"),
            hidId = $("#hidId"),
            txtIP = $("#txtIP"),
            cboCampus = $("#cboCampus"),
            gridAreasAtencion = $("#gridAreasAtencion"),
            chkActivo = $("#chkActivo"),

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
            frmPantallas.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtIP", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboCampus", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList },
                    { input: "#gridAreasAtencion", action: "select", message: "Debes seleccionar al menos un área de atención", rule: utils.ruleJqxGridSelectedRows(gridAreasAtencion) }
                ]
            });
        }


        function initGrid() {
            var campos = [
                { name: "Id", type: "long" },
                { name: "Campus", type: "string" },
                { name: "AreasAtencionStr", type: "string" },
                { name: "Activo", type: "bool" },
                { name: "ActivoStr", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridPantallas, "RegistroPantallas", "ObtenerGridConfiguracionesPantallas", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };


            gridPantallas.jqxGrid({
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
                    { text: "Campus", dataField: "Campus", width: "15%" },
                    { text: "Areas de atencion", dataField: "AreasAtencionStr", width: "35%" },
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

            frmPantallas.on("validationSuccess", function (e) {
                var registro = getRegistroPantalla();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "RegistroPantallas", "Guardar", registro, dialogo, function (response) {
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
            var controlesValores = [txtIP, chkActivo];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");

            gridAreasAtencion.jqxGrid({ disabled: false });
            chkActivo.jqxCheckBox({ disabled: false });
            cboCampus.jqxDropDownList({ disabled: false });

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
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }


            modalPantallas.modal("show");
        }

        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "RegistroPantallas", "Consultar", { id: id }, dialogo, function (response) {
                setRegistroPantalla(response);
            });
        }

        function limpiar() {
            hidId.val("");
            txtIP.val("");
            chkActivo.val(0);
            cboCampus.jqxDropDownList("clearSelection");
            frmPantallas.jqxValidator("hide");
            if (readyGridAreasAtencion) {
                gridAreasAtencion.jqxGrid("clearselection");
            }
        }

        function guardar() {
            frmPantallas.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridPantallas.jqxGrid("updatebounddata");
        }

        function actualizarGridAreasAtencion() {
            gridAreasAtencion.jqxGrid("updatebounddata");
        }

        function setRegistroPantalla(pantalla) {
            hidId.val(pantalla.Id);
            txtIP.val(pantalla.IP);
            utils.setDropDownListValue(cboCampus, pantalla.CampusPS);
            utils.selectRows(gridAreasAtencion, pantalla.IdsAreasAtencion);
            chkActivo.val(pantalla.Activo);
        }

        function getRegistroPantalla() {
            var areasSeleccionadas = utils.getRowsSelected(gridAreasAtencion),
                areas = [];

            for (var i = 0; i < areasSeleccionadas.length; i++) {
                areas.push({ Id: areasSeleccionadas[i].uid });
            }

            return {
                pantalla: {
                    Id: hidId.val(),
                    IP: txtIP.val(),
                    CampusPS: cboCampus.jqxDropDownList("getSelectedItem").value,
                    CampusDescr: cboCampus.jqxDropDownList("getSelectedItem").label,
                    Activo: chkActivo.val(),
                    AreasAtencion: areas
                }
            }
        }


        init();
    }();


});