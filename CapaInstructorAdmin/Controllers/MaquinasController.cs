using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaTablas;

namespace CapaInstructorAdmin.Controllers
{
    public class MaquinasController : Controller
    {
        // GET: Maquina
        public ActionResult Index()
        {
            CD_MAQUINA Maquina = new CD_MAQUINA();
            List<Maquina> listaMaquinas = Maquina.Listar();
            return View(listaMaquinas);
        }

        [HttpPost]
        public ActionResult EditarMaquina(Maquina maquina)
        {
            List<Instructor> instructores = new CD_INSTRUCTOR().Listar();
            ViewBag.Instructores = instructores.Select(i => new SelectListItem
            {
                Value = i.idInstructor.ToString(),
                Text = $"{i.idInstructor} - {i.NombreInstructor}"
            }).ToList();

            ViewBag.Estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Óptima", Text = "Óptima" },
                new SelectListItem { Value = "Mal estado", Text = "Mal estado" },
                new SelectListItem { Value = "En reparación", Text = "En reparación" },
                new SelectListItem { Value = "Desactivada o fuera de uso", Text = "Desactivada o fuera de uso" }
            };

            if (ModelState.IsValid)
            {
                try
                {
                    CN_MAQUINA cnMaquina = new CN_MAQUINA();
                    cnMaquina.EditarMaquina(maquina);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error al editar la máquina: " + ex.Message;
                    return View(maquina);
                }
            }
            else
            {
                ViewBag.Error = "Debe llenar los campos correctamente";
                return View(maquina);
            }
        }

        public ActionResult Editar(int numeroMaquina)
        {
            // Obtener la lista de instructores
            List<Instructor> instructores = new CD_INSTRUCTOR().Listar();

            // Crear una lista de SelectListItems combinando ID y nombre
            var selectListItems = instructores.Select(i => new SelectListItem
            {
                Value = i.idInstructor.ToString(),
                Text = $"{i.idInstructor} - {i.NombreInstructor}"
            }).ToList();

            // Pasar la lista de instructores a la vista mediante ViewBag
            ViewBag.Instructores = selectListItems;

            // Crear la lista de opciones de estado
            ViewBag.Estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Óptima", Text = "Óptima" },
                new SelectListItem { Value = "Mal estado", Text = "Mal estado" },
                new SelectListItem { Value = "En reparación", Text = "En reparación" },
                new SelectListItem { Value = "Desactivada o fuera de uso", Text = "Desactivada o fuera de uso" }
            };

            CN_MAQUINA cnMaquina = new CN_MAQUINA();
            Maquina maquina = cnMaquina.ObtenerMaquinaPorNumero(numeroMaquina);

            if (maquina == null)
            {
                return HttpNotFound();
            }

            return View(maquina);
        }


        [HttpPost]
        public ActionResult Registrar(int idInstructor, string NombreMaquina, string EstadoMaquina)
        {
            Instructor oInstructor = new CD_INSTRUCTOR().Listar()
                .FirstOrDefault(item => item.idInstructor == idInstructor);

            if (oInstructor == null)
            {
                ViewBag.Error = "El ID del instructor no existe";
                return RedirectToAction("Registrar", "Maquinas");
            }

            try
            {
                CN_MAQUINA cnMaquina = new CN_MAQUINA();
                Maquina maquina = new Maquina
                {
                    instructor = new Instructor { idInstructor = idInstructor },
                    NombreMaquina = NombreMaquina,
                    EstadoMaquina = EstadoMaquina
                };

                cnMaquina.RegistrarMaquina(maquina);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al registrar la máquina: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult RegistrarMaquina()
        {
            // Obtener la lista de instructores
            List<Instructor> instructores = new CD_INSTRUCTOR().Listar();

            // Crear una lista de SelectListItems combinando ID y nombre
            var selectListItems = instructores.Select(i => new SelectListItem
            {
                Value = i.idInstructor.ToString(),
                Text = $"{i.idInstructor} - {i.NombreInstructor}"
            }).ToList();

            // Pasar la lista de instructores a la vista mediante ViewBag
            ViewBag.Instructores = selectListItems;

            // Crear la lista de opciones de estado
            ViewBag.Estados = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Óptima", Text = "Óptima" },
                    new SelectListItem { Value = "Mal estado", Text = "Mal estado" },
                    new SelectListItem { Value = "En reparación", Text = "En reparación" },
                    new SelectListItem { Value = "Desactivada o fuera de uso", Text = "Desactivada o fuera de uso" }
                };

            return View();
        }

        public ActionResult EliminarMaquina(int numeroMaquina)
        {
            try
            {
                CN_MAQUINA maquina = new CN_MAQUINA();
                maquina.EliminarMaquina(numeroMaquina);
                TempData["SuccessMessage"] = "Máquina eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }
            return RedirectToAction("Index", "Maquinas");
        }
    }
}

