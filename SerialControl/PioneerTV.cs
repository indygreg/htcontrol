using System;
using System.Collections.Generic;
using System.Threading;


namespace SerialControl {
    public class PioneerTv : SerialControlledDevice {
        protected static TimeSpan InputSwitchDelay = TimeSpan.FromMilliseconds(1000);
        protected static TimeSpan PowerDelay = TimeSpan.FromSeconds(3);

        protected enum ResponseParseState {
            Beginning,
            Middle
        }

        protected bool power = false;
        protected bool osd = false;
        protected string input;
        protected int volume;

        protected ResponseParseState ResponseState = ResponseParseState.Beginning;
        protected String PartialResponse = String.Empty;

        protected Queue<String> Responses = new Queue<String>();
        protected Object Locker = new Object();
        protected AutoResetEvent ResponsesAvailable = new AutoResetEvent(false); 

        public PioneerTv(string port)
            : base(port) {
            //sp.ReadTimeout = 1000;

            this.RefreshStatus();
        }

        /// <summary>
        /// Grabs status of the TV and populates class variables
        /// </summary>
        /// <remarks>
        /// Visual OSD is disabled during refresh to avoid any user disruptions
        /// </remarks>
        public void RefreshStatus() {
            string OSD = this.SendCommand("OSD");

            bool previousOsd;

            if(OSD == "S00") {
                osd = false;
            } else {
                osd = true;
                this.OSDOff();
            }

            previousOsd = osd;

            this.GetInput();
            this.GetVolume();

            this.power = this.input == "XXX" ? false : true;

            if(previousOsd) {
                this.OSDOn();
            }
        }

        public String SendCommand(String command, String parameter) {
            return this.SendCommand(command, parameter, MinimumCommandDelay);
        }

        public string SendCommand(string command, string parameter, TimeSpan delay) {
            if(command == null) {
                throw new ArgumentNullException(command);
            }

            if(command.Length != 3) {
                throw new ArgumentException("argument must be 3 characters long", command);
            }

            if(parameter is string && parameter.Length != 3) {
                throw new ArgumentException("argument must be characters long", parameter);
            }

            int length = parameter == null ? 7 : 10;

            byte[] buffer = new byte[length];

            buffer[0] = 2;
            buffer[1] = 42;
            buffer[2] = 42;

            byte[] commandBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(command);

            if(commandBytes.Length != 3) {
                throw new Exception("unable to convert command to ascii string");
            }

            buffer[3] = commandBytes[0];
            buffer[4] = commandBytes[1];
            buffer[5] = commandBytes[2];

            if(parameter is string) {
                byte[] argumentBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(parameter);

                if(argumentBytes.Length != 3) {
                    throw new Exception("unable to convert parameter to ascii string");
                }

                buffer[6] = argumentBytes[0];
                buffer[7] = argumentBytes[1];
                buffer[8] = argumentBytes[2];
            }

            buffer[parameter == null ? 6 : 9] = 3;

            this.Write(buffer, delay);

            if(!this.ResponsesAvailable.WaitOne(TimeSpan.FromMilliseconds(1000))) {
                return "";
            }

            lock(this.Locker) {
                return this.Responses.Dequeue();
            }
        }

        public string SendCommand(string command) {
            return SendCommand(command, null);
        }

        public string SendCommand(String command, TimeSpan delay) {
            return this.SendCommand(command, null, delay);
        }

        protected override void OnAvailableBytes(byte[] buffer, int count) {
            // responses start with 0x02 and end with 0x03. everything in between is payload
            foreach(Byte b in buffer) {
                if (this.ResponseState == ResponseParseState.Beginning) {
                    if (b != 0x02) {
                        throw new Exception("Unknown response format from TV");
                    }

                    this.ResponseState = ResponseParseState.Middle;
                    continue;
                }

                // else we must be in the middle

                // end of transmission
                if (b == 0x03) {
                    this.HandleFullResponse(this.PartialResponse);
                    this.ResponseState = ResponseParseState.Beginning;
                    this.PartialResponse = "";
                    continue;
                }

                this.PartialResponse += (char)b;
            }
        }

        protected void HandleFullResponse(String response) {
            lock(this.Locker) {
                this.Responses.Enqueue(response);
                this.ResponsesAvailable.Set();
            }
        }

        public string GetInput() {
            string val = SendCommand("INP");
            input = val;
            return val;
        }

        public void SetInput(int input) {
            if(input < 1 || input > 8) {
                throw new ArgumentOutOfRangeException("input", "input must be between 1 and 8");
            }

            string arg = String.Format("S0{0}", input.ToString());

            if(this.input == arg)
                return;

            SendCommand("INP", arg, InputSwitchDelay);
            this.input = arg;
        }

        public void SetInputHDMI1() {
            SetInput(4);
        }

        public void SetInputHDMI2() {
            SetInput(5);
        }

        public void PowerOn() {
            SendCommand("PON", PowerDelay);
            power = true;
            GetInput();
            GetVolume();
        }

        public void PowerOff() {
            SendCommand("POF");
            power = false;
            input = "XXX";
        }

        public void MuteOn() {
            SendCommand("AMT", "S01");
        }

        public void MuteOff() {
            SendCommand("AMT", "S00");
        }

        public void SetVolume(int volume) {
            if(volume < 0 || volume > 60)
                throw new ArgumentOutOfRangeException("volume must be between 0 and 60");

            string vol;

            // my n00b C# skills show
            if(volume > 10) {
                vol = String.Format("0{0}", volume.ToString());
            } else if(volume > 0) {
                vol = String.Format("00{0}", volume.ToString());
            } else {
                vol = "000";
            }

            SendCommand("VOL", vol);
        }

        public int GetVolume() {
            //volume = int.Parse(SendCommand("VOL"));
            //return volume;
            return 0;
        }

        public void OSDOff() {
            SendCommand("OSD", "S00");
        }

        public void OSDOn() {
            SendCommand("OSD", "S01");
        }

        public bool PoweredOn {
            get { return power; }
        }

        public string Input {
            get { return input; }
        }

        public int Volume {
            get { return volume; }
        }

        public bool OSD {
            get { return osd; }
        }
    }
}
