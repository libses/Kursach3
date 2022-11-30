namespace Kursach
{
    public class Node
    {
        public bool IsMine { get; set; }
        public Point Point { get; set; }
        public List<Node> Children { get; set; }
        public int D { get; set; }
        public Node? Parent { get; set; }
        public int Number { get; set; }
        public HashSet<Point> Visited = new HashSet<Point>();
        public Node(int number, Point point, bool isMine)
        {
            Number = number;
            Children = new List<Node>();
            Point = point;
            Visited.Add(point);
            IsMine = isMine;
        }

        public void AddParent(Node parent)
        {
            Visited.UnionWith(parent.Visited);
            parent.Children.Add(this);
            Parent = parent;
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

        public void GetAllChilds(List<Node> list, Game game)
        {
            if (Children.Count == 0)
            {
                list.Add(this);
            }
            else
            {
                if (!game.Field[this.Point].IsVisited)
                {
                    foreach (var child in Children)
                    {
                        child.GetAllChilds(list, game);
                    }
                }       
            }
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
