using HospitalesSaludIntegral.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Logica
{
    class CLpagoplanilla
    {
        CDpagoplanilla cd_pagoplanilla = new CDpagoplanilla();
        public DateTime MtdFechaHoy()
        {
            return DateTime.Now;
        }

        public double MtdSueldoBono(double Sueldo)
        {
            double Bono = 0;
            Bono = Sueldo * 0.12;

            return Bono;
        }

        public double MtdSueldoTotal(double Sueldo, double Bono, int HorasX)
        {
            double Total = 0;
            Total = Sueldo + Bono + (HorasX * 20);
            return Total;
        }
    }
}
