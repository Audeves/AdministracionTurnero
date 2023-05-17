$(function () {

    window.ITSON.Usuario = function () {

        var utils = ITSON.Utils,
            mensajes = ITSON.Mensajes,
            tema = utils.getTema(),

            btnNuevo = $("#btnNuevo"),
            gridUsuarios = $("#gridUsuarios"),
            wdwUsuarios = $("#wdwUsuarios"),
            wdwTitulo = $("#wdwTitulo"),
            frmUsuarios = $("#frmUsuarios"),
            hidId = $("#hidId"),
            btnBuscarUsuarios = $("#btnBuscarUsuarios"),
            gridBuscarUsuarios = $("#gridBuscarUsuarios"),
            chkActivo = $("#chkActivo"),
            gridPerfiles = $("#gridPerfiles"),
            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),


            readyGridPerfiles = false;


        function init() {
            initFrmWindow();
            initControles();
            initValidaciones();
            initGrid();
            initGridPerfiles();
            initEventos();
        }

        function initFrmWindow() {
            wdwUsuarios.jqxWindow({
                width: "40%",
                height: 580,
                showCollapseButton: true,
                theme: tema,
                autoOpen: false,
                resizable: false,
                isModal: true,
                initContent: function () {
                    wdwUsuarios.jqxWindow("focus");
                }
            });
            wdwUsuarios.on("close", function (event) {
                btnBuscarUsuarios.jqxDropDownButton("close");
            });
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });
            utils.createGridBuscadorProfesores("CombosComunes", "ObtenerEmpleados", btnBuscarUsuarios, gridBuscarUsuarios, onSelectBuscadorUsuarios);
        }

        function onSelectBuscadorUsuarios(usuario) {
            
        }

        

        function initValidaciones() {
            frmUsuarios.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#gridPerfiles", action: "select", message: "Debes seleccionar al menos un rol", rule: utils.ruleJqxGridSelectedRows(gridPerfiles) },
                    { input: "#btnBuscarUsuarios", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownButton }
                ]
            });
        }

        function initEventos() {
            btnNuevo.click(function (e) { abrirModal(utils.accionModal.nuevo); });
            btnCerrar.click(function (e) { wdwUsuarios.jqxWindow("close"); });

            btnAccion.click(function (e) {
                guardar();
            });

            frmUsuarios.on("validationSuccess", function (e) {
                var registro = getUsuario();
                var dialogo = mensajes.Cargando("Guardando registro");
                if (registro) {
                    utils.ajaxRequest("POST", null, "Usuario", "Guardar", registro, dialogo, function (response) {
                        if (response.Estatus == utils.respuestaEstatus.ok) {
                            wdwUsuarios.jqxWindow("close");
                            mensajes.Mensaje("Guardar", response.Mensaje);
                            actualizarGrid();
                            limpiar();
                        } else if (response.Estatus == utils.respuestaEstatus.error) {
                            wdwUsuarios.jqxWindow("close");
                            mensajes.Mensaje("Guardar", response.Mensaje + " " + response.Datos);
                        }
                    });
                }
            });
        }

        function initGrid() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Emplid", type: "string" },
                { name: "Nombre", type: "string" },
                { name: "DptoAdscripcion", type: "string" },
                { name: "CuentaDominio", type: "string" },
                { name: "ActivoStr", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridUsuarios, "Usuario", "ObtenerGridUsuarios", fields);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };

            gridUsuarios.jqxGrid({
                width: "99%",
                height: 400,
                theme: tema,
                columnsresize: true,
                //pageable: true,
                localization: utils.getIdiomaGrid(),
                source: dataAdapter,
                //virtualmode: true,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                //pagesize: 10,
                //pagesizeoptions: ['5', '10', '15', '20'],
                autoshowloadelement: false,
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Emplid", dataField: "Emplid", width: "10%" },
                    { text: "Nombre", dataField: "Nombre", width: "25%" },
                    { text: "Departamento adscripción", dataField: "DptoAdscripcion", width: "20%" },
                    { text: "Cuenta dominio", dataField: "CuentaDominio", width: "15%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "10%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridUsuarios.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridUsuarios);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridUsuarios);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
                //$('.btnDetalle').unbind();
                //$('.btnEditar').unbind();
                //$('.btnDetalle').click(function () {
                //    var row = utils.getRowSelected(gridUsuarios);
                //    abrirModal(utils.accionModal.detalle, row);
                //});
                //$('.btnEditar').click(function () {
                //    var row = utils.getRowSelected(gridUsuarios);
                //    abrirModal(utils.accionModal.editar, row);
                //});
            });
        }

        function initGridPerfiles() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridPerfiles, "Usuario", "ObtenerGridPerfilesElegibles", fields);


            gridPerfiles.jqxGrid({
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
                autoshowloadelement: false,
                ready: function (e) {
                    readyGridPerfiles = true;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Todos", dataField: "Nombre", width: "90%" }
                ]
            });
        }

        function abrirModal(accion, row) {
            limpiar();
            //var tituloModal = wdwTitulo;
            //var controlesAccionesEtiquetas = { btnAccion, tituloModal };
            var controlesValores = {  };

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");
            
            if (accion == utils.accionModal.nuevo) {
                estadoControlesJqWidgets(false);
            } else if (accion == utils.accionModal.editar) {
                consultar(row.Id);
                estadoControlesJqWidgets(false);
                estadoEnEditar();
            } else {
                consultar(row.Id);
                estadoControlesJqWidgets(true);
            }

            utils.onModalCreating(accion, row, btnAccion, wdwTitulo, controlesValores);
            wdwUsuarios.jqxWindow("open");
        }

        function estadoEnEditar() {
            btnBuscarUsuarios.jqxDropDownButton({ disabled: true });
        }

        function estadoControlesJqWidgets(desahilitar) {
            btnBuscarUsuarios.jqxDropDownButton({ disabled: desahilitar });
            chkActivo.jqxCheckBox({ disabled: desahilitar });
            //gridPerfiles.jqxGrid({ disabled: desahilitar });
        }


        function guardar() {
            frmUsuarios.jqxValidator("validate");
        }


        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Usuario", "Consultar", { id: id }, dialogo, function (response) {
                setUsuario(response);
            });
        }


        function getUsuario() {
            var idUsuarioExistente = hidId.val();
            var empleado = utils.getRowSelected(gridBuscarUsuarios);

            var perfilesSeleccionados = utils.getRowsSelected(gridPerfiles),
                perfiles = [];

            for (var i = 0; i < perfilesSeleccionados.length; i++) {
                perfiles.push({ Id: perfilesSeleccionados[i].uid });
            }

            if (idUsuarioExistente > 0) {
                return {
                    usuario: {
                        Id: hidId.val(),
                        Activo: chkActivo.val(),
                        Perfiles: perfiles
                    }
                }
            } else {
                return {
                    usuario: {
                        Id: hidId.val(),
                        Emplid: empleado.Emplid,
                        Nombre: empleado.Nombre,
                        CuentaDominio: empleado.CuentaDominio,
                        DptoAdscripcion: empleado.DptoAdscripcion,
                        ClaveDptoAdscripcion: empleado.ClaveDptoAdscripcion,
                        Activo: chkActivo.val(),
                        Perfiles: perfiles
                    }
                }
            }

            
        }

        function setUsuario(usuario) {
            hidId.val(usuario.Id);
            utils.setDropDownButtonGridValue(btnBuscarUsuarios, usuario.Nombre);
            utils.selectRows(gridPerfiles, usuario.IdsPerfiles);
            chkActivo.val(usuario.Activo);
        }

        function actualizarGrid() {
            gridUsuarios.jqxGrid("updatebounddata");
        }

        function limpiar() {
            hidId.val("");
            gridBuscarUsuarios.jqxGrid("selectrow", -1);
            chkActivo.val(0);
            if (readyGridPerfiles) {
                gridPerfiles.jqxGrid("clearSelection");
            }
            frmUsuarios.jqxValidator("hide");
        }


        init();

    }();


});