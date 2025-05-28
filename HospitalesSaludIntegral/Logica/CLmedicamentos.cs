using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Logica
{
    class CLmedicamentos
    {
        public DateTime MtdFechaHoy()
        {
            return DateTime.Now;
        }

        public int MtdCostoMedicamento(string TipoMedicamento)
        {
            if (TipoMedicamento == "Jarabe") return 150;
            else if (TipoMedicamento == "Suero") return 25;
            else if (TipoMedicamento == "Crema") return 75;
            else if (TipoMedicamento == "Tableta") return 25;
            else if (TipoMedicamento == "Inyeccion") return 125;

            return 0;
        }
    }
}
