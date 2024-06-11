using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaTablas
{
    public class Rutina
    {
        public int idRutina { get; set; }
        public string Descripcion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string nombreRutina { get; set; }
        public Aprendiz aprendiz { get; set; }
    }
}
