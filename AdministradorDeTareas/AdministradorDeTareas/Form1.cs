using System;
// Se utiliza la librería Diagnostics para leer el uso de los programas y el sistema en general
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdministradorDeTareas
{
    // La clase principal del formulario que hereda de MetroFramework.Forms.MetroForm para la interfaz moderna
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent(); // Inicializa los componentes del formulario
            UpdateProcessList(); // Llama al método que actualiza la lista de procesos
            timer1.Enabled = true; // Activa el temporizador para la actualización automática
        }

        // Método que actualiza la lista de procesos en el DataGridView
        private void UpdateProcessList()
        {
            dgvAdmin.Rows.Clear(); // Limpia las filas anteriores del DataGridView
            foreach (Process p in Process.GetProcesses()) // Recorre todos los procesos en ejecución
            {
                int n = dgvAdmin.Rows.Add(); // Añade una nueva fila en el DataGridView
                // Asigna los valores de los procesos a las celdas de la fila
                dgvAdmin.Rows[n].Cells[0].Value = p.ProcessName; // Nombre del proceso
                dgvAdmin.Rows[n].Cells[1].Value = p.Id; // ID del proceso
                dgvAdmin.Rows[n].Cells[2].Value = p.WorkingSet64; // Memoria física en uso (bytes)
                dgvAdmin.Rows[n].Cells[3].Value = p.VirtualMemorySize64; // Memoria virtual en uso (bytes)
                dgvAdmin.Rows[n].Cells[4].Value = p.SessionId; // ID de la sesión del proceso
            }
            // Actualiza el contador de procesos visibles en el DataGridView
            txtContador.Text = "Procesos actuales ejecutando: " + dgvAdmin.Rows.Count.ToString();
        }

        // Evento que se dispara cuando se presiona el botón "Actualizar"
        private void btnActu_Click(object sender, EventArgs e)
        {
            UpdateProcessList(); // Llama al método para actualizar la lista de procesos
        }

        // Evento que se dispara cuando se presiona el botón "Finalizar" para terminar un proceso
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                // Busca y finaliza el proceso cuyo nombre coincide con el texto ingresado
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName == txtProcess.Text) // Verifica si el nombre del proceso coincide
                    {
                        p.Kill(); // Mata el proceso si lo encuentra
                    }
                }
            }
            catch (Exception x) // Captura cualquier excepción que ocurra al intentar matar el proceso
            {
                // Muestra un mensaje de error si ocurre alguna excepción
                MessageBox.Show("No se ha seleccionado ningun proceso " + x, "Error al eliminar proceso", MessageBoxButtons.OK);
            }
        }

        // Evento que se dispara cuando se presiona el botón "Salir"
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close(); // Cierra la aplicación
        }

        // Evento que ocurre cuando se hace clic en una celda del DataGridView
        private void dgvAdmin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Al hacer clic en una fila, el nombre del proceso se muestra en el cuadro de texto
            txtProcess.Text = dgvAdmin.CurrentRow.Cells[0].Value.ToString();
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {
            // Método vacío para manejo del clic en una etiqueta, si fuera necesario
        }

        // Evento que se dispara con el temporizador (timer) para actualizar el uso de CPU y RAM
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Obtiene el valor de uso del CPU y RAM a partir de contadores de rendimiento
            float fcpu = pCPU.NextValue();
            float fram = pRAM.NextValue();

            // Actualiza las barras de progreso para mostrar el uso de CPU y RAM
            metroProgressBarCPU.Value = (int)fcpu;
            metroProgressBarRAM.Value = (int)fram;

            // Actualiza las etiquetas de texto con el porcentaje de uso
            lblCPU.Text = string.Format("{0:0.00%}", fcpu);
            lblRAM.Text = string.Format("{0:0.00%}", fram);

            // Añade los valores a las series del gráfico para mostrar el historial del uso
            chart1.Series["CPU"].Points.AddY(fcpu);
            chart1.Series["RAM"].Points.AddY(fram);
        }
    }
}
