using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entidades
{

    public abstract class Tareas
    {
        public string Nombre { get; set; }
        public TipoTarea Tipo { get; set; }
        public TimeSpan Hora { get; set; }
        public Repeticion Repetir { get; set; }
        public EstadoTarea Estado { get; set; }
        public string Descripcion { get; set; }
        public virtual DateTime Fecha { get; set; }

        /// <summary>
        /// Constructor de la clase Tareas.
        /// </summary>
        /// <param name="nombre">Nombre de la tarea.</param>
        /// <param name="tipo">Tipo de la tarea (Rutinaria o Irregular).</param>
        /// <param name="hora">Hora de la tarea.</param>
        /// <param name="repetir">Frecuencia de repetición de la tarea.</param>
        /// <param name="descripcion">Descripción de la tarea.</param>
        /// <param name="estado">Estado inicial de la tarea.</param>

        public Tareas(string nombre, TipoTarea tipo, TimeSpan hora, Repeticion repetir, string descripcion, EstadoTarea estado)
        {
            SetNombre(nombre);
            SetTipo(tipo);
            SetHora(hora);
            SetRepetir(repetir);
            SetDescripcion(descripcion);
            SetEstado(estado);
        }
        /// <summary>
        /// Establece el nombre de la tarea.
        /// </summary>
        /// <param name="nombre">Nuevo nombre de la tarea.</param>

        public void SetNombre(string nombre)
        {
            ValidarTextoNoVacio(nombre, "nombre de la tarea");
            this.Nombre = nombre;
        }
        /// <summary>
        /// Establece el tipo de la tarea.
        /// </summary>
        /// <param name="tipo">Nuevo tipo de la tarea.</param>

        private void SetTipo(TipoTarea tipo)
        {
            this.Tipo = tipo;
        }
        /// <summary>
        /// Establece la hora de la tarea.
        /// </summary>
        /// <param name="hora">Nueva hora de la tarea.</param>

        public void SetHora(TimeSpan hora)
        {
            if (hora < TimeSpan.Zero || hora >= TimeSpan.FromDays(1))
            {
                throw new ArgumentException("La hora debe estar en el rango de 00:00 a 23:59.", nameof(hora));
            }

            this.Hora = hora;
        }
        /// <summary>
        /// Establece la frecuencia de repetición de la tarea.
        /// </summary>
        /// <param name="repetir">Nueva frecuencia de repetición de la tarea.</param>

        private void SetRepetir(Repeticion repetir)
        {
            this.Repetir = repetir;
        }

        /// <summary>
        /// Establece la descripción de la tarea.
        /// </summary>
        /// <param name="descripcion">Nueva descripción de la tarea.</param>
        private void SetDescripcion(string descripcion)
        {
            this.Descripcion = descripcion;
        }

        /// <summary>
        /// Establece el estado de la tarea.
        /// </summary>
        /// <param name="estado">Nuevo estado de la tarea.</param>
        private void SetEstado(EstadoTarea estado)
        {
            this.Estado = estado;
        }
        /// <summary>
        /// Valida que un texto no esté vacío o contenga solo espacios en blanco.
        /// </summary>
        /// <param name="texto">Texto a validar.</param>
        /// <param name="nombreCampo">Nombre del campo asociado al texto.</param>

        private static void ValidarTextoNoVacio(string texto, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                throw new ArgumentException($"El {nombreCampo} no puede estar vacío o contener solo espacios en blanco.", nameof(texto));
            }
        }

        /// <summary>
        /// Convierte el estado de la tarea a cadena.
        /// </summary>
        /// <returns>Cadena que representa el estado de la tarea.</returns>
        public string EstadoToString()
        {

            return this.Estado.ToString();
        }

        /// <summary>
        /// Devuelve una representación en cadena de la tarea.
        /// </summary>
        /// <returns>Cadena que representa la tarea.</returns>
        public override string ToString()
        {

            return $"(Tarea) - {this.Nombre}";
        }

        /// <summary>
        /// Convierte la hora de la tarea a una cadena con formato.
        /// </summary>
        /// <returns>Cadena que representa la hora de la tarea.</returns>
        public string HoraToString()
        {
            return Hora.ToString(@"hh\:mm");
        }

    }
}
