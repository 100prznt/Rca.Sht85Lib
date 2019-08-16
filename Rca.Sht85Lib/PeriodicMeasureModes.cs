using Rca.Sht85Lib.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib
{
    public enum PeriodicMeasureModes
    {
        Low0_5Hz = Sht85Commands.MEAS_PERI_05_L,
        Low1Hz = Sht85Commands.MEAS_PERI_1_L,
        Low2Hz = Sht85Commands.MEAS_PERI_2_L,
        Low4Hz = Sht85Commands.MEAS_PERI_4_L,
        Low10Hz = Sht85Commands.MEAS_PERI_10_L,
        Medium0_5Hz = Sht85Commands.MEAS_PERI_05_M,
        Medium1Hz = Sht85Commands.MEAS_PERI_1_M,
        Medium2Hz = Sht85Commands.MEAS_PERI_2_M,
        Medium4Hz = Sht85Commands.MEAS_PERI_4_M,
        Medium10Hz = Sht85Commands.MEAS_PERI_10_M,
        High0_5Hz = Sht85Commands.MEAS_PERI_05_H,
        High1Hz = Sht85Commands.MEAS_PERI_1_H,
        High2Hz = Sht85Commands.MEAS_PERI_2_H,
        High4Hz = Sht85Commands.MEAS_PERI_4_H,
        High10Hz = Sht85Commands.MEAS_PERI_10_H,
    }
}
