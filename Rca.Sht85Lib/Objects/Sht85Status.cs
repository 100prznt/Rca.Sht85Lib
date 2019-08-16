using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rca.Sht85Lib.Objects
{
    /// <summary>
    /// SHT85 Status register
    /// </summary>
    public class Sht85Status
    {
        /// <summary>
        /// Write data checksum status
        /// false: checksum of last write transfer was correct
        /// true: checksum of last write transfer failed
        /// </summary>
        public bool WriteDataCrcStatus { get; set; }

        /// <summary>
        /// Command status
        /// false: last command executed successfully
        /// true: last command not processed. It was either invalid, failed the integrated command checksum
        /// </summary>
        public bool CommandStatus { get; set; }

        /// <summary>
        /// System reset detected 
        /// false: no reset detected since last 'clear status register' command
        /// true: reset detected(hard reset, soft reset command or supply fail)
        /// </summary>
        public bool ResetDetected { get; set; }

        /// <summary>
        /// T tracking alert
        /// false: no alert
        /// true: alert
        /// </summary>
        public bool TTrackingAlert { get; set; }

        /// <summary>
        /// RH tracking alert
        /// false: no alert
        /// true: alert
        /// </summary>
        public bool RhTrackingAlert { get; set; }

        /// <summary>
        /// Heater status
        /// false: Heater OFF
        /// true: Heater ON
        /// </summary>
        public bool HeaterStatus { get; set; }

        /// <summary>
        /// Alert pending status
        /// false: no pending alerts
        /// true: at least one pending alert
        /// </summary>
        public bool AlertPendingStatus { get; set; }

        public static Sht85Status Parse(byte[] data)
        {
            if (data.Length != 2)
                throw new ArgumentOutOfRangeException("Length of data must be 2 byte!");

            var bits = new BitArray(data);
            var status = new Sht85Status()
            {
                WriteDataCrcStatus = bits.Get(0),
                CommandStatus = bits.Get(1),
                ResetDetected = bits.Get(4),
                TTrackingAlert = bits.Get(10),
                RhTrackingAlert = bits.Get(11),
                HeaterStatus = bits.Get(13),
                AlertPendingStatus = bits.Get(15)
            };

            return status;
        }
    }
}
