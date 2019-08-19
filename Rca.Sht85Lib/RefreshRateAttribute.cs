using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RefreshRateAttribute : Attribute
    {
        /// <summary>
        /// Frequency in [Hz]
        /// </summary>
        public double Frequency { get; set; }

        /// <summary>
        /// period time in [ms]
        /// calculated by frequency
        /// </summary>
        public int PeriodTime
        {
            get
            {
                return (int)(1000 / Frequency);
            }
        }

        public RefreshRateAttribute(double frequency)
        {
            Frequency = frequency;
        }
    }
}
