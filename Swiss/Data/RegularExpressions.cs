namespace Swiss
{
    /// <summary>
    /// Class holds data for various commonly used Regex patterns
    /// </summary>
    public class RegexPatterns
    {
        public static readonly string Digits = @"\D+";
        public static readonly string WhiteSpace = @"\s+";
        public static readonly string Word = @"\W";
        public static readonly string NonWordChar = @"[^\w\.@-]";

        public static readonly string HREFs = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
        public static readonly string Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public static readonly string Date = @"^((0[1-9]|1[012])[- /.]0?[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$";

        public static readonly string DoubleQuotes = "\"([^\"]*)\"";
        public static readonly string SingleQuotes = "'([^']*)'";

        public static readonly string Guid = @"[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        public static readonly string Questioners = "^who |^is|^what |^when |^where |^why |^how |[?]";

        #region HTML

        public static readonly string LinkHtml = @"(<a.*?>.*?</a>)";
        public static readonly string HrefHtml = @"href=\""(.*?)\""";
        public static readonly string TableHtml = @"<table[^>]*>(.*)</table>";

        #endregion
    }
}
