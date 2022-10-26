namespace Kursach;

public enum Dirs
{
    SS, SB, BS, BB
}

public static class AI
{
    public static Point MaxDifference(Game game)
    {
        var validNearPoints = GetGoodPoints(game);
        var sweetSpot = validNearPoints
            .Select(x => (x, x.GetNearGoodPoints(game).Any() ? x.GetNearGoodPoints(game).Max(x => game.Field[x].Number) : 10))
            .MaxBy(x => game.Field[x.x].Number - x.Item2).x;
        return sweetSpot;
    }

    public static Point SmartAI(Game game)
    {
        var cp = game.IsFirstMove ? new Point(game.Field.N, game.Field.N) : game.CurrentPoint;
        //var myself = game.Players[game.playerCounter];
        //var mySum = game.PlayerVisited[myself].Sum(x => x.Number);
        //var other = game.Players[(game.playerCounter + 1) % 2];
        //var hisSum = game.PlayerVisited[other].Sum(x => x.Number);
        //var diff = mySum - hisSum;
        (Point, int) sth = (new Point(0, 0), 0);
        for (int i = 4; i < 20; i += 2)
        {
            sth = DiveIntoM(i, cp, game);
            Console.WriteLine(sth.Item2);
            if (sth.Item2 > 0)
            {
                Console.WriteLine("return");
                return sth.Item1;
            }
        }

        Console.WriteLine("return");
        return sth.Item1;
    }

    public static Point Max(Game game)
    {
        var goodPoints = GetGoodPoints(game);
        return goodPoints.Any() ? goodPoints.MaxBy(x => game.Field[x].Number) : throw new Exception();
    }
    
    public static Point Random(Game game)
    {
        var r = new Random();
        var goodPoints = GetGoodPoints(game);
        if (!goodPoints.Any()) throw new Exception();
        var i = r.Next(0, goodPoints.Length);
        return goodPoints[i];
    }

    public static Point MaxDifferenceFourSlices(Game game)
    {
        var cp = game.IsFirstMove ? new Point(game.Field.N, game.Field.N) : game.CurrentPoint;
        var sth = DiveInto(6, cp, game);
        return sth.Item1;
    }

    public static Point DeepPurple(Game game)
    {
        //minmax?
        var queue = new Queue<Node>();
        var start = game.CurrentPoint ?? new Point(game.Field.N, game.Field.N);
        var startNode = new Node(0, start);
        queue.Enqueue(startNode);
        int depthValue = 1;
        int depth = 0;
        while (depth < 6 && queue.Count > 0)
        {
            var node = queue.Dequeue();
            depthValue--;
            var nicePoints = new Point[1];
            if (game.IsFirstMove)
            {
                nicePoints = node.Point.GetNearGoodFirstPoints(game);
                game.IsFirstMove = false;
            }
            else
            {
                nicePoints = node.Point.GetNearGoodPoints(game);
            }

            foreach (var point in nicePoints)
            {
                var coeff = depth % 2 == 0 ? 1 : -1;
                var number = point.GetNumber(game);
                var childNode = new Node(coeff * number, point);
                childNode.AddParent(node);
                queue.Enqueue(childNode);
            }

            if (depthValue == 0)
            {
                depthValue = queue.Count;
                depth++;
            }
        }

        var nodeR = startNode.Children.MaxBy(x => x.Number);
        return nodeR.Point;
    }

    private static (Point, int) DiveInto(int depth, Point point, Game game)
    {
        //враг может выиграть засчёт того что рано закончит.
        //четность клеток и первый/стартовый ход влияет
        //логика диагоналей. это черно-белая доска.
        //у кого больше клеток -- у того больше шанс вина
        //логика проигрывания -- улучшение алгосика
        var points = game.IsFirstMove ? GetStartPoints(game) : point.GetNearGoodPoints(game);
        game.IsFirstMove = false;

        //STARTING DEPTH IS 0 2 4 6 8...
        if (depth == 0 || points.Length == 0)
        {
            return (point, point.GetNumber(game));
        }
        else
        {
            var startingSum = depth % 2 == 0 ? point.GetNumber(game) : -point.GetNumber(game);
            var max = int.MinValue;
            Point innerGood = points.First();

            foreach (var innerPoint in points)
            {
                var result = DiveInto(depth - 1, innerPoint, game);
                if (result.Item2 > max)
                {
                    max = result.Item2;
                    innerGood = result.Item1;
                }
            }
            //возможно подыгрывает врагу. надо написать два варианта и сравнить.
            return (innerGood, max + startingSum);
        }
    }

    private static (Point, int) DiveIntoM(int depth, Point point, Game game)
    {
        //враг может выиграть засчёт того что рано закончит.
        //четность клеток и первый/стартовый ход влияет
        //логика диагоналей. это черно-белая доска.
        //у кого больше клеток -- у того больше шанс вина
        //логика проигрывания -- улучшение алгосика
        var points = game.IsFirstMove ? GetStartPoints(game) : point.GetNearGoodPoints(game);
        game.IsFirstMove = false;

        //STARTING DEPTH IS 0 2 4 6 8...
        if (depth == 0 || points.Length == 0)
        {
            return (point, point.GetNumber(game));
        }
        else
        {
            var startingSum = depth % 2 == 0 ? point.GetNumber(game) : -point.GetNumber(game);
            var max = int.MinValue;
            Point innerGood = points.First();

            foreach (var innerPoint in points)
            {
                var result = DiveInto(depth - 1, innerPoint, game);
                if (result.Item2 > max)
                {
                    max = result.Item2;
                    innerGood = result.Item1;
                }
            }

            return (innerGood, max + startingSum);
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

public static class GameExtensions
{
    public static Point[] GetGoodNearPoints(this Game game)
    {
        return game.CurrentPoint.GetNearGoodPoints(game);
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

    static void Main(string[] args)
    {
        var aiPlayers = new Dictionary<AIPlayer, int>()
        {
            //{ new AIPlayer(AI.Max, "Максимальная клетка"), 0 },
            //{ new AIPlayer(AI.Random, "Рандом"), 0 },
            //{ new AIPlayer(AI.MaxDifference, "Максимальное матожидание одного хода"), 0 },
            { new AIPlayer(AI.MaxDifferenceFourSlices, "Максимальное ожидание нескольких ходов"), 0 },
            { new AIPlayer(AI.DeepPurple, "DeepPurple AI"), 0 }
        };


        var gamesPlayed = 100;
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
