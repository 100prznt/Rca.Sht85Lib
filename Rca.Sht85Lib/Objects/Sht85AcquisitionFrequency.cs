using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib.Objects
{
    public enum Sht85AcquisitionFrequency
    {
        /// <summary>
        /// 0.5 measurements per second
        /// </summary>
        MPS_0_5,
        /// <summary>
        /// 1 measurement per second
        /// </summary>
        MPS_1,
        /// <summary>
        /// 2 measurements per second
        /// </summary>
        MPS_2,
        /// <summary>
        /// 4 measurements per second
        /// </summary>
        MPS_4,
        /// <summary>
        /// 10 measurements per second
        /// </summary>
        MPS_10
    }
}
