using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CN_REESTABLECERC
    {
        private CD_REESTABLECERC CD = new CD_REESTABLECERC();

        public void ReestablecerContraseña(string nombreInstructor, string nuevaContraseña)
        {
            CD.ReestablecerContraseña(nombreInstructor, nuevaContraseña);
        }
    }
}
