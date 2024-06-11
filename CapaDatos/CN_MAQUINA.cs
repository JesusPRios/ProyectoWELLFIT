using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaTablas;

namespace CapaDatos
{
    public class CN_MAQUINA
    {
        private CD_MAQUINA CD = new CD_MAQUINA();

        public List<Maquina> Listar()
        {
            return CD.Listar();
        }

        public void RegistrarMaquina(Maquina obj)
        {
            CD.RegistrarMaquina(obj);
        }

        public void EditarMaquina(Maquina obj)
        {
            CD.EditarMaquina(obj);
        }

        public void EliminarMaquina(int numeroMaquina)
        {
            CD.EliminarMaquina(numeroMaquina);
        }

        public Maquina ObtenerMaquinaPorNumero(int numeroMaquina)
        {
            return CD.ObtenerMaquinaPorNumero(numeroMaquina);
        }
    }
}
