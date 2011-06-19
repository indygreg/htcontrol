using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;

namespace SpeechControl
{
    /// <summary>
    /// Speech control driver for home theater
    /// </summary>
    public class ComponentControl : IDisposable
    {
        private const string RecognizerId = "SR_MS_en-US_Kinect_10.0";

        protected KinectAudioSource AudioSource;
        protected RecognizerInfo Recognizer;
        protected SpeechRecognitionEngine Engine;
        protected Stream AudioStream;

        public enum SpeechCommand {
            TelevisionOn,
            TelevisionOff,
            WatchTelevision,
            WatchMovie,
            ViewComputer,
        }

        public delegate void SpeechCommandReceivedHandler(SpeechCommand command);

        /// <summary>
        /// Fired when a command is received
        /// </summary>
        public event SpeechCommandReceivedHandler CommandReceived;

        public ComponentControl()
        {
            this.AudioSource = new KinectAudioSource();

            this.AudioSource.FeatureMode = true;
            this.AudioSource.AutomaticGainControl = false;
            this.AudioSource.SystemMode = SystemMode.OptibeamArrayOnly;
            this.AudioSource.BeamChanged += new EventHandler<BeamChangedEventArgs>(AudioSource_BeamChanged);
            
            this.Recognizer = SpeechRecognitionEngine.InstalledRecognizers().Where(r => r.Id == RecognizerId).FirstOrDefault();

            if(this.Recognizer == null) {
                throw new Exception("Could not find Kinect speech recognizer");
            }

            this.Engine = new SpeechRecognitionEngine(Recognizer.Id);
            this.Engine.UnloadAllGrammars();

            this.LoadGrammer();

            this.AudioStream = this.AudioSource.Start();
            this.Engine.SetInputToAudioStream(this.AudioStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1,
                                                      32000, 2, null));

            this.Engine.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(Engine_SpeechHypothesized);

            this.Engine.RecognizeAsync(RecognizeMode.Multiple);
            Console.WriteLine("Speech recognition initialized");
        }

        void AudioSource_BeamChanged(object sender, BeamChangedEventArgs e)
        {
            Console.WriteLine("Beam angle: " + e.Angle);
        }

        protected void LoadGrammer() {
            var gb = new GrammarBuilder();
            gb.Culture = this.Recognizer.Culture;

            gb.Append("turn on television");
            var g = new Grammar(gb);
            g.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(TurnOnTelevision);
            this.Engine.LoadGrammar(g);

            gb = new GrammarBuilder("turn off television");
            gb.Culture = this.Recognizer.Culture;
            g = new Grammar(gb);
            g.SpeechRecognized += TurnOffTelevision;
            this.Engine.LoadGrammar(g);

            gb = new GrammarBuilder("watch television");
            gb.Culture = this.Recognizer.Culture;
            g = new Grammar(gb);
            g.SpeechRecognized += WatchTelevision;
            this.Engine.LoadGrammar(g);

            gb = new GrammarBuilder("watch a movie");
            gb.Culture = this.Recognizer.Culture;
            g = new Grammar(gb);
            g.SpeechRecognized += WatchMovie;
            this.Engine.LoadGrammar(g);

            gb = new GrammarBuilder("view computer");
            gb.Culture = this.Recognizer.Culture;
            g = new Grammar(gb);
            g.SpeechRecognized += ViewComputer;
            this.Engine.LoadGrammar(g);
        }

        void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.WriteLine("Speech hypothesized: " + e.Result.Text);
        }

        void TurnOnTelevision(object sender, SpeechRecognizedEventArgs e) {
            this.RaiseEvent(e, SpeechCommand.TelevisionOn);
        }

        void TurnOffTelevision(object sender, SpeechRecognizedEventArgs e) {
            this.RaiseEvent(e, SpeechCommand.TelevisionOff);
        }

        void WatchTelevision(object sender, SpeechRecognizedEventArgs e) {
            this.RaiseEvent(e, SpeechCommand.WatchTelevision);
        }

        void WatchMovie(object sender, SpeechRecognizedEventArgs e) {
            this.RaiseEvent(e, SpeechCommand.WatchMovie);
        }

        void ViewComputer(object sender, SpeechRecognizedEventArgs e) {
            this.RaiseEvent(e, SpeechCommand.ViewComputer);
        }

        protected void RaiseEvent(SpeechRecognizedEventArgs e, SpeechCommand command) {
            Console.WriteLine(e.Result.Confidence + " " + e.Result.Text);

            if (e.Result.Confidence < 0.9) return;

            var ev = this.CommandReceived;
            if (ev != null) {
                ev(command);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.AudioStream != null)
                this.AudioStream.Dispose();

            if (this.AudioSource != null)
                this.AudioSource.Dispose();

            if (this.Engine != null)
                this.Engine.Dispose();
        }

        #endregion
    }
}
