using CapaDatos;
using CapaTablas;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using Microsoft.Extensions.Configuration;


namespace CapaInstructorAdmin.Controllers
{

    public class AccesoController : Controller
    { 

        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Name, string contraseña, string TipoUser)
        {
            Name = Name?.Trim();
            contraseña = contraseña?.Trim();
            TipoUser = TipoUser?.Trim();

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Debe llenar los campos anteriores";
                return View();
            }

            if (string.IsNullOrEmpty(TipoUser))
            {
                ViewBag.Error = "Debe seleccionar un tipo de usuario";
                return View();
            }

            if (TipoUser == "Instructor")
            {
                Instructor oInstructor = new CD_INSTRUCTOR().Listar()
                    .FirstOrDefault(item => item.NombreInstructor.Equals(Name, StringComparison.OrdinalIgnoreCase) && item.Contraseña == contraseña);

                if (oInstructor == null)
                {
                    ViewBag.Error = "Nombre o Contraseña incorrectas";
                    return View();
                }

                Session["idInstructor"] = oInstructor.idInstructor;

                FormsAuthentication.SetAuthCookie(oInstructor.NombreInstructor, false);
                return RedirectToAction("Index", "Home");
            }
            else if (TipoUser == "Aprendiz")
            {
                Aprendiz oAprendiz = new CD_APRENDIZ().Listar()
                    .FirstOrDefault(item => item.Nombre.Equals(Name, StringComparison.OrdinalIgnoreCase) && item.Contraseña == contraseña);

                if (oAprendiz == null)
                {
                    ViewBag.Error = "Nombre o Contraseña incorrectas";
                    return View();
                }

                // Asignar el id del aprendiz a la sesión
                Session["IdAprendiz"] = oAprendiz.Identificacion;

                FormsAuthentication.SetAuthCookie(oAprendiz.Nombre, false);
                return RedirectToAction("Index", "Aprendiz");
            }
            else
            {
                ViewBag.Error = "Tipo de usuario no válido";
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Acceso");
        }

        public ActionResult ReestablecerContraseña()
        {
            return View();
        }

        private string GenerarContraseñaAleatoria()
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            string contraseña = new string(Enumerable.Repeat(caracteres, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return contraseña;
        }

        public ActionResult RegistrarAdmin()
        {
            return View();
        }

        public ActionResult RegistrarAprendiz()
        {
            return View();
        }

        public ActionResult RegistrarInstructor(int idInstructor, string NombreInstructor, string Contraseña)
        {
            try
            {
                CD_INSTRUCTOR instructor = new CD_INSTRUCTOR();

                // Obtener la lista de instructores
                List<Instructor> listaInstructores = instructor.Listar();

                // Verificar si ya existe un instructor con el mismo ID o Nombre
                bool instructorExiste = listaInstructores.Any(i => i.idInstructor == idInstructor || i.NombreInstructor == NombreInstructor);

                if (instructorExiste)
                {
                    ViewBag.Error = "El instructor con el mismo ID o Nombre ya existe.";
                    return View("RegistrarAdmin"); // O redirige a una vista de error si prefieres
                }

                // Si no existe, registrar el nuevo instructor
                Instructor oInstructor = new Instructor
                {
                    idInstructor = idInstructor,
                    NombreInstructor = NombreInstructor,
                    Contraseña = Contraseña
                };

                instructor.RegistrarInstructor(idInstructor, NombreInstructor, Contraseña);
                TempData["Success"] = "Instructor registrado exitosamente.";
                return RedirectToAction("RegistrarAdmin");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al registrar el instructor: " + ex.Message;
                return RedirectToAction("RegistrarAdmin");
            }
        }


        public ActionResult RegistrarAprendices(int idAprendiz, string NombreAprendiz, string tipoDocumento, int numeroFicha, string programaFormacion, string EstadoAprendiz, string Contraseña)
        {
            try
            {
                CD_APRENDIZ aprenidiz = new CD_APRENDIZ();

                List<Aprendiz> listaAprendiz = aprenidiz.Listar();

                // Verificar si ya existe un instructor con el mismo ID o Nombre
                bool aprendizexiste = listaAprendiz.Any(i => i.Identificacion == idAprendiz || i.Nombre == NombreAprendiz);

                if (aprendizexiste)
                {
                    ViewBag.Error = "El aprendiz ya existe.";
                    return View("RegistrarAprendiz"); // O redirige a una vista de error si prefieres
                }


                CD_APRENDIZ aprendiz = new CD_APRENDIZ();
                Aprendiz nuevoAprendiz = new Aprendiz
                {
                    Identificacion = idAprendiz,
                    Nombre = NombreAprendiz,
                    tipoDocumento = tipoDocumento,
                    Ficha = numeroFicha,
                    ProgramaFormacion = programaFormacion,
                    Estado = EstadoAprendiz,
                    Contraseña = Contraseña
                };

                aprendiz.RegistrarAprendiz(nuevoAprendiz);
                TempData["Success"] = "Registro exito!";
                return RedirectToAction("RegistrarAprendiz");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al registrar el aprendiz: " + ex.Message;
                return RedirectToAction("RegistrarAprendiz");
            }
        }

    }
}
