using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CapaTablas;

namespace CapaInstructorAdmin.Controllers
{
    public class AprendizController : Controller
    {
        private CD_INSTRUCTOR cdInstructor = new CD_INSTRUCTOR();

        // GET: Aprendiz
        public ActionResult Index()
        {
            int idInstructor = Convert.ToInt32(Session["idInstructor"]);
            string codigoDiario = cdInstructor.ObtenerCodigoDiario(idInstructor);

            // Pasar el código diario a la vista
            ViewBag.CodigoDiario = codigoDiario;

            return View();
        }

        // GET: Aprendiz/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Aprendiz/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Aprendiz/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Aprendiz/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Aprendiz/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Aprendiz/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Aprendiz/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
