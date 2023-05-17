$(function () {

    if (!window.ITSON) {
        window.ITSON = {};
    }

    window.ITSON.Mensajes = function () {

        //Funcion que crea el div para mandar un mensaje a pantalla
        function Mensaje(msgTitulo, msgCuerpo) {
            var body = "<div class='modal fade' id='dMensaje' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>";
            body += "<div class='modal-dialog modal-dialog-centered' role='document'>";
            body += "<div class='modal-content'>";
            body += "<div class='modal-header'>";
            body += "<h5 class='modal-title' id='exampleModalCenterTitle'><b>" + msgTitulo + "</b></h5>";
            body += "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>";
            body += "<span aria-hidden='true'>&times;</span>";
            body += "</button>";
            body += "</div>";
            body += "<div class='modal-body'>";
            body += msgCuerpo;
            body += "</div>";
            body += "<div class='modal-footer'>";
            body += "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Cerrar</button>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            $(body).modal('show');
        }

        //Funcion que crea el div de cargando
        function Cargando(titulo) {
            var body = "<div class='modal' id='myModal' data-backdrop='static' data-keyboard='false'>";
            body += "<div class='modal-dialog modal-dialog-centered' role='document'>";
            body += "<div class='modal-content'>";
            body += "<div class='modal-header'>";
            body += "<h4 class='modal-title'>" + titulo + "</h4>";
            body += "</div>";
            body += "<div class='modal-body'>";
            body += "<div class='progress-bar progress-bar-striped progress-bar-animated' role='progressbar' aria-valuenow='75' aria-valuemin='0' aria-valuemax='100' style='width: 100%'>";
            body += "<div class='progress-bar' style='width: 100%'>";
            body += "</div>";
            body += "<div class='modal-footer'>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            return $(body);
        }

        //Funcion que crea el div para mandar un mensaje a pantalla
        function MensajeConfirmacion(msgTitulo, msgCuerpo, okCallBack) {
            var idConfirmar = "btnConfirmar";
            var body = "<div class='modal fade' id='dMensaje' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>";
            body += "<div class='modal-dialog modal-dialog-centered' role='document'>";
            body += "<div class='modal-content'>";
            body += "<div class='modal-header'>";
            body += "<h5 class='modal-title' id='exampleModalCenterTitle'><b>" + msgTitulo + "</b></h5>";
            body += "<button type='button' class='close' data-dismiss='modal' aria-label='Close'>";
            body += "<span aria-hidden='true'>&times;</span>";
            body += "</button>";
            body += "</div>";
            body += "<div class='modal-body'>";
            body += msgCuerpo;
            body += "</div>";
            body += "<div class='modal-footer'>";
            body += "<button type='button' class='btn btn-secondary' data-dismiss='modal'>Cancelar</button>";
            body += "<input type='button' id='"+idConfirmar+"'class='btn btn-primary' value='Confirmar'>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            body += "</div>";
            var dialogo = $(body);
            dialogo.modal('show');

            //Agregar funcion callback al boton despues de que el modal se haya mostrado en el navegador...
            dialogo.on("shown.bs.modal", function () {
                var btnConfirmar = $("#" + idConfirmar); 
                btnConfirmar.unbind();
                btnConfirmar.on("click", function (e) {
                    dialogo.modal("hide");
                    okCallBack();
                });
            });

            //Al cerrar el dialogo mato el boton de confirmacion
            dialogo.on('hidden.bs.modal', function () {
                $(this).data('bs.modal', null);
                $("#" + idConfirmar).remove();
            });
            
        }

        return {
            Mensaje: Mensaje,
            Cargando: Cargando,
            MensajeConfirmacion: MensajeConfirmacion
        };
    }();
});