using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Datos
{
    class CDpagoplanilla
    {
        CDconexion cd_conexion = new CDconexion();
        public DataTable MtdConsultarPagoPlanilla()
        {
            string QueryConsultar = "Select * from tbl_PagoEmpleados";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(QueryConsultar, cd_conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            SqlAdap.Fill(Dt);
            cd_conexion.MtdCerrarConexion();
            return Dt;
        }
        public DataTable MtdConsultarEmpleados()
        {
            DataTable TablaEmpleados = new DataTable();
            string consulta = "SELECT CodigoEmpleado, Nombres FROM tbl_Empleados";

            using (SqlCommand comando = new SqlCommand(consulta, cd_conexion.MtdAbrirConexion()))
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                adaptador.Fill(TablaEmpleados);
            }

            cd_conexion.MtdCerrarConexion();
            return TablaEmpleados;
        }

        public double MtdConsultarSueldo(double CodigoEmpleado)
        {
            double sueldo = 0;
            string consulta = "SELECT Sueldo FROM tbl_Empleados WHERE CodigoEmpleado = @CodigoEmpleado";
            using (SqlCommand comando = new SqlCommand(consulta, cd_conexion.MtdAbrirConexion()))
            {
                comando.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
                object resultado = comando.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    sueldo = Convert.ToInt32(resultado);
                }
            }
            cd_conexion.MtdCerrarConexion();
            return sueldo;
        }
        public void MtdAgregarPagoPlanilla(int CodigoEmpleado, DateTime FechaPago, double Sueldo, double Bono, double MontoHorasExtra, double TotalMonto, string Estado, DateTime FechaAuditoria, string UsuarioAuditoria)
        {
            string QueryAgregar = "Insert into tbl_PagoEmpleados (CodigoEmpleado, FechaPago, Sueldo, Bono, MontoHorasExtra, TotalMonto, Estado, FechaAuditoria, UsuarioAuditoria) values (@CodigoEmpleado, @FechaPago, @Sueldo, @Bono, @MontoHorasExtra, @TotalMonto, @Estado, @FechaAuditoria, @UsuarioAuditoria)";
            SqlCommand cmd = new SqlCommand(QueryAgregar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            cmd.Parameters.AddWithValue("@FechaPago", FechaPago);
            cmd.Parameters.AddWithValue("@Sueldo", Sueldo);
            cmd.Parameters.AddWithValue("@Bono", Bono);
            cmd.Parameters.AddWithValue("@MontoHorasExtra", MontoHorasExtra);
            cmd.Parameters.AddWithValue("@TotalMonto", TotalMonto);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
            cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }

        public void MtdActualizarPagoPlanilla(int CodigoPago, int CodigoEmpleado, DateTime FechaPago, double Sueldo, double Bono, double MontoHorasExtra, double TotalMonto, string Estado, DateTime FechaAuditoria, string UsuarioAuditoria)
        {
            string QueryActualizar = "Update tbl_PagoEmpleados set CodigoEmpleado=@CodigoEmpleado, FechaPago=@FechaPago, Sueldo=@Sueldo, Bono=@Bono, MontoHorasExtra=@MontoHorasExtra, TotalMonto=@TotalMonto, Estado=@Estado, FechaAuditoria=@FechaAuditoria, UsuarioAuditoria=@UsuarioAuditoria where CodigoPago=@CodigoPago";
            SqlCommand cmd = new SqlCommand(QueryActualizar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            cmd.Parameters.AddWithValue("@FechaPago", FechaPago);
            cmd.Parameters.AddWithValue("@Sueldo", Sueldo);
            cmd.Parameters.AddWithValue("@Bono", Bono);
            cmd.Parameters.AddWithValue("@MontoHorasExtra", MontoHorasExtra);
            cmd.Parameters.AddWithValue("@TotalMonto", TotalMonto);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
            cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }

        public void MtdEliminarPagoPlanilla(int CodigoPago)
        {
            string QueryEliminar = "Delete from tbl_PagoEmpleados where CodigoPago=@CodigoPago";
            SqlCommand cmd = new SqlCommand(QueryEliminar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoPago", CodigoPago);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
    }
}
