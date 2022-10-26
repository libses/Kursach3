namespace Kursach;

public class AIPlayer : IPlayer
{
    private string name;
    public AIPlayer(Func<Game, Point> ai, string name)
    {
        DeterminePoint = ai;
        this.name = name;
    }

    Func<Game, Point> DeterminePoint;
    public Point GetMove(Game game)
    {
        var p = DeterminePoint(game);
        return p;
    }

    public string GetName()
    {
        return name;
    }

    public Point GetFirstMove(Game game)
    {
        throw new NotImplementedException();
    }
}
