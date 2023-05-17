$(function () {


    window.ITSON.AreasAtencion = function () {

        var utils = ITSON.Utils,
            tema = utils.getTema(),
            mensajes = ITSON.Mensajes,

            btnNuevo = $("#btnNuevo"),
            gridAreasAtencion = $("#gridAreasAtencion"),

            modalAreasAtencion = $("#modalAreasAtencion"),
            tituloModal = $("#tituloModal"),
            frmAreasAtencion = $("#frmAreasAtencion"),
            hidId = $("#hidId"),
            txtNombre = $("#txtNombre"),
            cboCampus = $("#cboCampus"),
            chkActivo = $("#chkActivo"),
            gridResponsables = $("#gridResponsables"),

            chkDisponibleCitas = $("#chkDisponibleCitas"),
            txtResponsablePermisoCitas = $("#txtResponsablePermisoCitas"),
            txtLugarCitas = $("#txtLugarCitas"),
            txtCorreoAgendada = $("#txtCorreoAgendada"),
            txtCorreoCancelada = $("#txtCorreoCancelada"),
            txtCorreoRemitenteCitas = $("#txtCorreoRemitenteCitas"),
            
            btnCerrar = $("#btnCerrar"),
            btnAccion = $("#btnAccion"),


            readyGridResponsables = false,
            accionModalActual = null

            ;

        function init() {
            initControles();
            initGrid();
            initGridResponsables();
            initValidaciones();
            initEventos();
        }

        function initControles() {
            chkActivo.jqxCheckBox({ theme: tema, checked: true });

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

            chkDisponibleCitas.jqxCheckBox({ theme: tema, checked: true });
        }

        function initEventos() {
            btnNuevo.click(function (e) { abrirModal(utils.accionModal.nuevo); });

            frmAreasAtencion.on("validationSuccess", function (e) {
                var dialogo = mensajes.Cargando("Guardando registro");
                var registro = getAreaAtencion();
                utils.ajaxRequest("POST", null, "AreasAtencion", "Guardar", registro, dialogo, function (response) {
                    if (response.Estatus == utils.respuestaEstatus.ok) {
                        modalAreasAtencion.modal("hide");
                        mensajes.Mensaje("Guardar", response.Mensaje);
                        actualizarGrid();
                        limpiar();
                        gridResponsables.jqxGrid("clearSelection");
                    } else if (response.Estatus == utils.respuestaEstatus.error) {
                        mensajes.Mensaje("Guardar", response.Mensaje);
                    }
                });
            });
        }

        function initValidaciones() {
            frmAreasAtencion.jqxValidator({
                hintType: "label",
                animationDuration: 0,
                rules: [
                    { input: "#txtNombre", message: utils.mensajesValidaciones.requerido, action: "keyup,blur", rule: "required" },
                    { input: "#cboCampus", action: "select", message: utils.mensajesValidaciones.requerido, rule: utils.ruleJqxDropDownList },
                    { input: "#gridResponsables", action: "select", message: "Debes seleccionar al menos un responsable", rule: utils.ruleJqxGridSelectedRows(gridResponsables) }
                ]
            });
        }

        function initGrid() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" },
                { name: "Campus", type: "string" }, 
                { name: "ActivoStr", type: "string" },
                { name: "DisponibleCitasStr", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridAreasAtencion, "AreasAtencion", "ObtenerGridAreasAtencion", fields);

            var botones = function (row, columnfield, value, defaulthtml, columnproperties, rowdata) {
                return '<button  type="button" id="btnDetalle" class="btnDetalle btn btn-info">Detalle</button>'
                    + '<button type="button" id="btnEditar" class="btnEditar btn btn-warning">Editar</button>';
            };

            gridAreasAtencion.jqxGrid({
                width: "99%",
                height: 400,
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
                    { text: "Nombre", dataField: "Nombre", width: "30%" },
                    { text: "Campus", dataField: "Campus", width: "20%" },
                    { text: "Estatus", dataField: "ActivoStr", width: "15%" },
                    { text: "Disponible sistema citas", datafield: "DisponibleCitasStr", width: "15%" },
                    { text: "Acciones", dataField: "Acciones", cellsrenderer: botones, width: "20%" }
                ]
            });

            gridAreasAtencion.bind('click', function (event) {
                var objetivo = event.target;
                if (objetivo) {
                    if (objetivo.id == "btnEditar") {
                        var row = utils.getRowSelected(gridAreasAtencion);
                        abrirModal(utils.accionModal.editar, row);
                    } else if (objetivo.id == "btnDetalle") {
                        var row = utils.getRowSelected(gridAreasAtencion);
                        abrirModal(utils.accionModal.detalle, row);
                    }
                }
            });
        }

        function initGridResponsables() {
            var fields = [
                { name: "Id", type: "long" },
                { name: "Nombre", type: "string" }
            ],
                dataAdapter = utils.generateGridAdapter(gridResponsables, "Usuario", "ObtenerUsuariosResponsablesActivos", fields);


            gridResponsables.jqxGrid({
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
                    readyGridResponsables = true;
                },
                columns: [
                    { text: "Id", dataField: "Id", hidden: true },
                    { text: "Todos", dataField: "Nombre", width: "90%" }
                ]
            });
        }

        function abrirModal(accion, row) {
            limpiar();
            accionModalActual = accion;
            //var controlesAccionesEtiquetas = { btnAccion, tituloModal };
            var controlesValores = [txtNombre, txtResponsablePermisoCitas, txtLugarCitas, txtCorreoAgendada, txtCorreoCancelada, txtCorreoRemitenteCitas];

            //Se quitan las posibles clases que tendria el boton para ingresar la clase de la accion
            btnAccion.removeClass("btn btn-primary");
            btnAccion.removeClass("btn btn-warning");
            btnAccion.removeClass("btn btn-danger");
            gridResponsables.jqxGrid({ disabled: false });
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
                cboCampus.jqxDropDownList({ disabled: true });
            }

            if (accion != utils.accionModal.nuevo) {
                //gridPermisos.jqxGrid("clearselection");
                consultar(row.Id);
            }


            modalAreasAtencion.modal("show");
        }

        function guardar() {
            frmAreasAtencion.jqxValidator("validate");
        }

        function consultar(id) {
            var dialogo = mensajes.Cargando("Obteniendo registro");
            utils.ajaxRequest("GET", true, "AreasAtencion", "Consultar", { id: id }, dialogo, function (response) {
                setAreaAtencion(response);
            });
        }


        function limpiar() {
            hidId.val("");
            txtNombre.val("");
            if (readyGridResponsables) {
                gridResponsables.jqxGrid("clearselection");
            }
            cboCampus.jqxDropDownList("clearSelection");
            chkActivo.val(true);
            frmAreasAtencion.jqxValidator("hide");

            chkDisponibleCitas.val(true);
            txtResponsablePermisoCitas.val("");
            txtLugarCitas.val("");
            txtCorreoAgendada.val("");
            txtCorreoCancelada.val("");
            txtCorreoRemitenteCitas.val("");
        }

        function actualizarGrid() {
            gridAreasAtencion.jqxGrid("updatebounddata");
        }



        function setAreaAtencion(areaAtencion) {
            hidId.val(areaAtencion.Id);
            txtNombre.val(areaAtencion.Nombre);
            utils.setDropDownListValue(cboCampus, areaAtencion.CampusPS);
            utils.selectRows(gridResponsables, areaAtencion.IdsResponsables);
            chkActivo.val(areaAtencion.Activo);

            chkDisponibleCitas.val(areaAtencion.DisponibleCitas);
            txtResponsablePermisoCitas.val(areaAtencion.ResponsablePermisoCitas);
            txtLugarCitas.val(areaAtencion.LugarCitas);
            txtCorreoAgendada.val(areaAtencion.CorreoAgendada);
            txtCorreoCancelada.val(areaAtencion.CorreoCancelada);
            txtCorreoRemitenteCitas.val(areaAtencion.CorreoRemitenteCitas);
        }

        function getAreaAtencion() {
            var responsablesSeleccionados = utils.getRowsSelected(gridResponsables),
                responsables = [];

            for (var i = 0; i < responsablesSeleccionados.length; i++) {
                responsables.push({ Id: responsablesSeleccionados[i].uid });
            }

            return {
                areaAtencion: {
                    Id: hidId.val(),
                    Nombre: txtNombre.val(),
                    CampusPS: cboCampus.jqxDropDownList("getSelectedItem").value,
                    CampusDescr: cboCampus.jqxDropDownList("getSelectedItem").label,
                    Activo: chkActivo.val(),
                    Responsables: responsables,

                    DisponibleCitas: chkDisponibleCitas.val(),
                    ResponsablePermisoCitas: txtResponsablePermisoCitas.val(),
                    LugarCitas: txtLugarCitas.val(),
                    CorreoAgendada: txtCorreoAgendada.val(),
                    CorreoCancelada: txtCorreoCancelada.val(),
                    CorreoRemitenteCitas: txtCorreoRemitenteCitas.val()
                }
            }
        }



        init();
    }();

});