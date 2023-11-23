using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades
{
    public class TareaIrregular : Tareas
    {
        /// <summary>
        /// Obtiene o establece la fecha específica de la tarea irregular.
        /// </summary>
        public override DateTime Fecha { get; set; }

        /// <summary>
        /// Constructor de la clase TareaIrregular.
        /// </summary>
        /// <param name="nombre">Nombre de la tarea irregular.</param>
        /// <param name="hora">Hora de la tarea irregular.</param>
        /// <param name="descripcion">Descripción de la tarea irregular.</param>
        /// <param name="estado">Estado inicial de la tarea irregular.</param>
        /// <param name="fecha">Fecha específica de la tarea irregular.</param>
        public TareaIrregular(string nombre, TimeSpan hora, string descripcion, EstadoTarea estado, DateTime fecha)
            : base(nombre, TipoTarea.Irregular, hora, Repeticion.Ninguna, descripcion, estado)
        {
            try
            {
                SetFecha(fecha);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Error al crear una tarea irregular.", ex);
            }
        }

        /// <summary>
        /// Establece la fecha específica para tareas irregulares.
        /// </summary>
        /// <param name="fecha">Nueva fecha para la tarea irregular.</param>
        private void SetFecha(DateTime fecha)
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
    }
}
