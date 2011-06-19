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
        private static string TelevisionPort = "COM3";
        private static string PreProPort = "COM4";

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

            this.TV = new SerialControl.PioneerTv(TelevisionPort);
            this.PrePro = new SerialControl.EmotivaPrePro(PreProPort);
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
                    this.SendCommands(() =>
                    {
                        if(!this.PrePro.PoweredOn)
                            this.PrePro.PowerOn();

                        this.PrePro.InputSAT();
                        return true;

                    }, () =>
                    {
                        if(!this.TV.PoweredOn)
                            this.TV.PowerOn();

                        this.TV.SetInputHDMI1();
                        this.TV.SetVolume(0);
                        return true;
                    });
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.WatchMovie:
                    this.SendCommands(() =>
                    {
                        if(!this.PrePro.PoweredOn)
                            this.PrePro.PowerOn();

                        this.PrePro.InputDVD();
                        this.PrePro.Input8Channel();

                        return true;
                    },
                    () =>
                    {

                        if(!this.TV.PoweredOn)
                            this.TV.PowerOn();

                        this.TV.SetInputHDMI1();
                        this.TV.SetVolume(0);

                        return true;
                    });
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.ViewComputer:
                    this.SendCommands(() =>
                    {
                        if(!this.TV.PoweredOn)
                            this.TV.PowerOn();

                        this.TV.SetInputHDMI2();
                        this.TV.SetVolume(20);

                        return true;
                    }, () =>
                    {
                        this.PrePro.PowerOff();
                        return true;
                    });

                    break;

                case SpeechControl.ComponentControl.SpeechCommand.PowerOff:
                    this.TV.PowerOff();
                    this.PrePro.PowerOff();

                    break;

                case SpeechControl.ComponentControl.SpeechCommand.ListenStereo:
                    if(!this.PrePro.PoweredOn)
                        this.PrePro.PowerOn();

                    this.PrePro.InputCD();

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

        protected void SendCommands(params Func<Boolean>[] expressions) {
            var threads = new List<Thread>();

            foreach (var expression in expressions) {
                var thread = new Thread(() => { expression(); });
                thread.SetApartmentState(ApartmentState.MTA);
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join(15000);
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
        }
    }
}
