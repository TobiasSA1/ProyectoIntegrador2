using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Entidades;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Entidades
{
    public class TareasManager : ITareaManager
    {


        public List<Tareas> Tareas { get; }

        public TareasManager()
        {
            this.Tareas = new List<Tareas>();
        }

        /// <summary>
        /// Agrega una nueva tarea a la lista de tareas gestionadas.
        /// </summary>
        /// <param name="tarea">Tarea que se agregará a la lista.</param>

        public void AgregarTarea(Tareas tarea)
        {
            this.Tareas.Add(tarea);
        }
        /// <summary>
        /// Busca una tarea por su nombre en la lista de tareas gestionadas.
        /// </summary>
        /// <param name="nombre">Nombre de la tarea a buscar.</param>
        /// <returns>La tarea encontrada o null si no se encuentra.</returns>

        private Tareas BuscarTareaPorNombre(string nombre)
        {
            return this.Tareas.FirstOrDefault(t => t.Nombre == nombre);
        }
        /// <summary>
        /// Elimina una tarea de la lista de tareas gestionadas.
        /// </summary>
        /// <param name="tarea">Tarea que se eliminará de la lista.</param>

        public void EliminarTarea(Tareas tarea)
        {
            // Encuentra la tarea en la lista y la elimina
            var tareaExistente = this.Tareas.FirstOrDefault(t => t.Nombre == tarea.Nombre);

            if (tareaExistente != null)
            {
                Tareas.Remove(tareaExistente);
            }
        }
        /// <summary>
        /// Marca una tarea como completa.
        /// </summary>
        /// <param name="tarea">Tarea que se marcará como completa.</param>

        public void MarcarCompleto(Tareas tarea)
        {

            if (tarea.Estado == EstadoTarea.Completo)
            {
                MarcarPendiente(tarea);

            }
            else
            {
                tarea.Estado = EstadoTarea.Completo;
            }
        }

        /// <summary>
        /// Marca una tarea como pendiente.
        /// </summary>
        /// <param name="tarea">Tarea que se marcará como pendiente.</param>

        public void MarcarPendiente(Tareas tarea)
        {
            tarea.Estado = EstadoTarea.Pendiente;
        }
        /// <summary>
        /// Marca una tarea como incompleta basándose en su estado y fecha.
        /// </summary>
        /// <param name="tarea">Tarea que se marcará como incompleta.</param>

        public void MarcarIncompleto(Tareas tarea)
        {
            if (tarea.Estado != EstadoTarea.Completo)
            {
                tarea.Estado = (EsHoy(tarea) && tarea.Hora <= DateTime.Now.TimeOfDay) ? EstadoTarea.Incompleto : EstadoTarea.Pendiente;
            }
        }
        /// <summary>
        /// Verifica si una tarea debe realizarse hoy.
        /// </summary>
        /// <param name="tarea">Tarea que se verificará.</param>
        /// <returns>True si la tarea debe realizarse hoy, False en caso contrario.</returns>

        public bool EsHoy(Tareas tarea)
        {
            if (tarea is TareaRutinaria rutinaria && rutinaria.diasSemanales.Contains(DateTime.Today.DayOfWeek))
            {
                return true;
            }
            var fechaActual = DateTime.Today;
            return tarea.Fecha.Date == fechaActual.Date;
        }
        /// <summary>
        /// Verifica si una tarea es recurrente.
        /// </summary>
        /// <param name="tarea">Tarea que se verificará.</param>
        /// <returns>True si la tarea es recurrente, False en caso contrario.</returns>
        public bool EsRecurrente(Tareas tarea)
        {
            return tarea.Repetir != Repeticion.Ninguna;
        }
        /// <summary>
        /// Verifica si una tarea es recurrente y debe realizarse hoy.
        /// </summary>
        /// <param name="tarea">Tarea que se verificará.</param>
        /// <returns>True si la tarea es recurrente y debe realizarse hoy, False en caso contrario.</returns>
        public bool EsRecurrenteEnDia(Tareas tarea)
        {
            if (!EsRecurrente(tarea))
            {
                if (EsHoy(tarea))
                {
                    return true;
                }
            }
            switch (tarea.Repetir)
            {
                case Repeticion.Diaria:
                    return true;

                case Repeticion.Dias:
                    return tarea is TareaRutinaria rutinaria && rutinaria.diasSemanales.Contains(DateTime.Today.DayOfWeek);

                case Repeticion.EnFechaEspecifica:

                    return tarea.Fecha.Date == DateTime.Today.Date;

                default:
                    return false;
            }
        }
        /// <summary>
        /// Obtiene la fecha formateada de una tarea según su repetición.
        /// </summary>
        /// <param name="tarea">Tarea de la cual se obtendrá la fecha formateada.</param>
        /// <returns>Fecha formateada de la tarea.</returns>

        public string ObtenerFechaFormateada(Tareas tarea)
        {
            switch (tarea.Repetir)
            {
                case Repeticion.Ninguna:
                    return tarea.Fecha.ToString("dd/MM/yyyy");

                case Repeticion.Diaria:
                    return "Todos los dias.";

                case Repeticion.Dias:
                    if (tarea is TareaRutinaria rutinaria)
                    {
                        var dias = rutinaria.MostrarDiasSemana();
                        return string.Join(", ", dias);
                    }
                    return string.Empty;

                case Repeticion.EnFechaEspecifica:
                    return tarea.Fecha.ToString("dd");

                default:
                    return string.Empty;
            }
        }

    }
}
