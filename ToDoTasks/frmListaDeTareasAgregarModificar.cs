using Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ToDoTasks
{
    public partial class frmListaDeTareasAgregarModificar : Form
    {

        private TareasManager tareasManager;

        private ListaDeTareasManager listadeTareasManager;

        private Tareas tareaSeleccionada;

        private ListaDeTareas listaSeleccionada;

        public event EventHandler<TareaAltaEventArgs> TareaAlta;

        public event EventHandler<TareaAltaEventArgs> TareaModificada;

        public event EventHandler<ListaAltaEventArgs> TareaModificadaEnLista;

        public frmListaDeTareasAgregarModificar(TareasManager tareasManager)
        {
            InitializeComponent();
            this.tareasManager = tareasManager;

            TareaAlta += MostrarMensajeAlta;
            TareaModificada += MostrarMensajeModificacion;
        }


        public frmListaDeTareasAgregarModificar(TareasManager tareasManager, Tareas tareaSeleccionada)
            : this(tareasManager)
        {
            this.tareaSeleccionada = tareaSeleccionada;

            // Verificar si la tarea seleccionada no es nula
            if (this.tareaSeleccionada is not null)
            {
                // Configurar los campos con los datos de la tarea seleccionada
                ConfigurarCamposTareaSeleccionada();
            }
        }

        public frmListaDeTareasAgregarModificar(TareasManager tareasManager, ListaDeTareas listaSeleccionada)
        : this(tareasManager)
        {
            this.listaSeleccionada = listaSeleccionada;

            if (this.listaSeleccionada is not null)
            {
                ConfigurarControlesDesdeListaSeleccionada();
            }
        }

        public frmListaDeTareasAgregarModificar(Tareas tarea, ListaDeTareasManager listaDeTareasManager, ListaDeTareas listaSeleccionada)
        {
            InitializeComponent();

            this.listaSeleccionada = listaSeleccionada;
            this.listadeTareasManager = listaDeTareasManager;
            this.tareaSeleccionada = tarea;

            TareaAlta += MostrarMensajeAlta;
            TareaModificada += MostrarMensajeModificacion;
            TareaModificadaEnLista += MostrarMensajeModificacionLista;

            //ESTE CONSTRUCTOR ES PARA MODIFICAR UNA TAREA DE LA LISTA SELECCIONADA.

            if (this.listaSeleccionada is not null && this.listadeTareasManager is not null && tareaSeleccionada is not null)
            {
                // Configurar controles desde la lista seleccionada
                ConfigurarControlesDesdeListaSeleccionada();

                ConfigurarCamposTareaSeleccionada();
            }
        }
        /// <summary>
        /// Configura los controles según la lista de tareas seleccionada, marcando y bloqueando los controles según el tipo de repetición.
        /// </summary>
        private void ConfigurarControlesDesdeListaSeleccionada()
        {
            if (listaSeleccionada is not null)
            {
                // Marcar el RadioButton correspondiente al Tipo
                if (listaSeleccionada.Tipo == TipoTarea.Rutinaria)
                {
                    RadioRutinaria.Checked = true;
                }
                else if (listaSeleccionada.Tipo == TipoTarea.Irregular)
                {
                    RadioIrregular.Checked = true;
                }

                // Marcar y bloquear los controles según el TipoRepeticion
                if (listaSeleccionada.Repetir == Repeticion.Dias)
                {
                    checkDias.Checked = true;
                    BloquearControlesDias();
                }
                else if (listaSeleccionada.Repetir == Repeticion.EnFechaEspecifica)
                {
                    checkFechaEspecifica.Checked = true;
                    BloquearControlesFechaEspecifica();
                }
                else if (listaSeleccionada.Repetir == Repeticion.Diaria)
                {
                    checkDiaria.Checked = true;
                    BloquearControlesDiaria();
                }

                RadioRutinaria.Enabled = false;
                RadioIrregular.Enabled = false;
                checkFechaEspecifica.Enabled = false;
                checkDiaria.Enabled = false;
                checkDias.Enabled = false;

            }
        }


        /// <summary>
        /// Configura los campos del formulario con los datos de la tarea actualmente seleccionada.
        /// </summary>
        private void ConfigurarCamposTareaSeleccionada()
        {
            richTextBoxNombreTareas.Text = tareaSeleccionada.Nombre;
            richTextBoxDescripcion.Text = tareaSeleccionada.Descripcion;
            maskedTextBoxHora.Text = tareaSeleccionada.Hora.ToString("hh\\:mm");

            ConfigurarCheckedListBoxDias();

            ActualizarInterfazSegunTarea();
        }

        /// <summary>
        /// Actualiza la interfaz gráfica según el tipo de tarea seleccionada.
        /// </summary>
        private void ActualizarInterfazSegunTarea()
        {

            if (tareaSeleccionada is TareaRutinaria tareaRutinaria)
            {
                RadioRutinaria.Checked = true;

                if (tareaRutinaria.Repetir == Repeticion.Dias)
                {
                    checkDias.Checked = true;

                }
                else if (tareaRutinaria.Repetir == Repeticion.EnFechaEspecifica)
                {
                    checkFechaEspecifica.Checked = true;

                }
                else if (tareaRutinaria.Repetir == Repeticion.Diaria)
                {
                    checkDiaria.Checked = true;

                }
            }
            else if (tareaSeleccionada is TareaIrregular)
            {
                RadioIrregular.Checked = true;

            }
        }

        /// <summary>
        /// Inicializa los elementos del formulario, como la lista de días, el rango mínimo de la fecha y la máscara del cuadro de texto de la hora.
        /// </summary>
        private void Inicializar()
        {
            checkedListBoxDias.Items.Add("Lunes");
            checkedListBoxDias.Items.Add("Martes");
            checkedListBoxDias.Items.Add("Miercoles");
            checkedListBoxDias.Items.Add("Jueves");
            checkedListBoxDias.Items.Add("Viernes");
            checkedListBoxDias.Items.Add("Sabado");
            checkedListBoxDias.Items.Add("Domingo");

            dateTimePicker.MinDate = DateTime.Today;
            maskedTextBoxHora.Mask = "99:99";
            maskedTextBoxHora.MaxLength = 5;
            //RadioIrregular.Checked = true;
        }

        /// <summary>
        /// Maneja el evento de foco en el cuadro de texto de la hora para seleccionar todo el contenido.
        /// </summary>
        private void maskedTextBoxHora_GotFocus(object sender, EventArgs e)
        {
            maskedTextBoxHora.Select(0, 0);
        }
        /// <summary>
        /// Maneja el evento de carga del formulario, llamando al método Inicializar y configurando el evento GotFocus para el cuadro de texto de la hora.
        /// </summary>
        private void frmListaDeTareasAgregarModificar_Load(object sender, EventArgs e)
        {
            Inicializar();

            maskedTextBoxHora.GotFocus += maskedTextBoxHora_GotFocus;

        }
        /// <summary>
        /// Maneja el evento de cambio en la selección del RadioButton Irregular, bloqueando los controles correspondientes.
        /// </summary>
        private void RadioIrregular_CheckedChanged_1(object sender, EventArgs e)
        {
            BloquearControlesIrregular();

        }
        /// <summary>
        /// Maneja el evento de cambio en la selección del RadioButton Rutinaria, desbloqueando todos los controles.
        /// </summary>
        private void RadioRutinaria_CheckedChanged(object sender, EventArgs e)
        {

            DesbloquearControles();

        }
        /// <summary>
        /// Maneja el evento de cambio en la selección del CheckBox Diaria, bloqueando o desbloqueando los controles correspondientes.
        /// </summary>
        private void CheckDiaria_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDiaria.Checked)
            {
                BloquearControlesDiaria();
            }
            else
            {
                DesbloquearControles();
            }
        }
        /// <summary>
        /// Maneja el evento de cambio en la selección del CheckBox FechaEspecifica, bloqueando o desbloqueando los controles correspondientes.
        /// </summary>
        private void CheckFechaEspecifica_CheckedChanged(object sender, EventArgs e)
        {
            if (checkFechaEspecifica.Checked)
            {
                BloquearControlesFechaEspecifica();
            }
            else
            {
                DesbloquearControles();
            }
        }
        /// <summary>
        /// Maneja el evento de cambio en la selección del CheckBox Dias, bloqueando o desbloqueando los controles correspondientes y desmarcando los días seleccionados.
        /// </summary>
        private void CheckDias_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDias.Checked)
            {
                BloquearControlesDias();
            }
            else
            {
                DesbloquearControles();

                foreach (int index in checkedListBoxDias.CheckedIndices)
                {
                    checkedListBoxDias.SetItemChecked(index, false);
                }
            }
        }

        /// <summary>
        /// Maneja el evento de clic en el botón "Cancelar", cerrando el formulario.
        /// </summary>
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Bloquea los controles para una tarea irregular, desmarcando los CheckBox correspondientes y deshabilitando los controles de selección.
        /// </summary>
        private void BloquearControlesIrregular()
        {
            // Bloquear controles para tipo irregular
            checkFechaEspecifica.Checked = false;
            checkDiaria.Checked = false;
            checkDias.Checked = false;

            checkFechaEspecifica.Enabled = false;
            checkDiaria.Enabled = false;
            checkDias.Enabled = false;
            checkedListBoxDias.Enabled = false;

        }
        /// <summary>
        /// Desbloquea los controles para una tarea rutinaria, habilitando los controles de selección según el tipo de repetición.
        /// </summary>
        private void DesbloquearControles()
        {
            // Desbloquear controles para tipo rutinario
            dateTimePicker.Enabled = true;
            checkFechaEspecifica.Enabled = true;
            checkDiaria.Enabled = true;
            checkDias.Enabled = true;
            checkedListBoxDias.Enabled = true;
        }
        /// <summary>
        /// Bloquea los controles para una tarea diaria, desmarcando el CheckBox de días y deshabilitando los controles de selección de días.
        /// </summary>
        private void BloquearControlesDiaria()
        {
            // Bloquear controles para tarea diaria
            checkFechaEspecifica.Enabled = false;
            dateTimePicker.Enabled = false;
            checkDias.Enabled = false;
            checkedListBoxDias.Enabled = false;

            foreach (int index in checkedListBoxDias.CheckedIndices)
            {
                checkedListBoxDias.SetItemChecked(index, false);
            }
        }
        /// <summary>
        /// Bloquea los controles para una tarea en fecha específica, desmarcando el CheckBox de días y deshabilitando los controles de selección de días.
        /// </summary>
        private void BloquearControlesFechaEspecifica()
        {
            // Bloquear controles para tarea en fecha específica
            checkDiaria.Enabled = false;
            checkDias.Enabled = false;
            dateTimePicker.Enabled = true;
            checkedListBoxDias.Enabled = false;

            foreach (int index in checkedListBoxDias.CheckedIndices)
            {
                checkedListBoxDias.SetItemChecked(index, false);
            }
        }
        /// <summary>
        /// Bloquea los controles para una tarea por días de la semana, desmarcando los CheckBox de fecha específica y diaria, y deshabilitando los controles de selección de días.
        /// </summary>
        private void BloquearControlesDias()
        {
            // Bloquear controles para tarea por días de la semana
            checkDiaria.Enabled = false;
            checkFechaEspecifica.Enabled = false;
            dateTimePicker.Enabled = false;
        }

        /// <summary>
        /// Agrega una nueva tarea al sistema según los datos ingresados por el usuario.
        /// </summary>
        private void AgregarTarea()
        {
            try
            {
                string nombre = ObtenerNombre();
                string descripcion = ObtenerDescripcion();
                TimeSpan hora = ObtenerHora();
                EstadoTarea estado = EstadoTarea.Pendiente;
                Repeticion repeticion;
                DateTime fechaSeleccionada;
                HashSet<DayOfWeek> dias = new HashSet<DayOfWeek>();

                if (RadioRutinaria.Checked)
                {
                    if (checkDias.Checked)
                    {
                        repeticion = Repeticion.Dias;
                        dias = ObtenerDiasSeleccionados();

                        var nuevaTarea = new TareaRutinaria(nombre, hora, repeticion, descripcion, estado, dias, DateTime.Today);


                        if (listaSeleccionada is not null)
                        {
                            listaSeleccionada.AgregarTarea(nuevaTarea);
                            TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                        else
                        {
                            tareasManager.AgregarTarea(nuevaTarea);
                            TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                    }
                    else if (checkFechaEspecifica.Checked)
                    {
                        repeticion = Repeticion.EnFechaEspecifica;
                        fechaSeleccionada = ObtenerFechaEspecifica();

                        var nuevaTarea = new TareaRutinaria(nombre, hora, repeticion, descripcion, estado, dias, fechaSeleccionada);

                        if (listaSeleccionada is not null)
                        {
                            listaSeleccionada.AgregarTarea(nuevaTarea);
                            TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                        else
                        {
                            tareasManager.AgregarTarea(nuevaTarea);
                            TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                    }
                    else if (checkDiaria.Checked)
                    {
                        repeticion = Repeticion.Diaria;
                        dias = ObtenerDiasDiarios();

                        var nuevaTarea = new TareaRutinaria(nombre, hora, repeticion, descripcion, estado, dias, DateTime.Today);

                        if (listaSeleccionada is not null)
                        {
                            listaSeleccionada.AgregarTarea(nuevaTarea);
                            TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                        else
                        {
                            tareasManager.AgregarTarea(nuevaTarea);
                            TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Por favor, seleccione una repetición para la tarea rutinaria.");
                    }
                }
                else if (RadioIrregular.Checked)
                {
                    fechaSeleccionada = ObtenerFechaIrregular();

                    var nuevaTarea = new TareaIrregular(nombre, hora, descripcion, estado, fechaSeleccionada);


                    if (listaSeleccionada is not null)
                    {
                        listaSeleccionada.AgregarTarea(nuevaTarea);
                        TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                    }

                    else
                    {
                        tareasManager.AgregarTarea(nuevaTarea);
                        TareaAlta?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                    }
                }

                //MessageBox.Show("Tarea agregada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ArgumentException ex)
            {
                // Manejar la excepción mostrando un mensaje de error específico
                MessageBox.Show($"Error al agregar la tarea: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Manejar otras excepciones mostrando un mensaje de error genérico
                MessageBox.Show($"Error al agregar la tarea: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Obtiene el nombre de la tarea desde el cuadro de texto y realiza validaciones.
        /// </summary>
        /// <returns>El nombre de la tarea.</returns>
        private string ObtenerNombre()
        {
            string nombre = richTextBoxNombreTareas.Text;
            ValidarNombre(nombre);
            return nombre;
        }
        /// <summary>
        /// Realiza la validación del nombre para asegurarse de que no exceda la longitud permitida.
        /// </summary>
        /// <param name="nombre">El nombre a validar.</param>
        private static void ValidarNombre(string nombre)
        {
            if (nombre.Length > 40)
            {
                throw new ArgumentException("El nombre no puede tener más de 40 caracteres.", nameof(nombre));
            }
        }
        /// <summary>
        /// Obtiene la descripción de la tarea desde el cuadro de texto.
        /// </summary>
        /// <returns>La descripción de la tarea.</returns>
        private string ObtenerDescripcion()
        {
            return richTextBoxDescripcion.Text;
        }
        /// <summary>
        /// Obtiene la hora de la tarea desde el cuadro de texto y realiza validaciones.
        /// </summary>
        /// <returns>La hora de la tarea.</returns>
        private TimeSpan ObtenerHora()
        {
            string tiempo = maskedTextBoxHora.Text;

            if (!TimeSpan.TryParse(tiempo, out TimeSpan hora))
            {
                throw new ArgumentException("El formato de la hora no es válido. Debe ser en formato HH:mm.", nameof(tiempo));
            }
            return hora;
        }
        /// <summary>
        /// Obtiene los días de la semana seleccionados por el usuario para tareas rutinarias.
        /// </summary>
        /// <returns>Conjunto de días de la semana seleccionados.</returns>
        private HashSet<DayOfWeek> ObtenerDiasSeleccionados()
        {
            Dictionary<string, DayOfWeek> mapeoDias = new Dictionary<string, DayOfWeek>
                {
                    { "Lunes", DayOfWeek.Monday },
                    { "Martes", DayOfWeek.Tuesday },
                    { "Miercoles", DayOfWeek.Wednesday },
                    { "Jueves", DayOfWeek.Thursday },
                    { "Viernes", DayOfWeek.Friday },
                    { "Sabado", DayOfWeek.Saturday },
                    { "Domingo", DayOfWeek.Sunday }
                };

            HashSet<DayOfWeek> dias = new HashSet<DayOfWeek>();

            // Validar que haya al menos un día seleccionado
            if (checkedListBoxDias.CheckedItems.Count == 0)
            {
                throw new ArgumentException("Seleccione al menos un día de la semana.", nameof(checkedListBoxDias));
            }

            foreach (string diaTexto in checkedListBoxDias.CheckedItems)
            {
                // Verifica si el nombre del día está en el mapeo
                if (mapeoDias.TryGetValue(diaTexto, out DayOfWeek dia))
                {
                    // Agrega el día al conjunto
                    dias.Add(dia);
                }
            }

            return dias;
        }
        /// <summary>
        /// Obtiene la fecha específica ingresada por el usuario.
        /// </summary>
        /// <returns>La fecha específica para la tarea rutinaria.</returns>
        private DateTime ObtenerFechaEspecifica()
        {
            return dateTimePicker.Value;
        }
        /// <summary>
        /// Obtiene un conjunto de días que representa los días diarios para tareas rutinarias.
        /// </summary>
        /// <returns>Conjunto de días diarios.</returns>
        private static HashSet<DayOfWeek> ObtenerDiasDiarios()
        {
            HashSet<DayOfWeek> diario = new HashSet<DayOfWeek>
            {
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday,
                DayOfWeek.Friday,
                DayOfWeek.Saturday,
                DayOfWeek.Sunday
            };
            return diario;
        }
        /// <summary>
        /// Obtiene la fecha ingresada por el usuario para tareas irregulares.
        /// </summary>
        /// <returns>La fecha específica para la tarea irregular.</returns>
        private DateTime ObtenerFechaIrregular()
        {
            return dateTimePicker.Value;
        }
        /// <summary>
        /// Maneja el evento de clic en el botón "Aceptar" para agregar o modificar una tarea.
        /// </summary>
        private void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tareaSeleccionada != null)
                {
                    ModificarTarea();
                }
                else
                {
                    AgregarTarea();
                }

                // Solo cerrar el formulario si la tarea se agrega o modifica exitosamente
                this.DialogResult = DialogResult.OK;

                //this.Close();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error, pero no cerrar el formulario
                MessageBox.Show($"Error al {(tareaSeleccionada != null ? "modificar" : "agregar")} la tarea: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Configura los elementos de la lista de días seleccionados según la tarea actualmente seleccionada.
        /// </summary>
        private void ConfigurarCheckedListBoxDias()
        {
            // Limpiar selecciones anteriores
            for (int i = 0; i < checkedListBoxDias.Items.Count; i++)
            {
                checkedListBoxDias.SetItemChecked(i, false);
            }

            // Configurar selecciones según la tarea seleccionada
            if (tareaSeleccionada is TareaRutinaria tareaRutinaria)
            {
                foreach (var dia in tareaRutinaria.diasSemanales)
                {
                    int index = ObtenerIndiceDia(dia);
                    // Verificar que el índice sea válido antes de intentar establecer el estado de verificación
                    if (index >= 0 && index < checkedListBoxDias.Items.Count)
                    {
                        checkedListBoxDias.SetItemChecked(index, true);
                    }
                }
            }
        }
        /// <summary>
        /// Obtiene el índice del día de la semana para la configuración del CheckedListBox.
        /// </summary>
        /// <param name="dia">Día de la semana.</param>
        /// <returns>Índice correspondiente en el CheckedListBox.</returns>
        private static int ObtenerIndiceDia(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return -1; // Día no válido
            }
        }
        /// <summary>
        /// Modifica una tarea existente según los datos ingresados por el usuario.
        /// </summary>
        private void ModificarTarea()
        {
            try
            {
                // Obtener los datos actuales de la tarea seleccionada
                string nombre = ObtenerNombre();
                string descripcion = ObtenerDescripcion();
                TimeSpan hora = ObtenerHora();
                EstadoTarea estado = tareaSeleccionada.Estado; // Mantener el estado actual
                Repeticion repeticion;
                DateTime fechaSeleccionada;
                HashSet<DayOfWeek> dias = new HashSet<DayOfWeek>();

                if (RadioRutinaria.Checked)
                {
                    if (checkDias.Checked)
                    {
                        repeticion = Repeticion.Dias;
                        dias = ObtenerDiasSeleccionados();
                        Tareas nuevaTarea = new TareaRutinaria(nombre, hora, repeticion, descripcion, estado, dias, DateTime.Today);

                        if (tareasManager is not null)
                        {
                            tareasManager.EliminarTarea(tareaSeleccionada);
                            tareasManager.AgregarTarea(nuevaTarea);
                            TareaModificada?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                        else
                        {
                            listadeTareasManager.EliminarTareaDeLista(listaSeleccionada, tareaSeleccionada);
                            listaSeleccionada.AgregarTarea(nuevaTarea);
                            TareaModificadaEnLista?.Invoke(this, new ListaAltaEventArgs(listaSeleccionada));
                        }
                    }
                    else if (checkFechaEspecifica.Checked)
                    {
                        repeticion = Repeticion.EnFechaEspecifica;
                        fechaSeleccionada = ObtenerFechaEspecifica();
                        Tareas nuevaTarea = new TareaRutinaria(nombre, hora, repeticion, descripcion, estado, dias, fechaSeleccionada);

                        if (tareasManager is not null)
                        {
                            tareasManager.EliminarTarea(tareaSeleccionada);
                            tareasManager.AgregarTarea(nuevaTarea);
                            TareaModificada?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                        else
                        {
                            listadeTareasManager.EliminarTareaDeLista(listaSeleccionada, tareaSeleccionada);
                            listaSeleccionada.AgregarTarea(nuevaTarea);
                            TareaModificadaEnLista?.Invoke(this, new ListaAltaEventArgs(listaSeleccionada));
                        }

                    }
                    else if (checkDiaria.Checked)
                    {
                        repeticion = Repeticion.Diaria;
                        dias = ObtenerDiasDiarios();
                        Tareas nuevaTarea = new TareaRutinaria(nombre, hora, repeticion, descripcion, estado, dias, DateTime.Today);

                        if (tareasManager is not null)
                        {
                            tareasManager.EliminarTarea(tareaSeleccionada);
                            tareasManager.AgregarTarea(nuevaTarea);
                            TareaModificada?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                        }
                        else
                        {
                            listadeTareasManager.EliminarTareaDeLista(listaSeleccionada, tareaSeleccionada);
                            listaSeleccionada.AgregarTarea(nuevaTarea);
                            TareaModificadaEnLista?.Invoke(this, new ListaAltaEventArgs(listaSeleccionada));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Por favor, seleccione una repetición para la tarea rutinaria.");
                    }
                }
                else if (RadioIrregular.Checked)
                {
                    repeticion = Repeticion.Ninguna;

                    fechaSeleccionada = ObtenerFechaIrregular();
                    Tareas nuevaTarea = new TareaIrregular(nombre, hora, descripcion, estado, fechaSeleccionada);

                    if (tareasManager is not null)
                    {
                        tareasManager.EliminarTarea(tareaSeleccionada);
                        tareasManager.AgregarTarea(nuevaTarea);
                        TareaModificada?.Invoke(this, new TareaAltaEventArgs(nuevaTarea));
                    }
                    else
                    {
                        listadeTareasManager.EliminarTareaDeLista(listaSeleccionada, tareaSeleccionada);
                        listaSeleccionada.AgregarTarea(nuevaTarea);
                        TareaModificadaEnLista?.Invoke(this, new ListaAltaEventArgs(listaSeleccionada));
                    }
                }
                else
                {
                    throw new ArgumentException("Por favor, seleccione un tipo de tarea.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar la tarea: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Muestra un mensaje de alta para la tarea.
        /// </summary>
        private void MostrarMensajeAlta(object sender, TareaAltaEventArgs e)
        {

            MessageBox.Show($"La TAREA | '{e.Tarea.Nombre}' dada de alta.", "Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Muestra un mensaje de modificacion de la tarea
        /// </summary>
        private void MostrarMensajeModificacion(object sender, TareaAltaEventArgs e)
        {

            MessageBox.Show($"La TAREA | '{e.Tarea.Nombre}' modificada.", "Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Muestra un mensaje de modificación para la tarea en la lista
        /// </summary>
        private void MostrarMensajeModificacionLista(object sender,ListaAltaEventArgs lista)
        {

            MessageBox.Show($"fue modificada la tarea en la LISTA | '{lista.Lista.Nombre}'.", "Éxito!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Argumentos para el evento de alta de tarea.
    /// </summary>
    public class TareaAltaEventArgs : EventArgs
    {
        public Tareas Tarea { get; }

        public TareaAltaEventArgs(Tareas tarea)
        {
            Tarea = tarea;
        }
    }

    /// <summary>
    /// Argumentos para el evento de modificación de lista.
    /// </summary>
    public class ListaAltaEventArgs : EventArgs
    {
        public ListaDeTareas Lista { get; }

        public ListaAltaEventArgs(ListaDeTareas listaDeTareas)
        {
            Lista = listaDeTareas;
        }
    }
}
