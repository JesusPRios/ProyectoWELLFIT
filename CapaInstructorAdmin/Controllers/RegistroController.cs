using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CapaDatos;
using CapaTablas;

namespace CapaInstructorAdmin.Controllers
{
    public class RegistroController : Controller
    {
        // GET: Registro
        public ActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            CD_ASISTENCIA CD = new CD_ASISTENCIA();
            List<Ingreso> lista = CD.Listar();

            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                lista = lista.Where(i => i.fechaIngreso >= fechaInicio.Value && i.fechaIngreso <= fechaFin.Value).ToList();
            }
            else
            {
                if (!lista.Any())
                {
                    ViewBag.Message = "No hay registros de ingreso para el rango de fechas seleccionado.";
                }
            }

            return View(lista);
        }
    }
}