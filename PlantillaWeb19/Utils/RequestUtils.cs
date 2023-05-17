using Entidades.Transporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PlantillaWeb19.Utils
{
    public static class RequestUtils
    {

        /// <summary>
        /// Metodo para convertir una lista de objetos en una lista de nueva de comboDTO
        /// </summary>
        /// <typeparam name="TEntity">Tipo de objeto a convertir</typeparam>
        /// <param name="list">Lista de </param>
        /// <param name="value">Nombre de la propiedad de la cual se extraera su identificador</param>
        /// <param name="label">Nombre de la propiedad de la cual se extraera su etiqueta</param>
        /// <returns>Lista de comboDTO</returns>
        public static IEnumerable<ComboDTO> ConvertToComboDTO<TEntity>(IEnumerable<TEntity> list, string value, string label) where TEntity : class
        {
            // extraccion del tipo y de sus propiedades
            Type type = typeof(TEntity);
            PropertyInfo valueProp = type.GetProperty(value);
            PropertyInfo labelProp = type.GetProperty(label);

            // validacion de las propiedades
            if (valueProp == null)
            {
                throw new ArgumentException("La propiedad 'value' no existe en la clase");
            }

            if (labelProp == null)
            {
                throw new ArgumentException("La propiedad 'label' no existe en la clase");
            }

            // lista
            IList<ComboDTO> listCombo = new List<ComboDTO>();

            object valueDTO = null;
            object labelDTO = null;

            // conversion
            foreach (TEntity obj in list)
            {
                valueDTO = valueProp.GetValue(obj, null);
                labelDTO = labelProp.GetValue(obj, null);

                listCombo.Add(new ComboDTO()
                {
                    Value = (valueDTO != null) ? valueDTO.ToString() : "",
                    Label = (labelDTO != null) ? labelDTO.ToString() : ""
                });
            }

            return listCombo;
        }

        /// <summary>
        /// Metodo de extension para obtener el valor de la descripcion de cada campo de un enumerador
        /// </summary>
        /// <param name="value">Valor del enumerador de donde se extraera la descripcion</param>
        /// <returns>Descripcion del campo o el valor de la propiedad convertida en cadane si el atributo 'Description' no esta especificado</returns>
        public static string GetDescription(this Enum value)
        {
            // tipo de datos
            Type type = value.GetType();
            // obtencion del atributo si existe, null en caso contrario
            DescriptionAttribute attr = (DescriptionAttribute)type.GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault();
            return (attr != null) ? attr.Description : value.ToString();
        }

        /// <summary>
        /// Metodo de extension para validar que si un archivo se subio al servidor
        /// </summary>
        /// <param name="file">Archivo a validar</param>
        /// <returns>True si el archivo existe, false en caso contrario</returns>
        public static bool HasFile(this HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para convertir los elementos de un enumerador en una lista de objetos de transporte 'ComboDTO'
        /// </summary>
        /// <typeparam name="TEnum">Tipo del enumerador a convertir</typeparam>
        /// <returns>Lista de elementos creada</returns>
        public static IEnumerable<ComboDTO> ConvertEnumToComboDTO<TEnum>()
        {
            Type type = typeof(TEnum);

            // si el tipo no es un Enum, aborta la conversion
            if (!type.IsEnum)
            {
                throw new ArgumentException("El tipo genérico 'TEnum' debe ser un enumerador");
            }

            IList<ComboDTO> combosList = new List<ComboDTO>();

            // se itera tomando cada valor valido del enumerador
            foreach (int v in Enum.GetValues(type))
            {
                // se convierte en un valor enum valido
                TEnum e = (TEnum)Enum.Parse(type, v.ToString());

                // se extra el atributo
                DescriptionAttribute attr = (DescriptionAttribute)type.GetField(e.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false).SingleOrDefault();

                // si existe, se toma su valor, sino se utiliza la conversion de string directamente del nombre de la propiedad
                ComboDTO cbo = new ComboDTO()
                {
                    Value = v.ToString(),
                    Label = (attr != null) ? attr.Description : e.ToString()
                };
                combosList.Add(cbo);
            }

            return combosList;
        }

    }
}