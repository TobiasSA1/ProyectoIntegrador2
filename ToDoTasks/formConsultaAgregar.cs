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
    public partial class formConsultaAgregar : Form
    {
        private ListaDeTareasManager listaDeTareasManager;
        private TareasManager tareasManager;

        /// <summary>
        /// Constructor por defecto del formulario <see cref="formConsultaAgregar"/>.
        /// Inicializa una nueva instancia del formulario.
        /// </summary>
        public formConsultaAgregar()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor con parámetros del formulario <see cref="formConsultaAgregar"/>.
        /// Inicializa una nueva instancia del formulario con los managers especificados.
        /// </summary>
        /// <param name="listaDeTareasManager">Instancia del manager de listas de tareas.</param>
        /// <param name="tareasManager">Instancia del manager de tareas.</param>
        public formConsultaAgregar(ListaDeTareasManager listaDeTareasManager, TareasManager tareasManager)
        {
            InitializeComponent();
            this.listaDeTareasManager = listaDeTareasManager;
            this.tareasManager = tareasManager;
        }

        private void formConsultaAgregar_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Método para agregar una lista.
        /// Abre el formulario para agregar listas y actualiza el estado del formulario principal si la operación es exitosa.
        /// </summary>
        private void AgregarLista()
        {
            formAgregarLista formAgregarLista = new formAgregarLista(listaDeTareasManager);

            if (formAgregarLista.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        /// <summary>
        /// Método para agregar una tarea.
        /// Abre el formulario para agregar o modificar tareas y actualiza el estado del formulario principal si la operación es exitosa.
        /// </summary>
        private void AgregarTarea()
        {
            frmListaDeTareasAgregarModificar formAgregarTarea = new frmListaDeTareasAgregarModificar(tareasManager);
            if (formAgregarTarea.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón de agregar tarea.
        /// Invoca el método para agregar una tarea.
        /// </summary>
        private void btnTarea_Click(object sender, EventArgs e)
        {
            AgregarTarea();
        }
        /// <summary>
        /// Evento que se ejecuta al hacer clic en el botón de agregar lista.
        /// Invoca el método para agregar una lista.
        /// </summary>
        private void btnLista_Click(object sender, EventArgs e)
        {
            AgregarLista();
        }

        private void formConsultaAgregar_Load_1(object sender, EventArgs e)
        {

        }

    }
}
