using HospitalesSaludIntegral.Datos;
using HospitalesSaludIntegral.Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalesSaludIntegral.Presentacion
{
    public partial class frmMedicamentos: Form
    {
        CDmedicamentos cd_medicamentos = new CDmedicamentos();
        CLmedicamentos cl_medicamentos = new CLmedicamentos();
        public frmMedicamentos()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtCodigoEmpleado_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MtdConsultarMedicamentos()
        {
            DataTable DtPagoPlanilla = cd_medicamentos.MtdConsultarMedicamentos();
            dgvMedicamentos.DataSource = DtPagoPlanilla;
        }
        private void cboxTipoMedicamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCosto.Text = cl_medicamentos.MtdCostoMedicamento(cboxTipoMedicamento.SelectedItem.ToString()).ToString("C", new CultureInfo("es-GT"));
        }

        private void frmMedicamentos_Load(object sender, EventArgs e)
        {
            MtdConsultarMedicamentos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Validartxt(txtStock);
            Validartxt(txtNombre);
            Validarcbox(cboxEstado);
            Validarcbox(cboxTipoMedicamento);

            if (cboxEstado.SelectedIndex <= 0 || cboxTipoMedicamento.SelectedIndex <= 0 || string.IsNullOrEmpty(txtStock.Text) || string.IsNullOrEmpty(txtNombre.Text))
            {

                MessageBox.Show("Favor completar formulario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string Nombre = txtNombre.Text;
                string TipoMedicamento = cboxTipoMedicamento.Text;
                int CostoMedicamento = cl_medicamentos.MtdCostoMedicamento(TipoMedicamento);
                DateTime FechaVencimiento = DtpFechaVencimiento.Value;
                string Estado = cboxEstado.Text;
                int Stock = int.Parse(txtStock.Text);
                DateTime FechaAuditoria = cl_medicamentos.MtdFechaHoy();
                string UsuarioAuditoria = "alguien";

                try
                {
                    cd_medicamentos.MtdAgregarMedicamentos(Nombre, TipoMedicamento, CostoMedicamento, Stock, FechaVencimiento, Estado, UsuarioAuditoria, FechaAuditoria);
                    MtdConsultarMedicamentos();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void MtdLimpiarCampos()
        {
            txtNombre.Clear();
            cboxTipoMedicamento.SelectedIndex = 0;
            cboxEstado.SelectedIndex = 0;
            txtStock.Clear();
            DtpFechaVencimiento.Value = cl_medicamentos.MtdFechaHoy();
            lblCosto.Text = "0.00";
            txtCodigoMedicamento.Clear();
        }
        private void Validartxt(Control control) // Verifica si el txt está vacío y de ser negativo cambia a rojo
        {
            control.BackColor = string.IsNullOrWhiteSpace(control.Text) ? Color.LightCoral : Color.White;
        }

        private void Validarcbox(ComboBox combo) // Verifica si el cbox está vacío o no tiene una selección válida y de ser negativo cambia a rojo
        {
            combo.BackColor = combo.SelectedIndex <= 0 ? Color.LightCoral : Color.White;
        }

        private void dgvMedicamentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvMedicamentos.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvMedicamentos.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila con datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cboxTipoMedicamento.Text = FilaSeleccionada.Cells["TipoMedicamento"].Value.ToString();
                DtpFechaVencimiento.Value = Convert.ToDateTime(FilaSeleccionada.Cells["FechaVencimiento"].Value);
                cboxEstado.Text = FilaSeleccionada.Cells["Estado"].Value.ToString();
                txtCodigoMedicamento.Text = FilaSeleccionada.Cells["CodigoMedicamento"].Value.ToString();
                txtStock.Text = FilaSeleccionada.Cells["Stock"].Value.ToString();
                txtNombre.Text = FilaSeleccionada.Cells["Nombre"].Value.ToString();

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Validartxt(txtStock);
            Validartxt(txtNombre);
            Validarcbox(cboxEstado);
            Validarcbox(cboxTipoMedicamento);

            if (cboxEstado.SelectedIndex <= 0 || cboxTipoMedicamento.SelectedIndex <= 0 || string.IsNullOrEmpty(txtStock.Text) || string.IsNullOrEmpty(txtNombre.Text))
            {

                MessageBox.Show("Favor completar formulario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int CodigoMedicamento = int.Parse(txtCodigoMedicamento.Text);
                string Nombre = txtNombre.Text;
                string TipoMedicamento = cboxTipoMedicamento.Text;
                int CostoMedicamento = cl_medicamentos.MtdCostoMedicamento(TipoMedicamento);
                DateTime FechaVencimiento = DtpFechaVencimiento.Value;
                string Estado = cboxEstado.Text;
                int Stock = int.Parse(txtStock.Text);
                DateTime FechaAuditoria = cl_medicamentos.MtdFechaHoy();
                string UsuarioAuditoria = "alguien";

                try
                {
                    cd_medicamentos.MtdActualizarMedicamentos(CodigoMedicamento, Nombre, TipoMedicamento, CostoMedicamento, Stock, FechaVencimiento, Estado, UsuarioAuditoria, FechaAuditoria);
                    MtdConsultarMedicamentos();
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
            if (string.IsNullOrEmpty(txtCodigoMedicamento.Text))
            {
                MessageBox.Show("Favor seleccionar fila a eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int CodigoMedicamento = int.Parse(txtCodigoMedicamento.Text);

                try
                {
                    cd_medicamentos.MtdEliminarMedicamentos(CodigoMedicamento);
                    MessageBox.Show("Datos eliminados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarMedicamentos();
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
