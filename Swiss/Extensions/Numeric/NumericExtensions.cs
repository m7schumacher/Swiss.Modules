using System;
using System.Collections.Generic;
using System.Linq;

namespace Swiss
{
    public static class NumericExtensions
    {
        #region Generation

        /// <summary>
        /// Method returns all multiples of this number less than or equal to a given limit
        /// </summary>
        public static List<int> GenerateMultiples(this int x, int limit) { return x.ToDouble().GenerateMultiples(limit).AllToInt().ToList(); }
        public static List<double> GenerateMultiples(this double x, int limit)
        {
            List<double> multiples = new List<double>();
            int count = (int)(limit / x) + 1;

            for (int i = 1; i < count; i++)
            {
                multiples.Add(x * i);
            }

            return multiples;
        }

        public static double ToDouble(this int x) { return x; }
        public static double ToDouble(this float x) { return (double)x; }

        public static int ToInt(this double x) { return (int)x; }

        /// <summary>
        /// Method rounds this number to a given number of decimal places
        /// </summary>
        public static double Round(this double x, int places) { return Math.Round(x, places); }
        public static double Round(this float x, int places) { return Math.Round(x, places); }

        /// <summary>
        /// Method converts this number to a datetime based on seconds since the epoch
        /// </summary>
        public static DateTime ToDate(this long secondsSinceEpoch)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(secondsSinceEpoch);
        }

        /// <summary>
        /// Method converts this number to a datetime based on seconds since the epoch
        /// </summary>
        public static DateTime ToDate(this int secondsSinceEpoch)
        {
            return ((long)secondsSinceEpoch).ToDate();
        }

        #endregion

        #region Nature

        /// <summary>
        /// Method determines if this number is an outlier given a median, standard deviation, and room for error
        /// </summary>
        public static bool IsOutlier(this double number, double median, double sd, double perc)
        {
            var distanceFromMean = Math.Abs(number - median);
            var percentageOfMean = distanceFromMean / median;

            return percentageOfMean > perc;
        }

        /// <summary>
        /// Method determines if this number is prime
        /// </summary>
        public static bool IsPrime(this int number) { return number.ToDouble().IsPrime(); }
        public static bool IsPrime(this double number)
        {
            if (number < 2) return false;
            if (number % 2 == 0) return (number == 2);

            double root = number.SquareRoot();
            for (int i = 3; i <= root; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }
        
        /// <summary>
        /// Method determines if this number is odd
        /// </summary>
        public static bool IsOdd(this double number) { return number % 2 != 0; }
        public static bool IsOdd(this int number) { return number % 2 != 0; }

        /// <summary>
        /// Method determines if this number is even
        /// </summary>
        public static bool IsEven(this double number) { return number % 2 == 0; }
        public static bool IsEven(this int number) { return number % 2 == 0; }

        /// <summary>
        /// Method determines if this number and another are both even
        /// </summary>
        public static bool BothEven(this int x, int y) { return IsEven(x) && IsEven(y); }
        public static bool BothEven(this double x, double y) { return IsEven(x) && IsEven(y); }

        /// <summary>
        /// Method determines if this number and another are both odd
        /// </summary>
        public static bool BothOdd(this int x, int y) { return IsOdd(x) && IsOdd(y); }
        public static bool BothOdd(this double x, double y) { return IsOdd(x) && IsOdd(y); }

        #endregion

        #region Components

        /// <summary>
        /// Method determines the square root of this number (floor or int)
        /// </summary>
        public static int SquareRoot(this int number) { return (int)Math.Sqrt((double)number); }
        public static double SquareRoot(this double number) { return Math.Sqrt(number); }

        /// <summary>
        /// Method gathers all factors of this number
        /// </summary>
        public static List<int> GetFactors(this int number) { return number.ToDouble().GetFactors().AllToInt().ToList(); }
        public static List<double> GetFactors(this double number)
        {
            List<double> factors = new List<double>();
            factors.Add(1);

            double root = number.SquareRoot();

            for (int i = 2; i < root; i++)
            {
                if (number % i == 0)
                {
                    factors.Add(i);
                    factors.Add(number / i);
                }
            }

            return factors;
        }

        /// <summary>
        /// Method finds the Greatest Common Factor between this number and another
        /// </summary>
        public static int FindGCF(this int x, int y) { return x.ToDouble().FindGCF(y.ToDouble()).ToInt(); }
        public static double FindGCF(this double x, double y)
        {
            double[] arr = new double[] { x, y };

            if (!arr.AllSameNature() || arr.AnyPrimes()) { return 1; }

            double num1 = Math.Min(x, y);
            double num2 = Math.Max(x, y);

            for (int i = num1.ToInt(); i > 0; i -= 2)
            {
                if (arr.AllDivisibleBy(i))
                {
                    return i;
                }
            }

            return 1;
        }

        /// <summary>
        /// Method finds the Least Common Multiple between this number and another
        /// </summary>
        public static int FindLCM(this int x, int y) { return x.ToDouble().FindLCM(y.ToDouble()).ToInt(); }
        public static double FindLCM(this double x, double y)
        {
            double num1, num2;

            if (x > y) { num1 = x; num2 = y; }
            else { num1 = y; num2 = x; }

            for (int i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return num1 * i;
                }
            }

            return num2;
        }

        #endregion

        #region Arithmetic

        //these are utterly pointless
        public static int Sum(this int first, int second) { return first + second; }
        public static double Sum(this double first, double second) { return first + second; }

        /// <summary>
        /// Method squares this number
        /// </summary>
        public static double Square(this int n) { return Math.Pow(n, 2); }
        public static double Square(this double n) { return Math.Pow(n, 2); }

        /// <summary>
        /// Method cubes this number
        /// </summary>
        public static double Cube(this int n) { return Math.Pow(n, 3); }
        public static double Cube(this double n) { return Math.Pow(n, 3); }

        /// <summary>
        /// Method returns the average of this number and another
        /// </summary>
        public static double Average(this int x, int y) { return (x + y) / 2; }
        public static double Average(this double x, double y) { return (x + y) / 2; }

        #endregion
    }
}
