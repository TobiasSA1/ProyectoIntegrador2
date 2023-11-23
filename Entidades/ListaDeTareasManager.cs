using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Entidades
{
    public class ListaDeTareasManager : IListaDeTareasManager
    {
        public List<ListaDeTareas> ListasDeTareas { get; }

        public ListaDeTareasManager()
        {
            ListasDeTareas = new List<ListaDeTareas>();
        }

        /// <summary>
        /// Agrega una nueva lista de tareas al gestor.
        /// </summary>
        /// <param name="lista">Lista de tareas a agregar.</param>
        public void AgregarListaDeTareas(ListaDeTareas lista)
        {
            try
            {
                ListasDeTareas.Add(lista);
            }
            catch (Exception ex)
            {

                throw new Exception("Error al agregar una lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Modifica el nombre de una lista de tareas existente.
        /// </summary>
        /// <param name="listaExistente">Lista de tareas existente.</param>
        /// <param name="nuevoNombre">Nuevo nombre para la lista de tareas.</param>
        public void ModificarListaDeTareas(ListaDeTareas listaExistente, string nuevoNombre)
        {
            try
            {
                if (ListasDeTareas.Contains(listaExistente))
                {
                    // Modificar el nombre de la lista existente
                    listaExistente.SetNombre(nuevoNombre);
                }
                else
                {
                    throw new ArgumentException("La lista especificada no pertenece a la ListaDeTareasManager.", nameof(listaExistente));
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al modificar una lista de tareas.", ex);
            }
        }
        /// <summary>
        /// Elimina una tarea de una lista de tareas.
        /// </summary>
        /// <param name="lista">Lista de tareas de la cual eliminar la tarea.</param>
        /// <param name="tarea">Tarea a eliminar de la lista.</param>
        public void EliminarTareaDeLista(ListaDeTareas lista, Tareas tarea)
        {
            try
            {
                if (ListasDeTareas.Contains(lista))
                {
                    lista.EliminarTarea(tarea);
                }
                else
                {
                    throw new ArgumentException("La lista especificada no pertenece a la ListaDeTareasManager.", nameof(lista));
                }
            }
            catch (Exception ex)
            {


                throw new Exception("Error al eliminar una tarea de la lista de tareas.", ex);
            }
        }

        /// <summary>
        /// Elimina una lista de tareas del gestor.
        /// </summary>
        /// <param name="lista">Lista de tareas a eliminar.</param>
        public void EliminarListaDeTareas(ListaDeTareas lista)
        {
            try
            {
                ListasDeTareas.Remove(lista);
            }
            catch (Exception ex)
            {


                throw new Exception("Error al eliminar una lista de tareas.", ex);
            }
        }

        /// <summary>
        /// Verifica si el gestor contiene una lista de tareas específica.
        /// </summary>
        /// <param name="lista">Lista de tareas a verificar.</param>
        /// <returns>True si la lista de tareas está contenida en el gestor; de lo contrario, false.</returns>
        public bool ContieneListaDeTareas(ListaDeTareas lista)
        {
            try
            {
                return ListasDeTareas.Contains(lista);
            }
            catch (Exception ex)
            {


                throw new Exception("Error al verificar si la lista de tareas contiene una lista específica.", ex);
            }
        }
        /// <summary>
        /// Obtiene los días únicos de la semana de un conjunto de tareas.
        /// </summary>
        /// <param name="tareas">Lista de tareas.</param>
        /// <returns>Cadena que representa los días únicos de la semana.</returns>
        public string ObtenerDiasUnicos(List<Tareas> tareas)
        {
            HashSet<DayOfWeek> diasUnicos = new HashSet<DayOfWeek>();

            foreach (var tarea in tareas)
            {
                if (tarea is TareaRutinaria rutinaria)
                {
                    diasUnicos.UnionWith(rutinaria.diasSemanales);
                }
            }

            // Días de la semana en inglés
            DayOfWeek[] diasEnIngles = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            // Días de la semana en español
            string[] diasEnEspanol = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

            // Traducir los días únicos a español y unirlos en una cadena
            string diasFormateados = string.Join(", ", diasUnicos.Select(d => TraducirDia(d, diasEnIngles, diasEnEspanol)));

            return diasFormateados;
        }

        /// <summary>
        /// Traduce el día de la semana de inglés a español.
        /// </summary>
        /// <param name="dia">Día de la semana en inglés.</param>
        /// <param name="diasEnIngles">Array de días de la semana en inglés.</param>
        /// <param name="diasEnEspanol">Array de días de la semana en español.</param>
        /// <returns>Día de la semana traducido a español o el día original si no se encuentra en la lista de traducción.</returns>

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

        /// <summary>
        /// Obtiene la fecha formateada para una lista de tareas específica.
        /// </summary>
        /// <param name="listaDeTareas">Lista de tareas.</param>
        /// <returns>Cadena que representa la fecha formateada.</returns>
        public string ObtenerFechaFormateada(ListaDeTareas listaDeTareas)
        {
            try
            {
                switch (listaDeTareas.Repetir)
                {
                    case Repeticion.Ninguna:
                        return string.Join(", ", listaDeTareas.Tareas.Select(t => ObtenerFechaFormateadaTarea(t)));

                    case Repeticion.Diaria:
                        return "Todos los días.";

                    case Repeticion.Dias:
                        if (listaDeTareas.Tareas.Any(t => t is TareaRutinaria))
                        {
                            // Obtener días únicos de todas las tareas rutinarias
                            string diasUnicos = ObtenerDiasUnicos(listaDeTareas.Tareas.ToList());

                            return diasUnicos.TrimEnd(',', ' ');
                        }
                        return string.Empty;

                    case Repeticion.EnFechaEspecifica:
                        return string.Join(", ", listaDeTareas.Tareas.Select(t => ObtenerFechaFormateadaTarea(t)));

                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Puedes agregar un manejador de excepciones específico o simplemente lanzar la excepción nuevamente
                throw new Exception("Error al obtener la fecha formateada de la lista de tareas.", ex);
            }
        }

        /// <summary>
        /// Obtiene la fecha formateada para una lista de tareas específica.
        /// </summary>
        /// <param name="listaDeTareas">Lista de tareas.</param>
        /// <returns>Cadena que representa la fecha formateada.</returns>

        private static string ObtenerFechaFormateadaTarea(Tareas tarea)
        {
            try
            {
                if(tarea.Repetir == Repeticion.Ninguna) 
                {
                    return tarea.Fecha.ToString("dd/MM/yyyy");
                }
                return tarea.Fecha.ToString("dd");

            }
            catch (Exception ex)
            {

                throw new Exception("Error al obtener la fecha formateada de la tarea.", ex);
            }
        }
    }
}
