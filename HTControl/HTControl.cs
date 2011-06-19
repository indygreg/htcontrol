using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HTControl {
    public partial class HTControl : Form {
        protected HomeTheaterController ht;

        public HTControl() {
            InitializeComponent();

            this.ht = new HomeTheaterController();
        }
    }
}
