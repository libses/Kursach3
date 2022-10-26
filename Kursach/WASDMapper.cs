namespace Kursach;

public class WASDMapper : IKeyboardMapper
{
    public Point Map(string key, Point current)
    {
        return key.ToUpper() switch
        {
            "A" => new Point(-1, 0) + current,
            "D" => new Point(1, 0) + current,
            "W" => new Point(0, -1) + current,
            "S" => new Point(0, 1) + current,
            _ => throw new Exception()
        };
    }
}
