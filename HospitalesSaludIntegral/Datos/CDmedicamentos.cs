using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Datos
{
    class CDmedicamentos
    {
        CDconexion cd_conexion = new CDconexion();
        public DataTable MtdConsultarMedicamentos()
        {
            string QueryConsultar = "Select * from tbl_Medicamentos";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(QueryConsultar, cd_conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            SqlAdap.Fill(Dt);
            cd_conexion.MtdCerrarConexion();
            return Dt;
        }

        public void MtdAgregarMedicamentos(string Nombre, string TipoMedicamento, int Costo, int Stock, DateTime FechaVencimiento, string Estado, string UsuarioAuditoria, DateTime FechaAuditoria)
        {
            string QueryInsertar = "INSERT INTO tbl_Medicamentos (Nombre, TipoMedicamento, Costo, Stock, FechaVencimiento, Estado, UsuarioAuditoria, FechaAuditoria) VALUES (@Nombre, @TipoMedicamento, @Costo, @Stock, @FechaVencimiento, @Estado, @UsuarioAuditoria, @FechaAuditoria)";
            using (SqlCommand cmd = new SqlCommand(QueryInsertar, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@TipoMedicamento", TipoMedicamento);
                cmd.Parameters.AddWithValue("@Costo", Costo);
                cmd.Parameters.AddWithValue("@Stock", Stock);
                cmd.Parameters.AddWithValue("@FechaVencimiento", FechaVencimiento);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
                cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
                cmd.ExecuteNonQuery();
            }
            cd_conexion.MtdCerrarConexion();
        }

        public void MtdActualizarMedicamentos (int CodigoMedicamento, string Nombre, string TipoMedicamento, int Costo, int Stock, DateTime FechaVencimiento, string Estado, string UsuarioAuditoria, DateTime FechaAuditoria)
        {
            string QueryInsertar = "UPDATE tbl_Medicamentos SET Nombre = @Nombre, TipoMedicamento = @TipoMedicamento, Costo = @Costo, Stock = @Stock, FechaVencimiento = @FechaVencimiento, Estado = @Estado, UsuarioAuditoria = @UsuarioAuditoria, FechaAuditoria = @FechaAuditoria WHERE CodigoMedicamento = @CodigoMedicamento";
            using (SqlCommand cmd = new SqlCommand(QueryInsertar, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@CodigoMedicamento", CodigoMedicamento);
                cmd.Parameters.AddWithValue("@Nombre", Nombre);
                cmd.Parameters.AddWithValue("@TipoMedicamento", TipoMedicamento);
                cmd.Parameters.AddWithValue("@Costo", Costo);
                cmd.Parameters.AddWithValue("@Stock", Stock);
                cmd.Parameters.AddWithValue("@FechaVencimiento", FechaVencimiento);
                cmd.Parameters.AddWithValue("@Estado", Estado);
                cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
                cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
                cmd.ExecuteNonQuery();
            }
            cd_conexion.MtdCerrarConexion();
        }
        public void MtdEliminarMedicamentos(int CodigoMedicamento)
        {
            string QueryEliminar = "DELETE FROM tbl_Medicamentos WHERE CodigoMedicamento = @CodigoMedicamento";
            using (SqlCommand cmd = new SqlCommand(QueryEliminar, cd_conexion.MtdAbrirConexion()))
            {
                cmd.Parameters.AddWithValue("@CodigoMedicamento", CodigoMedicamento);
                cmd.ExecuteNonQuery();
            }
            cd_conexion.MtdCerrarConexion();
        }
    }
}
