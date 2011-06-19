using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Xml.Serialization;

namespace SerialControl
{
    public class EmotivaException : Exception
    {
        public EmotivaException(String s)
            : base(s)
        {

        }
    }

    public class EmotivaPrePro
    {
        protected String _port;
        protected SerialPort _sp;

        public EmotivaPrePro(string port)
        {
            _port = port;
            _InitPort();
        }

        protected void _InitPort()
        {
            _sp = new SerialPort(_port);
            _sp.BaudRate = 9600;
            _sp.Parity = 0;
            _sp.StopBits = (StopBits)1;
            _sp.DataBits = 8;

            _sp.Open();
            _sp.ReadTimeout = 1000;
        }

        public SerialPort GetSerialPort()
        {
            return _sp;
        }

        public void SendCommand(String command)
        {
            byte[] bytes = new byte[command.Length];

            for (int i = 0; i < command.Length; i++)
            {
                bytes[i] = Convert.ToByte(command[i]);
            }

            this._write(bytes);
        }

        // sends a command with an extension
        public void SendCommand(String command, String extension)
        {
            byte[] bytes = new byte[command.Length + extension.Length];

            for (int i = 0; i < command.Length; i++)
            {
                bytes[i] = Convert.ToByte(command[i]);
            }

            for (int i = 0; i < extension.Length; i++)
            {
                bytes[i + command.Length] = Convert.ToByte(extension[i]);
            }

            this._write(bytes);
        }

        protected void _write(byte[] buffer)
        {
            _sp.Write(buffer, 0, buffer.Length);
        }

        public void PowerToggle()
        {
            SendCommand("@111");
        }

        public void PowerOn()
        {
            SendCommand("@112");
        }

        public void PowerOff()
        {
            SendCommand("@113");
        }

        public void InputCD()
        {
            SendCommand("@114");
        }

        public void InputTape()
        {
            SendCommand("@115");
        }

        public void InputSAT()
        {
            SendCommand("@116");
        }

        public void InputDVD()
        {
            SendCommand("@117");
        }

        public void InputPhono()
        {
            SendCommand("@118");
        }

        public void InputTuner()
        {
            SendCommand("@119");
        }

        public void InputVID1()
        {
            SendCommand("@11A");
        }

        public void InputVCR()
        {
            SendCommand("@11B");
        }

        public void InputVID2()
        {
            SendCommand("@11C");
        }

        public void Input8Channel()
        {
            SendCommand("@133");
        }

        public void DSPUp()
        {
            SendCommand("@11D");
        }

        public void DSPDown()
        {
            SendCommand("@13W");
        }

        public void DSPStereo()
        {
            SendCommand("@11E");
        }

        public void DSPProLogic()
        {
            SendCommand("@11F");
        }

        public void DSPJazzClub()
        {
            SendCommand("@11K");
        }

        public void DSPParty()
        {
            SendCommand("@134");
        }

        public void DSPNeo6()
        {
            SendCommand("@13H");
        }

        public void DSPSourceDirect()
        {
            SendCommand("@13J");
        }

        public void MuteToggle()
        {
            SendCommand("@11P");
        }

        public void MuteOn()
        {
            SendCommand("@11Q");
        }

        public void MuteOff()
        {
            SendCommand("@11R");
        }

        public void VolumeUp()
        {
            SendCommand("@11S");
        }

        public void VolumeDown()
        {
            SendCommand("@11T");
        }

        // sets the volume to a given level
        // level must be between 0 and 99
        public void VolumeSet(int level)
        {
            if (level < 0 || level > 99)
            {
                throw new EmotivaException("Volume level must be between 0 and 99");
            }

            SendCommand("@11U", String.Format("{0:00}", level));
        }

        public void RearTrimUp()
        {
            SendCommand("@11g");
        }

        public void RearTrimDown()
        {
            SendCommand("@11j");
        }

        public void RearTrimZero()
        {
            SendCommand("@11h");
        }

