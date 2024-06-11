using CapaDatos;
using CapaTablas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaInstructorAdmin.Controllers
{
    public class AsistenciaController : Controller
    {
        // GET: Asistencia
        public ActionResult Index()
        {
            int idAprendiz = Convert.ToInt32(Session["IdAprendiz"]);
            CD_ASISTENCIA CD = new CD_ASISTENCIA();
            List<Ingreso> lista = CD.ListarAsistenciaAprendiz(idAprendiz);
            return View(lista);
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


        [HttpPost]
        public JsonResult ValidarCodigoIngreso(string codigoIngreso)
        {
            codigoIngreso = codigoIngreso?.Trim();
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);
            // Llamamos al método para obtener el código diario del instructor
            string codigoDiario = ObtenerCodigoDiario(idInstructor);

            // Verificamos si el código ingresado coincide con el código diario
            if (codigoIngreso == codigoDiario)
            {
                return Json(new { success = true, message = "Código correcto. Puede proceder." });
            }
            else
            {
                return Json(new { success = false, message = "Código incorrecto. Por favor, inténtelo de nuevo." });
            }
        }


        public ActionResult Editar(int numeroIngreso)
        {
            // Obtener la lista de instructores y aprendices
            List<Instructor> instructores = new CD_INSTRUCTOR().Listar();
            List<Aprendiz> aprendiz = new CD_APRENDIZ().Listar();

            // Crear una lista de SelectListItems combinando ID y nombre para instructores
            var selectListItemsInstructores = instructores.Select(i => new SelectListItem
            {
                Value = i.idInstructor.ToString(),
                Text = $"{i.idInstructor} - {i.NombreInstructor}"
            }).ToList();

            // Crear una lista de SelectListItems combinando ID y nombre para aprendices
            var selectListItemsAprendices = aprendiz.Select(a => new SelectListItem
            {
                Value = a.Identificacion.ToString(),
                Text = $"{a.Identificacion} - {a.Nombre}"
            }).ToList();

            // Obtener el ingreso
            CN_ASISTENCIA cnAsistencia = new CN_ASISTENCIA();
            Ingreso ingreso = cnAsistencia.ObtenerIngresoPorNumero(numeroIngreso);

            if (ingreso == null)
            {
                System.Diagnostics.Debug.WriteLine($"No se encontró un ingreso con el número: {numeroIngreso}");
                return HttpNotFound();
            }

            // Asignar las listas de instructores y aprendices al ViewBag
            ViewBag.Instructores = new SelectList(selectListItemsInstructores, "Value", "Text", ingreso.instructor.idInstructor);
            ViewBag.Aprendiz = new SelectList(selectListItemsAprendices, "Value", "Text", ingreso.aprendiz.Identificacion);

            return View(ingreso);
        }


        [HttpPost]
        public ActionResult RegistrarSalida(int numeroIngreso)
        {
            try
            {
                CN_ASISTENCIA cnAsistencia = new CN_ASISTENCIA();
                Ingreso ingreso = cnAsistencia.ObtenerIngresoPorNumero(numeroIngreso);

                if (ingreso == null)
                {
                    return Json(new { success = false, message = "Ingreso no encontrado." });
                }

                cnAsistencia.EditarAsistencia(ingreso);

                return Json(new { success = true, message = "Salida registrada exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public ActionResult EliminarAsistencia(int numeroIngreso)
        {
            try
            {
                CD_ASISTENCIA cdAsistencia = new CD_ASISTENCIA();
                Ingreso ingresoAEliminar = new Ingreso
                {
                    numeroIngreso = numeroIngreso
                };
                cdAsistencia.EliminarIngreso(ingresoAEliminar);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar la asistencia: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Registrar(int idInstructor, int idAprendiz)
        {
            if (idInstructor == 0 || idAprendiz == 0)
            {
                ViewBag.Error = "Debe llenar todos los campos";
                return View();
            }

            try
            {
                CN_ASISTENCIA cdAsistencia = new CN_ASISTENCIA();
                Ingreso ingreso = new Ingreso
                {
                    instructor = new Instructor { idInstructor = idInstructor },
                    aprendiz = new Aprendiz { Identificacion = idAprendiz }
                };

                cdAsistencia.RegistrarAsistencia(ingreso);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al registrar la asistencia: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult RegistrarIngreso()
        {
            // Obtener la lista de instructores
            List<Instructor> instructores = new CD_INSTRUCTOR().Listar();
            List<Aprendiz> aprendiz = new CD_APRENDIZ().Listar();

            // Crear una lista de SelectListItems combinando ID y nombre
            var selectListItems = instructores.Select(i => new SelectListItem
            {
                Value = i.idInstructor.ToString(),
                Text = $"{i.idInstructor} - {i.NombreInstructor}"
            }).ToList();

            var selectListItemsAprendiz = aprendiz.Select(i => new SelectListItem
            {
                Value = i.Identificacion.ToString(),
                Text = $"{i.Identificacion} - {i.Nombre}"
            }).ToList();

            // Pasar la lista de instructores a la vista mediante ViewBag
            ViewBag.Instructores = selectListItems;
            ViewBag.Aprendiz = selectListItemsAprendiz;

            return View();
        }
    }
}
