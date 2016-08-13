using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace Swiss
{
    public static class StringExtensions
    {
        #region Trimming

        /// <summary>
        /// Method transform all whitespace (new lines, tabs, etc) into single spaces
        /// </summary>
        public static string NormalizeWhiteSpace(this string str)
        {
            return Regex.Replace(str, RegexPatterns.WhiteSpace, " ");
        }

        /// <summary>
        /// Method removes a given set of characters from the entirety this string
        /// </summary>
        public static string TrimCharactersWithin(this string str, char[] chars)
        {
            char[] copy = str.Where(s => !chars.Contains(s)).ToArray();
            return new string(copy);
        }

        /// <summary>
        /// Method removes a given set of characters from the end of this string
        /// </summary>
        public static string TrimCharactersFromEnd(this string value, char[] chars)
        {
            int removeLength = 0;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                char let = value[i];

                if (chars.Contains(let))
                {
                    removeLength++;
                }
                else
                {
                    break;
                }
            }

            if (removeLength > 0)
            {
                return value.Substring(0, value.Length - removeLength);
            }

            return value;
        }

        /// <summary>
        /// Method removes basic punctuation from the end of this string
        /// </summary>
        public static string TrimBasicPunctuationFromEnd(this string str)
        {
            return str.TrimEnd('?', '.', ',', '!');
        }

        /// <summary>
        /// Method removes any an all punctuation from this string
        /// </summary>
        public static string TrimPunctuation(this string str)
        {
            return Regex.Replace(str, @"[^\w\s]", "");//remove anything that's not words or spaces
        }

        /// <summary>
        /// Method removes a given set of strings from this string
        /// </summary>
        public static string RemoveAll(this string str, params string[] bits)
        {
            foreach(string bit in bits)
            {
                str = str.Remove(bit);
            }

            return str;
        }

        #endregion

        #region Parsing

        /// <summary>
        /// Method parses all digits found in this string
        /// </summary>
        public static string[] GetAllDigits(this string str)
        {
            return Regex.Split(str, RegexPatterns.Digits).WhereNotEmpty().ToArray();
        }

        /// <summary>
        /// Method parses all individual words found in this string
        /// </summary>
        public static string[] GetAllWords(this string input)
        {
            return Regex.Split(input, RegexPatterns.Word).WhereNotEmpty().ToArray();
        }

        /// <summary>
        /// Method parses all upper-case words found in this string
        /// </summary>
        public static string[] GetAllUpperCasedWords(this string input)
        {
            List<string> uppers = new List<string>();

            foreach (string word in GetAllWords(input))
            {
                if (IsUpperCase(word))
                {
                    uppers.Add(word);
                }
            }

            return uppers.ToArray();
        }

        /// <summary>
        /// Method parses all text in double quotes found in this string
        /// </summary>
        public static string[] GetTextInDoubleQuotes(this string input)
        {
            List<string> quotes = new List<string>();
            Regex reg = new Regex(RegexPatterns.DoubleQuotes);
            MatchCollection matches = reg.Matches(input);

            foreach (Match mtc in matches)
            {
                quotes.Add(mtc.Groups[1].Value);
            }

            return quotes.WhereNotEmpty().ToArray();
        }

        /// <summary>
        /// Method parses all text in single quotes found in this string
        /// </summary>
        public static string[] GetTextInSingleQuotes(this string input)
        {
            List<string> quotes = new List<string>();
            Regex reg = new Regex(RegexPatterns.SingleQuotes);
            MatchCollection matches = reg.Matches(input);

            foreach (Match mtc in matches)
            {
                quotes.Add(mtc.Groups[1].Value);
            }

            return quotes.WhereNotEmpty().ToArray();
        }

        /// <summary>
        /// Method parses all in any form of quotes found in this string
        /// </summary>
        public static string[] GetTextInAllQuotes(this string input)
        {
            string[] doubles = input.GetTextInDoubleQuotes();
            string[] singles = input.GetTextInSingleQuotes();

            return doubles.CombineWith(singles).ToArray();
        }

        #endregion

        #region Splitting

        /// <summary>
        /// Method splits this string on a string delimeter
        /// </summary>
        public static string[] Split(this string str, string splitter)
        {
            return Regex.Split(str, splitter);
        }

        /// <summary>
        /// Method splits this string on any form of whitespace
        /// </summary>
        public static string[] SplitOnWhiteSpace(this string str)
        {
            return Regex.Split(str, @"\s+")
                .WhereNotEmpty()
                .ToArray();
        }

        /// <summary>
        /// Method splits this string on any digits
        /// </summary>
        public static string[] SplitOnDigits(this string input)
        {
            return Regex.Split(input, @"\d")
                .TrimAll()
                .WhereNotEmpty()
                .ToArray();
        }

        /// <summary>
        /// Method splits this string on new line characters
        /// </summary>
        public static string[] SplitOnNewLines(this string input)
        {
            string[] targets = new string[] { "\n" };
            return input.Split(targets, StringSplitOptions.None)
                .TrimAll()
                .ToArray();
        }

        #endregion

        #region Alterations

        /// <summary>
        /// Method makes the first character of all words in this string upper-case
        /// </summary>
        public static string UpperCaseFirstLetters(this string str)
        {
            return Regex.Replace(str, @"\b[a-z]\w+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }

        /// <summary>
        /// Method removes all forms of spacing from this string
        /// </summary>
        public static string RemoveSpaces(this string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }

        /// <summary>
        /// Method removes a given string from this string
        /// </summary>
        public static string Remove(this string input, string rem = "")
        {
            return Regex.Replace(input, rem, string.Empty);
        }

        /// <summary>
        /// Method makes the matching portion of this string and a given string upper-case
        /// </summary>
        public static string Highlight(this string input, string text)
        {
            return text.Length > 0 ? input.Replace(text, text.ToUpper()) : input;
        }

        #endregion

        #region Generation

        /// <summary>
        /// Method generates a random string with a given number of characters
        /// </summary>
        public static string GenerateRandomString(int numChars)
        {
            string result = string.Empty;
            Random rnd = new Random();

            for (int i = 0; i < numChars; i++)
            {
                int num = rnd.Next(0, 26);//get random number between 1 and 26 from rnd
                char c = (char)('a' + num);//use number to generate random letter

                result += c;//add random letter to letters
            }

            return result;
        }

        /// <summary>
        /// Method deserializes this JSON string
        /// </summary>
        public static T DeserializeJSON<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        #endregion

        #region Checking

        /// <summary>
        /// Method checks if this string is null or empty and replaces with a default value if it is
        /// </summary>
        public static string OrValue(this string str, string @default = "")
        {
            return String.IsNullOrWhiteSpace(str) ? @default : str;
        }

        /// <summary>
        /// Method checks if this string is a valid URL
        /// </summary>
        public static bool IsURL(this string str)
        {
            return Uri.IsWellFormedUriString(str, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Method checks if the first character of this string is in upper-case
        /// </summary>
        public static bool IsUpperCase(this string word)
        {
            return !String.IsNullOrEmpty(word) && Char.IsUpper(word[0]);
        }

        /// <summary>
        /// Method checks for equality between this string and a given string regardless of case
        /// </summary>
        public static bool EqualsIgnoreCase(this string str, string input)
        {
            return str.Equals(input, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Method checks for containment between this string and a given string regardless of case
        /// </summary>
        public static bool ContainsIgnoreCase(this string str, string input)
        {
            return str.ToLower().Contains(input.ToLower());
        }

        /// <summary>
        /// Method determines whether this string contains each of the elements in a given set of strings
        /// </summary>
        public static bool ContainsMany(this string str, string[] bits)
        {
            return bits.All(bit => str.Contains(bit));
        }

        /// <summary>
        /// Method determines whether this string contains any of the elements in a given set of strings
        /// </summary>
        public static bool ContainsAny(this string str, IEnumerable<string> bits)
        {
            return bits.Any(bt => str.ContainsIgnoreCase(bt));
        }

        /// <summary>
        /// Method determines whether this string equals any of the elements in a given set of strings
        /// </summary>
        public static bool EqualsAny(this string str, IEnumerable<string> bits)
        {
            return bits.Any(bt => str.EqualsIgnoreCase(bt));
        }

        /// <summary>
        /// Method checks for starts with between this string and a given string regardless of case
        /// </summary>
        public static bool StartsWithIgnoreCase(this string str, string input)
        {
            return str.ToLower().StartsWith(input.ToLower());
        }

        /// <summary>
        /// Method checks for ends with between this string and a given string regardless of case
        /// </summary>
        public static bool EndsWithIgnoreCase(this string str, string input)
        {
            return str.ToLower().EndsWith(input.ToLower());
        }

        /// <summary>
        /// Method determines how close this string is compared to a given string
        /// </summary>
        public static double HowCloseTo(this string str, string input)
        {
            char[] mine = str.ToLower().RemoveSpaces().ToCharArray();
            char[] theirs = input.ToLower().RemoveSpaces().ToCharArray();

            int xIndex = 0;
            int yIndex = 0;

            double misses = Math.Abs(mine.Length - theirs.Length);

            while (xIndex < mine.Length && yIndex < theirs.Length)
            {
                if (mine[xIndex] == theirs[yIndex])
                {
                    xIndex++;
                    yIndex++;
                }
                else
                {
                    misses++;

                    if (mine.Length > theirs.Length) xIndex++;
                    else yIndex++;
                }
            }

            var result = 1 - (misses / Math.Max(mine.Length, theirs.Length));
            return result > 0 ? result : 0;
        }

        #endregion

        #region Regex

        /// <summary>
        /// Method returns all matches found of a regex pattern in this string 
        /// </summary>
        public static string[] GetMatches(this string input, string pattern)
        {
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(input);

            string[] output = new string[matches.Count];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = matches[i].Success ? matches[i].Value : string.Empty;
            }

            return output;
        }

        #endregion

        #region Mutating

        /// <summary>
        /// Method converts this string to an int
        /// </summary>
        public static int ToInt(this string input)
        {
            double asDouble = Convert.ToDouble(input);
            return asDouble.ToInt();
        }

        /// <summary>
        /// Method converts this string to a double
        /// </summary>
        public static double ToDouble(this string input)
        {
            return Convert.ToDouble(input);
        }

        /// <summary>
        /// Method converts this string to a DateTime
        /// </summary>
        public static DateTime ToDate(this string input)
        {
            return Convert.ToDateTime(input);
        }

        #endregion

        #region Files

        /// <summary>
        /// Method reads this string as if it were a file
        /// </summary>
        public static string ReadAsFile(this string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Method reads this string as if it were a file with lines
        /// </summary>
        public static string[] ReadAsLinesOfFile(this string path)
        {
            return File.ReadAllLines(path);
        }

        /// <summary>
        /// Method gets the extenion, if it exists, of this string as a file
        /// </summary>
        public static string GetExtension(this string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// Method writes this string out to a file at a given location
        /// </summary>
        public static void WriteToFile(this string text, string path)
        {
            string[] line = new string[] { text };
            File.WriteAllLines(path, line);
        }

        /// <summary>
        /// Method writes this string out to a file on the desktop
        /// </summary>
        public static void WriteToDesktop(this string text, string name)
        {
            string[] line = new string[] { text };
            File.WriteAllLines(Folders.Desktop + "/" + name, line);
        }

        #endregion
    }
}
