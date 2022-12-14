namespace Kursach;

public static class PointExtensions 
{
    public static Point[] Directions = new Point[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    public static Point[] FirstDirections = new Point[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0), new(1, 1), new(1, -1), new(-1, 1), new(-1, -1), new(0, 0) };
    public static IEnumerable<Point> GetNearPoints(this Point point)
    {
        foreach (var dir in Directions)
        {
            yield return dir + point;
        }
    }

    public static IEnumerable<Point> GetNearFirstPoints(this Point point)
    {
        foreach (var dir in FirstDirections)
        {
            yield return dir + point;
        }
    }

    public static IEnumerable<Point> GetNearGoodPoints(this Point point, Game game)
    {
        var near = point.GetNearPoints();
        foreach (var p in near)
        {
            if (p.IsInsideField(game.Field.Size) && !game.Field[p].IsVisited)
            {
                yield return p;
            }
        }
    }

    public static IEnumerable<Point> GetNearGoodFirstPoints(this Point point, Game game)
    {
        return point.GetNearFirstPoints().Where(x => x.IsInsideField(game.Field.Size) && !game.Field.Map[x.X, x.Y].IsVisited);
    }

    public static int GetNumber(this Point point, Game game)
    {
        return game.Field[point].Number;
    }
}
