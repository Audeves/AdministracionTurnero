$(function () {


    window.ITSON.Tramites = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridTramites = $("#gridTramites"),

            modalTramites = $("#modalTramites"),
            tituloModal = $("#tituloModal"),
            frmTramites = $("#frmTramites"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            cboProceso = $("#cboProceso"),
            chkActivo = $("#chkActivo"),
            divCorreoExpediente = $("#divCorreoExpediente"),
            chkRequiereExpediente = $("#chkRequiereExpediente"),
            txtCorreoExpediente = $("#txtCorreoExpediente"),

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
            chkRequiereExpediente.jqxCheckBox({ theme: tema });

            divCorreoExpediente.hide();

            var dataFields = [
                { name: "Label" },
                { name: "Value" },
                { name: "IdAreaAtencionProceso" },
            ];

            cboProceso.jqxDropDownList({
                source: utils.generateComboAdapter("Procesos", "ObtenerComboProcesos", dataFields),
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
            frmTramites.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboProceso", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList }
                ]
            });
        }

        function overwriteRules(shown) {
            frmTramites.jqxValidator("hide");
            frmTramites.jqxValidator({ rules: null });
            if (shown) {
                return [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboProceso", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList },
                    { input: "#txtCorreoExpediente", action: "select", message: utils.mensajesValidaciones.email, rule: "email" },
                    { input: "#txtCorreoExpediente", action: "select", message: utils.mensajesValidaciones.requerido, rule: "required" }
                ]
            } else {
                txtCorreoExpediente.val("");
                return [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboProceso", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList }
                ]
            }
        }

        function initGrid() {
            var campos = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Proceso", type: "string" },
                { name: "Activo", type: "bool" },
                { name: "ActivoStr", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridTramites, "Tramites", "ObtenerGridTramites", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };


            gridTramites.jqxGrid({
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
                    { text: "Proceso", dataField: "Proceso", width: "30%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "20%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridTramites.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridTramites);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridTramites);
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

            frmTramites.on("validationSuccess", function (e) {
                var registro = getTramite();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "Tramites", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            modalTramites.modal("hide");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGrid();
                            limpiar();
                        } else if (response.Estatus == utils.respuestaEstatus.error) {
                            mensajes.Mensaje("Guardar", response.Mensaje);
                        }
                    });
                }
            });

            chkRequiereExpediente.on("change", function (e) {
                var checked = e.args.checked;
                if (checked) {
                    divCorreoExpediente.show();
                } else {
                    divCorreoExpediente.hide();
                }
                frmTramites.jqxValidator({ rules: overwriteRules(checked) });
            });

        }

        function abrirModal(accion, row) {
            limpiar();
            accionModalActual = accion;
            //var tituloModal = accion + " modulo";
            //var controlesAccionesEtiquetas = [ btnAccion, tituloModal ];
            var controlesValores = [txtNombre, chkActivo, chkRequiereExpediente, txtCorreoExpediente];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");
            chkActivo.jqxCheckBox({ disabled: false });
            cboProceso.jqxDropDownList({ disabled: false });
            chkRequiereExpediente.jqxCheckBox({ disabled: false });

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
                cboProceso.jqxDropDownList({ disabled: true });
                chkRequiereExpediente.jqxCheckBox({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }


            modalTramites.modal("show");
        }


        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Tramites", "Consultar", { id: id }, dialogo, function (response) {
                setTramite(response);
            });
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            chkActivo.val(0);
            cboProceso.jqxDropDownList("clearSelection");
            frmTramites.jqxValidator("hide");
            chkRequiereExpediente.val(0);
            txtCorreoExpediente.val("");
            frmTramites.jqxValidator({ rules: overwriteRules(false) });
        }

        function guardar() {
            frmTramites.jqxValidator("validate");
        }

        function actualizarGrid() {
            gridTramites.jqxGrid("updatebounddata");
        }


        function setTramite(tramite) {
            hidId.val(tramite.Id);
            txtNombre.val(tramite.Nombre);
            utils.setDropDownListValue(cboProceso, tramite.IdProceso);
            chkActivo.val(tramite.Activo);
            chkRequiereExpediente.val(tramite.RequiereExpediente);
            txtCorreoExpediente.val(tramite.CorreoExpediente);
        }

        function getTramite() {
            return {
                tramite: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    IdProceso: cboProceso.val(),
                    IdAreaAtencionProceso: utils.getWindowDropDownListProperty(cboProceso, "IdAreaAtencionProceso"),
                    Activo: chkActivo.val(),
                    RequiereExpediente: chkRequiereExpediente.val(),
                    CorreoExpediente: txtCorreoExpediente.val(),
                    EstatusRegistro: accionModalActual
                }
            }
        }



        init();
    }();




});