using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades
{
    public class TareaRutinaria : Tareas
    {
        /// <summary>
        /// Días de la semana en los que se repite la tarea.
        /// </summary>
        public HashSet<DayOfWeek> diasSemanales { get; set; }

        /// <summary>
        /// Constructor de la clase TareaRutinaria.
        /// </summary>
        /// <param name="nombre">Nombre de la tarea rutinaria.</param>
        /// <param name="hora">Hora de la tarea rutinaria.</param>
        /// <param name="repetir">Frecuencia de repetición de la tarea rutinaria.</param>
        /// <param name="descripcion">Descripción de la tarea rutinaria.</param>
        /// <param name="estado">Estado inicial de la tarea rutinaria.</param>
        /// <param name="dias">Días de la semana en los que se repite la tarea rutinaria.</param>
        /// <param name="fecha">Fecha específica para tareas rutinarias con repetición en fecha específica.</param>
        public TareaRutinaria(string nombre, TimeSpan hora, Repeticion repetir, string descripcion, EstadoTarea estado, HashSet<DayOfWeek> dias, DateTime fecha)
            : base(nombre, TipoTarea.Rutinaria, hora, repetir, descripcion, estado)
        {
            try
            {
                // Validar que la repetición sea correcta para una tarea rutinaria
                if (repetir != Repeticion.Dias && repetir != Repeticion.Diaria && repetir != Repeticion.EnFechaEspecifica)
                {
                    throw new ArgumentException("La repetición no es válida para una tarea rutinaria.", nameof(repetir));
                }

                this.diasSemanales = dias;

                if (repetir == Repeticion.EnFechaEspecifica)
                {
                   SetFecha(fecha);

                }
                else
                {
                    this.Fecha = DateTime.MinValue;
                }
            }
            catch (ArgumentException ex)
            {

                throw new ArgumentException("Error al crear una tarea rutinaria.", ex);
            }
        }

        /// <summary>
        /// Establece la fecha específica para tareas rutinarias con repetición en fecha específica.
        /// </summary>
        /// <param name="fecha">Nueva fecha para la tarea rutinaria.</param>
        public void SetFecha(DateTime fecha)
        {
            try
            {
                if (fecha < DateTime.Today)
                {
                    throw new ArgumentException("La fecha no puede ser anterior a la fecha actual.", nameof(fecha));
                }

                this.Fecha = fecha;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Error al establecer la fecha de la tarea irregular.", ex);
            }
        }

        /// <summary>
        /// Devuelve una cadena que representa los días de la semana en los que se repite la tarea rutinaria.
        /// </summary>
        /// <returns>Cadena con los días de la semana de la tarea rutinaria.</returns>

        public string MostrarDiasSemana()
        {
            try
            {
                if (base.Repetir == Repeticion.Ninguna)
                {
                    return string.Empty;
                }

                if (base.Repetir == Repeticion.Diaria)
                {
                    return "Diaria";
                }

                // Días de la semana en inglés
                DayOfWeek[] diasEnIngles = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
                // Días de la semana en español
                string[] diasEnEspanol = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

                // Obtiene los días directamente de la propiedad
                HashSet<DayOfWeek> diasHashSet = this.diasSemanales;

                // Traducción y formateo de días
                var diasTraducidos = diasEnIngles.Where(dia => diasHashSet.Contains(dia))
                                                 .Select(dia => TraducirDia(dia, diasEnIngles, diasEnEspanol));

                return string.Join(", ", diasTraducidos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al mostrar los días de la semana de la tarea rutinaria.", ex);
            }
        }
        /// <summary>
        /// Traduce un día de la semana de inglés a español.
        /// </summary>
        /// <param name="dia">Día de la semana en inglés.</param>
        /// <param name="diasEnIngles">Array de días de la semana en inglés.</param>
        /// <param name="diasEnEspanol">Array de días de la semana en español.</param>
        /// <returns>Día de la semana traducido a español.</returns>

        private static string TraducirDia(DayOfWeek dia, DayOfWeek[] diasEnIngles, string[] diasEnEspanol)
        {
            try
            {
                int indice = Array.IndexOf(diasEnIngles, dia);

                if (indice >= 0 && indice < diasEnEspanol.Length)
                {
                    return diasEnEspanol[indice];
                }

                // Si no se encuentra en la lista de traducción, se devuelve el día original
                return dia.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al traducir el día de la tarea rutinaria.", ex);
            }
        }
    }
}
