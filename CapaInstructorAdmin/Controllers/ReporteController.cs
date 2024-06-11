using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using CapaTablas;

namespace CapaInstructorAdmin.Controllers
{
    public class ReporteController : Controller
    {
        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult GenerarReporte(string tipo, string mes)
        {
            // Verificar si hay registros para el tipo de reporte y mes seleccionado
            switch (tipo)
            {
                case "Asistencia":
                    if (!ConsultarAsistencia(mes).Any())
                    {
                        ViewBag.MostrarCarga = true;
                        return Content("No hay registros de asistencia para el mes seleccionado.");
                    }
                    break;

                case "Maquinas":
                    if (!ConsultarMaquina().Any())
                    {
                        ViewBag.MostrarCarga = true;
                        return Content("No hay registros de máquinas.");
                    }
                    break;

                default:
                    return Content("Tipo de reporte no válido");
            }

            // Si hay registros disponibles, continuar con la generación del reporte
            Reporte nuevoReporte = new Reporte
            {
                Tipo = tipo,
                fechaReporte = DateTime.Now
            };

            CD_REPORTE cdReporte = new CD_REPORTE();
            cdReporte.InsertarReporte(nuevoReporte);

            List<object> dataSource = new List<object>();
            string reportPath = Server.MapPath($"~/Reportes/Reporte{tipo}.rpt");
            ReportDocument rd = new ReportDocument();

            switch (tipo)
            {
                case "Asistencia":
                    dataSource = ConsultarAsistencia(mes);
                    ViewBag.MostrarCarga = true;
                    break;

                case "Maquinas":
                    dataSource = ConsultarMaquina();
                    ViewBag.MostrarCarga = true;
                    break;

                default:
                    return Content("Tipo de reporte no válido");
            }

            if (dataSource == null || !dataSource.Any())
            {
                return Content("No hay registros para el tipo y mes seleccionados.");
            }

            rd.Load(reportPath);
            rd.SetDataSource(dataSource);

            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            byte[] pdfByteArray = new byte[stream.Length];
            stream.Read(pdfByteArray, 0, (int)stream.Length);
            stream.Close();

            return File(pdfByteArray, "application/pdf");
        }

        public ActionResult DescargarReporte(string tipo, string mes)
        {
            try
            {
                List<object> dataSource = new List<object>();

                switch (tipo)
                {
                    case "Asistencia":
                        dataSource = ConsultarAsistencia(mes);
                        break;

                    case "Maquinas":
                        dataSource = ConsultarMaquina();
                        break;

                    default:
                        return Content("Tipo de reporte no válido");
                }

                if (dataSource == null || !dataSource.Any())
                {
                    return Content("No hay registros para el tipo y mes seleccionados.");
                }

                string reportPath = Server.MapPath($"~/Reportes/Reporte{tipo}.rpt");
                ReportDocument rd = new ReportDocument();
                rd.Load(reportPath);
                rd.SetDataSource(dataSource);

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", tipo + ".pdf");
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }

        public List<object> ConsultarMaquina()
        {
            CN_MAQUINA maquina = new CN_MAQUINA();
            List<CapaTablas.Maquina> listaMaquinas = maquina.Listar();
            return listaMaquinas.Cast<object>().ToList();
        }

        public List<object> ConsultarAsistencia(string mes)
        {
            CN_ASISTENCIA asistencia = new CN_ASISTENCIA();
            // Filtrar la lista por el mes especificado
            List<CapaTablas.Ingreso> lista = asistencia.Listar()
                .Where(a => a.fechaIngreso.ToString("yyyy-MM") == mes)
                .ToList();

            List<object> listaAsistencia = new List<object>();

            foreach (var item in lista)
            {

                listaAsistencia.Add(new
                {
                    numeroIngreso = item.numeroIngreso,
                    idInstructor = item.instructor.idInstructor,
                    Identificacion = item.aprendiz.Identificacion,
                    fechaIngreso = item.fechaIngreso.ToString("yyyy-MM-dd") // Formatear solo la fecha de ingreso
                });
            }

            return listaAsistencia;
        }
    }
}