using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using SerialControl;

namespace HTControl {
    /// <summary>
    /// Represents the master controller for the home theater
    /// </summary>
    public class HomeTheaterController : IDisposable {
        private static string TelevisionPort = "COM3";
        private static string PreProPort = "COM4";
        private static string OppoPort = "COM5";

        protected enum TheaterMode {
            Off,
            Oppo,
            Dvr,
        }

        protected TheaterMode Mode = TheaterMode.Off;

        protected SpeechControl.ComponentControl SpeechComponentControl;
        protected Thread SpeechThread;

        protected SerialControl.PioneerTv TV;
        protected SerialControl.EmotivaPrePro PrePro;
        protected SerialControl.OppoBdp83 Oppo;
        protected IrControl.UsbUirt Ir;

        public HomeTheaterController() {
            /*
            this.SpeechThread = new Thread(() =>
            {
                this.SpeechComponentControl = new SpeechControl.ComponentControl();
                this.SpeechComponentControl.CommandReceived += new SpeechControl.ComponentControl.SpeechCommandReceivedHandler(ProcessSpeechCommand);
            });
            this.SpeechThread.SetApartmentState(ApartmentState.MTA);
            this.SpeechThread.Start();
            */

            this.InitializeComponents();
        }

        protected void InitializeComponents() {
            this.TV = new SerialControl.PioneerTv(TelevisionPort);
            this.PrePro = new SerialControl.EmotivaPrePro(PreProPort);
            this.Oppo = new SerialControl.OppoBdp83(OppoPort);
            this.Ir = new IrControl.UsbUirt();

            this.Oppo.OnDiscTypeUpdate += OppoOnDiscTypeUpdate;
            this.Oppo.OnAudioTypeUpdate += OppoOnAudioUpdate;
            this.Oppo.OnPowerUpdate += OppoOnPowerUpdate;

            this.Ir.OnDvrOk += WatchDvr;
            this.Ir.OnDvrChannelDown += WatchDvr;
            this.Ir.OnDvrChannelUp += WatchDvr;
            this.Ir.OnDvrAv += DvrOnAv;
            this.Ir.OnOppoOk += OppoOnOk;
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
                    if(!this.PrePro.PoweredOn)
                        this.PrePro.PowerOn();

                    this.PrePro.InputSAT();
                    if(!this.TV.PoweredOn)
                        this.TV.PowerOn();

                    this.TV.SetInputHDMI1();
                    this.TV.SetVolume(0);
                    break;

                case SpeechControl.ComponentControl.SpeechCommand.WatchMovie:
                    this.Oppo.Eject();

                    if(!this.PrePro.PoweredOn)
                        this.PrePro.PowerOn();

                    this.PrePro.InputDVD();
                    this.PrePro.Input8Channel();

                    if(!this.TV.PoweredOn)
                        this.TV.PowerOn();

                    this.TV.SetInputHDMI1();
                    this.TV.SetVolume(0);

                    break;

                case SpeechControl.ComponentControl.SpeechCommand.ViewComputer:

                    if(!this.TV.PoweredOn)
                        this.TV.PowerOn();

                    this.TV.SetInputHDMI2();
                    this.TV.SetVolume(0);

                    if(!this.PrePro.PoweredOn) {
                        this.PrePro.PowerOn();
                    }

                    this.PrePro.InputVID1();


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
            if(this.SpeechThread != null && this.SpeechThread.IsAlive)
                this.SpeechThread.Abort();
        }

        /// Called when Oppo power status changes. If turning off, we turn off
        /// everything. If turning on, we warm up the prepro
        protected void OppoOnPowerUpdate(Boolean power) {
            if(power) {
                this.ListenAudioOnOppo(true);
            } else {
                if(this.Mode == TheaterMode.Dvr)
                    return;

                this.Mode = TheaterMode.Off;
                this.TV.PowerOff();
                this.PrePro.PowerOff();
            }
        }

        protected void OppoOnDiscTypeUpdate(OppoBdp83.DiscType type) {
            if((type & OppoBdp83.DiscType.Video) > 0) {
                this.WatchVideoOnOppo();
            } else {
                // we assume it is an audio disc
                switch(type) {
                    case OppoBdp83.DiscType.DVDAudio:
                    case OppoBdp83.DiscType.SuperAudioCD:
                        this.ListenAudioOnOppo(true);
                        break;

                    default:
                        this.ListenAudioOnOppo(false);
                        break;
                }
            }
        }

        /// When the Oppo changes audio tracks, ensure we are on the most appropriate input
        /// The stereo DACs are higher quality (theoretically), so we use them if we have
        /// 2 channels or less
        protected void OppoOnAudioUpdate(OppoBdp83.AudioType audioType, Int32 currentTrack, Int32 availableTracks, String language, OppoBdp83.ChannelsDescription channels) {
            switch(channels) {
                case OppoBdp83.ChannelsDescription.Mono:
                case OppoBdp83.ChannelsDescription.Stereo:
                    this.PrePro.InputCD();
                    break;

                default:
                    this.PrePro.Input8Channel();
                    break;
            }
        }

        protected void OppoOnOk() {
            if(this.Mode == TheaterMode.Oppo)
                return;

            this.WatchVideoOnOppo();
        }

        protected void WatchDvr() {
            this.Mode = TheaterMode.Dvr;

            if(!this.PrePro.PoweredOn)
                this.PrePro.PowerOn();

            this.PrePro.InputSAT();

            if(!this.TV.PoweredOn)
                this.TV.PowerOn();

            this.TV.SetInputHDMI1();
            this.TV.SetVolume(0);
        }

        protected void DvrOnAv() {
            this.Mode = TheaterMode.Off;

            this.TV.PowerOff();
            this.PrePro.PowerOff();
        }

        protected void WatchVideoOnOppo() {
            this.Mode = TheaterMode.Oppo;

            if(!this.PrePro.PoweredOn) {
                this.PrePro.PowerOn();
            }

            // switches HDMI to video
            this.PrePro.InputDVD();

            // switches audio to reasonable default
            this.PrePro.Input8Channel();

            if(!this.TV.PoweredOn) {
                this.TV.PowerOn();
            }

            this.TV.SetInputHDMI1();
            this.TV.SetVolume(0);
        }

        protected void ListenAudioOnOppo(Boolean multichannel) {
            this.Mode = TheaterMode.Oppo;

            if(!this.PrePro.PoweredOn) {
                this.PrePro.PowerOn();
            }

            if(multichannel) {
                this.PrePro.Input8Channel();
            } else {
                this.PrePro.InputCD();
            }
        }
    }
}
