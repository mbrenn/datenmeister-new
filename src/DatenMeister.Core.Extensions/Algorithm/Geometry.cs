namespace DatenMeister.Core.Extensions.Algorithm;

public class Geometry
{
    /// <summary>
    /// Gets the information whether the two given lines intersect each other
    /// </summary>
    /// <returns>true, if the both lines intersect</returns>
    public static bool Intersect(double p0X, double p0Y, double p1X, double p1Y, double p2X, double p2Y, double p3X,
        double p3Y)
    {
        var s1X = p1X - p0X;
        var s1Y = p1Y - p0Y;
        var s2X = p3X - p2X;
        var s2Y = p3Y - p2Y;

        double s, t;
        s = (-s1Y * (p0X - p2X) + s1X * (p0Y - p2Y)) / (-s2X * s1Y + s1X * s2Y);
        t = (s2X * (p0Y - p2Y) - s2Y * (p0X - p2X)) / (-s2X * s1Y + s1X * s2Y);

        return s >= 0 && s <= 1 && t >= 0 && t <= 1;
    }
}