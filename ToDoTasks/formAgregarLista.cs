using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoTasks
{
    public partial class formAgregarLista : Form
    {
        private ListaDeTareasManager listaDeTareasManager;

        private ListaDeTareas listaSeleccionada;
        /// <summary>
        /// Constructor del formulario <see cref="formAgregarLista"/>.
        /// Inicializa una nueva instancia del formulario para agregar listas.
        /// </summary>
        /// <param name="listaDeTareasManager">Instancia del manager de listas de tareas.</param>

        public formAgregarLista(ListaDeTareasManager listaDeTareasManager)
        {
            InitializeComponent();

            this.listaDeTareasManager = listaDeTareasManager;

        }
        /// <summary>
        /// Constructor del formulario <see cref="formAgregarLista"/> utilizado para modificar una lista existente.
        /// Inicializa una nueva instancia del formulario con la lista seleccionada.
        /// </summary>
        /// <param name="listaSeleccionada">Lista de tareas seleccionada para modificación.</param>
        /// <param name="listaDeTareasManager">Instancia del manager de listas de tareas.</param>

        public formAgregarLista(ListaDeTareas listaSeleccionada, ListaDeTareasManager listaDeTareasManager) : this(listaDeTareasManager)
        {

            this.listaSeleccionada = listaSeleccionada;

            if (this.listaSeleccionada is not null)
            {


                ConfigurarCamposListaSeleccionada();
                BloquearControlesParaModificacion();
            }
        }
        /// <summary>
        /// Configura los campos del formulario según la lista seleccionada para modificación.
        /// </summary>

        private void ConfigurarCamposListaSeleccionada()
        {
            richTextBoxNombreLista.Text = listaSeleccionada.Nombre;

            ActualizarInterfazSegunLista();
        }
        /// <summary>
        /// Bloquea los controles del formulario para la modificación de listas.
        /// </summary>

        private void BloquearControlesParaModificacion()
        {
            // Bloquear todos los controles de configuración, excepto el nombre
            radioRutinaria.Enabled = false;
            radioIrregular.Enabled = false;
            checkFechaEspecifica.Enabled = false;
            checkDiaria.Enabled = false;
            checkDias.Enabled = false;
        }
        /// <summary>
        /// Actualiza la interfaz del formulario según la lista seleccionada.
        /// </summary>

        private void ActualizarInterfazSegunLista()
        {

            if (listaSeleccionada.Tipo == TipoTarea.Rutinaria)
            {
                radioRutinaria.Checked = true;

                if (listaSeleccionada.Repetir == Repeticion.Dias)
                {
                    checkDias.Checked = true;

                }
                else if (listaSeleccionada.Repetir == Repeticion.EnFechaEspecifica)
                {
                    checkFechaEspecifica.Checked = true;

                }
                else if (listaSeleccionada.Repetir == Repeticion.Diaria)
                {
                    checkDiaria.Checked = true;

                }
            }
            else if (listaSeleccionada.Tipo == TipoTarea.Irregular)
            {
                radioIrregular.Checked = true;

            }
        }


        private void formAgregarLista_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Bloquea los controles para el tipo de tarea "Irregular".
        /// </summary>
        private void BloquearControlesIrregular()
        {

            checkFechaEspecifica.Enabled = false;
            checkDiaria.Enabled = false;
            checkDias.Enabled = false;
        }
        /// <summary>
        /// Desbloquea los controles para el tipo de tarea "Rutinaria".
        /// </summary>
        private void DesbloquearControlesRutinaria()
        {
            checkFechaEspecifica.Enabled = true;
            checkDiaria.Enabled = true;
            checkDias.Enabled = true;
        }
        /// <summary>
        /// Maneja el evento de cambio de selección del radio de tipo "Irregular".
        /// </summary>
        private void RadioIrregular_CheckedChanged(object sender, EventArgs e)
        {
            BloquearControlesIrregular();
        }
        /// <summary>
        /// Maneja el evento de cambio de selección del radio de tipo "Rutinaria".
        /// </summary>
        private void RadioRutinaria_CheckedChanged(object sender, EventArgs e)
        {
            DesbloquearControlesRutinaria();
        }
        /// <summary>
        /// Maneja el evento de cambio de estado del checkbox de tarea diaria.
        /// </summary>
        private void CheckDiaria_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDiaria.Checked)
            {
                BloquearControlesDiaria();
            }
            else
            {
                DesbloquearControlesRutinaria();
            }
        }
        /// <summary>
        /// Maneja el evento de cambio de estado del checkbox de fecha específica.
        /// </summary>
        private void CheckFechaEspecifica_CheckedChanged(object sender, EventArgs e)
        {
            if (checkFechaEspecifica.Checked)
            {
                BloquearControlesFechaEspecifica();
            }
            else
            {
                DesbloquearControlesRutinaria();
            }
        }
        /// <summary>
        /// Maneja el evento de cambio de estado del checkbox de días de la semana.
        /// </summary>
        private void CheckDias_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDias.Checked)
            {
                BloquearControlesDias();
            }
            else
            {
                DesbloquearControlesRutinaria();
            }
        }
        /// <summary>
        /// Bloquea los controles para una tarea diaria.
        /// </summary>
        private void BloquearControlesDiaria()
        {
            checkFechaEspecifica.Enabled = false;
            checkDias.Enabled = false;
        }
        /// <summary>
        /// Bloquea los controles para una tarea en fecha específica.
        /// </summary>
        private void BloquearControlesFechaEspecifica()
        {
            // Bloquear controles para tarea en fecha específica
            checkDiaria.Enabled = false;
            checkDias.Enabled = false;
        }
        /// <summary>
        /// Bloquea los controles para una tarea por días de la semana.
        /// </summary>
        private void BloquearControlesDias()
        {
            // Bloquear controles para tarea por días de la semana
            checkDiaria.Enabled = false;
            checkFechaEspecifica.Enabled = false;
        }


        /// <summary>
        /// Maneja el evento de clic en el botón para agregar o modificar la lista de tareas.
        /// </summary>
        private void btnAgregarLista_Click(object sender, EventArgs e)
        {
            try
            {
                AgregarLista();

                // Solo cerrar el formulario si la tarea se agrega exitosamente
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error, pero no cerrar el formulario
                MessageBox.Show($"Error al agregar la tarea: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Maneja el evento de clic en el botón para cancelar la operación y cerrar el formulario.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Obtiene el nombre ingresado para la lista desde el campo de texto.
        /// </summary>
        /// <returns>Nombre de la lista.</returns>
        private string ObtenerNombreLista()
        {
            string nombre = richTextBoxNombreLista.Text;
            ValidarNombreLista(nombre);
            return nombre;
        }
        /// <summary>
        /// Valida que el nombre de la lista no esté vacío o nulo.
        /// </summary>
        /// <param name="nombre">Nombre de la lista.</param>
        private static void ValidarNombreLista(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException("El nombre de la lista no puede estar vacío.", nameof(nombre));
            }
        }
        /// <summary>
        /// Obtiene el tipo de tarea seleccionado en el formulario.
        /// </summary>
        /// <returns>Tipo de tarea (Rutinaria o Irregular).</returns>
        private TipoTarea ObtenerTipoTarea()
        {
            return radioRutinaria.Checked ? TipoTarea.Rutinaria : TipoTarea.Irregular;
        }
        /// <summary>
        /// Obtiene el tipo de repetición seleccionado en el formulario.
        /// </summary>
        /// <returns>Tipo de repetición (EnFechaEspecifica, Diaria, Dias o Ninguna).</returns>

        private Repeticion ObtenerRepeticion()
        {
            if (checkFechaEspecifica.Checked)
            {
                return Repeticion.EnFechaEspecifica;
            }
            else if (checkDiaria.Checked)
            {
                return Repeticion.Diaria;
            }
            else if (checkDias.Checked)
            {
                return Repeticion.Dias;
            }
            else
            {
                // Puedes ajustar esto según tus necesidades, por ejemplo, lanzar una excepción
                return Repeticion.Ninguna;
            }
        }
        /// <summary>
        /// Agrega o modifica una lista de tareas según la información ingresada en el formulario.
        /// </summary>

        private void AgregarLista()
        {
            try
            {
                string nombre = ObtenerNombreLista();
                TipoTarea tipo = ObtenerTipoTarea();
                Repeticion repeticion = ObtenerRepeticion();

                var nuevaLista = new ListaDeTareas(nombre, tipo, repeticion);

                if (listaSeleccionada is not null)
                {
                    listaDeTareasManager.ModificarListaDeTareas(listaSeleccionada, nombre);
                }
                else
                {
                    listaDeTareasManager.AgregarListaDeTareas(nuevaLista);
                }

                MessageBox.Show("Lista de tareas agregada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException ex)
            {
                // Manejar la excepción mostrando un mensaje de error específico
                MessageBox.Show($"Error al agregar la lista de tareas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones mostrando un mensaje de error genérico
                MessageBox.Show($"Error al agregar la lista de tareas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
