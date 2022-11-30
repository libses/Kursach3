namespace Kursach
{
    public class Node
    {
        public bool IsMine { get; set; }
        public Point Point { get; set; }
        public List<Node> Children { get; set; }
        public int D { get; set; }
        public int Number { get; set; }
        public List<Point> Visited;
        public Node(int number, Point point, bool isMine, bool isFirstMove)
        {
            Number = number;
            Children = new List<Node>();
            Point = point;
            Visited = new List<Point>();
            if (!isFirstMove)
            {
                Visited.Add(point);
            }
            
            IsMine = isMine;
        }

        public Node(int number, Point point, bool isMine, bool isFirstMove, Node parent)
        {
            Number = number;
            Children = new List<Node>();
            Point = point;
            Visited = new List<Point>(parent.Visited);
            if (!isFirstMove)
            {
                Visited.Add(point);
            }

            parent.Children.Add(this);
            IsMine = isMine;
        }


        public void AddParent(Node parent)
        {
            Visited.AddRange(parent.Visited);
            parent.Children.Add(this);
        }

        public void CountAllPaths()
        {
            foreach (var child in Children)
            {
                child.D = child.Number + D;
                child.CountAllPaths();
            }
        }

        public void ClearTrash(Game game)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var current = Children[i];
                if (game.Field[current.Point].IsVisited)
                {
                    Children.RemoveAt(i);
                }
                else
                {
                    current.ClearTrash(game);
                }
            }
        }

        public List<Node> GetAllChilds(List<Node> list, Game game)
        {
            if (Children.Count == 0)
            {
                list.Add(this);
            }
            else
            {
                foreach (var child in Children)
                {
                    child.GetAllChilds(list, game);
                }
            }

            return list;
        }

        public int GetMagicValue()
        {
            if (Children.Count == 0)
            {
                return D;
            }
            else
            {
                if (!IsMine)
                {
                    return Children.Max(x => x.GetMagicValue());
                }
                else
                {
                    return Children.Min(x => x.GetMagicValue());
                }
            }
        }

        public override string ToString()
        {
            return $"{Point}, Number: {Number}";
        }
    }
}
