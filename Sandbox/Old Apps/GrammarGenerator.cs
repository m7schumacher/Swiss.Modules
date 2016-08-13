using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Recognition;
using System.Text.RegularExpressions;

namespace Sandbox.Old_Apps
{
    public class GrammarGenerator : Project
    {
        private Dictionary<string, string[]> options = new Dictionary<string, string[]>()
        {
            { "how {} is it", new string[] { "windy", "cold", "warm", "hot", "humid" } },
            { "what is the {} like", new string[] { "wind", "temperature", "temp", "humidity", "windchill" } },
            { "weather {}", new string[] { "today", "tomorrow", "tomorrow night" } }
        };

        public GrammarGenerator()
        {
            SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();

            GrammarBuilder builder = new GrammarBuilder();

            foreach(string key in options.Keys)
            {
                Choices choices = new Choices(options[key]);

                string[] bits = Regex.Split(key, "{}");

                builder.Append(bits[0].Trim());
                builder.Append(choices);

                if(!String.IsNullOrWhiteSpace(bits[1]))
                    builder.Append(bits[1].Trim());
            }

            Grammar gram = new Grammar(builder);
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
