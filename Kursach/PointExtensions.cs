namespace Kursach;

public static class PointExtensions 
{
    public static Point[] Directions = new Point[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0) };
    public static Point[] FirstDirections = new Point[] { new(0, 1), new(0, -1), new(1, 0), new(-1, 0), new(1, 1), new(1, -1), new(-1, 1), new(-1, -1), new(0, 0) };
    public static Point[] GetNearPoints(this Point point)
    {
        return Directions.Select(x => x + point).ToArray();
    }

    public static Point[] GetNearFirstPoints(this Point point)
    {
        return FirstDirections.Select(x => x + point).ToArray();
    }

    public static Point[] GetNearGoodPoints(this Point point, Game game)
    {
        return point.GetNearPoints().Where(x => x.IsInsideField(game.Field.Size) && !game.Field.Map[x.X, x.Y].IsVisited).ToArray();
    }

    public static Point[] GetNearGoodFirstPoints(this Point point, Game game)
    {
        return point.GetNearFirstPoints().Where(x => x.IsInsideField(game.Field.Size) && !game.Field.Map[x.X, x.Y].IsVisited).ToArray();
    }

    public static int GetNumber(this Point point, Game game)
    {
        return game.Field[point].Number;
    }
}
