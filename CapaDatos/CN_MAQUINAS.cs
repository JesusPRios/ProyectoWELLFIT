using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaTablas;

namespace CapaDatos
{
    public class CN_MAQUINAS
    {
        private CD_MAQUINA CD = new CD_MAQUINA();

        public List<Maquina> Listar(int idInstructor)
        {
            return CD.Listar(idInstructor);
        }
    }
}
