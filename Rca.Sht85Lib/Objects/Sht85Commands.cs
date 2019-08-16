using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib.Objects
{
    /// <summary>
    /// SHT85 sensor commands
    /// </summary>
    internal enum Sht85Commands : UInt16
    {
        /// <summary>
        /// Read serial number
        /// </summary>
        READ_SERIALNBR = 0x3780,
        /// <summary>
        /// Read status register
        /// </summary>
        READ_STATUS = 0xF32D,
        /// <summary>
        /// Clear status register
        /// </summary>
        CLEAR_STATUS = 0x3041,
        /// <summary>
        /// Enabled heater
        /// </summary>
        HEATER_ENABLE = 0x306D,
        /// <summary>
        /// Disable heater
        /// </summary>
        HEATER_DISABLE = 0x3066,
        /// <summary>
        /// Soft reset
        /// </summary>
        SOFT_RESET = 0x30A2,
        /// <summary>
        /// Single measurement, high repeatability
        /// </summary>
        MEAS_SINGLE_H = 0x2400,
        /// <summary>
        /// Single measurement, medium repeatability
        /// </summary>
        MEAS_SINGLE_M = 0x240B,
        /// <summary>
        /// Single measurement, low repeatability
        /// </summary>
        MEAS_SINGLE_L = 0x2416,
        /// <summary>
        /// Periodic meas. 0.5 mps, high repeatability
        /// </summary>
        MEAS_PERI_05_H = 0x2032,
        /// <summary>
        /// Periodic meas. 0.5 mps, medium repeatability
        /// </summary>
        MEAS_PERI_05_M = 0x2024,
        /// <summary>
        /// Periodic meas. 0.5 mps, low repeatability
        /// </summary>
        MEAS_PERI_05_L = 0x202F,
        /// <summary>
        /// Periodic meas. 1 mps, high repeatability
        /// </summary>
        MEAS_PERI_1_H = 0x2130,
        /// <summary>
        /// Periodic meas. 1 mps, medium repeatability
        /// </summary>
        MEAS_PERI_1_M = 0x2126,
        /// <summary>
        /// Periodic meas. 1 mps, low repeatability
        /// </summary>
        MEAS_PERI_1_L = 0x212D,
        /// <summary>
        /// Periodic meas. 2 mps, high repeatability
        /// </summary>
        MEAS_PERI_2_H = 0x2236,
        /// <summary>
        /// Periodic meas. 2 mps, medium repeatability
        /// </summary>
        MEAS_PERI_2_M = 0x2220,
        /// <summary>
        /// Periodic meas. 2 mps, low repeatability
        /// </summary>
        MEAS_PERI_2_L = 0x222B,
        /// <summary>
        /// Periodic meas. 4 mps, high repeatability
        /// </summary>
        MEAS_PERI_4_H = 0x2334,
        /// <summary>
        /// Periodic meas. 4 mps, medium repeatability
        /// </summary>
        MEAS_PERI_4_M = 0x2322,
        /// <summary>
        /// Periodic meas. 4 mps, low repeatability
        /// </summary>
        MEAS_PERI_4_L = 0x2329,
        /// <summary>
        /// Periodic meas. 10 mps, high repeatability
        /// </summary>
        MEAS_PERI_10_H = 0x2737,
        /// <summary>
        /// Periodic meas. 10 mps, medium repeatability
        /// </summary>
        MEAS_PERI_10_M = 0x2721,
        /// <summary>
        /// Periodic meas. 10 mps, low repeatability
        /// </summary>
        MEAS_PERI_10_L = 0x272A,
        /// <summary>
        /// Readout measurements for periodic mode
        /// </summary>
        FETCH_DATA = 0xE000,
        /// <summary>
        /// Stop periodic measurement
        /// </summary>
        BREAK = 0x3093
    }
}
