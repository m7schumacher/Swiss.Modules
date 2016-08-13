using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.Tokenize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Swiss.Machine
{
    public enum POS
    {
        Noun,
        Verb,
        Adjective,
        Adverb,
        Other
    }

    public class Token
    {
        public POS Type { get; set; }

        public string Word { get; set; }
        public string Tag { get; set; }

        public Token(string word, string tag)
        {
            Word = word;
            Tag = tag;

            Type = GetType(tag);
        }

        private POS GetType(string tok)
        {
            return tok.Equals("NN") ? POS.Noun :
                   tok.Equals("JJ") ? POS.Adjective :
                   tok.StartsWith("VB") ? POS.Verb :
                   tok.StartsWith("RB") ? POS.Adverb :
                   POS.Other;
        }

        public override string ToString()
        {
            return string.Format("{0} | {1}", Word, Tag);
        }
    }

    public class Sentence
    {
        public string Subject { get; set; }
        public string Text { get; set; }

        public List<Token> Tokens { get; set; }
        public List<Token> Nouns { get { return Tokens.Where(tk => tk.Type == POS.Noun).ToList(); } }
        public List<Token> Verbs { get { return Tokens.Where(tk => tk.Type == POS.Verb).ToList(); } }
        public List<Token> Adjectives { get { return Tokens.Where(tk => tk.Type == POS.Adjective).ToList(); } }
        public List<Token> Adverbs { get { return Tokens.Where(tk => tk.Type == POS.Adverb).ToList(); } }

        public bool IsQuestion { get; set; }
    }

    public class NaturalLanguageUtility
    {
        #region Resources

        private static string TaggerName = "Swiss.Machine.Files.EnglishPOS.nbin";
        private static string TokenizerName = "Swiss.Machine.Files.EnglishTok.nbin";

        private static Stream GetResource(string nameOfResource)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(nameOfResource);
        }

        private static EnglishMaximumEntropyTokenizer GetTokenizer()
        {
            //var stream = GetResource(TokenizerName);
            //var tokenizer = new EnglishMaximumEntropyTokenizer(stream);

            //return tokenizer;
            return null;
        }

        private static EnglishMaximumEntropyPosTagger GetTagger()
        {
            //var stream = GetResource(TaggerName);
            //var tokenizer = new EnglishMaximumEntropyPosTagger(stream);

            //return tokenizer;
            return null;
        }

        #endregion

        private static string Filter(string input)
        {
            string output = input.Trim()
                               .TrimPunctuation()
                               .ToLower();

            return output;
        }

        public static Sentence TagSentence(string sentence)
        {
            Regex questioners = new Regex(RegexPatterns.Questioners);

            Sentence result = new Sentence()
            {
                Text = sentence,
                IsQuestion = questioners.IsMatch(sentence),
                Tokens = new List<Token>()
            };

            sentence = Filter(sentence);
            EnglishMaximumEntropyPosTagger mPosTagger = GetTagger();

            string[] tokens = TokenizeSentence(sentence);
            var tags = mPosTagger.Tag(tokens);

            for (int i = 0; i < tokens.Length; i++)
            {
                Token tok = new Token(tokens[i], tags[i]);
                result.Tokens.Add(tok);
            }

            return result;
        }

        public static string[] TokenizeSentence(string sentence)
        {
            sentence = Filter(sentence);

            EnglishMaximumEntropyTokenizer mTokenizer = GetTokenizer();
            return mTokenizer.Tokenize(sentence);
        }
    }
}
