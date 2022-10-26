namespace Kursach;

public interface IPlayer
{
    public Point GetMove(Game game);

    public Point GetFirstMove(Game game);

    public string GetName();
}
