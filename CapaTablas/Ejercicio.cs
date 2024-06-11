using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaTablas
{
    public class Ejercicio
    {
        public int IdEjercicio { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Dificultad { get; set; }
        public Maquina maquina { get; set; }
    }
}
