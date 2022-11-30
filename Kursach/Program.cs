using System.Xml.Linq;

namespace Kursach;

public enum Dirs
{
    SS, SB, BS, BB
}

public static class AI
{
    public static Point Max(Game game)
    {
        var goodPoints = GetGoodPoints(game);
        return goodPoints.Any() ? goodPoints.MaxBy(x => game.Field[x].Number) : throw new Exception();
    }

    public static Point DeepPurple(Game game)
    {
        //minmax?
        var queue = new Queue<Node>();
        var start = game.CurrentPoint ?? new Point(game.Field.N, game.Field.N);
        var startNode = new Node(0, start, false);
        queue.Enqueue(startNode);
        int depthValue = 1;
        int depth = 0;
        var allNodes = new List<Node>();
        while (depth < 8 && queue.Count > 0)
        {
            var node = queue.Dequeue();
            allNodes.Add(node);
            depthValue--;
            var nicePoints = GetNicePoints(game, node).Where(x => !node.Visited.Contains(x));

            foreach (var point in nicePoints)
            {
                var number = depth % 2 == 0 ? point.GetNumber(game) : -point.GetNumber(game);
                var childNode = new Node(number, point, depth % 2 == 0);
                childNode.AddParent(node);
                queue.Enqueue(childNode);
            }

            if (depthValue == 0)
            {
                depthValue = queue.Count;
                depth++;
            }
        }

        startNode.CountAllPaths();
        return startNode.Children.MaxBy(x => x.GetMagicValue()).Point;
    }

    private static Point[] GetNicePoints(Game game, Node node)
    {
        if (game.IsFirstMove)
        {
            game.IsFirstMove = false;
            return node.Point.GetNearGoodFirstPoints(game);
            
        }
        else
        {
            return node.Point.GetNearGoodPoints(game);
        }

    }

    private static Point[] GetStartPoints(Game game)
    {
        var res = new Point(game.Field.N, game.Field.N).GetNearGoodFirstPoints(game);
        return res;
    }

    private static Point[] GetGoodPoints(Game game)
    {
        var res = game.IsFirstMove ? GetStartPoints(game) : game.CurrentPoint.GetNearGoodPoints(game);
        return res;
    }
}

public class Program
{
    private static Game GetGame()
    {
        var game = new Game(5);
        game.IsSilent = true;
        //game.CurrentPoint = new Point(5, 5);
        //game.Field.Map[game.CurrentPoint.X, game.CurrentPoint.Y].IsVisited = true;
        return game;
    }

    private static int[,] GetMatrix()
    {
        var matrix = new int[11, 11];
        for (int y = 0; y < 11; y++)
        {
            var temp = Console.ReadLine().Split().Select(int.Parse).ToArray();
            for (int x = 0; x < 11; x++)
            {
                matrix[x, y] = temp[x];
            }
        }

        return matrix;
    }

    static void Main(string[] args)
    {
        var aiPlayers = new Dictionary<AIPlayer, int>()
        {
            { new AIPlayer(AI.Max, "Максимальная клетка"), 0 },
            { new AIPlayer(AI.DeepPurple, "DeepPurple AI"), 0 },
        };


        var gamesPlayed = 20;
        foreach (var firstPlayer in aiPlayers.Keys)
        {
            foreach (var secondPlayer in aiPlayers.Keys)
            {
                if (firstPlayer == secondPlayer)
                {
                    continue;
                }

                Console.WriteLine();
                for (int i = 0; i < gamesPlayed; i++)
                {
                    var game = GetGame();
                    game.RegisterPlayer(firstPlayer);
                    game.RegisterPlayer(secondPlayer);
                    game.StartGame();
                    var firstSum = game.PlayerVisited[firstPlayer].Sum(x => x.Number);
                    var secondSum = game.PlayerVisited[secondPlayer].Sum(x => x.Number);
                    if (firstSum > secondSum)
                    {
                        aiPlayers[firstPlayer]++;
                    }

                    if (secondSum > firstSum)
                    {
                        aiPlayers[secondPlayer]++;
                    }
                }
            }
        }

        foreach (var player in aiPlayers.Keys)
        {
            var winrate = (double)aiPlayers[player] / (gamesPlayed * (aiPlayers.Count) * (aiPlayers.Count - 1));
            Console.WriteLine($"Winrate of {player.GetName()} is {winrate}");
        }
    }
}
