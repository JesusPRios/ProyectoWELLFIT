using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaTablas;
using Microsoft.OData.Edm;

namespace CapaDatos
{
    public class CD_ASISTENCIA
    {
        public List<Ingreso> ListarAsistenciaAprendiz(int idAprendiz)
        {
            List<Ingreso> Lista = new List<Ingreso>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    // Nombre del procedimiento almacenado
                    string storedProcedureName = "MostrarAsistenciaPorAprendiz";

                    SqlCommand cmd = new SqlCommand(storedProcedureName, oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idAprendiz", SqlDbType.Int).Value = idAprendiz;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(
                                new Ingreso()
                                {
                                    numeroIngreso = Convert.ToInt32(dr["numeroIngreso"]),
                                    instructor = new Instructor() { 
                                        idInstructor = Convert.ToInt32(dr["idInstructor"]),
                                        NombreInstructor = dr["NombreInstructor"].ToString()
                                    },
                                    aprendiz = new Aprendiz() { Identificacion = Convert.ToInt32(dr["Identificacion"]) },
                                    fechaIngreso = Convert.ToDateTime(dr["fechaIngreso"]).Date,
                                    horaIngreso = TimeSpan.Parse(dr["horaIngreso"].ToString()),
                                    horaSalida = dr["horaSalida"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["horaSalida"].ToString())
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Ingreso>();
            }

            return Lista;
        }


        public List<Ingreso> Listar()
        {
            List<Ingreso> Lista = new List<Ingreso>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    // Modificar la consulta para incluir la unión con la tabla Aprendiz
                    string Query = @"
                        SELECT i.numeroIngreso, i.idInstructor, i.Identificacion, i.fechaIngreso, i.horaIngreso, i.horaSalida,
                               a.Nombre, a.Ficha
                        FROM Ingreso i
                        JOIN Aprendiz a ON i.Identificacion = a.Identificacion";

                    SqlCommand cmd = new SqlCommand(Query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(
                                new Ingreso()
                                {
                                    numeroIngreso = Convert.ToInt32(dr["numeroIngreso"]),
                                    instructor = new Instructor() { idInstructor = Convert.ToInt32(dr["idInstructor"]) },
                                    aprendiz = new Aprendiz()
                                    {
                                        Identificacion = Convert.ToInt32(dr["Identificacion"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        Ficha = Convert.ToInt32(dr["Ficha"])
                                    },
                                    fechaIngreso = Convert.ToDateTime(dr["fechaIngreso"]).Date,
                                    horaIngreso = TimeSpan.Parse(dr["horaIngreso"].ToString()),
                                    horaSalida = dr["horaSalida"] == DBNull.Value ? (TimeSpan?)null : TimeSpan.Parse(dr["horaSalida"].ToString())
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Ingreso>();
            }

            return Lista;
        }


        public int RegistrarAsistencia(Ingreso obj)
        {
            int idIngreso = 0; // Variable para almacenar el número del ingreso generado

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarIngreso", oconexion);
                    cmd.Parameters.AddWithValue("@idInstructor", obj.instructor.idInstructor);
                    cmd.Parameters.AddWithValue("@Identificacion", obj.aprendiz.Identificacion);
                    cmd.Parameters.Add("@idIngreso", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idIngreso = Convert.ToInt32(cmd.Parameters["@idIngreso"].Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar la asistencia: " + ex.Message);
            }

            return idIngreso;
        }

        public void EditarAsistencia(Ingreso obj)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EditarIngreso", oconexion);
                    cmd.Parameters.AddWithValue("@numeroIngreso", obj.numeroIngreso);
                    cmd.Parameters.AddWithValue("@idInstructor", obj.instructor.idInstructor);
                    cmd.Parameters.AddWithValue("@Identificacion", obj.aprendiz.Identificacion);
                    cmd.Parameters.AddWithValue("@fechaIngreso", obj.fechaIngreso);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al editar la asistencia: " + ex.ToString());
            }
        }

        public void EliminarIngreso(Ingreso obj)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ElimiarAsistencia", oconexion);
                    cmd.Parameters.AddWithValue("@numeroIngreso", obj.numeroIngreso);
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

        public string FormatTimeTo12Hour(TimeSpan time)
        {
            return DateTime.Today.Add(time).ToString("hh:mm tt");
        }


        public static TimeSpan TruncateToSeconds(TimeSpan time)
        {
            return new TimeSpan(time.Hours, time.Minutes, time.Seconds);
        }


        public Ingreso ObtenerIngresoPorNumero(int numeroIngreso)
        {
            Ingreso ingreso = null;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ObtenerIngresoPorNumero", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@numeroIngreso", numeroIngreso);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            ingreso = new Ingreso()
                            {
                                numeroIngreso = Convert.ToInt32(dr["numeroIngreso"]),
                                instructor = new Instructor() { idInstructor = Convert.ToInt32(dr["idInstructor"]) },
                                aprendiz = new Aprendiz() { Identificacion = Convert.ToInt32(dr["Identificacion"]) },
                                fechaIngreso = Convert.ToDateTime(dr["fechaIngreso"]).Date,
                                horaIngreso = TruncateToSeconds(TimeSpan.Parse(dr["horaIngreso"].ToString())),
                                horaSalida = dr["horaSalida"] != DBNull.Value ? (TimeSpan?)TruncateToSeconds(TimeSpan.Parse(dr["horaSalida"].ToString())) : null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return ingreso;
        }
    }
}