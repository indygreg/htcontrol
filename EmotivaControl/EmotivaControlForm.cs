using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Xml.Serialization;
using SerialControl;

namespace EmotivaControl
{
    public partial class EmotivaControlForm : Form
    {
        private const int WM_APP = 0x8000;

        protected EmotivaPrePro _processor;
        protected SerialPort _port;

        public EmotivaControlForm()
        {
            InitializeComponent();

            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBoxPorts.Items.Add(s);
            }

            if (comboBoxPorts.Items.Count >= 1)
            {
                    comboBoxPorts.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No serial ports were found on your system");
                this.Close();
            }

            
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                case WM_APP + 1:

                    switch (m.WParam.ToInt32())
                    {
                        case 1:
                            _processor.PowerOn();
                            break;

                        case 2:
                            _processor.PowerOff();
                            break;
                    }
                                       

                    break;
            }
            base.WndProc(ref m);
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_DoubleClick(object sender,
                                     System.EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void btnPowerOn_Click(object sender, EventArgs e)
        {
            _processor.PowerOn();
        }

        private void btnPowerOff_Click(object sender, EventArgs e)
        {
            _processor.PowerOff();
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            _processor.VolumeSet(Convert.ToInt32(trackBarVolume.Value));
        }

        private void radioButtonCD_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCD.Checked)
            {
                _processor.InputCD();
            }
        }

        private void radioButtonSAT_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSAT.Checked)
            {
                _processor.InputSAT();
            }
        }

        private void radioButtonDVD_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDVD.Checked)
            {
                _processor.InputDVD();
            }
        }

        private void radioButtonPhono_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPhono.Checked)
            {
                _processor.InputPhono();
            }
        }

        private void radioButtonTuner_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTuner.Checked)
            {
                _processor.InputTuner();
            }
        }

        private void radioButton8Channel_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8Channel.Checked)
            {
                _processor.Input8Channel();
            }
        }

        private void radioButtonVID1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonVID1.Checked)
            {
                _processor.InputVID1();
            }
        }

        private void radioButtonVID2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonVID2.Checked)
            {
                _processor.InputVID2();
            }
        }

        private void radioButtonVCR_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonVCR.Checked)
            {
                _processor.InputVCR();
            }
        }

        private void radioButtonTape_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTape.Checked)
            {
                _processor.InputTape();
            }
        }

        private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            String port = comboBoxPorts.SelectedItem.ToString();

            _processor = new EmotivaPrePro(port);

           //_port = _processor.GetSerialPort();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            _processor.SendCommand("@14M");
            _processor.SendCommand("@14K");
            _processor.SendCommand("@14L");

            textBox1.Text = _port.ReadExisting();
        }

    }
}
