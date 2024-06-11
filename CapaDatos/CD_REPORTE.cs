using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaTablas;

namespace CapaDatos
{
    public class CD_REPORTE
    {
        public int InsertarReporte(Reporte obj)
        {
            int idReporte = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("InsertarReporte", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Tipo", obj.Tipo);
                    cmd.Parameters.Add("@idReporte", SqlDbType.Int).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idReporte = Convert.ToInt32(cmd.Parameters["@idReporte"].Value);
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                Console.WriteLine("Error al insertar el reporte: " + ex.Message);
            }

            return idReporte;
        }
    }
}
