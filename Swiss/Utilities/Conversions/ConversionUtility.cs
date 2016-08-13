namespace Swiss
{
    /// <summary>
    /// Utility class for basic numeric or other conversions
    /// </summary>
    public class ConversionUtility
    {
        public static double FeetToMeters(double feet) { return feet * 0.304800610; }
        public static double MetersToFeet(double meters) { return meters / 0.304800610; }

        public static double FeetToCentimeters(double feet) { return InchesToCentimeters(feet * 12); }
        public static double CentimetersToFeet(double centimeters){ return CentimetersToInches(centimeters) / 12; }

        public static double InchesToCentimeters(double inches) { return inches * 2.54; }
        public static double CentimetersToInches(double centimeters) { return centimeters / 2.54; }

        public static double MilesToKilometers(double miles) { return miles * 1.609344; }
        public static double KilometersToMiles(double kilometers){ return kilometers * 0.621371192; }
    }
}
