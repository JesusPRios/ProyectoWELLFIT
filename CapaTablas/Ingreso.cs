using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaTablas
{
    public class Ingreso
    {
        public int numeroIngreso { get; set; }
        public Instructor instructor { get; set; }
        public Aprendiz aprendiz { get; set; }
        public DateTime fechaIngreso { get; set; }
        public TimeSpan horaIngreso { get; set; }
        public TimeSpan? horaSalida { get; set; }

        // Propiedades para las horas formateadas
        public string horaIngresoFormatted
        {
            get
            {
                return DateTime.Today.Add(horaIngreso).ToString("hh:mm tt");
            }
        }

        public string horaSalidaFormatted
        {
            get
            {
                return horaSalida.HasValue ? DateTime.Today.Add(horaSalida.Value).ToString("hh:mm tt") : null;
            }
        }
    }
}
