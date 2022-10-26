namespace Kursach;

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public bool IsInsideRectangle(int x1, int x2, int y1, int y2)
    {
        var xMin = Math.Min(x1, x2);
        var yMin = Math.Min(y1, y2);
        var xMax = Math.Max(x1, x2);
        var yMax = Math.Max(y1, y2);
        return X >= xMin && X <= xMax && Y >= yMin && Y <= yMax;
    }

    public bool IsInsideField(int size)
    {
        return X >= 0 && X < size && Y >= 0 && Y < size;
    }

    public static Point operator+(Point f, Point s)
    {
        return new Point(f.X + s.X, f.Y + s.Y);
    }

    public static Point operator -(Point f, Point s)
    {
        return new Point(f.X - s.X, f.Y - s.Y);
    }

    public override string ToString()
    {
        return $"X = {X}, Y = {Y}";
    }
}
