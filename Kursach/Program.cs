namespace Kursach;

public static class AI
{
    public static IEnumerable<Point> Nicest(IEnumerable<Point> points, List<Point> visited)
    {
        foreach (var point in points)
        {
            if (!visited.Contains(point))
            {
                yield return point;
            }
        }
    }

    public static Point DeepPurple(Game game)
    {
        var queue = new Queue<Node>();
        var start = game.CurrentPoint ?? new Point(game.Field.N, game.Field.N);
        var startNode = new Node(0, start, false, game.CurrentPoint == null);
        queue.Enqueue(startNode);
        int depthValue = 1;
        int depth = 0;
        var allNodes = new List<Node>();
        while (depth < 14 && queue.Count > 0)
        {
            var node = queue.Dequeue();
            allNodes.Add(node);
            depthValue--;
            var nicePoints = Nicest(GetNicePoints(game, node), node.Visited);
            foreach (var point in nicePoints)
            {
                var number = depth % 2 == 0 ? point.GetNumber(game) : -point.GetNumber(game);
                var childNode = new Node(number, point, depth % 2 == 0, false);
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
        var res = startNode.Children.MaxBy(x => x.GetMagicValue());

        if (res == null)
        {
            return new Point(-1, -1);
        }

        return res.Point;
    }

    private static IEnumerable<Point> GetNicePoints(Game game, Node node)
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

    private static IEnumerable<Point> GetStartPoints(Game game)
    {
        var res = new Point(game.Field.N, game.Field.N).GetNearGoodFirstPoints(game);
        return res;
    }

    private static IEnumerable<Point> GetGoodPoints(Game game)
    {
        var res = game.IsFirstMove ? GetStartPoints(game) : game.CurrentPoint.GetNearGoodPoints(game);
        return res;
    }
}

public class Program
{
    private static Game game;

    static void Main(string[] args)
    {
        FirstTime();
        var res = AI.DeepPurple(game);
        game.Field[res].IsVisited = true;
        Console.WriteLine($"{res.Y} {res.X}");
        while (true)
        {
            var game = SecondTime();
            res = AI.DeepPurple(game);
            if (res.X != -1)
            {
                game.Field[res].IsVisited = true;
                game.CurrentPoint = res;
            }
            else
            {
                return;
            }

            Console.WriteLine($"{res.Y} {res.X}");
        }
    }

    private static Game FirstTime()
    {
        var n = int.Parse(Console.ReadLine());
        game = new Game((n - 1) / 2);

        for (var y = 0; y < n ; y++)
        {
            var row = Console.ReadLine().Split().Select(x => byte.Parse(x)).ToArray();
            for (int x = 0; x < n; x++)
            {
                game.Field.Map[x, y] = new Cell(row[x]);
                game.Field.Map[x, y].IsVisited = row[x] < 0;
            }
        }

        var inp = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
        if (inp[0] == -1)
        {
            game.IsFirstMove = true;
        }
        else
        {
            game.IsFirstMove = false;
            game.CurrentPoint = new Point(inp[1], inp[0]);
            game.Field[game.CurrentPoint].IsVisited = true;
        }

        return game;
    }
    
    private static Game SecondTime()
    {
        var point = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
        var pointP = new Point(point[1], point[0]);
        game.Field[pointP].IsVisited = true;
        game.CurrentPoint = pointP;
        return game;
    }
}
