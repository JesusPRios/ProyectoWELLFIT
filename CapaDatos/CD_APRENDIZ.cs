using CapaTablas;
using OpenTelemetry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_APRENDIZ
    {
        public List<Aprendiz> Listar()
        {
            List<Aprendiz> Lista = new List<Aprendiz>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string Query = "select Identificacion,tipoDocumento,Nombre,Contraseña,Ficha,ProgramaFormacion,Estado from Aprendiz";

                    SqlCommand cmd = new SqlCommand(Query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(
                                new Aprendiz()
                                {
                                    Identificacion = Convert.ToInt32(dr["Identificacion"]),
                                    tipoDocumento = dr["tipoDocumento"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Contraseña = dr["Contraseña"].ToString(),
                                    Ficha = Convert.ToInt32(dr["Ficha"]),
                                    ProgramaFormacion = dr["ProgramaFormacion"].ToString(),
                                    Estado = dr["Estado"].ToString()
                                    
                                }
                                );
                        }
                    }
                }
            }
            catch
            {
                Lista = new List<Aprendiz>();
            }

            return Lista;
        }

        public void RegistrarAprendiz(Aprendiz obj)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarAprendiz", oconexion);
                    cmd.Parameters.AddWithValue("@xidAprendiz", obj.Identificacion);
                    cmd.Parameters.AddWithValue("@xtipoDocumento", obj.tipoDocumento);
                    cmd.Parameters.AddWithValue("@xNombreAprendiz", obj.Nombre);
                    cmd.Parameters.AddWithValue("@xContraseña", obj.Contraseña);
                    cmd.Parameters.AddWithValue("@xnumeroFicha", obj.Ficha);
                    cmd.Parameters.AddWithValue("@xprogramaFormacion", obj.ProgramaFormacion);
                    cmd.Parameters.AddWithValue("@xEstadoAprendiz", obj.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar al aprendiz: " + ex.Message);
            }
        }
    }
}
