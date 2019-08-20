using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib.Physics
{
    /// <summary>
    /// Physical calculator for dew point and absloute humidity
    /// </summary>
    public class Calculator
    {
        #region Constants
        /// <summary>
        /// Absoluter Nullpunkt = -273,15 °C
        /// </summary>
        const double ABS_ZEROPOINT = -273.15;
        /// <summary>
        /// MW – molare Masse von reinem Wasser = 18,01528 g/mol
        /// </summary>
        const double M_WATER = 18.01528;
        /// <summary>
        /// R - universelle Gaskonstante = 8,31446261815324 kg m^2 / s^2 mol K
        /// </summary>
        const double R = 8.31446261815324;
        readonly MagnusParameter MAG_WATER = MagnusParameter.OverWater();
        #endregion Constants

        #region Constructor
        /// <summary>
        /// New calculator with default parameters for 'over water'
        /// </summary>
        public Calculator()
        {
            //
        }
        #endregion Constructor

        #region Services
        /// <summary>
        /// Absolute humidity calculation
        /// </summary>
        /// <param name="values">Item1: Timestamp (not in use for calculation)
        /// Item2: Temperature in [°C]
        /// Item3: Relative humidity in [%RH]</param>
        /// <returns>Absolute humidity in [%]</returns>
        public double AbsoluteHumidity(Tuple<DateTime, double, double> values)
        {
            return AbsoluteHumidity(values.Item2, values.Item3);
        }

        /// <summary>
        /// Absolute humidity calculation
        /// </summary>
        /// <param name="values">Item1: Temperature in [°C]
        /// Item2: Relative humidity in [%RH]</param>
        /// <returns>Absolute humidity in [%]</returns>
        public double AbsoluteHumidity(Tuple<double, double> values)
        {
            return AbsoluteHumidity(values.Item1, values.Item2);
        }

        /// <summary>
        /// Absolute humidity calculation
        /// </summary>
        /// <param name="temperature">Temperature in [°C]</param>
        /// <param name="relHumidity">Relative humidity in [%RH]</param>
        /// <returns>Absolute humidity in [%]</returns>
        public double AbsoluteHumidity(double temperature, double relHumidity)
        {
            var t = temperature - ABS_ZEROPOINT;
            var ps = SaturationVapourPressure(temperature, relHumidity);
            var pv = VapourPressure(relHumidity, ps);

            var absHumidity = (M_WATER / R * pv / t) * 100;

            return absHumidity;
        }

        /// <summary>
        /// Dew point calculation
        /// </summary>
        /// <param name="values">Item1: Timestamp (not in use for calculation)
        /// Item2: Temperature in [°C]
        /// Item3: Relative humidity in [%RH]</param>
        /// <returns>Absolute humidity in [%]</returns>
        public double DewPoint(Tuple<DateTime, double, double> values)
        {
            return DewPoint(values.Item2, values.Item3);
        }

        /// <summary>
        /// Dew point calculation
        /// </summary>
        /// <param name="values">Item1: Temperature in [°C]
        /// Item2: Relative humidity in [%RH]</param>
        /// <returns>Absolute humidity in [%]</returns>
        public double DewPoint(Tuple<double, double> values)
        {
            return DewPoint(values.Item1, values.Item2);
        }

        /// <summary>
        /// Dew point calculation
        /// </summary>
        /// <param name="temperature">Temperature in [°C]</param>
        /// <param name="relHumidity">Relative humidity in [%RH]</param>
        /// <returns>Dew point temperature in [°C]</returns>
        public double DewPoint(double temperature, double relHumidity)
        {
            CheckValues(temperature, relHumidity, MAG_WATER);

            var dp = MAG_WATER.K3 * ((MAG_WATER.K2 * temperature / (MAG_WATER.K3 + temperature) + Math.Log(relHumidity / 100)) /
                (MAG_WATER.K2 * MAG_WATER.K3 / (MAG_WATER.K3 + temperature) - Math.Log(relHumidity / 100)));

            return dp;
        }

        /// <summary>
        /// Saturation-vapour-pressure calculation
        /// (Sättingungsdampfdruck)
        /// </summary>
        /// <param name="temperature">Temperature in [°C]</param>
        /// <param name="relHumidity">Relative humidity in [%RH]</param>
        /// <returns>Saturation vapour pressure in [hPa]</returns>
        public double SaturationVapourPressure(double temperature, double relHumidity)
        {
            CheckValues(temperature, relHumidity, MAG_WATER);

            double ps = MAG_WATER.K1 * Math.Exp(MAG_WATER.K2 * temperature / (MAG_WATER.K3 + temperature));

            return ps;
        }

        /// <summary>
        /// Vapour-pressure calculation
        /// (Dampfdruck)
        /// </summary>
        /// <param name="temperature">Temperature in [°C]</param>
        /// <param name="relHumidity">Relative humidity in [%RH]</param>
        /// <returns></returns>
        public double VapourPressure(double relHumidity, double saturationVapourPressure)
        {
            double vp = relHumidity / 100 * saturationVapourPressure;

            return vp;
        }
        #endregion Services
        
        #region Internal services
        private void CheckValues(double temperature, double relHumidity, MagnusParameter magnusParas)
        {
            if (temperature < magnusParas.LowerLimit || temperature > magnusParas.UpperLimit)
                throw new NotSupportedException($"Calculation only for a temperature range between {magnusParas.LowerLimit} and {magnusParas.UpperLimit} °C.");

            if (relHumidity < 0 || relHumidity > 100)
                throw new ArgumentOutOfRangeException($"Realtive humidity value ({relHumidity.ToString("F2")} %RH) out of valid range, 0 ... 100 %RH.");
        }

        #endregion Internal services

        #region Embedded structs
        /// <summary>
        /// Magnusparameters for the Magnus-Formula
        /// https://de.wikipedia.org/wiki/Magnus-Formel
        /// </summary>
        public struct MagnusParameter
        {
            /// <summary>
            /// K1 in [hPa]
            /// </summary>
            public double K1 { get; private set; }
            /// <summary>
            /// K2 factor
            /// </summary>
            public double K2 { get; private set; }
            /// <summary>
            /// K3 in [°C]
            /// </summary>
            public double K3 { get; private set; }


            /// <summary>
            /// Valid range upper limit in [°C]
            /// </summary>
            public double UpperLimit { get; private set; }
            /// <summary>
            /// Valid range lower limit in [°C]
            /// </summary>
            public double LowerLimit { get; private set; }


            /// <summary>
            /// Magnus parameter over water
            /// Valid from -45 °C to 60 °C
            /// </summary>
            /// <returns></returns>
            public static MagnusParameter OverWater()
            {
                return new MagnusParameter() { K1 = 6.112, K2 = 17.62, K3 = 243.12, UpperLimit = 60, LowerLimit = -45 };
            }

            /// <summary>
            /// Magnus parameter over ice
            /// Valid from -65 °C to 0.01 °C
            /// </summary>
            /// <returns></returns>
            public static MagnusParameter OverIce()
            {
                return new MagnusParameter() { K1 = 6.112, K2 = 22.46, K3 = 272.62, UpperLimit = 0.01, LowerLimit = -65 };
            }
        }

        #endregion Embedded structs
    }
}
