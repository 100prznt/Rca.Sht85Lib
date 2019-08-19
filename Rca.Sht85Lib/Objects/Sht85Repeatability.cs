using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib.Objects
{
    /// <summary>
    /// Repeatability
    /// </summary>
    public enum Sht85Repeatability
    {
        /// <summary>
        /// Low repeatability
        /// Max. duration time: 15.5 ms (typ: 12.5 ms)
        /// 
        /// </summary>
        High = 0x00,
        /// <summary>
        /// Low repeatability
        /// Max. duration time: 6.5 ms (typ: 4.5 ms)
        /// 
        /// </summary>
        Medium = 0x0B,
        /// <summary>
        /// Low repeatability
        /// Max. duration time: 4.5 ms (typ: 2.5 ms)
        /// </summary>
        Low = 0x16
    }
}
