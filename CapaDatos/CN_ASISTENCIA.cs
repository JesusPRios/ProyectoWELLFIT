using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaTablas;

namespace CapaDatos
{
    public class CN_ASISTENCIA
    {
        private CD_ASISTENCIA CD = new CD_ASISTENCIA();

        public List<Ingreso> Listar()
        {
            return CD.Listar();
        }

        public List<Ingreso> ListarAsistenciaAprendiz(int idAprendiz)
        {
            return CD.ListarAsistenciaAprendiz(idAprendiz);
        }

        public void RegistrarAsistencia(Ingreso obj)
        {
            CD.RegistrarAsistencia(obj);
        }

        public void EditarAsistencia(Ingreso obj)
        {
            CD.EditarAsistencia(obj);
        }

        public void EliminarIngreso(Ingreso obj)
        {
            CD.EliminarIngreso(obj);
        }

        public Ingreso ObtenerIngresoPorNumero(int numeroIngreso)
        {
            return CD.ObtenerIngresoPorNumero(numeroIngreso);
        }
    }
}
