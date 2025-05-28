using HospitalesSaludIntegral.Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalesSaludIntegral.Logica
{
    class CLpagocitas
    {
        public DateTime MtdFechaHoy()
        {
            return DateTime.Now;
        }

        public double MtdMontoCita(DataTable Costos)
        {
            if (Costos.Rows.Count == 0) return 0;

            double costoTratamientos = Convert.ToDouble(Costos.Rows[0]["CostoTratamientos"]);
            double costoHabitacion = Convert.ToDouble(Costos.Rows[0]["CostoHabitacion"]);

            return costoTratamientos + costoHabitacion;
        }
        public double MtdImpuestoPago(double MontoCita)
        {
            return MontoCita * 0.12;
        }
        public double DescuentoPago(double MontoCita)
        {
            if (MontoCita > 0 && MontoCita <= 500) return MontoCita * 0.03;
            else if (MontoCita > 500 && MontoCita <= 5000) return MontoCita * 0.05;
            else if (MontoCita > 5000) return MontoCita * 0.10;
            return 0;
        }
        public double MtdTotalPago(double MontoCita, double Impuesto, double Descuento)
        {
            return MontoCita + Impuesto - Descuento;

        }
    }
}
