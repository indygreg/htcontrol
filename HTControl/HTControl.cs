using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace HTControl {
    public partial class HTControl : Form {
        protected SpeechControl.ComponentControl SpeechComponentControl;
        protected Thread SpeechThread;

        protected SerialControl.PioneerTv TV;

        public HTControl() {
            InitializeComponent();

            this.SpeechThread = new Thread(() =>
            {
                this.SpeechComponentControl = new SpeechControl.ComponentControl();
                this.SpeechComponentControl.CommandReceived +=new SpeechControl.ComponentControl.SpeechCommandReceivedHandler(ProcessSpeechCommand);
            });
            this.SpeechThread.Start();

            this.TV = new SerialControl.PioneerTv("COM3");
        }

        protected void ProcessSpeechCommand(SpeechControl.ComponentControl.SpeechCommand command) {
            switch(command) {
                case SpeechControl.ComponentControl.SpeechCommand.TelevisionOff:
                    this.TV.PowerOff();
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.TelevisionOn:
                    this.TV.PowerOn();
                    break;

                default:
                    Console.WriteLine("Unknown speech command received");

                    break;
            }
        }
    }
}
