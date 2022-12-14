using System.Text;

namespace Kursach;

public class Game
{
    public bool IsSilent;
    public Point CurrentPoint;
    public Field Field;

    public bool IsFirstMove;

    public List<IPlayer> Players;
    public int playerCounter;
    public Dictionary<IPlayer, List<Cell>> PlayerVisited = new Dictionary<IPlayer, List<Cell>>();

    public Game(int n)
    {
        Field = new Field(n);
        IsFirstMove = true;
        Players = new List<IPlayer>();
    }

    public Game(byte[,] matrix)
    {
        Field = new Field(matrix);
        IsFirstMove = true;
        Players = new List<IPlayer>();
    }

    public void RegisterPlayer(IPlayer player)
    {
        Players.Add(player);
        PlayerVisited.Add(player, new List<Cell>());
    }

    private void AskForMove()
    {
        var player = Players[playerCounter];
        var point = player.GetMove(this);
        if (CurrentPoint != null)
        {
            if ((Math.Abs(CurrentPoint.X - point.X) + Math.Abs(CurrentPoint.Y - point.Y)) > 1)
            {
                throw new Exception(player.GetName() + "мухлюет");
            }
        }
        if (!IsSilent)
        {
            Console.WriteLine($"{player.GetName()} ходит");
        }

        PlayerVisited[player].Add(Field.Map[point.X, point.Y]);
        if (Field.Map[point.X, point.Y].IsVisited)
        {
            throw new Exception("VISITED");
        }

        Field.Map[point.X, point.Y].IsVisited = true;
        CurrentPoint = point;
        playerCounter = (playerCounter + 1) % Players.Count;
    }

    public void StartGame()
    {
        AskForMove();
        IsFirstMove = false;
        while (!IsEndPoint())
        {
            if (IsSilent)
            {

            }
            else
            {
                Console.WriteLine(this);
            }

            AskForMove();
        }

        foreach (var player in Players)
        {
            if (!IsSilent)
            {
                Console.WriteLine(player.GetName());
                Console.WriteLine(PlayerVisited[player].Sum(x => x.Number));
                Console.WriteLine();
            }
        }
    }

    private bool IsEndPoint()
    {
        return !CurrentPoint
            .GetNearPoints()
            .Where(x => x.IsInsideField(Field.Size))
            .Where(x => !Field.Map[x.X, x.Y].IsVisited)
            .Any();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < Field.Size; y++)
        {
            for (var x = 0; x < Field.Size; x++)
            {
                var onCurrent = CurrentPoint.X == x && CurrentPoint.Y == y;
                var specialSymbol = onCurrent ? "*" : " ";
                var number = Field.Map[x, y].IsVisited && !onCurrent ? " " : Field.Map[x, y].ToString();
                sb.Append(specialSymbol + number + specialSymbol);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