        public void CenterTrimeUp()
        {
            SendCommand("@11k");
        }

        public void CenterTrimZero()
        {
            SendCommand("@11m");
        }

        public void CenterTrimDown()
        {
            SendCommand("@11n");
        }

        public void SubTrimUp()
        {
            SendCommand("@11p");
        }

        public void SubTrimZero()
        {
            SendCommand("@11q");
        }

        public void SubTrimDown()
        {
            SendCommand("@11r");
        }

        public void BassUp()
        {
            SendCommand("@11s");
        }

        public void BassZero()
        {
            SendCommand("@11t");
        }

        public void BassDown()
        {
            SendCommand("@11u");
        }

        public void TrebleUp()
        {
            SendCommand("@11v");
        }

        public void TrebleZero()
        {
            SendCommand("@11w");
        }

        public void TrebleDown()
        {
            SendCommand("@11x");
        }

        public void TunerAM()
        {
            SendCommand("@11y");
        }

        public void TunerFM()
        {
            SendCommand("@11z");
        }

        public void TunerStepUp()
        {
            SendCommand("@121");
        }

        public void TunerStepDown()
        {
            SendCommand("@122");
        }

        public void TunerScanUp()
        {
            SendCommand("@123");
        }

        public void TunerScanDown()
        {
            SendCommand("@124");
        }

        public void TunerPresetUp()
        {
            SendCommand("@125");
        }

        public void TunerPresetDown()
        {
            SendCommand("@126");
        }

        // @TODO Implement Tuner Direct functions

        public void TunerRecallPreset(int preset)
        {
            if (preset < 1 || preset > 100)
            {
                throw new EmotivaException("Preset must be between 1 and 99");
            }

            SendCommand("@129", preset.ToString("%02d"));
        }

        public void TunerProgramPreset(int preset)
        {
            if (preset < 1 || preset > 100)
            {
                throw new EmotivaException("Preset must be between 1 and 99");
            }

            SendCommand("@12A", preset.ToString("%02d"));
        }

        // @TODO Implement Program preset direct

        public void SideAxisOn()
        {
            SendCommand("@12G");
        }

        public void SideAxisOff()
        {
            SendCommand("@12H");
        }

        // @TODO Invesigate extension values
        public void ResetSystem()
        {
            SendCommand("@12L321");
        }

        public void PurgeEEPROM()
        {
            SendCommand("@12M123");
        }

        // @TODO Implement Broadcast only functions -- are they needed?

        public void SetMON2()
        {
            SendCommand("@12T");
        }

        public void SetVCR2()
        {
            SendCommand("@12U");
        }

        public void SetVideoDefaultSVideo()
        {
            SendCommand("@14B");
        }

        public void SetVideoDefaultComposite()
        {
            SendCommand("@14C");
        }

        public void DolbyExOn()
        {
            SendCommand("@13K");
        }

        public void DolbyExOff()
        {
            SendCommand("@13L");
        }

        public void Z2PowerToggle()
        {
            SendCommand("@13M");
        }

        public void Z2PowerOn()
        {
            SendCommand("@13N");
        }

        public void Z2PowerOff()
        {
            SendCommand("@13P");
        }

        public void ZoneToggle()
        {
            SendCommand("@137");
        }

        public void Z2BalanceLeft()
        {
            SendCommand("@13X");
        }

        public void Z2BalanceRight()
        {
            SendCommand("@13Y");
        }

        public void Z2BalanceCenter()
        {
            SendCommand("@13Z");
        }

        public void Z2MuteToggle()
        {
            SendCommand("@13Q");
        }

        public void Z2MuteOn()
        {
            SendCommand("@13R");
        }

        public void Z2MuteOff()
        {
            SendCommand("@13S");
        }

        public void Z2VolumeUp()
        {
            SendCommand("@13T");
        }

        public void Z2VolumeDown()
        {
            SendCommand("@13U");
        }

