using CapaDatos;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CapaInstructorAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly CD_INSTRUCTOR cdInstructor = new CD_INSTRUCTOR();

        // GET: Home
        public ActionResult Index()
        {
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);
            string codigoDiario = ObtenerCodigoDiario(idInstructor);
            ViewBag.CodigoDiario = codigoDiario;
            return View();
        }

        [HttpPost]
        public ActionResult GenerarCodigoDiarioAction()
        {
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);
            string codigoDiario = ObtenerCodigoDiario(idInstructor);

            if (string.IsNullOrEmpty(codigoDiario))
            {
                // Generar el código diario solo si no existe para la fecha actual
                string fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
                codigoDiario = GenerarCodigoDiarioAleatorio();
                cdInstructor.ActualizarCodigoDiario(idInstructor, codigoDiario, fechaActual);
            }

            ViewBag.CodigoDiario = codigoDiario;
            return View("Index");
        }

        // Método para obtener el código diario del instructor
        private string ObtenerCodigoDiario(int idInstructor)
        {
            return cdInstructor.ObtenerCodigoDiario(idInstructor);
        }

        // Método para generar un código diario aleatorio
        private string GenerarCodigoDiarioAleatorio()
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            string codigoDiario = new string(Enumerable.Repeat(caracteres, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return codigoDiario;
        }
    }
}