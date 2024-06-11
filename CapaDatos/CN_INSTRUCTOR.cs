using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaTablas;

namespace CapaDatos
{
    public class CN_INSTRUCTOR
    {
        private CD_INSTRUCTOR CD = new CD_INSTRUCTOR();

        public List<Instructor> Listar()
        {
            return CD.Listar();
        }

        public void RegistrarInstructor(int idInstructor, string NombreInstructor, string Contraseña)
        {
            CD.RegistrarInstructor(idInstructor, NombreInstructor, Contraseña);
        }
    }
}
