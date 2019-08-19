
using Rca.Sht85Lib.Helpers;
using Rca.Sht85Lib.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace Rca.Sht85Lib
{
    /// <summary>
    /// Humidity and Temperature Sensor
    /// </summary>
    public class Sht85 : IDisposable
    {
        #region Constants
        private const int I2C_READ_ATTEMPTS = 3;
        private const int DEFAULT_ADDRESS = 0x44; //SHT85 address
        protected const int PROCESSING_DELAY = 1; //in [ms]

        #endregion Constants

        #region Members
        private I2cDevice m_Sensor { get; set; }
        private int m_RemainingI2cReadAttempts;

        private BackgroundWorker m_FetchDataWorker;

        #endregion Members

        #region Properties
        /// <summary>
        /// I2C bus speed
        /// </summary>
        public I2cBusSpeed BusSpeed { get; set; } = I2cBusSpeed.FastMode;

        /// <summary>
        /// Specific serial number
        /// </summary>
        public UInt32 SerialNumber
        {
            get => GetSerialNumber();
        }

        /// <summary>
        /// Heater control
        /// </summary>
        public bool HeaterState
        {
            get => GetHeaterState();
            set => SetHeaterState(value);
        }

        #endregion Properties

        #region Constructor
        /// <summary>
        /// Generate a new SHT85 sensor object
        /// </summary>
        /// <param name="slaveAddress">I2C address</param>
        public Sht85(byte slaveAddress = DEFAULT_ADDRESS)
        {
            Init(slaveAddress);
        }

        /// <summary>
        /// Kill the current sensor object
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion Constructor

        #region Services
        /// <summary>
        /// Perform a soft reset
        /// </summary>
        public void Reset()
        {
            WriteCommand(Sht85Commands.SOFT_RESET);
            SpinWait.SpinUntil(() => false, 2);
        }

        /// <summary>
        /// Perform a single reading
        /// </summary>
        /// <param name="repeatability">Repeatability</param>
        /// <returns>Measvalues
        /// Item1: Temperature in °C
        /// Item2: Humidity in %RH</returns>
        public Tuple<double, double> SingleShot(Sht85Repeatability repeatability = Sht85Repeatability.Low)
        {
            switch (repeatability)
            {
                case Sht85Repeatability.High:
                    WriteCommand(Sht85Commands.MEAS_SINGLE_H);
                    break;
                case Sht85Repeatability.Medium:
                    WriteCommand(Sht85Commands.MEAS_SINGLE_M);
                    break;
                case Sht85Repeatability.Low:
                    WriteCommand(Sht85Commands.MEAS_SINGLE_L);
                    break;
            }

            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);

            var data = ReadWordsAndCrc(2);

            var t = ConvertTemperature(data.Range(0, 2));
            var rh = ConvertHumidity(data.Range(2, 2));

            return new Tuple<double, double>(t, rh);
        }

        /// <summary>
        /// Start periodic data acquisition mode
        /// Each measurement fires an event on <seealso cref="NewMeasData"/>
        /// </summary>
        /// <param name="mode">Repeatability and frequency</param>
        public void StartPeriodicDataAcquisitionMode(PeriodicMeasureModes mode)
        {
            WriteCommand((Sht85Commands)mode);
            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);

            m_FetchDataWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };

            m_FetchDataWorker.DoWork += new DoWorkEventHandler(FetchDataWorker_DoWork);
            m_FetchDataWorker.RunWorkerAsync(mode.GetPeriodTime());
        }


        /// <summary>
        /// Stop the periodic data acquisition mode
        /// </summary>
        public void StopPeriodicDataAcquisitionMode()
        {
            WriteCommand(Sht85Commands.BREAK);

            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);

            if (m_FetchDataWorker != null && m_FetchDataWorker.WorkerSupportsCancellation == true)
                m_FetchDataWorker.CancelAsync();
        }

        /// <summary>
        /// Read the SHT85 status register
        /// </summary>
        /// <returns>SHT85 status</returns>
        public Sht85Status GetStatus()
        {
            WriteCommand(Sht85Commands.READ_STATUS);

            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);

            var data = ReadWordsAndCrc(1);

            return Sht85Status.Parse(data);
        }

        /// <summary>
        /// Clear the status register
        /// </summary>
        public void ClearStatus()
        {
            WriteCommand(Sht85Commands.CLEAR_STATUS);

            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);
        }

        #endregion Services

        #region Internal services
        private void Init(int slaveAddress)
        {
            InitSensor(slaveAddress).Wait();

            //Softreset hier!
        }

        private async Task InitSensor(int slaveAddress)
        {
            var settings = new I2cConnectionSettings(slaveAddress) { BusSpeed = BusSpeed };

            string aqs = I2cDevice.GetDeviceSelector();
            var dis = await DeviceInformation.FindAllAsync(aqs);
            m_Sensor = await I2cDevice.FromIdAsync(dis[0].Id, settings);
        }

        private double ConvertTemperature(byte[] data)
        {
            var rawValue = BitConverter.ToUInt16(data, 0);
            return -45 + 175 * (rawValue / (Math.Pow(2, 16) - 1));
        }

        private double ConvertHumidity(byte[] data)
        {
            var rawValue = BitConverter.ToUInt16(data, 0);
            return 100 * (rawValue / (Math.Pow(2, 16) - 1));
        }

        /// <summary>
        /// Read a number of data words (1 word = 2 bytes)
        /// </summary>
        /// <param name="wordCount">Words to read</param>
        /// <param name="reRead">Reading is a rereading, decrease reading attemp counter and wait for processing delay</param>
        /// <returns>Raw data bytes, without checksum</returns>
        private byte[] ReadWordsAndCrc(int wordCount, bool reRead = false)
        {
            if (reRead)
            {
                m_RemainingI2cReadAttempts--;
                Debug.WriteLine("Perform re-reading. Remaining attempts: " + m_RemainingI2cReadAttempts);
                SpinWait.SpinUntil(() => false, PROCESSING_DELAY);
            }
            else
            {
                m_RemainingI2cReadAttempts = I2C_READ_ATTEMPTS;
                Debug.WriteLine("Perform initial reading.");
            }

            var buffer = new byte[wordCount * 3];
            try
            {
                var readingResult = m_Sensor.ReadPartial(buffer);
                
                if (readingResult.Status == I2cTransferStatus.SlaveAddressNotAcknowledged && m_RemainingI2cReadAttempts > 0)
                    return ReadWordsAndCrc(wordCount, true);
                else if (readingResult.Status != I2cTransferStatus.FullTransfer)
                    throw new Exception("I2C read fails! Status: " + readingResult.Status);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            var result = new List<byte>();

            for (int i = 0; i < wordCount; i++)
            {

                if (CheckCrc(buffer.Range(i * 3, 3), out byte[] word))
                    result.AddRange(word);
                else
                    throw new Exception($"Invalid checksum for word {i + 1}!");
            }
            return result.ToArray();
        }

        private bool CheckCrc(byte[] buffer, out byte[] data)
        {
            data = null;

            if (buffer.Length != 3)
                return false;

            if (Crc8.ComputeChecksum(buffer) == 0)
            {
                data = new byte[2] { buffer[1], buffer[0] }; //order MSB and LSB
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Send a  command to the sensor
        /// </summary>
        private I2cTransferStatus WriteCommand(Sht85Commands command)
        {
            try
            {
                var cmd = BitConverter.GetBytes((UInt16)command);

                var result = m_Sensor.WritePartial(cmd.Reverse().ToArray());
                return result.Status;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return I2cTransferStatus.UnknownError;
            }
        }

        private bool CheckValue(byte[] data)
        {
            if (data.Length != 3)
                return false;
            else
                return Crc8.ComputeChecksum(data) == 0;
        }

        private void FetchDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int refreshRate = (int)e.Argument - (2 * PROCESSING_DELAY); //TODO: Find out optimal refresh rate.
            Debug.WriteLine("Refreshrate = " + refreshRate + " ms");

            while (!m_FetchDataWorker.CancellationPending)
            {
                var res = WriteCommand(Sht85Commands.FETCH_DATA);
                Debug.WriteLine("Fetch status: " + res);

                try
                {
                    var data = ReadWordsAndCrc(2);

                    var t = ConvertTemperature(data.Range(0, 2));
                    var rh = ConvertHumidity(data.Range(2, 2));

                    NewMeasData?.Invoke(new Tuple<DateTime, double, double>(DateTime.Now, t, rh));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                SpinWait.SpinUntil(() => false, refreshRate);
            }
        }

        #region Sht85 services, Access by properties

        private UInt32 GetSerialNumber()
        {
            WriteCommand(Sht85Commands.READ_SERIALNBR);

            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);

            var data = ReadWordsAndCrc(2);

            byte[] serial = new byte[4];
            serial[0] = data[2];
            serial[1] = data[3];
            serial[2] = data[0];
            serial[3] = data[1];

            return BitConverter.ToUInt32(serial, 0);
        }

        private bool GetHeaterState()
        {
            var status = GetStatus();

            return status.HeaterStatus;
        }

        private void SetHeaterState(bool state)
        {
            if (state)
                WriteCommand(Sht85Commands.HEATER_ENABLE);
            else
                WriteCommand(Sht85Commands.HEATER_DISABLE);

            SpinWait.SpinUntil(() => false, PROCESSING_DELAY);
        }

        #endregion Sht85 services, Access by properties

        #endregion Internal services

        #region Events
        /// <summary>
        /// Delegate for new measdata
        /// </summary>
        /// <param name="measData">Measdata
        /// Item1: Timespamp
        /// Item2: Temperature in °C
        /// Item3: Humidity in %RH</param>
        public delegate void NewMeasDataEventHandler(Tuple<DateTime, double, double> measData);

        /// <summary>
        /// Event for periodic data acquisition mode 
        /// </summary>
        public event NewMeasDataEventHandler NewMeasData;

        #endregion Events
    }
}
