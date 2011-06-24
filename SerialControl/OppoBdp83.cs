using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SerialControl {
    /// <summary>
    /// Control interface for Oppo BDP-83 Blu-ray player
    /// </summary>
    /// <remarks>
    /// Each command starts with a '#' followed by a 3 character command code.
    /// A CR indicates end of command. The player also sends a response for
    /// every command. The player can also "asynchronously" send responses
    /// indicating player events.
    /// </remarks>
    public class OppoBdp83 : SerialControlledDevice {
        protected enum ResponseParseState {
            Beginning,
            Middle,
        }

        public enum PlaybackState {
            NoDisc,
            LoadingDisc,
            OpeningTray,
            ClosingTray,
            PlaybackStarted,
            PlaybackPaused,
            PlaybackStopped,
            ForwardStepMode,
            ReverseStepMode,
            FastForward1,
            FastForward2,
            FastForward3,
            FastForward4,
            FastForward5,
            FastReverse1,
            FastReverse2,
            FastReverse3,
            FastReverse4,
            FastReverse5,
            SlowForward1,
            SlowForward2,
            SlowForward3,
            SlowForward4,
            SlowForward5,
            SlowReverse1,
            SlowReverse2,
            SlowReverse3,
            SlowReverse4,
            SlowReverse5,
            HomeMenu,
            MediaCenter,
        }

        [Flags]
        public enum DiscType {
            Bluray = 1,
            DVDVideo = 2,
            DVDAudio = 4,
            SuperAudioCD = 8,
            CDDA = 16,
            HighDefinitionCD = 32,
            DataDisc = 64,
            VideoCD = 128,
            SuperVideoCD = 256,
            Video = Bluray | DVDVideo | VideoCD | SuperVideoCD,
        }

        public enum AudioType {
            DolbyDigital,
            DolbyDigitalPlus,
            DolbyTrueHD,
            DTS,
            DTSHDHighResolution,
            DTSHDMasterAudio,
            LPCM,
            MPEG,
            CD,
            Unknown,
        }

        public enum ChannelsDescription {
            Mono,
            Stereo,
            Surround51,
            Surround71,
            Unknown,
        }

        public delegate void PowerUpdate(Boolean PowerState);

        public delegate void PlaybackUpdate(PlaybackState State);

        public delegate void VolumeUpdate(Boolean Mute, Int32 Volume);

        public delegate void DiscTypeUpdate(DiscType Type);

        public delegate void AudioTypeUpdate(AudioType AudioType, Int32 CurrentTrack, Int32 AvailableTracks, String Language, ChannelsDescription Channels);

        protected ResponseParseState ResponseState = ResponseParseState.Beginning;

        protected String PartialResponse = "";

        protected Object Locker = new Object();

        protected Queue<String> Responses = new Queue<String>();

        public OppoBdp83(String port) : base(port) {

        }

        /// <summary>
        /// Fires whenever a power update event is received
        /// </summary>
        public event PowerUpdate OnPowerUpdate;

        /// <summary>
        /// Fires whenever the playback state changes
        /// </summary>
        public event PlaybackUpdate OnPlaybackUpdate;

        /// <summary>
        /// Fires whenever volume state changes
        /// </summary>
        public event VolumeUpdate OnVolumeUpdate;

        /// <summary>
        /// Fires whenever a new disc type is detected
        /// </summary>
        public event DiscTypeUpdate OnDiscTypeUpdate;

        /// <summary>
        /// Fires whenever a new audio track is encountered
        /// </summary>
        public event AudioTypeUpdate OnAudioTypeUpdate;

        protected override void OnAvailableBytes(byte[] buffer, int count) {
            foreach(Byte c in buffer) {
                if(this.ResponseState == ResponseParseState.Beginning) {
                    if(c != 0x40) { // @
                        throw new Exception("Invalid response format seen");
                    }

                    this.ResponseState = ResponseParseState.Middle;
                    continue;
                }

                // else

                // carriage return is end of response
                if(c == 0x0d) {
                    this.HandleFullResponse(this.PartialResponse);

                    this.PartialResponse = "";
                    this.ResponseState = ResponseParseState.Beginning;
                    continue;
                }

                this.PartialResponse += (char)c;
            }
        }

        protected void HandleFullResponse(String response) {
            // power status update
            if(response.StartsWith("UPW ")) {
                if(response.Length != 5) {
                    throw new Exception("Length of UPW resposne not recognized");
                }

                Boolean state = response[4] == '1';

                var e = this.OnPowerUpdate;
                if(e != null) {
                    e(state);
                }

            // playback update
            } else if(response.StartsWith("UPL ")) {
                if(response.Length != 8) {
                    throw new Exception("Length of UPL response not recognized");
                }

                var param = response.Substring(4);

                PlaybackState state;
                switch(param) {
                    case "DISC":
                        state = PlaybackState.NoDisc;
                        break;

                    case "LOAD":
                        state = PlaybackState.LoadingDisc;
                        break;

                    case "OPEN":
                        state = PlaybackState.OpeningTray;
                        break;

                    case "CLOS":
                        state = PlaybackState.ClosingTray;
                        break;

                    case "PLAY":
                        state = PlaybackState.PlaybackStarted;
                        break;

                    case "PAUS":
                        state = PlaybackState.PlaybackPaused;
                        break;

                    case "STOP":
                        state = PlaybackState.PlaybackStopped;
                        break;

                    case "STPF":
                        state = PlaybackState.ForwardStepMode;
                        break;

                    case "STPR":
                        state = PlaybackState.ReverseStepMode;
                        break;

                    case "FFW1":
                        state = PlaybackState.FastForward1;
                        break;

                    case "FFW2":
                        state = PlaybackState.FastForward2;
                        break;

                    case "FFW3":
                        state = PlaybackState.FastForward3;
                        break;

                    case "FFW4":
                        state = PlaybackState.FastForward4;
                        break;

                    case "FFW5":
                        state = PlaybackState.FastForward5;
                        break;

                    case "FRV1":
                        state = PlaybackState.FastReverse1;
                        break;

                    case "FRV2":
                        state = PlaybackState.FastReverse2;
                        break;

                    case "FRV3":
                        state = PlaybackState.FastReverse3;
                        break;

                    case "FRV4":
                        state = PlaybackState.FastReverse4;
                        break;

                    case "FRV5":
                        state = PlaybackState.FastReverse5;
                        break;

                    case "SFW1":
                        state = PlaybackState.SlowForward1;
                        break;

                    case "SFW2":
                        state = PlaybackState.SlowForward2;
                        break;

                    case "SFW3":
                        state = PlaybackState.SlowForward3;
                        break;

                    case "SFW4":
                        state = PlaybackState.SlowForward4;
                        break;

                    case "SFW5":
                        state = PlaybackState.SlowForward5;
                        break;

                    case "SRV1":
                        state = PlaybackState.SlowReverse1;
                        break;

                    case "SRV2":
                        state = PlaybackState.SlowReverse2;
                        break;

                    case "SRV3":
                        state = PlaybackState.SlowReverse3;
                        break;

                    case "SRV4":
                        state = PlaybackState.SlowReverse4;
                        break;

                    case "SRV5":
                        state = PlaybackState.SlowReverse5;
                        break;

                    case "HOME":
                        state = PlaybackState.HomeMenu;
                        break;

                    case "MCTR":
                        state = PlaybackState.MediaCenter;
                        break;

                    default:
                        throw new Exception("Unknown update state parameter: " + param);
                        break;
                }

                var e = this.OnPlaybackUpdate;
                if(e != null) {
                    e(state);
                }

            } else if(response.StartsWith("UVL ")) {
                if(response.Length != 7) {
                    throw new Exception("Invalid parameter to UVL response");
                }

                var param = response.Substring(4);
                Boolean mute = false;
                Int32 volume = -1;

                if(param.Equals("MUT")) {
                    mute = true;
                } else {
                    volume = Int32.Parse(param);
                }

                var e = this.OnVolumeUpdate;
                if(e != null) {
                    e(mute, volume);
                }
            } else if(response.StartsWith("UDT ")) {
                if(response.Length != 8) {
                    throw new Exception("Invalid parameter to UDT response");
                }

                var param = response.Substring(4);

                DiscType type;

                switch(param) {
                    case "BDMV":
                        type = DiscType.Bluray;
                        break;

                    case "DVDV":
                        type = DiscType.DVDVideo;
                        break;

                    case "DVDA":
                        type = DiscType.DVDAudio;
                        break;

                    case "SACD":
                        type = DiscType.SuperAudioCD;
                        break;

                    case "CDDA":
                        type = DiscType.CDDA;
                        break;

                    case "HDCD":
                        type = DiscType.HighDefinitionCD;
                        break;

                    case "DATA":
                        type = DiscType.DataDisc;
                        break;

                    case "VCD2":
                        type = DiscType.VideoCD;
                        break;

                    case "SVCD":
                        type = DiscType.SuperVideoCD;
                        break;

                    default:
                        throw new Exception("Unknown disc type seen " + param);
                        break;
                }

                var e = this.OnDiscTypeUpdate;
                if(e != null) {
                    e(type);
                }
            } else if(response.StartsWith("UAT ")) {
                var parms = response.Split(new char[] { (char)' ' });

                if(parms.Length != 5) {
                    throw new Exception("Unknown format of UAT response");
                }

                AudioType audioType;
                switch(parms[1]) {
                    case "DD":
                        audioType = AudioType.DolbyDigital;
                        break;

                    case "DP":
                        audioType = AudioType.DolbyDigitalPlus;
                        break;

                    case "DT":
                        audioType = AudioType.DolbyTrueHD;
                        break;

                    case "TS":
                        audioType = AudioType.DTS;
                        break;

                    case "TH":
                        audioType = AudioType.DTSHDHighResolution;
                        break;

                    case "TM":
                        audioType = AudioType.DTSHDMasterAudio;
                        break;

                    case "PC":
                        audioType = AudioType.LPCM;
                        break;

                    case "MP":
                        audioType = AudioType.MPEG;
                        break;

                    case "CD":
                        audioType = AudioType.CD;
                        break;

                    case "UN":
                        audioType = AudioType.Unknown;
                        break;

                    default:
                        throw new Exception("Unknown audio type parameter");
                        break;
                }

                Int32 currentTrack = Int32.Parse(parms[2].Substring(0, 2));
                Int32 availableTracks = Int32.Parse(parms[2].Substring(3, 2));

                String language = parms[3];

                ChannelsDescription channels;

                switch(parms[4]) {
                    case "1.0":
                        channels = ChannelsDescription.Mono;
                        break;

                    case "2.0":
                        channels = ChannelsDescription.Stereo;
                        break;

                    case "5.1":
                        channels = ChannelsDescription.Surround51;
                        break;

                    case "7.1":
                        channels = ChannelsDescription.Surround71;
                        break;

                    case "0.0":
                        channels = ChannelsDescription.Unknown;
                        break;

                    default:
                        throw new Exception("Unknown channels description");
                        break;
                }

                var e = this.OnAudioTypeUpdate;
                if(e != null) {
                    e(audioType, currentTrack, availableTracks, language, channels);
                }
            } else if(response.StartsWith("UST ")) {
                //throw new NotImplementedException("Subtitle update not yet supported");
            } else if(response.StartsWith("UTC ")) {
                // Time Code Update not yet supported
            } else if(response.StartsWith("UVO ")) {
                // Video Resolution Update not yet supported
            } else {
                // not an update response, so it must be a command response
                lock(this.Locker) {
                    this.Responses.Enqueue(response);
                }
            }
        }

        protected void SendCommand(String command) {
            Int32 length = command.Length + 2;

            var buffer = new byte[length];

            buffer[0] = 0x23; // #
            buffer[length - 1] = 0x0d; // carriage return

            var commandBytes = Encoding.ASCII.GetBytes(command);
            commandBytes.CopyTo(buffer, 1);

            this.Write(buffer);
        }

        public void Eject() {
            this.SendCommand("EJT");
        }

        public void PowerOn() {
            this.SendCommand("PON");
        }

        public void PowerOff() {
            this.SendCommand("POF");
        }

        public void Mute() {
            this.SendCommand("MUT");
        }

        public void Number1() {
            this.SendCommand("NU1");
        }

        public void Number2() {
            this.SendCommand("NU2");
        }

        public void Number3() {
            this.SendCommand("NU3");
        }

        public void Number4() {
            this.SendCommand("NU4");
        }

        public void Number5() {
            this.SendCommand("NU5");
        }

        public void Number6() {
            this.SendCommand("NU6");
        }

        public void Number7() {
            this.SendCommand("NU7");
        }

        public void Number8() {
            this.SendCommand("NU8");
        }

        public void Number9() {
            this.SendCommand("NU9");
        }

        public void Number0() {
            this.SendCommand("NU0");
        }
    }
}
