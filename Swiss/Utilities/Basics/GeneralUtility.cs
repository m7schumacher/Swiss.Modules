using System;

namespace Swiss
{
    public class GeneralUtility
    {
        public static K SetNullSafe<T, K>(T input, Func<T,K> transform)
        {
            if (input != null)
            {
                return transform(input);
            }
            else
            {
                return default(K);
            }
        }
    }
}
