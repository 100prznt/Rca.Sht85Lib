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
        [RefreshRate(0.5)]
        Low0_5Hz = Sht85Commands.MEAS_PERI_05_L,
        [RefreshRate(1)]
        Low1Hz = Sht85Commands.MEAS_PERI_1_L,
        [RefreshRate(2)]
        Low2Hz = Sht85Commands.MEAS_PERI_2_L,
        [RefreshRate(4)]
        Low4Hz = Sht85Commands.MEAS_PERI_4_L,
        [RefreshRate(10)]
        Low10Hz = Sht85Commands.MEAS_PERI_10_L,
        [RefreshRate(0.5)]
        Medium0_5Hz = Sht85Commands.MEAS_PERI_05_M,
        [RefreshRate(1)]
        Medium1Hz = Sht85Commands.MEAS_PERI_1_M,
        [RefreshRate(2)]
        Medium2Hz = Sht85Commands.MEAS_PERI_2_M,
        [RefreshRate(4)]
        Medium4Hz = Sht85Commands.MEAS_PERI_4_M,
        [RefreshRate(10)]
        Medium10Hz = Sht85Commands.MEAS_PERI_10_M,
        [RefreshRate(0.5)]
        High0_5Hz = Sht85Commands.MEAS_PERI_05_H,
        [RefreshRate(1)]
        High1Hz = Sht85Commands.MEAS_PERI_1_H,
        [RefreshRate(2)]
        High2Hz = Sht85Commands.MEAS_PERI_2_H,
        [RefreshRate(4)]
        High4Hz = Sht85Commands.MEAS_PERI_4_H,
        [RefreshRate(10)]
        High10Hz = Sht85Commands.MEAS_PERI_10_H,
    }
}
