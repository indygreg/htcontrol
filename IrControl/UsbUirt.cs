using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsbUirt;

namespace HTControl.IrControl {
    public class UsbUirt {
        public delegate void GenericCommand();

        private static readonly TimeSpan MinimumDuration = TimeSpan.FromMilliseconds(750);

        protected Controller controller = new Controller();

        public event GenericCommand OnDvrOk;
        public event GenericCommand OnDvrAv;
        public event GenericCommand OnDvrChannelUp;
        public event GenericCommand OnDvrChannelDown;
        public event GenericCommand OnOppoOk;

        protected Dictionary<String, DateTime> LastSeen = new Dictionary<string, DateTime>();
        protected Object Locker = new Object();

        public UsbUirt() {
            this.controller.Received += OnReceive;
        }

        void OnReceive(object sender, ReceivedEventArgs e) {
            Boolean valid = false;
            var code = e.IRCode;
            var now = DateTime.UtcNow;

            // we keep track of when we see codes so we don't fire too often
            lock(this.Locker) {
                DateTime lastTime;
                if(this.LastSeen.TryGetValue(code, out lastTime)) {
                    if(now - lastTime > MinimumDuration)
                        valid = true;
                } else {
                    valid = true;
                }

                if(valid)
                    this.LastSeen[code] = now;
            }

            //Console.WriteLine("IR RX: " + e.IRCode + " " + valid);

            if(!valid)
                return;

            GenericCommand ev = null;

            switch(code) {
                case "230A011050A5":
                    ev = this.OnDvrOk;
                    break;

                case "230A000450A5":
                    ev = this.OnDvrAv;
                    break;

                case "2308151050A5":
                    ev = this.OnDvrChannelUp;
                    break;

                case "2308150450A5":
                    ev = this.OnDvrChannelDown;
                    break;

                case "435B541051F4":
                    ev = this.OnOppoOk;
                    break;

                default:

                    break;
            }

            if(ev != null) {
                ev();
            }
        }
    }
}
