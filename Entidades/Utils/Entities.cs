using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Entidades.Utils
{
    public static class Entities
    {
        public static T Clone<T>(this T source) 
        {
            var serializer = new DataContractSerializer(typeof(T));
            using(var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, source);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(memoryStream);
            }
        }
    }
}
