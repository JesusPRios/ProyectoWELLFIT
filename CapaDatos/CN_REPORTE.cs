using CapaTablas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CN_REPORTE
    {
        private CD_REPORTE CD = new CD_REPORTE();

        public int InsertarReporte(Reporte obj)
        {
            return CD.InsertarReporte(obj);
        }
    }
}
