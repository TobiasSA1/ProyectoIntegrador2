using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades 
{

    public class ListaDeTareas
    {
        private readonly List<Tareas> tareas;
        public string Nombre { get; private set; }
        public TipoTarea Tipo { get; private set; }
        public IReadOnlyList<Tareas> Tareas => tareas;
        public Repeticion Repetir { get; private set; }

        /// <summary>
        /// Crea una nueva instancia de la clase ListaDeTareas.
        /// </summary>
        /// <param name="nombre">Nombre de la lista de tareas.</param>
        /// <param name="tipo">Tipo de la lista de tareas.</param>
        /// <param name="repetir">Repetición de la lista de tareas.</param>

        public ListaDeTareas(string nombre, TipoTarea tipo, Repeticion repetir)
        {
            try
            {
                SetNombre(nombre);
                SetTipo(tipo);
                SetRepetir(repetir);
                this.tareas = new List<Tareas>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear una lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Agrega una tarea a la lista de tareas.
        /// </summary>
        /// <param name="tarea">Tarea a agregar.</param>

        public void AgregarTarea(Tareas tarea)
        {
            try
            {
                this.tareas.Add(tarea);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar una tarea a la lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Elimina una tarea de la lista de tareas.
        /// </summary>
        /// <param name="tarea">Tarea a eliminar.</param>

        public void EliminarTarea(Tareas tarea)
        {
            try
            {
                if (tareas.Contains(tarea))
                {
                    tareas.Remove(tarea);
                }
                else
                {
                    throw new ArgumentException("La tarea especificada no pertenece a esta lista de tareas.", nameof(tarea));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar una tarea de la lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Verifica si la lista de tareas contiene una tarea específica.
        /// </summary>
        /// <param name="tarea">Tarea a verificar.</param>
        /// <returns>True si la lista de tareas contiene la tarea, false en caso contrario.</returns>

        public bool ContieneTarea(Tareas tarea)
        {
            try
            {
                return this.tareas.Contains(tarea);
            }
            catch (Exception ex)
            {

                throw new Exception("Error al verificar si la lista de tareas contiene una tarea específica.", ex);
            }
        }
        /// <summary>
        /// Establece el nombre de la lista de tareas.
        /// </summary>
        /// <param name="nombre">Nombre a establecer.</param>

        public void SetNombre(string nombre)
        {
            try
            {
                ValidarTextoNoVacio(nombre, "nombre de la lista");
                this.Nombre = nombre;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al establecer el nombre de la lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Establece el tipo de la lista de tareas.
        /// </summary>
        /// <param name="tipo">Tipo a establecer.</param>

        protected void SetTipo(TipoTarea tipo)
        {
            try
            {
                this.Tipo = tipo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al establecer el tipo de la lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Establece la repetición de la lista de tareas.
        /// </summary>
        /// <param name="repetir">Repetición a establecer.</param>

        private void SetRepetir(Repeticion repetir)
        {
            try
            {
                // Puedes agregar validaciones adicionales según sea necesario.
                this.Repetir = repetir;
            }
            catch (Exception ex)
            {

                throw new Exception("Error al establecer la repetición de la lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Valida que el texto no esté vacío.
        /// </summary>
        /// <param name="texto">Texto a validar.</param>
        /// <param name="nombreCampo">Nombre del campo que se está validando.</param>

        private static void ValidarTextoNoVacio(string texto, string nombreCampo)
        {
            try
            {
                if (string.IsNullOrEmpty(texto))
                {
                    throw new ArgumentException($"El {nombreCampo} no puede estar vacío.", nameof(texto));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar que el texto no esté vacío.", ex);
            }
        }
        /// <summary>
        /// Obtiene la representación de cadena de la lista de tareas.
        /// </summary>
        /// <returns>Cadena que representa la lista de tareas.</returns>

        public override string ToString()
        {
            try
            {
                return $"(Lista de Tareas) - {this.Nombre}";
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la representación de cadena de la lista de tareas.", ex);
            }
        }
    }
}

