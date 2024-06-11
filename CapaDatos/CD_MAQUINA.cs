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
    public class CD_MAQUINA
    {
        public List<Maquina> Listar()
        {
            List<Maquina> Lista = new List<Maquina>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string Query = "SELECT * FROM Maquina";

                    SqlCommand cmd = new SqlCommand(Query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(
                                new Maquina()
                                {
                                    numeroMaquina = Convert.ToInt32(dr["numeroMaquina"]),
                                    instructor = new Instructor() { idInstructor = Convert.ToInt32(dr["idInstructor"]) },
                                    NombreMaquina = dr["NombreMaquina"].ToString(),
                                    EstadoMaquina = dr["EstadoMaquina"].ToString(),
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Maquina>();
            }

            return Lista;

        }


        public void EditarMaquina(Maquina obj)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EditarMaquina", oconexion);
                    cmd.Parameters.AddWithValue("@numeroMaquina", obj.numeroMaquina);
                    cmd.Parameters.AddWithValue("@idInstructor", obj.instructor.idInstructor);
                    cmd.Parameters.AddWithValue("@NombreMaquina", obj.NombreMaquina);
                    cmd.Parameters.AddWithValue("@EstadoMaquina", obj.EstadoMaquina);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void EliminarMaquina(int numeroMaquina)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EliminarMaquina", oconexion);
                    cmd.Parameters.AddWithValue("@numeroMaquina", numeroMaquina);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("No se puede eliminar la máquina porque está siendo utilizada en otra parte del sistema.", ex);
            }
        }


        public int RegistrarMaquina(Maquina obj)
        {
            int idMaquina = 0; // Variable para almacenar el número de máquina generado
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarMaquina", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idMaquina", SqlDbType.Int).Direction = ParameterDirection.Output; // Definir el parámetro de salida
                    cmd.Parameters.AddWithValue("@idInstructor", obj.instructor.idInstructor);
                    cmd.Parameters.AddWithValue("@NombreMaquina", obj.NombreMaquina);
                    cmd.Parameters.AddWithValue("@EstadoMaquina", obj.EstadoMaquina);
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Obtener el valor del parámetro de salida
                    idMaquina = Convert.ToInt32(cmd.Parameters["@idMaquina"].Value);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine(ex.ToString());
            }
            return idMaquina; // Devolver el número de máquina generado
        }

        public Maquina ObtenerMaquinaPorNumero(int numeroMaquina)
        {
            Maquina maquina = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ObtenerMaquinaPorNumero", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@numeroMaquina", numeroMaquina);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            maquina = new Maquina()
                            {
                                numeroMaquina = Convert.ToInt32(dr["numeroMaquina"]),
                                instructor = new Instructor() { idInstructor = Convert.ToInt32(dr["idInstructor"]) },
                                NombreMaquina = dr["NombreMaquina"].ToString(),
                                EstadoMaquina = dr["EstadoMaquina"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                ex.ToString();
            }

            return maquina;
        }

    }
}