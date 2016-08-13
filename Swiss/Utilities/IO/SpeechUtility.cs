using System.Speech.Synthesis;
using System.Windows.Forms;

namespace Swiss
{
    /// <summary>
    /// Utility class capable of using Microsoft Speech Synthesis to speak text given to it
    /// </summary>
    public class SpeechUtility
    {
        private static SpeechSynthesizer _synth;
        private static PromptBuilder _prompt;

        private static SpeechSynthesizer Synth
        {
            get { return _synth ?? (_synth = new SpeechSynthesizer()); }
        }

        private static PromptBuilder Prompt
        {
            get { return _prompt ?? (_prompt = new PromptBuilder()); }
        }

        private static bool IsFast = false;

        /// <summary>
        /// Method either speaks text or outputs a messagebox with text depending on quiet boolean
        /// </summary>
        public static void SpeakText(string text, bool quiet)
        {
            if (quiet) { WhisperText(text); }
            else { SpeakText(text); }
        }

        private static void SpeakText(string text)
        {
            if (!IsFast)
            {
                Synth.Rate += 1;
                IsFast = true;
            }

            Synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);

            Prompt.ClearContent();
            Prompt.AppendText(text);

            Synth.Speak(Prompt);
        }

        private static void WhisperText(string text)
        {
            MessageBox.Show(text);
        }
    }
}
