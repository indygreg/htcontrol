using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace HTControl {
    /// <summary>
    /// Represents the master controller for the home theater
    /// </summary>
    public class HomeTheaterController : IDisposable {
        protected SpeechControl.ComponentControl SpeechComponentControl;
        protected Thread SpeechThread;

        protected SerialControl.PioneerTv TV;
        protected SerialControl.EmotivaPrePro PrePro;

        public HomeTheaterController() {
            this.SpeechThread = new Thread(() =>
            {
                this.SpeechComponentControl = new SpeechControl.ComponentControl();
                this.SpeechComponentControl.CommandReceived += new SpeechControl.ComponentControl.SpeechCommandReceivedHandler(ProcessSpeechCommand);
            });
            this.SpeechThread.SetApartmentState(ApartmentState.MTA);
            this.SpeechThread.Start();

            this.TV = new SerialControl.PioneerTv("COM3");
            this.PrePro = new SerialControl.EmotivaPrePro("COM4");
        }


        protected void ProcessSpeechCommand(SpeechControl.ComponentControl.SpeechCommand command) {
            switch(command) {
                case SpeechControl.ComponentControl.SpeechCommand.TelevisionOff:
                    this.TV.PowerOff();
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.TelevisionOn:
                    this.TV.PowerOn();
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.WatchTelevision:
                    if(!this.TV.PoweredOn)
                        this.TV.PowerOn();

                    this.TV.SetInputHDMI1();
                    this.PrePro.InputSAT();
                    this.TV.SetVolume(0);
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.WatchMovie:
                    if(!this.TV.PoweredOn)
                        this.TV.PowerOn();

                    this.TV.SetInputHDMI1();
                    this.TV.SetVolume(0);
                    this.PrePro.PowerOn();
                    this.PrePro.InputDVD();
                    this.PrePro.Input8Channel();

                    break;

                case SpeechControl.ComponentControl.SpeechCommand.ViewComputer:
                    if(!this.TV.PoweredOn)
                        this.TV.PowerOn();

                    this.TV.SetInputHDMI2();
                    this.TV.SetVolume(20);
                    this.PrePro.PowerOff();

                    break;

                case SpeechControl.ComponentControl.SpeechCommand.PowerOff:
                    this.TV.PowerOff();
                    this.PrePro.PowerOff();

                    break;

                default:
                    Console.WriteLine("Unknown speech command received");

                    break;
            }
        }

        public void Dispose() {
            if (this.SpeechThread != null && this.SpeechThread.IsAlive)
                this.SpeechThread.Abort();
        }
    }
}