        public void Z2VolumeSet(int level)
        {
            if (level < 0 || level > 99)
            {
                throw new EmotivaException("level must be between 0 and 99");
            }

            SendCommand("@13V", level.ToString("%02d"));
        }

        public void Z2InputCD()
        {
            SendCommand("@138");
        }

        public void Z2InputTape()
        {
            SendCommand("@139");
        }

        public void Z2InputSAT()
        {
            SendCommand("@13A");
        }

        public void Z2InputDVD()
        {
            SendCommand("@13B");
        }

        public void Z2InputPhono()
        {
            SendCommand("@13C");
        }

        public void Z2InputTuner()
        {
            SendCommand("@13D");
        }

        public void Z2InputVid1()
        {
            SendCommand("@13E");
        }


        public void Z2InputVCR()
        {
            SendCommand("@13F");
        }

        public void Z2InputVid2()
        {
            SendCommand("@13G");
        }

        public void Z2InputMain()
        {
            SendCommand("@14A");
        }

        public void KeyMenu()
        {
            SendCommand("@141");
        }

        public void KeyUp()
        {
            SendCommand("@142");
        }

        public void KeyDown()
        {
            SendCommand("@143");
        }

        public void KeyRight()
        {
            SendCommand("@144");
        }

        public void KeyLeft()
        {
            SendCommand("@145");
        }

        public void KeyEnter()
        {
            SendCommand("@146");
        }

        public void KeyExit()
        {
            SendCommand("@147");
        }

        public void AmpOn()
        {
            SendCommand("@14D");
        }

        public void AmpOff()
        {
            SendCommand("@14E");
        }

        public void AmpAuxSurroundBack()
        {
            SendCommand("@14F");
        }

        public void AmpAuxSide()
        {
            SendCommand("@14G");
        }

        public void AmpAuxZone2()
        {
            SendCommand("@14H");
        }

        // @TODO Implement Get functions

        public void GetMainZoneStatus()
        {
            SendCommand("@14M");
        }

        public void DSPPLIIMusic()
        {
            SendCommand("@14P");
        }

        public void DSPPLIICinema()
        {
            SendCommand("@14Q");
        }

        public void DSPPLIIMatrix()
        {
            SendCommand("@14R");
        }

        public void DSPNeo6Music()
        {
            SendCommand("@14S");
        }

        public void DSPNeo6Cinema()
        {
            SendCommand("@14T");
        }

        public void DTSLFEMusic()
        {
            SendCommand("@14U");
        }

        public void DTSLFECinema()
        {
            SendCommand("@14V");
        }

        public void SubNormal()
        {
            SendCommand("@14W");
        }

        public void SubEnhanced()
        {
            SendCommand("@14X");
        }

        public void SpeakersMainLarge()
        {
            SendCommand("@14Y");
        }

        public void SpeakersMainSmall()
        {
            SendCommand("@14Z");
        }

        public void SpeakersSurroundLarge()
        {
            SendCommand("@15A");
        }

        public void SpeakersSurroundSmall()
        {
            SendCommand("@15B");
        }

        public void SpeakersSurroundOff()
        {
            SendCommand("@15C");
        }

        public void SpeakersCenterLarge()
        {
            SendCommand("@15D");
        }

        public void SpeakersCenterSmall()
        {
            SendCommand("@15E");
        }

        public void SpeakersCenterOff()
        {
            SendCommand("@15F");
        }

        public void SpeakersSurroundBack2Large()
        {
            SendCommand("@15G");
        }

        public void SpeakersSurroundBack2Small()
        {
            SendCommand("@15H");
        }

        public void SpeakersSurroundBack1Large()
        {
            SendCommand("@15J");
        }

        public void SpeakersSurroundBack1Small()
        {
            SendCommand("@15K");
        }

        public void SpeakersSurroundBackOff()
        {
            SendCommand("@15L");
        }

        public void SpeakersSubwooferOn()
        {
            SendCommand("@15M");
        }

        public void SpeakersSubwooferOff()
        {
            SendCommand("@15N");
        }
    }
}
