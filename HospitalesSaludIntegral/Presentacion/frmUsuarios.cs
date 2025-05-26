using HospitalesSaludIntegral.Datos;
using HospitalesSaludIntegral.Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalesSaludIntegral
{
    public partial class frmUsuarios : Form
    {
        CLusuarios cl_usuarios = new CLusuarios();
        CDusuarios cd_usuarios = new CDusuarios();
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            lblFecha.Text = cl_usuarios.MtdFechaHoy().ToString("d");
            MtdConsultarUsuarios(); //carga los datos al dgv
            CargarCboxEmpleados();  //carga el cboxEmpleados
            cboxEstado.SelectedIndex = 0; // Selecciona la fila vacía por defecto en el ComboBox Estado
            cboxTipoUsuario.SelectedIndex = 0; // Selecciona la fila vacía por defecto en el ComboBox TipoUsuario

        }

        private void CargarCboxEmpleados() //carga los datos al cbox
        {
            DataTable tblEmpleados = cd_usuarios.MtdConsultarEmpleados();

            // Fila vacía al inicio para que aparezca en blanco el cbox
            DataRow filaVacia = tblEmpleados.NewRow(); // Crea la fila vacia
            filaVacia["CodigoEmpleado"] = 0;
            filaVacia["Nombres"] = "Seleccione...";
            tblEmpleados.Rows.InsertAt(filaVacia, 0);

            cboxCodigoEmpleado.DataSource = tblEmpleados; // Asignamos la tabla tblEmpleados al ComboBox
            cboxCodigoEmpleado.DisplayMember = "Nombres"; //Se muestan los nombre pero rl valor interno es el codigo del empleado
            cboxCodigoEmpleado.ValueMember = "CodigoEmpleado";
            cboxCodigoEmpleado.SelectedIndex = 0;   // Selecciona la fila vacía por defecto
        }

        private void MtdConsultarUsuarios()
        {
            DataTable DtUsuarios = cd_usuarios.MtdConsultarUsuarios();
            dgvUsuarios.DataSource = DtUsuarios;
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxTipoUsuario.Text) || string.IsNullOrEmpty(cboxEstado.Text) || cboxCodigoEmpleado.SelectedIndex <= 0 || string.IsNullOrEmpty(txtpassword.Text))
            {
                
                Validartxt(txtusuario);
                Validartxt(txtpassword);
                Validarcbox(cboxTipoUsuario);
                Validarcbox(cboxEstado);
                Validarcbox(cboxCodigoEmpleado);

                MessageBox.Show("Favor completar formulario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (cd_usuarios.MtdUsuarioExistente(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue))) //Valida que no tenga usuario
                {
                    MessageBox.Show("El empleado ya tiene un usuario asignado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.SelectedValue);
                string Usuario = txtusuario.Text;
                string Clave = txtpassword.Text;
                string TipoUsuario = cboxTipoUsuario.Text;
                string Estado = cboxEstado.Text;
                DateTime FechaAuditoria = cl_usuarios.MtdFechaHoy();
                string UsuarioAuditoria = "alguien";

                try
                {
                    cd_usuarios.MtdAgregarUsuarios(CodigoEmpleado, Usuario, Clave, TipoUsuario, Estado, FechaAuditoria, UsuarioAuditoria);
                    MessageBox.Show("Datos agregados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarUsuarios();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Validartxt(Control control) // Verifica si el txt está vacío y de ser negativo cambia a rojo
        {
            control.BackColor = string.IsNullOrWhiteSpace(control.Text) ? Color.LightCoral : Color.White;
        }

        private void Validarcbox(ComboBox combo) // Verifica si el cbox está vacío o no tiene una selección válida y de ser negativo cambia a rojo
        {
            combo.BackColor = combo.SelectedIndex <= 0 ? Color.LightCoral : Color.White; 
        }

        private void MtdLimpiarCampos() // Limpia los campos del formulario
        {
            cboxCodigoEmpleado.SelectedIndex = 0;
            txtusuario.Clear();
            txtpassword.Clear();
            cboxTipoUsuario.SelectedIndex = 0;
            cboxEstado.SelectedIndex = 0;
            txtusuario.Focus(); // Coloca el cursor en el primer campo de texto
            txtCodigoUsuario.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvUsuarios.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvUsuarios.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila con datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtCodigoUsuario.Text = FilaSeleccionada.Cells["CodigoUsuario"].Value.ToString();
                txtpassword.Text = FilaSeleccionada.Cells["Clave"].Value.ToString();
                cboxTipoUsuario.Text = FilaSeleccionada.Cells["TipoUsuario"].Value.ToString();
                cboxEstado.Text = FilaSeleccionada.Cells["Estado"].Value.ToString();
                txtusuario.Text = FilaSeleccionada.Cells["Usuario"].Value.ToString();
                cboxCodigoEmpleado.SelectedValue = Convert.ToInt32(FilaSeleccionada.Cells["CodigoEmpleado"].Value);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxTipoUsuario.Text) || string.IsNullOrEmpty(cboxEstado.Text) || cboxCodigoEmpleado.SelectedIndex <= 0 || string.IsNullOrEmpty(txtpassword.Text) || string.IsNullOrEmpty(txtCodigoUsuario.Text))
            {

                Validartxt(txtusuario);
                Validartxt(txtpassword);
                Validarcbox(cboxTipoUsuario);
                Validarcbox(cboxEstado);
                Validarcbox(cboxCodigoEmpleado);

                if (string.IsNullOrEmpty(txtCodigoUsuario.Text))
                {
                    MessageBox.Show("Favor seleccionar la fila a editar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Favor completar formulario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    int CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text);
                    int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.SelectedValue);
                    string Usuario = txtusuario.Text;
                    string Clave = txtpassword.Text;
                    string TipoUsuario = cboxTipoUsuario.Text;
                    string Estado = cboxEstado.Text;
                    DateTime FechaAuditoria = cl_usuarios.MtdFechaHoy();
                    string UsuarioAuditoria = "Admin";

                    cd_usuarios.MtdActualizarUsuarios(CodigoUsuario,CodigoEmpleado, Usuario, Clave, TipoUsuario, Estado, FechaAuditoria, UsuarioAuditoria);
                    MessageBox.Show("Datos Actualizados Correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarUsuarios();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoUsuario.Text))
            {
                MessageBox.Show("Favor seleccionar fila a eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int CodigoUsuario = int.Parse(txtCodigoUsuario.Text);

                try
                {
                    cd_usuarios.MtdEliminarUsuarios(CodigoUsuario);
                    MessageBox.Show("Datos eliminados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarUsuarios();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
