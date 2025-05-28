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

namespace HospitalesSaludIntegral
{
    public partial class frmPagoCitas : Form
    {
        CDpagocitas cd_pagocita = new CDpagocitas();
        CLpagocitas cl_pagocita = new CLpagocitas();
        public frmPagoCitas()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmPagoCitas_Load(object sender, EventArgs e)
        {
            lblFecha.Text = cl_pagocita.MtdFechaHoy().ToString("d");
            MtdConsultarPagoCitas();
            CargarCboxCitas();
            cboxTipoPago.SelectedIndex = 0;
        }
        private void CargarCboxCitas()
        {

            DataTable TablaCitas = cd_pagocita.MtdConsultarCitas();

            DataRow fila = TablaCitas.NewRow();
            fila["CodigoCita"] = 0;
            TablaCitas.Rows.InsertAt(fila, 0);

            cboxCodigoCita.DataSource = TablaCitas;
            cboxCodigoCita.DataSource = TablaCitas;
            cboxCodigoCita.DisplayMember = "CodigoCita";
            cboxCodigoCita.ValueMember = "CodigoCita";

        }
        private void MtdConsultarPagoCitas()
        {
            DataTable DtPagoPlanilla = cd_pagocita.MtdConsultarPagoCita();
            dgvPagoCitas.DataSource = DtPagoPlanilla;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPagoCita.Text))
            {
                MessageBox.Show("Favor seleccionar fila a eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int CodigoPagoCita = int.Parse(txtCodigoPagoCita.Text);

                try
                {
                    cd_pagocita.MtdEliminarPagoCita(CodigoPagoCita);
                    MessageBox.Show("Datos eliminados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarPagoCitas();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cboxCodigoCita_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxCodigoCita.SelectedIndex <= 0) return;
            lblMontoCita.Text = cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(int.Parse(cboxCodigoCita.Text))).ToString("C", new CultureInfo("es-GT"));
            lblImpuesto.Text = cl_pagocita.MtdImpuestoPago(cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(int.Parse(cboxCodigoCita.Text)))).ToString("C", new CultureInfo("es-GT"));
            lblDescuento.Text = cl_pagocita.DescuentoPago(cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(int.Parse(cboxCodigoCita.Text)))).ToString("C", new CultureInfo("es-GT"));
            lblTotal.Text = cl_pagocita.MtdTotalPago(
                cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(int.Parse(cboxCodigoCita.Text))),
                cl_pagocita.MtdImpuestoPago(cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(int.Parse(cboxCodigoCita.Text)))),
                cl_pagocita.DescuentoPago(cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(int.Parse(cboxCodigoCita.Text))))
            ).ToString("C", new CultureInfo("es-GT"));
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            Validarcbox(cboxTipoPago);
            Validarcbox(cboxCodigoCita);

            if (cboxCodigoCita.SelectedIndex <= 0 || cboxTipoPago.SelectedIndex <= 0)
            {
                MessageBox.Show("Favor completar formulario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                int CodigoCita = int.Parse(cboxCodigoCita.Text);
                double MontoCita = cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(CodigoCita));
                double Impuesto = cl_pagocita.MtdImpuestoPago(MontoCita);
                double Descuento = cl_pagocita.DescuentoPago(MontoCita);
                double TotalPago = cl_pagocita.MtdTotalPago(MontoCita, Impuesto, Descuento);
                DateTime FechaPago = DtpFechaPago.Value;
                string TipoPago = cboxTipoPago.Text;
                string UsuarioAuditoria = "Alguien";
                DateTime FechaAuditoria = cl_pagocita.MtdFechaHoy();


                try
                {
                    cd_pagocita.MtdAgregarPagoCita(
                        CodigoCita,
                        MontoCita,
                        Impuesto,
                        Descuento,
                        TotalPago,
                        FechaPago,
                        TipoPago,
                        UsuarioAuditoria,
                        FechaAuditoria
                    );
                    MessageBox.Show("Datos agregados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarPagoCitas();
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
            txtCodigoPagoCita.Clear();
            cboxCodigoCita.SelectedIndex = 0;
            lblMontoCita.Text = "0.00";
            lblImpuesto.Text = "0.00";
            lblDescuento.Text = "0.00";
            lblTotal.Text = "0.00";
            cboxTipoPago.SelectedIndex = 0;
        }

        private void Validarcbox(ComboBox combo) // Verifica si el cbox está vacío o no tiene una selección válida y de ser negativo cambia a rojo
        {
            combo.BackColor = combo.SelectedIndex <= 0 ? Color.LightCoral : Color.White;
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvPagoCitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvPagoCitas.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvPagoCitas.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila con datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                cboxCodigoCita.Text = FilaSeleccionada.Cells["CodigoCita"].Value.ToString();
                DtpFechaPago.Value = Convert.ToDateTime(FilaSeleccionada.Cells["FechaPago"].Value);
                cboxTipoPago.Text = FilaSeleccionada.Cells["TipoPago"].Value.ToString();
                txtCodigoPagoCita.Text = FilaSeleccionada.Cells["CodigoPagoCita"].Value.ToString();

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Validarcbox(cboxTipoPago);
            Validarcbox(cboxCodigoCita);

            if (cboxCodigoCita.SelectedIndex <= 0 || cboxTipoPago.SelectedIndex <= 0)
            {
                if (string.IsNullOrEmpty(txtCodigoPagoCita.Text))
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

                int CodigoCita = int.Parse(cboxCodigoCita.Text);
                double MontoCita = cl_pagocita.MtdMontoCita(cd_pagocita.MtdConsultarCostos(CodigoCita));
                double Impuesto = cl_pagocita.MtdImpuestoPago(MontoCita);
                double Descuento = cl_pagocita.DescuentoPago(MontoCita);
                double TotalPago = cl_pagocita.MtdTotalPago(MontoCita, Impuesto, Descuento);
                DateTime FechaPago = DtpFechaPago.Value;
                string TipoPago = cboxTipoPago.Text;
                string UsuarioAuditoria = "Alguien";
                DateTime FechaAuditoria = cl_pagocita.MtdFechaHoy();


                try
                {
                   cd_pagocita.MtdEditarPagoCita(
                        int.Parse(txtCodigoPagoCita.Text),
                        CodigoCita,
                        MontoCita,
                        Impuesto,
                        Descuento,
                        TotalPago,
                        FechaPago,
                        TipoPago,
                        UsuarioAuditoria,
                        FechaAuditoria
                    );
                    MessageBox.Show("Datos editados correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarPagoCitas();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }
    }
}
