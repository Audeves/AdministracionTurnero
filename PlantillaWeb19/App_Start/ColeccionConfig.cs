using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PlantillaWeb19.App_Start
{
    public class ColeccionConfig
    {

        public static void RegistrarColecciones(BundleCollection bundles)
        {
            //Estilos template
            bundles.Add(new StyleBundle("~/css")
                .Include("~/Template/css/all.min.css")
                .Include("~/Template/css/bootstrap.min.css")
                .Include("~/Template/css/sb-admin-2.css")
                );

            bundles.Add(new StyleBundle("~/css/jqwidgets")
                .Include("~/Template/css/jqwidgets/jqx.windowsphone.css")
                .Include("~/Template/css/jqwidgets/jqx.android.css")
                .Include("~/Template/css/jqwidgets/jqx.arctic.css")
                //.Include("~/Template/css/jqwidgets/jqx.base.css")
                .Include("~/Template/css/jqwidgets/jqx.black.css")
                .Include("~/Template/css/jqwidgets/jqx.blackberry.css")
                //.Include("~/Template/css/jqwidgets/jqx.bootstrap.css")
                .Include("~/Template/css/jqwidgets/jqx.classic.css")
                .Include("~/Template/css/jqwidgets/jqx.dark.css")
                .Include("~/Template/css/jqwidgets/jqx.darkblue.css")
                .Include("~/Template/css/jqwidgets/jqx.energyblue.css")
                .Include("~/Template/css/jqwidgets/jqx.flat.css")
                .Include("~/Template/css/jqwidgets/jqx.fresh.css")
                .Include("~/Template/css/jqwidgets/jqx.glacier.css")
                .Include("~/Template/css/jqwidgets/jqx.highcontrast.css")
                .Include("~/Template/css/jqwidgets/jqx.light.css")
                .Include("~/Template/css/jqwidgets/jqx.material-green.css")
                .Include("~/Template/css/jqwidgets/jqx.material-purple.css")
                .Include("~/Template/css/jqwidgets/jqx.material.css")
                .Include("~/Template/css/jqwidgets/jqx.metro.css")
                .Include("~/Template/css/jqwidgets/jqx.metrodark.css")
                .Include("~/Template/css/jqwidgets/jqx.mobile.css")
                .Include("~/Template/css/jqwidgets/jqx.office.css")
                .Include("~/Template/css/jqwidgets/jqx.orange.css")
                .Include("~/Template/css/jqwidgets/jqx.shinyblack.css")
                .Include("~/Template/css/jqwidgets/jqx.summer.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-darkness.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-le-frog.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-lightness.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-overcast.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-redmond.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-smoothness.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-start.css")
                .Include("~/Template/css/jqwidgets/jqx.ui-sunny.css")
                .Include("~/Template/css/jqwidgets/jqx.web.css")
                );

            bundles.Add(new ScriptBundle("~/js/template")
                .Include("~/Template/js/jquery-3.1.1.min.js")
                //.Include("~/Scripts/Librerias/jquery-3.1.1.min.js")
                .Include("~/Template/js/bootstrap.min.js")
                .Include("~/Template/js/sb-admin-2.js")
                );
            bundles.Add(new ScriptBundle("~/js/appLogin")
                .Include("~/Scripts/App/General/utils.js")
                .Include("~/Scripts/App/General/mensajes.js")
                .Include("~/Scripts/App/General/login.js")
                );
            bundles.Add(new ScriptBundle("~/js/app")
                .Include("~/Scripts/App/General/utils.js")
                .Include("~/Scripts/App/General/mensajes.js")
                .Include("~/Scripts/App/General/layoutPage.js")
                );


            //Individuales por pantalla...
            bundles.Add(new ScriptBundle("~/js/modulos")
                .Include("~/Scripts/App/Administracion/modulo.js")
                );
            bundles.Add(new ScriptBundle("~/js/perfiles")
                .Include("~/Scripts/App/Administracion/perfil.js")
                );
            bundles.Add(new ScriptBundle("~/js/usuarios")
                .Include("~/Scripts/App/Administracion/usuario.js")
                );
            bundles.Add(new ScriptBundle("~/js/pantallas")
                .Include("~/Scripts/App/Administracion/pantallas.js")
                );



            bundles.Add(new ScriptBundle("~/js/areasAtencion")
                .Include("~/Scripts/App/Configuracion/areasAtencion.js")
                );

            bundles.Add(new ScriptBundle("~/js/procesos")
                .Include("~/Scripts/App/Configuracion/procesos.js")
                );
            bundles.Add(new ScriptBundle("~/js/tramites")
                .Include("~/Scripts/App/Configuracion/tramites.js")
                );
            bundles.Add(new ScriptBundle("~/js/turneros")
                .Include("~/Scripts/App/Configuracion/turneros.js")
                );
            bundles.Add(new ScriptBundle("~/js/registroPantallas")
                .Include("~/Scripts/App/Configuracion/registroPantallas.js")
                );
            bundles.Add(new ScriptBundle("~/js/modulosAtencion")
                .Include("~/Scripts/App/Configuracion/modulosAtencion.js")
                );


            bundles.Add(new ScriptBundle("~/js/atencionModulo")
                .Include("~/Scripts/App/Principales/atencionModulo.js")
                );




            bundles.Add(new ScriptBundle("~/js/administracionSolicitudesCita")
                .Include("~/Scripts/App/Citas/administracionSolicitudesCita.js")
                );

            bundles.Add(new ScriptBundle("~/js/atencionCitasHoy")
                .Include("~/Scripts/App/Citas/atencionCitasHoy.js")
                );




            bundles.Add(new ScriptBundle("~/js/lib")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcore.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcore.elements.js")
                .Include("~/Scripts/Librerias/jquery.validate.min.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqx-all.js")
                .Include("~/Scripts/Librerias/jqwidgets/globalization/globalize.js")
                .Include("~/Scripts/Librerias/jqwidgets/globalization/globalize.culture.es-MX.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxangular.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxbargauge.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxbulletchart.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxbuttongroup.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxbuttons.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcalendar.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxchart.annotations.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxchart.api.js")
                //.Include("~/Scripts/Librerias/jqwidgets/jqxchart.core.js")
                //.Include("~/Scripts/Librerias/jqwidgets/jqxchart.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxchart.rangeselector.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxchart.waterfall.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcheckbox.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcolorpicker.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcombobox.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcomplexinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcore.elements.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxcore.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdata.export.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdata.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdatatable.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdate.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdatetimeinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdocking.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdockinglayout.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdockpanel.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdragdrop.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdraw.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdropdownbutton.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxdropdownlist.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxeditor.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxexpander.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxfileupload.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxform.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxformattedinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgauge.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.aggregates.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.columnsreorder.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.columnsresize.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.edit.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.export.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.filter.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.grouping.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.pager.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.selection.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.sort.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxgrid.storage.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxheatmap.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxkanban.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxknob.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxknockout.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxlayout.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxlistbox.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxlistmenu.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxloader.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxmaskedinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxmaterialcolorpicker.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxmenu.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxnavbar.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxnavigationbar.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxnotification.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxnumberinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxpanel.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxpasswordinput.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxpivot.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxpivotdesigner.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxpivotgrid.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxpopover.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxprogressbar.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxradiobutton.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxrangeselector.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxrating.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxresponse.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxresponsivepanel.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxribbon.js")
                //.Include("~/Scripts/Librerias/jqwidgets/jqxscheduler.api.js")
                //.Include("~/Scripts/Librerias/jqwidgets/jqxscheduler.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxscrollbar.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxscrollview.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxslider.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxsortable.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxsplitter.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxswitchbutton.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtabs.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtagcloud.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtextarea.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtimepicker.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtoolbar.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtooltip.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtouch.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtree.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtreegrid.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxtreemap.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxvalidator.js")
                .Include("~/Scripts/Librerias/jqwidgets/jqxwindow.js")
                );

            //bundles.Add(new StyleBundle("")
            //    .
            //    );

            //BundleTable.EnableOptimizations = true;
        }

    }
}