using CapaTablas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_RUTINA
    {
        public List<Rutina> Listar(int idAprendiz)
        {
            List<Rutina> rutinas = new List<Rutina>();

            using (SqlConnection connection = new SqlConnection(Conexion.cn))
            {
                using (SqlCommand command = new SqlCommand("ListarRutinasPorIdAprendiz", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Identificacion", idAprendiz);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Rutina rutina = new Rutina
                        {
                            idRutina = Convert.ToInt32(reader["idRutina"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]),
                            nombreRutina = reader["nombreRutina"].ToString(),
                            aprendiz = new Aprendiz() { Identificacion = idAprendiz }
                        };
                        rutinas.Add(rutina);
                    }
                    reader.Close();
                }
            }

            return rutinas;
        }

        //public int RegistrarRutina(Rutina rutina)
        //{
        //    int idRutina = 0;

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(Conexion.cn))
        //        {
        //            using (SqlCommand command = new SqlCommand("RegistrarRutina", connection))
        //            {
        //                // Parámetros del procedimiento almacenado
        //                command.Parameters.AddWithValue("@Descripcion", rutina.Descripcion);
        //                command.Parameters.AddWithValue("@nombreRutina", rutina.nombreRutina);
        //                command.Parameters.AddWithValue("@Identificacion", rutina.aprendiz.Identificacion);
        //                command.Parameters.Add("@idRutina", SqlDbType.Int).Direction = ParameterDirection.Output;
        //                command.CommandType = CommandType.StoredProcedure;

        //                connection.Open();
        //                command.ExecuteNonQuery();

        //                // Después de ejecutar el procedimiento almacenado, obtén el valor del parámetro de salida
        //                idRutina = Convert.ToInt32(command.Parameters["@idRutina"].Value);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de excepciones
        //        Console.WriteLine(ex.ToString());
        //        idRutina = 0;
        //    }

        //    return idRutina;
        //}

        //public void RegistrarEjerciciosEnRutina(int idRutina, string ejerciciosParaRegistrar)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(Conexion.cn))
        //        {
        //            using (SqlCommand command = new SqlCommand("RegistrarEjercicios", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@idRutina", idRutina);
        //                command.Parameters.AddWithValue("@EjerciciosParaRegistrar", ejerciciosParaRegistrar);

        //                connection.Open();
        //                command.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo general de excepciones
        //        Console.WriteLine("Error al intentar registrar los ejercicios en la rutina: " + ex.Message);
        //    }
        //}

      


        public Rutina ObtenerRutinaPorId(int idRutina, int identificacion)
        {
            Rutina rutina = null;

            using (SqlConnection connection = new SqlConnection(Conexion.cn))
            {
                using (SqlCommand command = new SqlCommand("ObtenerRutinaPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idRutina", idRutina);
                    command.Parameters.AddWithValue("@Identificacion", identificacion);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rutina = new Rutina
                            {
                                idRutina = Convert.ToInt32(reader["idRutina"]),
                                Descripcion = reader["Descripcion"].ToString(),
                                fechaCreacion = Convert.ToDateTime(reader["fechaCreacion"]),
                                nombreRutina = reader["nombreRutina"].ToString(),
                                aprendiz = new Aprendiz() { Identificacion = identificacion }
                            };
                        }
                    }
                }
            }

            return rutina;
        }

        public List<Ejercicio> ObtenerEjerciciosPorIdRutina(Rutina obj)
        {
            List<Ejercicio> ejercicios = new List<Ejercicio>();

            using (SqlConnection connection = new SqlConnection(Conexion.cn))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("ObtenerEjerciciosPorIdRutina", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRutina", obj.idRutina);
                cmd.Parameters.AddWithValue("@Identificacion", obj.aprendiz.Identificacion);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ejercicio ejercicio = new Ejercicio
                        {
                            IdEjercicio = Convert.ToInt32(reader["idEjercicio"]),
                            Nombre = reader["Nombre"].ToString(),
                            Tipo = reader["Tipo"].ToString(),
                            Dificultad = reader["Dificultad"].ToString(),
                            maquina = new Maquina()
                            {
                                NombreMaquina = reader["NombreMaquina"].ToString(),
                                numeroMaquina = Convert.ToInt32(reader["numeroMaquina"]) 
                            }
                        };

                        ejercicios.Add(ejercicio);
                    }
                }
            }

            return ejercicios;
        }

        public void EliminarRutina(Rutina obj)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EliminarRutina", oconexion);
                    cmd.Parameters.AddWithValue("@idRutina", obj.idRutina);
                    cmd.Parameters.AddWithValue("@Identificacion", obj.aprendiz.Identificacion);
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

        public void EditarRutina(Rutina rutina)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.cn))
            {
                using (SqlCommand command = new SqlCommand("EditarRutina", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@idRutina", rutina.idRutina);
                    command.Parameters.AddWithValue("@Identificacion", rutina.aprendiz.Identificacion);
                    command.Parameters.AddWithValue("@descripcion", rutina.Descripcion);
                    command.Parameters.AddWithValue("@fechaCreacion", rutina.fechaCreacion);
                    command.Parameters.AddWithValue("@nombreRutina", rutina.nombreRutina);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditarEjerciciosDeRutina(int idRutina, int Identificacion, int IdEjercicio, string Nombre, string Tipo, string Dificultad, int NumeroMaquina)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cn))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("EditarEjerciciosDeRutina", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Añadir los parámetros al comando
                    cmd.Parameters.Add("@idRutina", SqlDbType.Int).Value = idRutina;
                    cmd.Parameters.Add("@Identificacion", SqlDbType.Int).Value = Identificacion;
                    cmd.Parameters.Add("@ejercicio_id", SqlDbType.Int).Value = IdEjercicio;
                    cmd.Parameters.Add("@ejercicio_nombre", SqlDbType.NVarChar, 450).Value = Nombre;
                    cmd.Parameters.Add("@ejercicio_tipo", SqlDbType.NVarChar, 400).Value = Tipo;
                    cmd.Parameters.Add("@ejercicio_dificultad", SqlDbType.NVarChar, 180).Value = Dificultad;
                    cmd.Parameters.Add("@numeroMaquina", SqlDbType.Int).Value = NumeroMaquina; // Nuevo parámetro para el número de la máquina

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }


        public void EliminarEjercicioDeRutina(int idRutina, int idEjercicio, int Identificacion)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("EliminarEjercicioDeRutina", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Añadir los parámetros al comando
                        cmd.Parameters.Add("@idRutina", SqlDbType.Int).Value = idRutina;
                        cmd.Parameters.Add("@Identificacion", SqlDbType.Int).Value = Identificacion;
                        cmd.Parameters.Add("@idEjercicio", SqlDbType.Int).Value = idEjercicio;

                        // Abrir la conexión y ejecutar el comando
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
