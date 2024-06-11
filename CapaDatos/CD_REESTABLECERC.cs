using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_REESTABLECERC
    {

        public void ReestablecerContraseña(string nombreUsuario, string nuevaContraseña)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ReestablecerContraseña", oconexion);
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    cmd.Parameters.AddWithValue("@NuevaContraseña", nuevaContraseña);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                Console.WriteLine("Error al restablecer la contraseña: " + ex.Message);
            }
        }
    }
}
