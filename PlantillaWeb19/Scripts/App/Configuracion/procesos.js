$(function () {

    window.ITSON.Procesos = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridProcesos = $("#gridProcesos"),

            modalProcesos = $("#modalProcesos"),
            tituloModal = $("#tituloModal"),
            frmProcesos = $("#frmProcesos"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            cboAreaAtencion = $("#cboAreaAtencion"),
            chkActivo = $("#chkActivo"),

            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),

            
            accionModalActual = null

            ;

        function init() {
            initControles();
            initGrid();
            initEventos();
            initValidaciones();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });

            cboAreaAtencion.jqxDropDownList({
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
            frmProcesos.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboAreaAtencion", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList }
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
                dataAdapter = utils.generateGridAdapter(gridProcesos, "Procesos", "ObtenerGridProcesos", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };


            gridProcesos.jqxGrid({
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
                    { text: "Area de atencion", dataField: "AreaAtencion", width: "30%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "20%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridProcesos.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridProcesos);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridProcesos);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
            });
        }

        function getGridFilters(data) {
            return $.extend(data, {
            });
        }

        function initEventos() {

            btnNuevo.click(function () {
                abrirModal(utils.accionModal.nuevo);
            });

            frmProcesos.on("validationSuccess", function (e) {
                var registro = getProceso();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "Procesos", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            modalProcesos.modal("hide");
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
            chkActivo.jqxCheckBox({ disabled: false });
            cboAreaAtencion.jqxDropDownList({ disabled: false });

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
                cboAreaAtencion.jqxDropDownList({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }
            

            modalProcesos.modal("show");
        }

        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Procesos", "Consultar", { id: id }, dialogo, function (response) {
                setProceso(response);
            });
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            chkActivo.val(0);
            cboAreaAtencion.jqxDropDownList("clearSelection");
            frmProcesos.jqxValidator("hide");
        }

        function guardar() {
            frmProcesos.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridProcesos.jqxGrid("updatebounddata");
        }


        function setProceso(proceso) {
            hidId.val(proceso.Id);
            txtNombre.val(proceso.Nombre);
            utils.setDropDownListValue(cboAreaAtencion, proceso.IdAreaAtencion);
            chkActivo.val(proceso.Activo);
        }

        function getProceso() {
            return {
                proceso: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    IdAreaAtencion: cboAreaAtencion.val(),
                    Activo: chkActivo.val(),
                    EstatusRegistro: accionModalActual
                }
            }
        }


        init();

    }();


});