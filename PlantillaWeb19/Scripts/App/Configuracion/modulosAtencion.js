$(function () {

    window.ITSON.ModulosAtencion = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridModulosAtencion = $("#gridModulosAtencion"),

            modalModulosAtencion = $("#modalModulosAtencion"),
            tituloModal = $("#tituloModal"),
            frmModulosAtencion = $("#frmModulosAtencion"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            chkActivo = $("#chkActivo"),
            cboArea = $("#cboArea"),
            gridProcesos = $("#gridProcesos"),
            gridUsuarios = $("#gridUsuarios"),

            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),

            readyGridProcesos = false,
            readyGridUsuarios = false,
            accionModalActual = null

            ;

        function init() {
            initControles();
            initGrid();
            initGridUsuarios();
            initGridProcesos();
            initEventos();
            initValidaciones();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });

            cboArea.jqxDropDownList({
                source: utils.generateComboAdapter("AreasAtencion", "ObtenerAreasAtencionUsuarioTotales"),
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
            frmModulosAtencion.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboArea", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList },
                    { input: "#gridProcesos", action: "select", message: "Debes seleccionar al menos un proceso", rule: utils.ruleJqxGridSelectedRows(gridProcesos) },
                    { input: "#gridUsuarios", action: "select", message: "Debes seleccionar al menos un usuario", rule: utils.ruleJqxGridSelectedRows(gridUsuarios) }
                ]
            });
        }

        function initGrid() {
            var campos = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "AreaAtencion", type: "string" },
                { name: "Activo", type: "bool" },
                { name: "ActivoStr", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridModulosAtencion, "ModulosAtencion", "ObtenerGridModulosAtencion", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };


            gridModulosAtencion.jqxGrid({
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
                    { text: "Nombre", dataField: "Nombre", width: "15%" },
                    { text: "Area de atencion", dataField: "AreaAtencion", width: "35%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "20%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridModulosAtencion.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridModulosAtencion);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridModulosAtencion);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
            });
        }


        function getGridFilters(data) {
            return $.extend(data, {
            });
        }

        function initGridProcesos() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapterSync(gridProcesos, "Procesos", "ObtenerProcesosPorAreaAtencion", fields, getGridFiltersProcesos);


            gridProcesos.jqxGrid({
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
                    readyGridProcesos = true;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Todos", dataField: "Nombre", width: "90%" }
                ]
            });
        }

        function getGridFiltersProcesos(data) {
            return $.extend(data, {
                idAreaAtencion: cboArea.val()
            });
        }

        function initGridUsuarios() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapterSync(gridUsuarios, "Usuario", "ConsultarUsuariosAtencionVentanillaActivos", fields, getGridFiltersUsuarios);


            gridUsuarios.jqxGrid({
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
                    readyGridUsuarios = true;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Todos", dataField: "Nombre", width: "90%" }
                ]
            });
        }

        function getGridFiltersUsuarios(data) {
            return $.extend(data, {
                idAreaAtencion: cboArea.val()
            });
        }

        function initEventos() {

            btnNuevo.click(function () {
                abrirModal(utils.accionModal.nuevo);
            });

            frmModulosAtencion.on("validationSuccess", function (e) {
                var registro = getModuloAtencion();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "ModulosAtencion", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            modalModulosAtencion.modal("hide");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGrid();
                            limpiar();
                        } else if (response.Estatus == utils.respuestaEstatus.error) {
                            mensajes.Mensaje("Guardar", response.Mensaje);
                        }
                    });
                }
            });

            cboArea.on("change", function (e) {
                var args = e.args.item.originalItem;
                if (args) {
                    if (readyGridProcesos) {
                        gridProcesos.jqxGrid("clearselection");
                    }
                    if (readyGridUsuarios) {
                        gridUsuarios.jqxGrid("clearselection");
                    }
                    actualizarGridProcesos();
                    actualizarGridUsuarios();
                }
            });
        }

        function abrirModal(accion, row) {
            limpiar();
            accionModalActual = accion;
            //var tituloModal = accion + " modulo";
            //var controlesAccionesEtiquetas = [ btnAccion, tituloModal ];
            var controlesValores = [txtNombre, chkActivo];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");

            gridProcesos.jqxGrid({ disabled: false });
            gridUsuarios.jqxGrid({ disabled: false });
            chkActivo.jqxCheckBox({ disabled: false });
            cboArea.jqxDropDownList({ disabled: false });

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
                //cboArea.jqxDropDownList({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }


            modalModulosAtencion.modal("show");
        }

        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "ModulosAtencion", "Consultar", { id: id }, dialogo, function (response) {
                setModuloAtencion(response);
            });
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            chkActivo.val(0);
            cboArea.jqxDropDownList("clearSelection");
            frmModulosAtencion.jqxValidator("hide");
            if (readyGridProcesos) {
                gridProcesos.jqxGrid("clearselection");
                actualizarGridProcesos();
            }
            if (readyGridUsuarios) {
                gridUsuarios.jqxGrid("clearselection");
                actualizarGridUsuarios();
            }
        }

        function guardar() {
            frmModulosAtencion.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridModulosAtencion.jqxGrid("updatebounddata");
        }

        function actualizarGridProcesos() {
            gridProcesos.jqxGrid("updatebounddata");
        }

        function actualizarGridUsuarios() {
            gridUsuarios.jqxGrid("updatebounddata");
        }

        function setModuloAtencion(moduloAtencion) {
            hidId.val(moduloAtencion.Id);
            txtNombre.val(moduloAtencion.Nombre);
            utils.setDropDownListValue(cboArea, moduloAtencion.IdAreaAtencion);
            utils.selectRows(gridProcesos, moduloAtencion.IdsProcesos);
            utils.selectRows(gridUsuarios, moduloAtencion.IdsUsuarios);
            chkActivo.val(moduloAtencion.Activo);
        }

        function getModuloAtencion() {
            var procesosSeleccionados = utils.getRowsSelected(gridProcesos),
                procesos = [];

            for (var i = 0; i < procesosSeleccionados.length; i++) {
                procesos.push({ Id: procesosSeleccionados[i].uid });
            }

            var usuariosSeleccionados = utils.getRowsSelected(gridUsuarios),
                usuarios = [];

            for (var i = 0; i < usuariosSeleccionados.length; i++) {
                usuarios.push({ Id: usuariosSeleccionados[i].uid });
            }

            return {
                moduloAtencion: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    IdAreaAtencion: cboArea.jqxDropDownList("getSelectedItem").value,
                    Activo: chkActivo.val(),
                    Procesos: procesos,
                    UsuariosAtencion: usuarios
                }
            }
        }



        init();
    }();


});