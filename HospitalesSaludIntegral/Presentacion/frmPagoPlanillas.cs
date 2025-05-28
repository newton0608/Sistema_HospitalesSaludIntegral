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
    public partial class frmPagoPlanillas: Form
    {
        CDpagoplanilla cd_pagoplanilla = new CDpagoplanilla();
        CLpagoplanilla cl_pagoplanilla = new CLpagoplanilla();
        CDconexion cd_conexion = new CDconexion();
        public frmPagoPlanillas()
        {
            InitializeComponent();
        }

        private void frmPagoPlanillas_Load(object sender, EventArgs e)
        {
            lblFecha.Text = cl_pagoplanilla.MtdFechaHoy().ToString("d");
            MtdConsultarPagoPlanilla(); //carga los datos al dgv
            CargarCboxEmpleadosCodigo(); //carga los datos al cbox de los nombre
           // CargarCboxEmpleadosCodigo(); //carga los datos al cbox de los codigos
        }
        private void CargarCboxEmpleadosCodigo()
        {
            //Primero se obtiene la tabla original de empleados
            DataTable tblOriginal = cd_pagoplanilla.MtdConsultarEmpleados();

            //Se crea una nueva tabla
            DataTable tblEmpleados = new DataTable();
            tblEmpleados.Columns.Add("CodigoEmpleado", typeof(int));
            tblEmpleados.Columns.Add("Nombres", typeof(string));
            tblEmpleados.Columns.Add("Mostrar", typeof(string)); // columna para mostrar

            //Agregamos la fila "Seleccione..."
            DataRow filaVacia = tblEmpleados.NewRow();
            filaVacia["CodigoEmpleado"] = -1;
            filaVacia["Nombres"] = "";
            filaVacia["Mostrar"] = "Seleccione...";
            tblEmpleados.Rows.Add(filaVacia);

            //Por [ultimo , se recorre la tabla original y se llena la nueva tabla con los datos de esta 
            foreach (DataRow row in tblOriginal.Rows)
            {
                DataRow newRow = tblEmpleados.NewRow();
                int codigo = Convert.ToInt32(row["CodigoEmpleado"]);
                string nombre = row["Nombres"].ToString();
                newRow["CodigoEmpleado"] = codigo;
                newRow["Nombres"] = nombre;
                newRow["Mostrar"] = $"{codigo} - {nombre}";
                tblEmpleados.Rows.Add(newRow);
            }
            
            // Cargar en el ComboBox
            cboxCodigoEmpleado.DataSource = tblEmpleados;
            cboxCodigoEmpleado.DisplayMember = "Mostrar";// lo que se muestra en el cbox
            cboxCodigoEmpleado.ValueMember = "CodigoEmpleado";// el valor del cbox
            cboxCodigoEmpleado.SelectedIndex = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cboxDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtCodigoEmpleado_TextChanged(object sender, EventArgs e)
        {

        }
        private void MtdConsultarPagoPlanilla()
        {
            DataTable DtPagoPlanilla = cd_pagoplanilla.MtdConsultarPagoPlanilla();
            dgvUsuarios.DataSource = DtPagoPlanilla;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void DtpFechaPago_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtHorasX.Text == "0") 
            { 
                txtHorasX.SelectAll(); // Selecciona todo el texto en el TextBox
            }
            if (string.IsNullOrEmpty(txtHorasX.Text))
            {
                txtHorasX.Text = "0";
                txtHorasX.SelectAll();
                return;
            }
            else
            {
                try
                {
                    lblTotal.Text = cl_pagoplanilla.MtdSueldoTotal(cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue)),
                        cl_pagoplanilla.MtdSueldoBono(cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue))), int.Parse(txtHorasX.Text)).ToString("C", new CultureInfo("es-GT"));
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtHorasX.Text = "0";
                }
                
            }
        }

        private void cboxCodigoEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue));
                if (cboxCodigoEmpleado.SelectedValue is int codigo && codigo != -1)
                {

                    lblSueldo.Text = cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue)).ToString("C", new CultureInfo("es-GT"));
                    lblBono.Text = cl_pagoplanilla.MtdSueldoBono(cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue))).ToString("C", new CultureInfo("es-GT"));
                    textBox1_TextChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       private void btnAgregar_Click(object sender, EventArgs e)
        {
            Validartxt(txtHorasX);
            Validarcbox(cboxEstado);
            Validarcbox(cboxCodigoEmpleado);

            if (string.IsNullOrEmpty(cboxEstado.Text) || cboxCodigoEmpleado.SelectedIndex <= 0 || string.IsNullOrEmpty(txtHorasX.Text))
            {

                

                MessageBox.Show("Favor completar formulario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.SelectedValue);
                DateTime FechaPago = DtpFechaPago.Value;
                double Sueldo = cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue));
                double Bono = cl_pagoplanilla.MtdSueldoBono(cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue)));
                int MontoHorasExtra = int.Parse(txtHorasX.Text)*20;
                double TotalMonto = cl_pagoplanilla.MtdSueldoTotal(Sueldo, Bono, int.Parse(txtHorasX.Text));
                string Estado = cboxEstado.Text;
                DateTime FechaAuditoria = cl_pagoplanilla.MtdFechaHoy();
                string UsuarioAuditoria = "alguien";

                try
                {
                    cd_pagoplanilla.MtdAgregarPagoPlanilla(CodigoEmpleado, FechaPago, Sueldo, Bono, MontoHorasExtra, TotalMonto, Estado, FechaAuditoria, UsuarioAuditoria);
                    MessageBox.Show("Datos agregados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarPagoPlanilla();
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

        private void MtdLimpiarCampos()
        {
            cboxCodigoEmpleado.SelectedIndex = 0;
            DtpFechaPago.Value = DateTime.Now;
            txtHorasX.Text = "0";
            cboxEstado.SelectedIndex = 0;
            txtCodigoPago.Text = string.Empty;
            lblSueldo.Text = "0.00";
            lblBono.Text = "0.00";
            lblTotal.Text = "0.00";
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Validartxt(txtHorasX);
            Validarcbox(cboxEstado);
            Validarcbox(cboxCodigoEmpleado);

            if (string.IsNullOrEmpty(cboxEstado.Text) || cboxCodigoEmpleado.SelectedIndex <= 0 || string.IsNullOrEmpty(txtHorasX.Text))
            {

                

                if (string.IsNullOrEmpty(txtCodigoPago.Text))
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
                    int CodigoPago = Convert.ToInt32(txtCodigoPago.Text);
                    int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.SelectedValue);
                    DateTime FechaPago = DtpFechaPago.Value;
                    double Sueldo = cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue));
                    double Bono = cl_pagoplanilla.MtdSueldoBono(cd_pagoplanilla.MtdConsultarSueldo(Convert.ToInt32(cboxCodigoEmpleado.SelectedValue)))*20;
                    double MontoHorasExtra = Convert.ToDouble(txtHorasX.Text);
                    double TotalMonto = cl_pagoplanilla.MtdSueldoTotal(Sueldo, Bono, int.Parse(txtHorasX.Text));
                    string Estado = cboxEstado.Text;
                    DateTime FechaAuditoria = cl_pagoplanilla.MtdFechaHoy();
                    string UsuarioAuditoria = "alguien";

                    cd_pagoplanilla.MtdActualizarPagoPlanilla(int.Parse(txtCodigoPago.Text),CodigoEmpleado, FechaPago, Sueldo, Bono, MontoHorasExtra, TotalMonto, Estado, FechaAuditoria, UsuarioAuditoria);
                    MessageBox.Show("Datos agregados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarPagoPlanilla();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
                cboxCodigoEmpleado.Text = FilaSeleccionada.Cells["CodigoEmpleado"].Value.ToString();
                DtpFechaPago.Value = Convert.ToDateTime(FilaSeleccionada.Cells["FechaPago"].Value);
                txtHorasX.Text = (Convert.ToInt32(FilaSeleccionada.Cells["MontoHorasExtra"].Value)/20).ToString();
                cboxEstado.Text = FilaSeleccionada.Cells["Estado"].Value.ToString();
                txtCodigoPago.Text = FilaSeleccionada.Cells["CodigoPago"].Value.ToString();
                cboxCodigoEmpleado.SelectedValue = Convert.ToInt32(FilaSeleccionada.Cells["CodigoEmpleado"].Value);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPago.Text))
            {
                MessageBox.Show("Favor seleccionar fila a eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int CodigoUsuario = int.Parse(txtCodigoPago.Text);

                try
                {
                    cd_pagoplanilla.MtdEliminarPagoPlanilla(CodigoUsuario);
                    MessageBox.Show("Datos eliminados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarPagoPlanilla();
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
