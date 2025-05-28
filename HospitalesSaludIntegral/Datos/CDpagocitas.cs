using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Datos
{
    class CDpagocitas
    {
        CDconexion cd_conexion = new CDconexion();
        public DataTable MtdConsultarPagoCita()
        {
            string QueryConsultar = "Select * from tbl_PagoCitas";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(QueryConsultar, cd_conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            SqlAdap.Fill(Dt);
            cd_conexion.MtdCerrarConexion();
            return Dt;
        }
        public DataTable MtdConsultarCitas()
        {
            DataTable TablaCitas = new DataTable();
            string Querycitas = "SELECT CodigoCita FROM tbl_Citas";

            using (SqlCommand cmd = new SqlCommand(Querycitas, cd_conexion.MtdAbrirConexion()))
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                adaptador.Fill(TablaCitas);
            }
            return TablaCitas;
        }

        public DataTable MtdConsultarCostos(int CodigoCita)
        {
            DataTable TablaCostos = new DataTable();
            string QueryCostos = "SELECT CostoTratamientos, CostoHabitacion FROM tbl_Citas where CodigoCita = @CodigoCita";

            using (SqlCommand cmd = new SqlCommand(QueryCostos, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@CodigoCita", CodigoCita);
                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                adaptador.Fill(TablaCostos);
            }
            return TablaCostos;
        }

        public void MtdAgregarPagoCita(int CodigoCita, double MontoCita, double Impuesto, double Descuento, double TotalPago, DateTime FechaPago, string TipoPago, string UsuarioAuditoria, DateTime FechaAuditoria)
        {
            string QueryInsertar = "INSERT INTO tbl_PagoCitas (CodigoCita, MontoCita, Impuestos, Descuento, TotalPago, FechaPago, TipoPago, UsuarioAuditoria, FechaAuditoria) VALUES (@CodigoCita, @MontoCita, @ImpuestoS, @Descuento, @TotalPago, @FechaPago, @TipoPago, @UsuarioAuditoria, @FechaAuditoria)";
            using (SqlCommand cmd = new SqlCommand(QueryInsertar, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@CodigoCita", CodigoCita);
                cmd.Parameters.AddWithValue("@MontoCita", MontoCita);
                cmd.Parameters.AddWithValue("@Impuestos", Impuesto);
                cmd.Parameters.AddWithValue("@Descuento", Descuento);
                cmd.Parameters.AddWithValue("@TotalPago", TotalPago);
                cmd.Parameters.AddWithValue("@FechaPago", FechaPago);
                cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
                cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
                cmd.ExecuteNonQuery();
            }
            cd_conexion.MtdCerrarConexion();
        }
        public void MtdEditarPagoCita(int CodigoPagoCita, int CodigoCita, double MontoCita, double Impuesto, double Descuento, double TotalPago, DateTime FechaPago, string TipoPago, string UsuarioAuditoria, DateTime FechaAuditoria)
        {
            string QueryEditar = "UPDATE tbl_PagoCitas SET MontoCita = @MontoCita, Impuestos = @ImpuestoS, Descuento = @Descuento, TotalPago = @TotalPago, FechaPago = @FechaPago, TipoPago = @TipoPago, UsuarioAuditoria = @UsuarioAuditoria, FechaAuditoria = @FechaAuditoria WHERE CodigoPagoCita = @CodigoPagoCita";
            using (SqlCommand cmd = new SqlCommand(QueryEditar, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@CodigoPagoCita", CodigoPagoCita);
                cmd.Parameters.AddWithValue("@CodigoCita", CodigoCita);
                cmd.Parameters.AddWithValue("@MontoCita", MontoCita);
                cmd.Parameters.AddWithValue("@Impuestos", Impuesto);
                cmd.Parameters.AddWithValue("@Descuento", Descuento);
                cmd.Parameters.AddWithValue("@TotalPago", TotalPago);
                cmd.Parameters.AddWithValue("@FechaPago", FechaPago);
                cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
                cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
                cmd.ExecuteNonQuery();
            }
            cd_conexion.MtdCerrarConexion();
        }

        public void MtdEliminarPagoCita(int CodigoPagoCita)
        {
            string QueryEliminar = "DELETE FROM tbl_PagoCitas WHERE CodigoPagoCita = @CodigoPagoCita";
            using (SqlCommand cmd = new SqlCommand(QueryEliminar, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@CodigoPagoCita", CodigoPagoCita);
                cmd.ExecuteNonQuery();
            }
            cd_conexion.MtdCerrarConexion();
        }
    }
}