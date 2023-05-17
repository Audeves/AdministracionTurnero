$(function () {

    if (!window.ITSON) {
        window.ITSON = {};
    }

    window.ITSON.Utils = function () {
        var rutaPublicacion = "/Turnero/"
            , respuestaEstatus = {
                ok: 1,
                error: 2
            }
            , accionModal = {
                nuevo: 'A',
                editar: 'C',
                eliminar: 'D',
                detalle: 'Detalle',
            }, mensajes = {
                seleccione: "Seleccione: "
            }, mensajesValidaciones = {
                requerido: "Este campo es requerido.",
                email: "Formato de correo electrónico invalido.",
                entero: "Se requiere un número entero."
            }, colsBuscadorProfesor = {
                emplid: "Emplid",
                nombre: "Nombre",
                cuentaDominio: "CuentaDominio",
                claveDptoAdscripcion: "ClaveDptoAdscripcion",
                dptoAdscripcion: "DptoAdscripcion",
                claveDireccionAcademica: "ClaveDireccionAcademica",
                direccionAcademica: "DireccionAcademica"
            },
            estatusCitas = {
                pendiente: 1,
                agendada: 2,
                cancelada: 40
            }


            ;

        function getTema() {
            return "bootstrap";
        }

        //Configuracion a usar en los grid para sobreescribir los textos por defecto.
        function getIdiomaGrid() {
            return {
                pagergotopagestring: "Ir a:",
                pagershowrowsstring: "Mostrando: ",
                pagerrangestring: " de ",
                pagernextbuttonstring: "Siguiente",
                pagerpreviousbuttonstring: "Anterior",
                sortascendingstring: "Ordenar Ascendentemente",
                sortdescendingstring: "Ordenar Descendentemente",
                sortremovestring: "Eliminar Ordenamiento",
                emptydatastring: "No hay registros para mostrar",
                filterchoosestring: "Seleccione:",
                loadtext: "Cargando"
            };
        }

        //Funcion para realizar las llamadas ajax a las acciones.
        function ajaxRequest(tipo, async, controlador, accion, params, dialogo, okCallback, errorCallback, completeCallback) {
            tipo = (!tipo) ? "GET" : tipo;
            async = (!async) ? true : false;

            dialogo.modal("show");

            $.ajax({
                cache: false,
                async: async,
                type: tipo,
                url: rutaPublicacion + controlador + "/" + accion,
                data: (tipo === "POST") ? JSON.stringify(params) : params,
                dataType: "json",
                contentType: "application/json;charset=utf-8",
                success: function (data, status, jqXHR) {
                    if (okCallback) {
                        okCallback(data);
                    }
                },
                error: function (jqXHR, status, errorThrown) {
                    evaluarErrorPeticion(jqXHR, status, errorThrown);
                    if (errorCallback) {
                        errorCallback();
                    }
                },
                complete: function (jqXHR, status) {
                    if (completeCallback) {
                        completeCallback();
                    }
                    // se cierra el dialogo de animacion de cargado
                    dialogo.modal("hide");
                }
            });
        }

        function evaluarErrorPeticion(jqXHR, status, error) {
            if (jqXHR.status === 403) {
                redirectAvisoPermisos();
            } else if (jqXHR.status === 401) {
                redirectSesionExpirada();
            }
        }

        //Redireccionar a un controlador y accion con parametros opcionales.
        function redirect(controller, action, params) {
            var queryString = "?";
            for (var prop in params) {
                queryString += prop + "=" + params[prop] + "&";
            }
            window.location = rutaPublicacion + controller + "/" + action + queryString;
        }

        /**
        * Funcion para mostrar un dialogo de sesion expirada
        */
        function redirectSesionExpirada() {
                redirect("Vistas", "SesionExpirada");
        }

        /**
        * Funcion para mostrar un dialogo de aviso por no tener permisos
        */
        function redirectAvisoPermisos() {
            redirect("Vistas", "Aviso");
        }

        /**
        * Funcion para generar un adaptador para combos
        * @param {string} controller - nombre del controlador a llamar
        * @param {string} action - accion del controlador a llamar
        * @param {array} [dataFields=undefined] - arreglo de objetos que especifica los campos
        * @param {object} [extraParams=undefined] - objeto con los parametros extra        
        * @param {object} [extraParamsFunction=undefined] - funcion con los parametros extra        
        * @param {function} [onLoad=undefined] - funcion que se llama cuando finaliza la carga de un combo
        * @returns {object} - objeto adaptador creado
        */
        function generateComboAdapter(controller, action, dataFields, extraParams, extraParamsFunction, onLoad) {
            // si no se especifican los campos, se asignan por defecto
            if (!dataFields) {
                dataFields = [
                    { name: "Value", type: "string" },
                    { name: "Label", type: "string" }
                ];
            }

            var queryString = "?";
            for (var prop in extraParams) {
                queryString += prop + "=" + extraParams[prop] + "&";
            }

            // fuente de datos
            var source = {
                datatype: "json",
                url: rutaPublicacion + controller + "/" + action + queryString,
                async: false,
                datafields: dataFields
            };

            // adaptador
            return new $.jqx.dataAdapter(source, {
                loadError: function (jqXHR, status, error) {
                    evaluarErrorPeticion(jqXHR, status, error);
                },
                loadComplete: function (array) {
                    if (onLoad) {
                        onLoad();
                    }
                },
                formatData: function (data) {
                    if (extraParamsFunction) {
                        extraParams = extraParamsFunction();
                    }
                    if (extraParams) {
                        $.extend(data, extraParams);
                    }
                    return data;
                }
            });
        }

        /**
        * Funcion para generar un adaptador local
        * @param {string} idField - nombre del campo que se utilizara como Id
        * @param {array} dataFields - lista de campos que contiene la lista data y que se utilizaran para generar el adaptador
        * @param {array} data - lista de elementos de la cual se creara el adaptador
        * @returns adaptador creado
        */
        function generateLocalAdapter(idField, dataFields, data) {
            return new $.jqx.dataAdapter({
                id: idField,
                datatype: "local",
                datafields: dataFields,
                localdata: data
            });
        }

        /**
        * Funcion para generar un adaptador para grids remotos
        * @param {object} grid - objeto jquery grid
        * @param {string} controller - nombre del controlador a llamar
        * @param {string} action - accion del controlador a llamar
        * @param {array} fields - listado de campos que se esperan recibir del servidor
        * @param {function} [filters=undefined] - funcion que proporciona los filtros extra para la busqueda
        * @param {string} [idField='Id'] - identificador del campo 'id' del registro, por default su valor es 'Id'
        * @returns {object} - objeto adaptador creado
        */
        function generateGridAdapter(grid, controller, action, fields, filters, idField) {
            var source = {
                datatype: "json",
                id: (!idField) ? "Id" : idField,
                url: rutaPublicacion + controller + "/" + action,
                datafields: fields,
                cache: false,
                root: "Rows",
                beforeprocessing: function (data) {
                    source.totalrecords = data.TotalRows;
                },
                sort: function () {
                    // actualiza despues de ordenar
                    grid.jqxGrid("updatebounddata");
                }
            };

            // se crea el objeto adaptador y se le aplican los filtros
            return new $.jqx.dataAdapter(source, {
                loadError: function (jqXHR, status, error) {
                    evaluarErrorPeticion(jqXHR, status, error);
                },
                formatData: function (data) {
                    var numFields = 20,
                        i = 0,
                        extraParams = {};
                    // si existe una funcion de filtros personalizada, se agregan los parametros extra
                    if (filters) {
                        data = filters(data);
                    }
                    // se toman los campos de filtrado del grid, si aplican
                    for (i = 0; i < numFields; i++) {
                        paramName = data["filterdatafield" + i],
                        paramValue = data["filtervalue" + i];
                        if (paramName) {
                            extraParams[paramName] = paramValue;
                        } else {
                            break;
                        }
                    }
                    // se agregan los parametros
                    $.extend(data, extraParams);
                    return data;
                }
            });
        }

        /**
        * Funcion para obtener los parametros especificados en la fila de filtrado del grid
        * @param {object} data - objeto con la informacion del filtrado
        * @returns {array} - lista de parametros
        */
        function getGridFilterParams(data) {
            var numFields = 20,
                i = 0,
                filterParams = {};
            // se toman los campos de filtrado del grid, si aplican
            for (i = 0; i < numFields; i++) {
                paramName = data["filterdatafield" + i],
                paramValue = data["filtervalue" + i];
                if (paramName) {
                    filterParams[paramName] = paramValue;
                } else {
                    break;
                }
            }
            return filterParams;
        }

        /**
        * Funcion para generar un adaptador para grids locales             
        * @param {array} fields - listado de campos que se esperan recibir del arreglo        
        * @param {array} data - arreglo de objetos que llenaran el grid   
        * @param {string} [idField='Id'] - identificador del campo 'id' del registro, por default su valor es 'Id'
        * @returns {object} - objeto adaptador creado
        */
        function generateGridLocalAdapter(fields, data, idField) {
            var source = {
                id: (!idField) ? "Id" : idField,
                datatype: "local",
                datafields: fields,
                localdata: data
            };

            // se crea el objeto adaptador
            return new $.jqx.dataAdapter(source);
        }


        /**
        * Funcion para generar un adaptador para grids remotos
        * @param {object} grid - objeto jquery grid
        * @param {string} controller - nombre del controlador a llamar
        * @param {string} action - accion del controlador a llamar
        * @param {array} fields - listado de campos que se esperan recibir del servidor
        * @param {function} [filters=undefined] - funcion que proporciona los filtros extra para la busqueda
        * @param {string} [idField='Id'] - identificador del campo 'id' del registro, por default su valor es 'Id'
        * @returns {object} - objeto adaptador creado
        */
        function generateGridAdapterSync(grid, controller, action, fields, filters, idField) {
            var source = {
                datatype: "json",
                id: (!idField) ? "Id" : idField,
                url: rutaPublicacion + controller + "/" + action,
                datafields: fields,
                cache: false,
                async: false,
                root: "Rows",
                beforeprocessing: function (data) {
                    source.totalrecords = data.TotalRows;
                },
                sort: function () {
                    // actualiza despues de ordenar
                    grid.jqxGrid("updatebounddata");
                }
            };

            // se crea el objeto adaptador y se le aplican los filtros
            return new $.jqx.dataAdapter(source, {
                loadError: function (jqXHR, status, error) {
                    evaluarErrorPeticion(jqXHR, status, error);
                },
                formatData: function (data) {
                    var numFields = 20,
                        i = 0,
                        extraParams = {};
                    // si existe una funcion de filtros personalizada, se agregan los parametros extra
                    if (filters) {
                        data = filters(data);
                    }
                    // se toman los campos de filtrado del grid, si aplican
                    for (i = 0; i < numFields; i++) {
                        paramName = data["filterdatafield" + i],
                            paramValue = data["filtervalue" + i];
                        if (paramName) {
                            extraParams[paramName] = paramValue;
                        } else {
                            break;
                        }
                    }
                    // se agregan los parametros
                    $.extend(data, extraParams);
                    return data;
                }
            });
        }


        function onModalCreating(accion, row, btnAccion, tituloModal, controlesValores) {
            //en controlesAccionesEtiquetas siempre llevaran el mismo nombre asi que puedo acceder a su instancia desde aca.
            if (accion == accionModal.nuevo) {
                btnAccion.attr("value", "Guardar");
                btnAccion.addClass("btn btn-primary");

                estadoControlesModal(controlesValores, false);

                btnAccion.show();

                tituloModal.text("Nuevo");
            } else if (accion == accionModal.detalle) {
                btnAccion.attr("value", "Detalle");
                
                estadoControlesModal(controlesValores, true);

                btnAccion.hide();

                tituloModal.text("Detalle");
            } else if (accion == accionModal.editar) {
                btnAccion.attr("value", "Editar");
                btnAccion.addClass("btn btn-warning");

                estadoControlesModal(controlesValores, false);
                btnAccion.show();
                tituloModal.text("Editar");
            } else if (accion == accionModal.eliminar) {
                btnAccion.attr("value", "Eliminar");
                btnAccion.addClass("btn btn-danger");

                estadoControlesModal(controlesValores, true);
                btnAccion.show();
                tituloModal.text("Eliminar");
            }


        }

        function estadoControlesModal(controlesValores, deshabilitar) {
            $.each(controlesValores, function (index, value) {
                value.prop("disabled", deshabilitar);
            });
            
        }

        /**
        * Funcion para obtener la informacion de la fila seleccionada en un grid
        * @params {object} grid - jqxGrid del cual se extraera la informacion
        * @returns {object} - objeto seleccionado o null si no hay ninguno
        */
        function getRowSelected(grid) {
            var rowIndex = -1,
                rowId = -1,
                rowData = null;
            // se extrae el indice de la fila
            rowIndex = grid.jqxGrid("getselectedrowindex");
            if (rowIndex !== -1) {
                // se extrae el id de la fila    
                rowId = grid.jqxGrid("getrowid", rowIndex);
                if (rowId >= 0) {
                    // se extra la informacion
                    rowData = grid.jqxGrid("getrowdatabyid", rowId);
                }
            }
            return rowData;
        }

        /**
        * Funcion para seleccionar un elemento de la lista con su valor identificador
        * @param {object} combo - objeto combo en el cual se seleccionara su valor
        * @param {string} value - valor del elemento a seleccionar
        */
        function setDropDownListValue(combo, value) {
            var item = combo.jqxDropDownList("getItemByValue", value);
            combo.jqxDropDownList("selectItem", item);
        }

        /**
        * Funcion de validacion para determinar si esta un elemento seleccionado (dropdownlist)
        * @param {object} combo - objeto combo
        * @returns {bool} - true si tiene un elemento seleccionado, false en caso contrario
        */
        function ruleJqxDropDownList(combo) {
            // extraccion del valor
            var value = combo.jqxDropDownList("val");
            if (value === "") {
                return false;
            } else {
                return true;
            }
        }


        /**
        * Funcion para obtener el valor de la propiedad de un elemento de un DropDownList que se encuentre dentro de una ventana (jqxWindow), 
        * previene el problema de que el dropdown, no se encuentre inicializado, devolviendo null, o el valor asociado al objeto seleccionado
        * @params {object} combo - objeto combo de donde se extraera el valor
        * @params {string} property - nombre de la propiedad a extraer
        * @returns {object} - objeto selecionado si existe uno, sino 'null'
        */
        function getWindowDropDownListProperty(combo, property) {
            var item = combo.jqxDropDownList("getSelectedItem");
            // se valida que existan los objetos antes de extraer la informacion
            if (item && item.originalItem && item.originalItem[property]) {
                return item.originalItem[property];
            } else {
                return null;
            }
        }


        /**
        * Funcion para seleccionar multiples files de un grid enviando un arreglo con los ids a seleccionar
        * @param {object} grid - grid de donde se seleccionaran los registros
        * @param {array} ids - arreglo de los ids a seleccionar
        */
        function selectRows(grid, ids) {
            var i = 0,
                rowIndex = -1;
            // seleccionar los permisos del rol
            for (i = 0; i < ids.length; i++) {
                // en base al Id del registro tomamos su indice para seleccionarlo
                rowIndex = grid.jqxGrid("getrowboundindexbyid", ids[i]);
                if (rowIndex != -1) {
                    grid.jqxGrid("selectrow", rowIndex);
                }
            }
        }

        /**
        * Funcion para obtener la informacion de las filas seleccionadas en un grid
        * @params {object} grid - jqxGrid del cual se extraera la informacion
        * @returns {array} - array de objetos seleccionados o null si no hay ninguno
        */
        function getRowsSelected(grid) {
            var rowIndexes = -1,
                rowId = -1,
                rowsData = new Array(),
                i = 0;
            // se extrae el indice de la fila
            rowIndexes = grid.jqxGrid("getselectedrowindexes");
            if (rowIndexes.length > 0) {
                for (i = 0; i < rowIndexes.length; i++) {
                    var rowIndex = rowIndexes[i];
                    if (rowIndex === undefined) {
                        continue;
                    }
                    // se extrae el id de la fila    
                    rowId = grid.jqxGrid("getrowid", rowIndexes[i]);
                    if (rowId) {
                        // se extra la informacion
                        rowsData.push(grid.jqxGrid("getrowdatabyid", rowId));
                    }
                }
            }
            return rowsData;
        }

        /**
        * Funcion que crea una funcion de validacion para comprobar si un objeto Grid tiene seleccionado al menos un elemento
        * @param {object} grid - objeto grid a validar
        * @returns {bool} - true si se satisface la validacion, false en caso contrario
        */
        function ruleJqxGridSelectedRows(grid) {
            return function (dropDown) {
                if (grid.jqxGrid("getselectedrowindexes").length > 0) {
                    return true;
                } else {
                    return false;
                }
            };
        }

        /**
        * Funcion que crea una funcion de validacion para comprobar si un objeto Grid tiene al menos un elemento
        * @param {object} grid - objeto grid a validar
        * @returns {bool} - true si se satisface la validacion, false en caso contrario
        */
        function ruleJqxGridHasRows(grid) {
            return function (dropDown) {
                if (grid.jqxGrid("getrows").length > 0) {
                    return true;
                } else {
                    return false;
                }
            };
        }

        /**
        * Funcion para crear el grid buscador de profesores
        * @param {string} controller - nombre del controlador a utilizar
        * @param {string} action - nombre de la accion (metodo) del controlador a llamar
        * @param {string} boton -boton que despliega el grid buscador
        * @param {string} grid - grid desplegado para busqueda de profesores        
        * @param {function} [onSelect=undefined] - funcion opcional a llamar después de seleccionar un elemento
        * @param {function} [filters=undefined] - funcion que proporciona los filtros extra para la busqueda
        * @param {bool} [showAddColumn=undefined] - indica si se debe mostrar la columna con el boton agregar
        */
        function createGridBuscadorProfesores(controller, action, boton, grid, onSelect, filters, showAddColumn) {
            // campos y adaptador
            var fields = [
                    { name: colsBuscadorProfesor.emplid, type: "string" },
                    { name: colsBuscadorProfesor.nombre, type: "string" },
                    { name: colsBuscadorProfesor.cuentaDominio, type: "string" },
                    { name: colsBuscadorProfesor.claveDptoAdscripcion, type: "string" },
                    { name: colsBuscadorProfesor.dptoAdscripcion, type: "string" },
                    { name: colsBuscadorProfesor.claveDireccionAcademica, type: "string" },
                    { name: colsBuscadorProfesor.direccionAcademica, type: "string" }
            ],
                dataAdapter = generateGridAdapter(grid, controller, action, fields, filters, "Emplid"),
                form = getFormControl(boton);   // formulario que contiene el boton (necesario para ocultar el mensaje de error al seleccionar un elemento)

            // inicializacion del boton selector
            boton.jqxDropDownButton({ width: 250, height: 25, theme: getTema(), enableBrowserBoundsDetection: true });
            // inicializacion del jqxGrid
            grid.jqxGrid({
                width: 650,
                source: dataAdapter,
                pageable: true,
                autoheight: true,
                filterable: true,
                showfilterrow: true,
                theme: getTema(),
                selectionmode: (showAddColumn) ? "singlecell" : "singlerow",
                virtualmode: true,
                rendergridrows: function () {
                    return dataAdapter.records;
                },
                localization: getIdiomaGrid(),
                columns: [
                    {
                        text: "Agregar", datafield: "Agregar", columntype: "button", width: 70, hidden: !showAddColumn, cellsrenderer: function () {
                            return "Agregar";
                        }, buttonclick: function (clickRow) {
                            var row = grid.jqxGrid("getrowdata", clickRow);
                            if (onSelect) {
                                onSelect(row);
                            }
                        }
                    },
                    { text: "ID", datafield: colsBuscadorProfesor.emplid, width: 100, filtertype: "textbox" },
                    { text: "Nombre empleado", datafield: colsBuscadorProfesor.nombre, width: 250, filtertype: "textbox" },
                    { text: "Cuenta dominio" , datafield: colsBuscadorProfesor.cuentaDominio, width: 100, filtertype: "textbox" },
                    { text: "Clave Dpto. Adscripción", datafield: colsBuscadorProfesor.claveDptoAdscripcion, width: 200, filtertype: "textbox", hidden: true },
                    { text: "Dpto. Adscripción", datafield: colsBuscadorProfesor.dptoAdscripcion, width: 200, filtertype: "textbox" },
                    { text: "Clave Dirección Académica", datafield: colsBuscadorProfesor.claveDireccionAcademica, width: 200, hidden: true },
                    { text: "Dirección Académica", datafield: colsBuscadorProfesor.direccionAcademica, width: 200, filtertype: "textbox", hidden: true }
                ]
            });
            // evento seleccion de registro
            grid.on("rowselect", function (event) {
                var args = event.args,
                    row = grid.jqxGrid("getrowdata", args.rowindex),
                    dropDownContent = "";
                // si se selecciono una fila, se muestra su contenido en el boton
                if (row) {
                    setDropDownButtonGridValue(boton, row[colsBuscadorProfesor.nombre]);
                } else {
                    setDropDownButtonGridValue(boton, mensajes.seleccione);
                }
                // se limpia el mensaje de error si se ha seleccionado un elemento
                if (form && form.id) {
                    hideErrorMessage("#" + form.id, "#" + boton.prop("id"));
                }
                //llamado de la funcion
                if (onSelect) {
                    onSelect(row);
                }
                boton.jqxDropDownButton("close");
            });

            // evento de filtrado
            grid.on("filter", function (event) {
                // el parametro 'filter' evita que el txt de la fila de filtros pierda el foco cuando se esten filtrando los datos
                grid.jqxGrid("updatebounddata", "filter");
                grid.jqxGrid("clearselection");
            });
            // fila seleccionada por default
            grid.jqxGrid("clearselection");
            setDropDownButtonGridValue(boton, mensajes.seleccione);
        }

        /**
        * Funcion para obtener el formulario que contiene al control especificado
        * @param {object} control - control del cual se extraera el formulario
        * @returns {object} - objeto formulario
        */
        function getFormControl(control) {
            return control.closest("form")[0]; // se toma el primer formulario
        }

        /**
        * Funcion para poner un valor en el boton desplegable con grid
        * @param {object} button - boton donde se colocara el valor
        * @param {string} value - valor a mostrar en el boton
        */
        function setDropDownButtonGridValue(button, value) {
            var dropDownContent = '<div style="position: relative; margin-left: 0px; margin-top: 5px;">' + value + '</div>';
            button.jqxDropDownButton("setContent", dropDownContent);
        }

        /**
        * Funcion para ocultar el mensaje de error de un control especifico
        * @param {string} idForm - identificador del formulario que contiene el control (debe incluir el '#')
        * @param {string} idControl - identificador del control al que se le ocultara el mensaje (debe incluir el '#')
        */
        function hideErrorMessage(idForm, idControl) {
            $(idForm).jqxValidator("hideHint", idControl);
        }

        /**
        * Funcion de validacion para determinar si esta un elemento seleccionado (dropdownbutton)
        * @param {object} dropDown - objeto dropDown
        * @returns {bool} - true si tiene un elemento seleccionado, false en caso contrario
        */
        function ruleJqxDropDownButton(dropDown) {
            // trick para navegar en el contenido del boton y extraer su resultado
            var value = dropDown.children().children().children(":first").children().html();
            if (value === "" || value === mensajes.seleccione) {
                return false;
            } else {
                return true;
            }
        }



        /**
        * Funcion para abrir un reporte en una pestania aparte
        * @param {string} controller - nombre del controlador
        * @param {string} action - accion a ejecutar
        * @param {object} params - parametros extra
        */
        function generateReport(controller, action, params) {
            var queryString = "?";
            for (var prop in params) {
                var value = params[prop] != null ? params[prop] : "";
                queryString += prop + "=" + value + "&";
            }
            window.open(rutaPublicacion + controller + "/" + action + queryString, "_blank");
        }


        //function getStringEstatusCita(estatus) {
        //    var result = "";
        //    switch (estatus) {
        //        case 1:
        //            result = estatusCitas.pendiente;
        //            break;
        //        case 2:
        //            result = estatusCitas.agendada;
        //            break;
        //        case 40:
        //            result = estatusCitas.cancelada;
        //            break;
        //    }
        //    return result;
        //}


        return {
            getTema: getTema,
            getIdiomaGrid: getIdiomaGrid,
            ajaxRequest: ajaxRequest,
            respuestaEstatus: respuestaEstatus,
            redirect: redirect,
            generateComboAdapter: generateComboAdapter,
            onModalCreating: onModalCreating,
            estadoControlesModal: estadoControlesModal,
            accionModal: accionModal,
            generateLocalAdapter: generateLocalAdapter,
            generateGridAdapter: generateGridAdapter,
            getGridFilterParams: getGridFilterParams,
            generateGridLocalAdapter: generateGridLocalAdapter,
            getRowSelected: getRowSelected,
            mensajes: mensajes,
            setDropDownListValue: setDropDownListValue,
            mensajesValidaciones: mensajesValidaciones,
            ruleJqxDropDownList: ruleJqxDropDownList,
            selectRows: selectRows,
            getRowsSelected: getRowsSelected,
            ruleJqxGridSelectedRows: ruleJqxGridSelectedRows,
            createGridBuscadorProfesores: createGridBuscadorProfesores,
            ruleJqxDropDownButton: ruleJqxDropDownButton,
            setDropDownButtonGridValue: setDropDownButtonGridValue,
            ruleJqxGridHasRows: ruleJqxGridHasRows,
            getWindowDropDownListProperty: getWindowDropDownListProperty,
            generateGridAdapterSync: generateGridAdapterSync,

            generateReport: generateReport,
            estatusCitas: estatusCitas,
            //getStringEstatusCita: getStringEstatusCita,
        }

    }();

});