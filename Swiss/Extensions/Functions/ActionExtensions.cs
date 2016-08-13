using System;

namespace Swiss
{
    public static class ActionExtensions
    {
        /// <summary>
        /// Method performs this action a given number of times
        /// </summary>
        public static void Do(this Action act, int times = 1)
        {
            for(int i = 0; i < times; i++)
            {
                act.Invoke();
            }
        }
    }
}
