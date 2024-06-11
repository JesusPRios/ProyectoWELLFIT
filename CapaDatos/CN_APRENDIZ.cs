using CapaTablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CN_APRENDIZ
    {
        private CD_APRENDIZ CD = new CD_APRENDIZ();

        public List<Aprendiz> Listar()
        {
            return CD.Listar();
        }

        public void RegistrarAprendiz(Aprendiz obj)
        {
            CD.RegistrarAprendiz(obj);
        }
    }
}
