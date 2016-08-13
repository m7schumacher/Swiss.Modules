using System;

namespace Swiss
{
    public static class BooleanExtensions
    {
        /// <summary>
        /// Method returns the result of a function if the boolean is true and the result of another if false
        /// </summary>
        public static T ReturnIfTrue<T>(this bool b, Func<T> isTrue, Func<T> isFalse)
        {
            Func<T> result = b ? isTrue : isFalse;
            return result();
        }

        /// <summary>
        /// Method returns the result of a function if the boolean is false and the result of another if true
        /// </summary>
        public static T ReturnIfFalse<T>(this bool b, Func<T> isTrue, Func<T> isFalse)
        {
            Func<T> result = b ? isFalse : isTrue;
            return result();
        }

        /// <summary>
        /// Method performs a given action if the boolean is true and performs another if false
        /// </summary>
        public static void DoIfTrue(this bool b, Action isTrue, Action isFalse = null)
        {
            Action result = b ? isTrue : isFalse;

            if(result != null)
            {
                result.Invoke();
            }
        }

        /// <summary>
        /// Method performs a given action if the boolean is false and performs another if true
        /// </summary>
        public static void DoIfFalse(this bool b, Action isFalse, Action isTrue = null)
        {
            Action result = !b ? isFalse : isTrue;

            if (result != null)
            {
                result.Invoke();
            }
        }
    }
}
