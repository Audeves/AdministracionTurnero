using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Entidades.Enumeradores;

namespace Entidades.Utils
{
   
    public static class EnumeradoresUtilsStatic
    {
        /// <summary>
        /// Metodo de extension para obtener el valor de la descripcion de cada campo de un enumerador
        /// </summary>
        /// <param name="value">Valor del enumerador de donde se extraera la descripcion</param>
        /// <returns>Descripcion del campo o el valor de la propiedad convertida en cadane si el atributo 'Description' no esta especificado</returns>
        public static string GetDescriptionEnum(this Enum value)
        {
            // tipo de datos
            Type type = value.GetType();
            // obtencion del atributo si existe, null en caso contrario
            DescriptionAttribute attr = (DescriptionAttribute)type.GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault();
            return (attr != null) ? attr.Description : value.ToString();
        }
    }
}
