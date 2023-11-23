using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUnitarios
{
    public class Clases
    {
        public static void ValidarTextoNoVacio(string texto, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                throw new ArgumentException($"El {nombreCampo} no puede estar vacío o contener solo espacios en blanco.", nameof(texto));
            }
        } 

    }
}
