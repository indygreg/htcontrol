
namespace SerialControl
{
    using System;
    using System.IO.Ports;

    /// <summary>
    /// Represents a device controllable via serial/RS-232
    /// </summary>
    public class SerialControlledDevice
    {
        protected SerialPort sp;

        /// <summary>
        /// Construct a new interface to a device on the named COM port
        /// </summary>
        /// <param name="port">port to connect to. e.g. "COM1"</param>
        public SerialControlledDevice(string port)
        {
            this.sp = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
            this.sp.Open();
        }

        /// <summary>
        /// Send data over the serial port
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte[] buffer)
        {
            this.sp.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Read existing data off the serial port
        /// </summary>
        /// <returns></returns>
        public string ReadExisting()
        {
            return this.sp.ReadExisting();
        }
    }
}
