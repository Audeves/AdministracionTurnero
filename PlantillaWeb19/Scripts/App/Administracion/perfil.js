$(function () {

    window.ITSON.Perfil = function () {

        var utils = ITSON.Utils,
            mensajes = ITSON.Mensajes,
            tema = utils.getTema(),

            btnNuevo = $("#btnNuevo"),
            gridPerfiles = $("#gridPerfiles"),

            modalPerfiles = $("#modalPerfiles"),
            tituloModal = $("#tituloModal"),
            frmPerfiles = $("#frmPerfiles"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            gridPermisos = $("#gridPermisos"),
            gridAreasAtencion = $("#gridAreasAtencion"),
            chkActivo = $("#chkActivo"),
            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),


            readyGridPermisos = false,
            readyGridAreasAtencion = false,
            accionModalActual = null;


        function init() {
            initControles();
            initEventos();
            initGridPermisos();
            initGridAreasAtencion();
            initGrid();
            initValidaciones();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema });
        }

        function initEventos() {
            btnNuevo.click(function (e) { abrirModal(utils.accionModal.nuevo); });

            frmPerfiles.on("validationSuccess", function (e) {
                var dialogo = mensajes.Cargando("Guardando registro");
                var registro = getPerfil();
                utils.ajaxRequest("POST", null, "Perfil", "Guardar", registro, dialogo, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        modalPerfiles.modal("hide");
                        mensajes.Mensaje("Guardar", response.Mensaje);
                        actualizarGrid();
                        limpiar();
                        gridPermisos.jqxGrid("clearSelection");
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Guardar", response.Mensaje);
                    }
                });
            });
        }

        function initValidaciones() {
            frmPerfiles.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#gridPermisos", action: "select", message: "Debes seleccionar al menos un permiso", rule: utils.ruleJqxGridSelectedRows(gridPermisos) },
                    { input: "#gridAreasAtencion", action: "select", message: "Debes seleccionar al menos un area de atencion", rule: utils.ruleJqxGridSelectedRows(gridAreasAtencion) }
                ]
            });
        }

        function initGrid() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "ActivoStr", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridPerfiles, "Perfil", "ObtenerGridPerfiles", fields);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };

            gridPerfiles.jqxGrid({
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
                    { text: "Nombre", dataField: "Nombre", width: "30%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "20%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridPerfiles.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridPerfiles);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridPerfiles);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
                //$('.btnDetalle').unbind();
                //$('.btnEditar').unbind();
                //$('.btnDetalle').click(function () {
                //    var row = utils.getRowSelected(gridPerfiles);
                //    abrirModal(utils.accionModal.detalle, row);
                //});
                //$('.btnEditar').click(function () {
                //    var row = utils.getRowSelected(gridPerfiles);
                //    abrirModal(utils.accionModal.editar, row);
                //});
            });
        }

        function abrirModal(accion, row) {
            limpiar();
            accionModalActual = accion;
            //var controlesAccionesEtiquetas = { btnAccion, tituloModal };
            var controlesValores = [ txtNombre ];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");
            gridPermisos.jqxGrid({ disabled: false });
            chkActivo.jqxCheckBox({ disabled: false });

            utils.onModalCreating(accion, row, btnAccion, tituloModal, controlesValores);

            if (accion != utils.accionModal.detalle) {
                btnAccion.unbind();
                btnAccion.click(function (e) {
                    guardar();
                });
            }

            if (accion == utils.accionModal.detalle) {
                //gridPermisos.jqxGrid({ disabled: true });
                chkActivo.jqxCheckBox({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }


            modalPerfiles.modal("show");
            
        }

        function initGridPermisos() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Modulo", type: "string" },
                { name: "SubModulo", type: "string" }
            ],
            dataAdapter = utils.generateGridAdapter(gridPermisos, "Perfil", "ObtenerGridPermisosPantallasElegibles", fields);

            gridPermisos.jqxGrid({
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
                groupsexpandedbydefault: true,  // los grupos se muestran expandidos por default
                groupable: true,            // indica que el grid es agrupable
                groups: ["Modulo", "SubModulo"],         // indica las columnas que se utilizaran para agrupar
                selectionmode: "checkbox",  // tipo de seleccion de filas
                showgroupsheader: false,    // bloquea la barra de agrupamientos
                showgroupmenuitems: false,  // bloquea el menu de columnas,
                ready: function (e) {
                    readyGridPermisos = true;
                },
                autoshowloadelement: false,
                columns: [
                    { text: "Id", dataField: "Id", width: 100, hidden: true },
                    { text: "Módulo", dataField: "Modulo", width: 100, hidden: true },
                    { text: "Submódulo", dataField: "SubModulo", width: 100, hidden: true },
                    { text: "Todos", dataField: "Nombre", width: 380 }
                ],
            });
        }

        function initGridAreasAtencion() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridAreasAtencion, "AreasAtencion", "ObtenerAreasAtencionUsuarioResponsable", fields);


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
                autoshowloadelement: false,
                ready: function (e) {
                    readyGridAreasAtencion = true;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Todos", dataField: "Nombre", width: "90%" }
                ]
            });
        }

        function guardar() {
            frmPerfiles.jqxValidator("validate");
        }

        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "Perfil", "Consultar", { id: id }, dialogo, function (response) {
                setPerfil(response);
            });
        }

        function setPerfil(perfil) {
            hidId.val(perfil.Id);
            txtNombre.val(perfil.Nombre);
            utils.selectRows(gridPermisos, perfil.IdsPantallas);
            chkActivo.val(perfil.Activo);
            utils.selectRows(gridAreasAtencion, perfil.IdsAreasAtencion);
        }

        function getPerfil() {
            var pantallasSeleccionadas = utils.getRowsSelected(gridPermisos),
                pantallas = [];

            for (var i = 0; i < pantallasSeleccionadas.length; i++) {
                pantallas.push({ Id: pantallasSeleccionadas[i].uid });
            }


            var areasAtencionSeleccionadas = utils.getRowsSelected(gridAreasAtencion),
                areasAtencion = [];

            for (var i = 0; i < areasAtencionSeleccionadas.length; i++) {
                areasAtencion.push({ Id: areasAtencionSeleccionadas[i].uid });
            }

            

            return {
                perfil: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    Pantallas: pantallas,
                    Activo: chkActivo.val(),
                    EstatusRegistro: accionModalActual,
                    AreasAtencion: areasAtencion
                }
            }
        }

        function actualizarGrid() {
            gridPerfiles.jqxGrid("updatebounddata");
        }

        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            if (readyGridPermisos) {
                gridPermisos.jqxGrid("clearselection");
            }
            if (readyGridAreasAtencion) {
                gridAreasAtencion.jqxGrid("clearselection");
            }
            chkActivo.val(0);
            frmPerfiles.jqxValidator("hide");
        }
        


        init();

    }();

});