using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using CapaTablas;

namespace CapaDatos
{
    public class CD_INSTRUCTOR
    {
        public List<Instructor> Listar()
        {
            List<Instructor> Lista = new List<Instructor>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string Query = "select idInstructor,NombreInstructor,Contraseña from Instructor";

                    SqlCommand cmd = new SqlCommand(Query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(
                                new Instructor()
                                {
                                    idInstructor = Convert.ToInt32(dr["idInstructor"]),
                                    NombreInstructor = dr["NombreInstructor"].ToString(),
                                    Contraseña = dr["Contraseña"].ToString(),
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Instructor>();
            }

            return Lista;

        }

        public void RegistrarInstructor(int idInstructor, string NombreInstructor, string Contraseña)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarInstructor", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idInstructor", idInstructor);
                    cmd.Parameters.AddWithValue("@NombreInstructor", NombreInstructor);
                    cmd.Parameters.AddWithValue("@ContraseñaInstructor", Contraseña);
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                Console.WriteLine("Error al registrar el administrador: " + ex.Message);
            }
        }

        public string ObtenerCodigoDiario(int idInstructor)
        {
            string codigoDiario = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT CodigoDiario FROM Instructor WHERE idInstructor = @IdInstructor AND FechaCodigoDiario = CONVERT(date, GETDATE())", conexion);
                    cmd.Parameters.AddWithValue("@IdInstructor", idInstructor);

                    conexion.Open();
                    codigoDiario = (string)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                Console.WriteLine("Error al obtener el código diario del instructor: " + ex.Message);
            }
            return codigoDiario;
        }

        public void ActualizarCodigoDiario(int idInstructor, string codigoDiario, string fechaActual)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ActualizarCodigoDiario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdInstructor", idInstructor);
                    cmd.Parameters.AddWithValue("@CodigoDiario", codigoDiario);
                    cmd.Parameters.AddWithValue("@Fecha", fechaActual);
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según sea necesario
                Console.WriteLine("Error al actualizar el código diario: " + ex.Message);
            }
        }
    }
}