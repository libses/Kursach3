namespace Kursach
{
    public class Node
    {
        public bool IsMine;
        public Point Point;
        public List<Node> Children;
        public int D;
        public int Number;
        public List<Point> Visited;
        public Node(int number, Point point, bool isMine, bool isFirstMove)
        {
            Number = number;
            Children = new List<Node>(4);
            Point = point;
            Visited = new List<Point>(16);
            if (!isFirstMove)
            {
                Visited.Add(point);
            }
            
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
