$(function () {

    window.ITSON.UnidadesContacto = function () {
        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridUnidades = $("#gridUnidades"),

            modalUnidades = $("#modalUnidades"),
            tituloModal = $("#tituloModal"),
            frmUnidades = $("#frmUnidades"),

            hidId = $("#hidId"),
            cboUnidadCampus = $("#cboUnidadCampus"),
            txtDireccion = $("#txtDireccion"),
            txtTelefono = $("#txtTelefono"),
            txtEmailAdministrador = $("#txtEmailAdministrador"),
            txtEmailEjecutivo = $("#txtEmailEjecutivo"),
            txtEmailAuxiliar = $("#txtEmailAuxiliar"),

            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),
            
            accionModalActual = 0;

        function iniciar() {
            initComboUnidades();
            initGrid();
            initEventos();
            initValidacion2();
        }

        function initComboUnidades() {
            cboUnidadCampus.jqxDropDownList({
                source: utils.generateComboAdapter("CombosComunes", "UnidadCampus"),
                width: "99%",
                height: 42,
                theme: tema,
                autoDropDownHeight: true,
                placeHolder: utils.mensajes.seleccione,
                displayMember: "Label",
                valueMember: "Value"
            });
        }

        function initEventos() {
            btnNuevo.click(function () {
                abrirModal(utils.accionModal.nuevo);
            });

            frmUnidades.on("validationSuccess", function () {
                var dialogo = mensajes.Cargando("Guardando registro");
                var registro = getUnidadContacto();
                utils.ajaxRequest("POST", null, "UnidadesContacto", "Guardar", registro, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        modalUnidades.modal("hide");
                        mensajes.Mensaje("Guardar", response.Mensaje);
                        actualizarGrid();
                        limpiar();
                    } else {
                        mensajes.Mensaje("Guardar", response.Mensaje);
                    }
                }, null, null, dialogo);
            });
        }

        function initValidacion2() {
            frmUnidades.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#cboUnidadCampus", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList },
                    { input: "#txtDireccion", action: "keyup,blur", message: utils.mensajesValidaciones.requerido, rule: "required" },
                    { input: "#txtTelefono", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#txtEmailAdministrador", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#txtEmailAdministrador", message: utils.mensajesValidaciones.email, action: "keyup,blur", rule: "email" },
                    { input: "#txtEmailEjecutivo", message: utils.mensajesValidaciones.email, action: "keyup,blur", rule: "email" },
                    { input: "#txtEmailAuxiliar", message: utils.mensajesValidaciones.email, action: "keyup,blur", rule: "email" },
                ]
            });
        }

        function initValidacion() {
            frmUnidades.validate({
                rules: {
                    cboUnidadCampus: {
                        required: utils.ruleJqxDropDownList(cboUnidadCampus),
                        min: 1
                    },
                    txtDireccion: {
                        required: true
                    },
                    txtTelefono: {
                        required: true
                    },
                    txtEmailAdministrador: {
                        required: true,
                        email: true
                    },
                    txtEmailEjecutivo: {
                        email: true
                    },
                    txtEmailAuxiliar: {
                        email: true
                    }
                },
                messages: {
                    cboUnidadCampus: {
                        required: utils.mensajesValidaciones.requerido,
                        min: utils.mensajesValidaciones.requerido
                    },
                    txtDireccion: {
                        required: utils.mensajesValidaciones.requerido
                    },
                    txtTelefono: {
                        required: utils.mensajesValidaciones.requerido
                    },
                    txtEmailAdministrador: {
                        required: utils.mensajesValidaciones.requerido,
                        email: utils.mensajesValidaciones.email
                    },
                    txtEmailEjecutivo: {
                        email: utils.mensajesValidaciones.email
                    },
                    txtEmailAuxiliar: {
                        email: utils.mensajesValidaciones.email
                    }
                }
            })
        }

        function abrirModal(accion, row) {
            limpiar();
            accionModalActual = accion;
            var controlesAccionesEtiquetas = { btnAccion, tituloModal };
            var controlesValores = { cboUnidadCampus, txtDireccion, txtTelefono, txtEmailAdministrador, txtEmailEjecutivo, txtEmailAuxiliar };

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            $(btnAccion).removeClass("btn btn-primary");
            $(btnAccion).removeClass("btn btn-warning");
            $(btnAccion).removeClass("btn btn-danger");
            //Al abrir el modal por defecto mi jqxdropdownlist va activo.
            cboUnidadCampus.jqxDropDownList({ disabled: false });

            if (accion != utils.accionModal.detalle) {
                btnAccion.unbind();
                btnAccion.click(function (e) {
                    guardar();
                });
            }

            if (accion != utils.accionModal.nuevo) {
                consultaRegistroOriginal(row.Id);
                //como el combo es un control diferente a los html tengo que deshabilitarlo de manera diferente...
                cboUnidadCampus.jqxDropDownList({ disabled: true });
            }

            if (accion == utils.accionModal.editar) {
                camposEditar();
            }



            utils.onModalCreating(accion, row, controlesAccionesEtiquetas, controlesValores);
            modalUnidades.modal("show");
        }

        //Por defecto al crear el grid en una accion que no sea guardar todos los controles se desactivan.
        //aqui modifico esto para cuando sea la accion editar especificar que campos se podran editar
        function camposEditar() {
            cboUnidadCampus.jqxDropDownList({ disabled: false });
            txtDireccion.prop("disabled", false);
            txtTelefono.prop("disabled", false);
            txtEmailAdministrador.prop("disabled", false);
            txtEmailEjecutivo.prop("disabled", false);
            txtEmailAuxiliar.prop("disabled", false);
        }

        function initGrid() {
            var campos = [
                { name: "Id", type: "long" },
                { name: "Direccion", type: "string" },
                { name: "Telefono", type: "string" },
                { name: "EmailAdministrador", type: "string" },
                { name: "EmailEjecutivo", type: "string" },
                { name: "EmailAuxiliar", type: "string" },
                { name: "IdUnidadCampus", type: "long" },
                { name: "UnidadCampus", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridUnidades, "UnidadesContacto", "ObtenerGridUnidadesContacto", campos, getGridFilters);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>'
                    + '<button type="button" id="btnEliminar" class="btnEliminar btn btn-danger">Eliminar</button>';
            };


            gridUnidades.jqxGrid({
                width: "100%",
                theme: tema,
                columnsresize: true,
                pageable: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                virtualmode: true,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Direccion", dataField: "Direccion", width: "30%" },
                    { text: "Telefono", dataField: "Telefono", width: "15%" },
                    { text: "Email administrador", dataField: "EmailAdministrador", width: "20%" },
                    { text: "Ubicacion", dataField: "UnidadCampus", width: "15%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridUnidades.bind('click', function (event) {
                $('.btnDetalle').unbind();
                $('.btnEditar').unbind();
                $('.btnEliminar').unbind();
                $('.btnDetalle').click(function (e) {
                    var row = utils.getRowSelected(gridUnidades);
                    abrirModal(utils.accionModal.detalle, row);
                    event.preventDefault();
                });
                $('.btnEditar').click(function (e) {
                    var row = utils.getRowSelected(gridUnidades);
                    abrirModal(utils.accionModal.editar, row);
                    event.preventDefault();
                });
                $('.btnEliminar').click(function (e) {
                    var row = utils.getRowSelected(gridUnidades);
                    abrirModal(utils.accionModal.eliminar, row);
                    event.preventDefault();
                });
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


        function getUnidadContacto() {
            return {
                unidadContacto: {
                    Id: hidId.val(),
                    IdUnidadCampus: cboUnidadCampus.val(),
                    Direccion: txtDireccion.val(),
                    Telefono: txtTelefono.val(),
                    EmailAdministrador: txtEmailAdministrador.val(),
                    EmailEjecutivo: txtEmailEjecutivo.val(),
                    EmailAuxiliar: txtEmailAuxiliar.val(),
                    EstatusRegistro: accionModalActual
                }
            }
        }

        function setUnidadContacto(unidadContacto) {
            hidId.val(unidadContacto.Id);
            utils.setDropDownListValue(cboUnidadCampus, unidadContacto.IdUnidadCampus);
            txtDireccion.val(unidadContacto.Direccion);
            txtTelefono.val(unidadContacto.Telefono);
            txtEmailAdministrador.val(unidadContacto.EmailAdministrador);
            txtEmailEjecutivo.val(unidadContacto.EmailEjecutivo);
            txtEmailAuxiliar.val(unidadContacto.EmailAuxiliar);
        }

        function limpiar() {
            hidId.val("");
            cboUnidadCampus.jqxDropDownList("clearSelection");
            txtDireccion.val("");
            txtTelefono.val("");
            txtEmailAdministrador.val("");
            txtEmailEjecutivo.val("");
            txtEmailAuxiliar.val("");
            accionModalActual = 0;
            frmUnidades.jqxValidator("hide");
        }

        function actualizarGrid() {
            gridUnidades.jqxGrid("updatebounddata");
        }

        function guardar() {
            frmUnidades.jqxValidator("validate");
        }

        function consultaRegistroOriginal(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "UnidadesContacto", "ConsultarPorId", { id: id }, function (response) {
                setUnidadContacto(response);
            }, null, null, dialogo);
        }



        iniciar();

    }();

});