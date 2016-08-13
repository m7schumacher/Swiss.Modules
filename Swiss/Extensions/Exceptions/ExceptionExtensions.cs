using System;
using System.Text;

namespace Swiss
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Method returns the entire message from this exception
        /// </summary>
        public static string FullMessage(this Exception exc)
        {
            StringBuilder builder = new StringBuilder();

            while(exc != null)
            {
                builder.AppendFormat("{0}{1}", exc.Message, Environment.NewLine);
                exc = exc.InnerException;
            }

            return builder.ToString();
        }
    }
}
