
namespace SerialControl
{
    using System;
    using System.IO.Ports;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Represents a device controllable via serial/RS-232
    /// </summary>
    public class SerialControlledDevice : IDisposable
    {
        private class Command {
            public byte[] Buffer { get; set; }
            public TimeSpan Delay { get; set; }
        }

        protected SerialPort sp;
        protected Thread thread;
        protected Boolean alive = true;
        protected Queue pendingWrites = new Queue();
        protected AutoResetEvent HaveWrites = new AutoResetEvent(false);

        protected TimeSpan MinimumCommandDelay = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Construct a new interface to a device on the named COM port
        /// </summary>
        /// <param name="port">port to connect to. e.g. "COM1"</param>
        public SerialControlledDevice(string port)
        {
            this.thread = new Thread(() => {
                this.sp = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
                this.sp.Open();

                while (this.alive) {
                    if(this.HaveWrites.WaitOne(500)) {
                        var commands = new List<Command>();

                        lock(this.pendingWrites.SyncRoot) {
                            foreach(var command in this.pendingWrites) {
                                commands.Add(command as Command);
                            }
                            this.pendingWrites.Clear();
                        }

                        foreach (var command in commands) {
                            var now = DateTime.UtcNow;

                            this.sp.Write(command.Buffer, 0, command.Buffer.Length);

                            Thread.Sleep(command.Delay);
                        }
                    }
                }
            });
            this.thread.Start();
        }

        /// <summary>
        /// Send data over the serial port
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte[] buffer)
        {
            this.Write(buffer, this.MinimumCommandDelay);
        }

        public void Write(byte[] buffer, TimeSpan delay) {
            lock(this.pendingWrites.SyncRoot) {
                var item = new Command { Buffer = buffer, Delay = delay };
                this.pendingWrites.Enqueue(item);
                this.HaveWrites.Set();
            }
        }

        /// <summary>
        /// Read existing data off the serial port
        /// </summary>
        /// <returns></returns>
        public string ReadExisting()
        {
            return this.sp.ReadExisting();
        }

        public void Dispose() {
            this.alive = false;

            this.thread.Join(2000);
            if(this.thread.IsAlive) {
                this.thread.Abort();
            }
        }
    }
}
