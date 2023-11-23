
namespace TestUnitarios
{
    [TestClass]
    public class ValidarTextoNoVacioTests
    {
        [TestMethod]
        public void ValidarTextoNoVacio_TextoNoVacio_NoDeberiaLanzarExcepcion()
        {
            // Arrange
            string texto = "Ejemplo";
            string nombreCampo = "NombreCampo";

        Clases.ValidarTextoNoVacio(texto, nombreCampo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidarTextoNoVacio_TextoVacio_DeberiaLanzarExcepcion()
        {
            // Arrange
            string texto = string.Empty;
            string nombreCampo = "NombreCampo";

            Clases.ValidarTextoNoVacio(texto, nombreCampo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidarTextoNoVacio_TextoNulo_DeberiaLanzarExcepcion()
        {
            // Arrange
            string texto = null;
            string nombreCampo = "NombreCampo";

            Clases.ValidarTextoNoVacio(texto, nombreCampo);
        }

    }
}