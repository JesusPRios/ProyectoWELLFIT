using CapaDatos;
using CapaTablas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace CapaInstructorAdmin.Controllers
{
    public class RutinaController : Controller
    {
        // GET: Rutina
        public ActionResult Index()
        {
            int idAprendiz = Convert.ToInt32(Session["IdAprendiz"]);
            CD_RUTINA cdRutina = new CD_RUTINA();

            // Llama al método ListarPorIdAprendiz para obtener la lista de rutinas asociadas al id del aprendiz
            List<Rutina> rutinas = cdRutina.Listar(idAprendiz);

            // Pasa la lista de rutinas a la vista
            return View(rutinas);
        }


        public ActionResult Details(int id, int identificacion)
        {
            CD_RUTINA cdRutina = new CD_RUTINA();
            Rutina rutina = cdRutina.ObtenerRutinaPorId(id, identificacion);

            if (rutina != null)
            {
                List<Ejercicio> ejercicios = cdRutina.ObtenerEjerciciosPorIdRutina(rutina);

                foreach (var ejercicio in ejercicios)
                {
                    if (ejercicio.maquina == null)
                    {
                        ejercicio.maquina = new Maquina { NombreMaquina = "Sin máquina seleccionada" };
                    }
                }

                var modelo = new EJERCICIO_has_RUTINA
                {
                    Rutina = rutina,
                    Ejercicios = ejercicios
                };

                return View(modelo);
            }
            else
            {
                // Manejar el caso donde la rutina no se encuentre o no pertenezca al aprendiz
                return RedirectToAction("Index");
            }
        }

        public ActionResult CrearRutina()
        {
            List<Maquina> maquinas = new CD_MAQUINA().Listar();
            ViewBag.Maquinas = maquinas;

            // Inicializar la propiedad Ejercicios del modelo EJERCICIO_has_RUTINA con una lista vacía
            var viewModel = new EJERCICIO_has_RUTINA
            {
                Rutina = new Rutina(),  // Asigna la rutina según sea necesario
                Ejercicios = new List<Ejercicio>() // Inicializa la lista de ejercicios
            };

            List<Aprendiz> aprendiz = new CD_APRENDIZ().Listar();

            var selectListItemsAprendices = aprendiz.Select(a => new SelectListItem
            {
                Value = a.Identificacion.ToString(),
                Text = $"{a.Identificacion} - {a.Nombre}"
            }).ToList();

            ViewBag.Dificultades = new List<SelectListItem>
            {
                new SelectListItem { Value = "Alta", Text = "Alta" },
                new SelectListItem { Value = "Media", Text = "Media" },
                new SelectListItem { Value = "Fácil", Text = "Fácil" }
            };

            ViewBag.Dificultades = ViewBag.Dificultades ?? new List<SelectListItem>();
            ViewBag.Aprendiz = new SelectList(selectListItemsAprendices, "Value", "Text");


            // Devolver la vista CrearRutina con el modelo inicializado
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CrearRutina(EJERCICIO_has_RUTINA obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convertir la lista de ejercicios a una lista de EjercicioPlano
                    var ejerciciosPlanos = obj.Ejercicios.Select(e => new EjerciciosTable
                    {
                        Nombre = e.Nombre,
                        Tipo = e.Tipo,
                        Dificultad = e.Dificultad,
                        numeroMaquina = e.maquina.numeroMaquina
                    }).ToList();

                    // Serializar la lista de ejercicios planos a JSON
                    string ejerciciosParaRegistrar = JsonConvert.SerializeObject(ejerciciosPlanos);

                    // Llamar al método para registrar la rutina y los ejercicios
                    int idRutina = RegistrarRutina(obj.Rutina, ejerciciosParaRegistrar);

                    if (idRutina > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "Error al crear la rutina";
                        return View(obj);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al crear la rutina: " + ex.Message;
                    return View(obj);
                }
            }
            else
            {
                ViewBag.Error = "Debe llenar los campos correctamente";
                return View(obj);
            }
        }


        public int RegistrarRutina(Rutina rutina, string ejerciciosParaRegistrar)
        {
            int idRutina = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand command = new SqlCommand("RegistrarRutinaYEjercicios", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros del procedimiento almacenado
                        command.Parameters.AddWithValue("@Descripcion", rutina.Descripcion);
                        command.Parameters.AddWithValue("@nombreRutina", rutina.nombreRutina);
                        command.Parameters.AddWithValue("@Identificacion", rutina.aprendiz.Identificacion);
                        command.Parameters.AddWithValue("@EjerciciosParaRegistrar", ejerciciosParaRegistrar);

                        SqlParameter idRutinaParam = new SqlParameter("@idRutina", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(idRutinaParam);

                        // Verificar el contenido del JSON
                        Console.WriteLine("JSON enviado al procedimiento almacenado:");
                        Console.WriteLine(ejerciciosParaRegistrar);

                        connection.Open();
                        command.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        idRutina = Convert.ToInt32(idRutinaParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error al registrar la rutina y ejercicios: " + ex.ToString());
                idRutina = 0;
            }

            return idRutina;
        }

        public ActionResult EliminarRutina(int id, int identificacion)
        {
            try
            {
                CD_RUTINA cdRutina = new CD_RUTINA();
                Rutina rutina = new Rutina
                {
                    idRutina = id,
                    aprendiz = new Aprendiz { Identificacion = identificacion }
                };

                cdRutina.EliminarRutina(rutina);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar la rutina: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditarRutina(int id, int identificacion)
        {
            CD_RUTINA cdRutina = new CD_RUTINA();
            Rutina rutina = cdRutina.ObtenerRutinaPorId(id, identificacion);

            List<Maquina> maquinas = new CD_MAQUINA().Listar();

            // Crear una lista de SelectListItems combinando ID y nombre para las máquinas
            var selectListItemsMaquinas = maquinas.Select(i => new SelectListItem
            {
                Value = i.numeroMaquina.ToString(),
                Text = $" {i.numeroMaquina} - {i.NombreMaquina}"
            }).ToList();

            ViewBag.Dificultades = new List<SelectListItem>
            {
                new SelectListItem { Value = "Alta", Text = "Alta" },
                new SelectListItem { Value = "Media", Text = "Media" },
                new SelectListItem { Value = "Fácil", Text = "Fácil" }
            };


            if (rutina != null)
            {
                List<Ejercicio> ejercicios = cdRutina.ObtenerEjerciciosPorIdRutina(rutina);

                // Obtener los números de máquina de los ejercicios
                var numerosMaquina = ejercicios.Select(e => e.maquina?.numeroMaquina).ToList();

                // Obtener la máquina del primer ejercicio si existe
                var maquinaSeleccionada = ejercicios.FirstOrDefault()?.maquina?.numeroMaquina;

                // Crear el SelectList para el ViewBag.Maquinas
                var selectListMaquinas = new List<SelectListItem>();

                // Agregar un elemento de "sin selección" al principio de la lista
                selectListMaquinas.Add(new SelectListItem { Value = "", Text = "Sin seleccionar" });

                // Agregar las máquinas al SelectList
                selectListMaquinas.AddRange(selectListItemsMaquinas);

                // Seleccionar la máquina del primer ejercicio si existe
                var maquinaSeleccionadaValue = maquinaSeleccionada != null ? maquinaSeleccionada.ToString() : "";

                // Asignar las listas de maquinas y dificultades al ViewBag
                ViewBag.Maquinas = new SelectList(selectListItemsMaquinas, "Value", "Text", maquinaSeleccionadaValue);

                ViewBag.Dificultades = ViewBag.Dificultades ?? new List<SelectListItem>();

                var viewModel = new EJERCICIO_has_RUTINA
                {
                    Rutina = rutina,
                    Ejercicios = ejercicios
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Editar(Rutina obj, int Identificacion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    obj.aprendiz = new Aprendiz { Identificacion = Identificacion };
                    CD_RUTINA cdRutina = new CD_RUTINA();
                    cdRutina.EditarRutina(obj);
                    return RedirectToAction("Index", new { id = obj.idRutina, Identificacion });
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al editar la rutina: " + ex.Message;
                    return View(obj);
                }
            }
            else
            {
                ViewBag.Error = "Debe llenar los campos correctamente";
                return View(obj);
            }
        }

        [HttpPost]
        public ActionResult EditarEjercicios(int idRutina, int identificacion, int idEjercicio, string Nombre, string Tipo, string Dificultad, int NumeroMaquina)
        {
            try
            {
                CD_RUTINA cdRutinaEjercicios = new CD_RUTINA();
                cdRutinaEjercicios.EditarEjerciciosDeRutina(idRutina, identificacion, idEjercicio, Nombre, Tipo, Dificultad, NumeroMaquina);

                return RedirectToAction("EditarRutina", new { id = idRutina, identificacion });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al editar el ejercicio de la rutina: " + ex.Message;
                return View();
            }
        }

        public ActionResult EliminarEjercicioDeRutina(int idRutina, int idEjercicio, int identificacion)
        {
            try
            {
                CD_RUTINA cdRutina = new CD_RUTINA();
                cdRutina.EliminarEjercicioDeRutina(idRutina, idEjercicio, identificacion);

                ViewBag.MostrarCarga = true;

                return RedirectToAction("EditarRutina", new { id = idRutina, identificacion }); // Cambio aquí

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar el ejercicio de la rutina: " + ex.Message;
                return RedirectToAction("EditarRutina", new { id = idRutina, identificacion }); // Cambio aquí
            }
        }
    }
}
