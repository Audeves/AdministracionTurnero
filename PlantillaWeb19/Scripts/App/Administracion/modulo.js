$(function () {

    window.ITSON.Modulo = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridModulos = $("#gridModulos"),

            modalModulos = $("#modalModulos"),
            tituloModal = $("#tituloModal"),
            frmModulos = $("#frmModulos"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            chkActivo = $("#chkActivo"),
            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),

            ///////////////////////////

            btnNuevoSubModulo = $("#btnNuevoSubModulo"),
            gridSubModulos = $("#gridSubModulos"),

            modalSubModulos = $("#modalSubModulos"),
            tituloModalSubModulos = $("#tituloModalSubModulos"),
            frmSubModulos = $("#frmSubModulos"),

            hidIdSubModulo = $("#hidIdSubModulo"),
            txtNombreSubModulo = $("#txtNombreSubModulo"),
            cboModulo = $("#cboModulo"),
            txtClaseIconoSubModulo = $("#txtClaseIconoSubModulo"),
            chkActivoSubModulo = $("#chkActivoSubModulo"),
            btnCerrarSubModulo = $("#btnCerrarSubModulo"),
            btnAccionSubModulo = $("#btnAccionSubModulo");



        function iniciar() {
            initControles();
            initValidaciones();
            initGrid();
            eventos();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });
        }

        function initValidaciones() {
            frmModulos.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                ],
            });
        }


        function initGrid() {
            var campos = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Activo", type: "bool" },
                { name: "ActivoStr", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridModulos, "Modulo", "ObtenerGridModulos", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    +'<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };


            gridModulos.jqxGrid({
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
                    { text: "Nombre", dataField: "Nombre", width: "30%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "20%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridModulos.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridModulos);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridModulos);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
                //$('.btnDetalle').unbind();
                //$('.btnEditar').unbind();
                //$('.btnDetalle').click(function () {
                //    var row = utils.getRowSelected(gridModulos);
                //    abrirModal(utils.accionModal.detalle, row);
                //});
                //$('.btnEditar').click(function () {
                //    var row = utils.getRowSelected(gridModulos);
                //    abrirModal(utils.accionModal.editar, row);
                //});
            });
        }

        /**
        * Funcion para obtener los parametros de busqueda del grid
        * @returns {object} - objeto con los parametros de busqueda
        */
        function getGridFilters(data) {
            return $.extend(data, {
            });
        }

        function eventos() {
            
            btnNuevo.click(function () {
                abrirModal(utils.accionModal.nuevo);
            });

            frmModulos.on("validationSuccess", function (e) {
                var registro = getModulo();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "Modulo", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            modalModulos.modal("hide");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGrid();
                            limpiar();
                            cboModulo.jqxDropDownList({
                                source: utils.generateComboAdapter("CombosComunes", "Modulos")
                            });
                            actualizarGridSubModulos();
                        } else if (response.Estatus == utils.respuestaEstatus.error) {
                            mensajes.Mensaje("Guardar", response.Mensaje);
                        }
                    });
                }
            });

        }

        function abrirModal(accion, row) {
            limpiar();
            //var tituloModal = accion + " modulo";
            //var controlesAccionesEtiquetas = [ btnAccion, tituloModal ];
            var controlesValores = [ txtNombre, chkActivo ];            

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");
            chkActivo.jqxCheckBox({ disabled: false });

            utils.onModalCreating(accion, row, btnAccion, tituloModal, controlesValores);

            if (accion != utils.accionModal.detalle) {
                btnAccion.unbind();
                btnAccion.click(function (e) {
                    guardar();
                });
            }

            if (accion != utils.accionModal.nuevo) {
                consultaRegistroOriginal(row.Id);
                if (accion == utils.accionModal.editar) {
                    chkActivo.jqxCheckBox({ disabled: false });
                } else {
                    chkActivo.jqxCheckBox({ disabled: true });
                }
            }

            modalModulos.modal("show");
        }

        function consultaRegistroOriginal(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Modulo", "Consultar", { id: id }, dialogo, function (response) {
                setModuloInfo(response);
            });
        }

        function setModuloInfo(modulo) {
            hidId.val(modulo.Id);
            txtNombre.val(modulo.Nombre);
            chkActivo.val(modulo.Activo);
        }

        function getModulo() {
            return {
                modulo: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    Activo: chkActivo.val()
                }
            }
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            chkActivo.val(0);
            frmModulos.jqxValidator("hide");
        }

        function guardar() {
            frmModulos.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridModulos.jqxGrid("updatebounddata");
        }



        iniciar();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function initSubModulo() {
            initControlesSubModulo();
            initValidacionesSubModulo();
            initGridSubModulos();
            initEventosSubModulo();
        }

        function initControlesSubModulo() {
            chkActivoSubModulo.jqxCheckBox({ theme: tema });

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
        }

        function initValidacionesSubModulo() {
            frmSubModulos.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombreSubModulo", action: "keyup,blur", message: utils.mensajesValidaciones.requerido, rule: "required" },
                    { input: "#cboModulo", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList }
                ]
            });
        }

        function initGridSubModulos() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Modulo", type: "string" },
                { name: "ClaseIcono", type: "string" },
                { name: "Activo", type: "bool" },
                { name: "ActivoStr", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridSubModulos, "Modulo", "ObtenerGridSubModulos", fields, getGridFiltersSubModulo);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalleSubModulo" class="btnDetalleSubModulo btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditarSubModulo" class="btnEditarSubModulo btn btn-warning">Editar</button>';
            };


            gridSubModulos.jqxGrid({
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
                    { text: "Nombre", dataField: "Nombre", width: "30%" },
                    { text: "Módulo", dataField: "Modulo", width: "30%" },
                    { text: "Clase Icono", dataField: "ClaseIcono", width: "10%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "10%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridSubModulos.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditarSubModulo") {
                        var row = utils.getRowSelected(gridSubModulos);
                        abrirModalSubModulo(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalleSubModulo") {
                        var row = utils.getRowSelected(gridSubModulos);
                        abrirModalSubModulo(utils.accionModal.detalle, row);
                    }
                }
                //$('.btnDetalleSubModulo').unbind();
                //$('.btnEditarSubModulo').unbind();
                //$('.btnDetalleSubModulo').click(function () {
                //    var row = utils.getRowSelected(gridSubModulos);
                //    abrirModalSubModulo(utils.accionModal.detalle, row);
                //});
                //$('.btnEditarSubModulo').click(function () {
                //    var row = utils.getRowSelected(gridSubModulos);
                //    abrirModalSubModulo(utils.accionModal.editar, row);
                //});
            });
        }

        function getGridFiltersSubModulo(data) {
            return $.extend(data, {
            });
        }

        function initEventosSubModulo() {
            btnNuevoSubModulo.click(function (e) { abrirModalSubModulo(utils.accionModal.nuevo) });

            frmSubModulos.on("validationSuccess", function (e) {
                var dialogo = mensajes.Cargando("Guardando registro");
                var registro = getSubModulo();
                if (registro) {
                    utils.ajaxRequest("POST", null, "Modulo", "GuardarSubModulo", registro, dialogo, function (response) {
                        if (response.Estatus = utils.respuestaEstatus.ok) {
                            modalSubModulos.modal("hide");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGridSubModulos();
                            limpiarSubModulo();
                        } else if (response.Estatus = utils.respuestaEstatus.error) {
                            mensajes.Mensaje("Guardar", response.Mensaje);
                        }
                    });
                }
            });
        }

        function abrirModalSubModulo(accion, row) {
            limpiarSubModulo();
            //Cambio de objeto el titulo y accion para no afectar el funcionamiento de utils...
            //var tituloModal = tituloModalSubModulos, btnAccion = btnAccionSubModulo;
            //var controlesAccionesEtiquetas = [ btnAccion, tituloModal ];
            var controlesValores = [ txtNombreSubModulo, cboModulo, chkActivoSubModulo ];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccionSubModulo.removeClass("btn btn-primary");
            btnAccionSubModulo.removeClass("btn btn-warning");
            btnAccionSubModulo.removeClass("btn btn-danger");
            chkActivoSubModulo.jqxCheckBox({ disabled: false });
            cboModulo.jqxDropDownList({ disabled: false });

            utils.onModalCreating(accion, row, btnAccionSubModulo, tituloModalSubModulos, controlesValores);

            if (accion != utils.accionModal.detalle) {
                btnAccionSubModulo.unbind();
                btnAccionSubModulo.click(function (e) {
                    guardarSubModulo();
                });
            }

            if (accion != utils.accionModal.nuevo) {
                consultarSubModulo(row.Id);
                chkActivoSubModulo.jqxCheckBox({ disabled: true });
                cboModulo.jqxDropDownList({ disabled: true });
            }

            if (accion == utils.accionModal.editar) {
                camposEditarSubModulo();
            }

            modalSubModulos.modal("show");
        }

        function camposEditarSubModulo() {
            chkActivoSubModulo.jqxCheckBox({ disabled: false });
            cboModulo.jqxDropDownList({ disabled: false });
        }

        function guardarSubModulo() {
            frmSubModulos.jqxValidator("validate");
        }

        function consultarSubModulo(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Modulo", "ConsultarSubModulo", { id: id }, dialogo, function (response) {
                setSubModulo(response);
            });
        }

        function setSubModulo(submodulo) {
            hidIdSubModulo.val(submodulo.Id);
            txtNombreSubModulo.val(submodulo.Nombre);
            utils.setDropDownListValue(cboModulo, submodulo.IdModulo);
            txtClaseIconoSubModulo.val(submodulo.ClaseIcono);
            chkActivoSubModulo.val(submodulo.Activo);
        }

        function getSubModulo() {
            return {
                subModulo: {
                    Id: hidIdSubModulo.val(),
                    Nombre: txtNombreSubModulo.val(),
                    IdModulo: cboModulo.val(),
                    ClaseIcono: txtClaseIconoSubModulo.val(),
                    Activo: chkActivoSubModulo.val()
                }
            }
        }

        function limpiarSubModulo() {
            hidIdSubModulo.val("");
            txtNombreSubModulo.val("");
            cboModulo.jqxDropDownList("clearSelection");
            txtClaseIconoSubModulo.val("");
            chkActivoSubModulo.val(0);
            frmSubModulos.jqxValidator("hide");
        }

        function actualizarGridSubModulos() {
            gridSubModulos.jqxGrid("updatebounddata");
        }


        initSubModulo();

    }();

});