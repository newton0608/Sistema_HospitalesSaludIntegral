using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Datos
{
    class CDusuarios
    {
        CDconexion cd_conexion = new CDconexion();

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

        public DataTable MtdConsultarUsuarios()
        {
            string QueryConsultar = "Select * from tbl_Usuarios";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(QueryConsultar, cd_conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            SqlAdap.Fill(Dt);
            cd_conexion.MtdCerrarConexion();
            return Dt;
        }

        public void MtdAgregarUsuarios(int CodigoEmpleado, string Usuario, string Clave, string TipoUsuario, string Estado, DateTime FechaAuditoria, string UsuarioAuditoria)
        {
            string QueryAgregar = "Insert into tbl_Usuarios (CodigoEmpleado, Usuario, Clave, TipoUsuario, Estado, FechaAuditoria, UsuarioAuditoria) values (@CodigoEmpleado, @Usuario, @Clave, @TipoUsuario, @Estado, @FechaAuditoria, @UsuarioAuditoria)";
            SqlCommand cmd = new SqlCommand(QueryAgregar, cd_conexion.MtdAbrirConexion());
            //cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);
            cmd.Parameters.AddWithValue("@Clave", Clave);
            cmd.Parameters.AddWithValue("@TipoUsuario", TipoUsuario);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
            cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }

        public void MtdActualizarUsuarios(int CodigoUsuario, int CodigoEmpleado, string Usuario, string Clave, string TipoUsuario, string Estado, DateTime FechaAuditoria, string UsuarioAuditoria)
        {
            string QueryActualizar = "Update tbl_Usuarios set CodigoEmpleado=@CodigoEmpleado, Usuario=@Usuario, Clave=@Clave, TipoUsuario=@TipoUsuario, Estado=@Estado, FechaAuditoria=@FechaAuditoria, UsuarioAuditoria=@UsuarioAuditoria where CodigoUsuario=@CodigoUsuario";
            SqlCommand cmd = new SqlCommand(QueryActualizar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            cmd.Parameters.AddWithValue("@Usuario", Usuario);
            cmd.Parameters.AddWithValue("@Clave", Clave);
            cmd.Parameters.AddWithValue("@TipoUsuario", TipoUsuario);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
            cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }

        public void MtdEliminarUsuarios(int CodigoUsuario)
        {
            string QueryEliminar = "Delete from tbl_Usuarios where CodigoUsuario=@CodigoUsuario";
            SqlCommand cmd = new SqlCommand(QueryEliminar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }

        public bool MtdUsuarioExistente(int codigoEmpleado) //Valida si ya el empleado ya tiene usuario
        {
            SqlConnection conexion = cd_conexion.MtdAbrirConexion();
            
                string query = "SELECT COUNT(*) FROM tbl_Usuarios WHERE CodigoEmpleado = @CodigoEmpleado"; //cuenta cuantas filas hay con el mismo valor de CodigoEmpleado
                cd_conexion.MtdAbrirConexion();
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@CodigoEmpleado", codigoEmpleado);
                int count = (int)cmd.ExecuteScalar();
                cd_conexion.MtdCerrarConexion();

                if (count > 0)
                {
                    return true; //Ya tiene usuario
                }
                else
                {
                    return false; //No tiene usuario
                }

        }


    }
}
