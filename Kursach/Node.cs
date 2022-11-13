namespace Kursach
{
    public class Node
    {
        public Point Point { get; set; }
        public List<Node> Children { get; set; }
        public Node? Parent { get; set; }
        public int Number { get; set; }
        public int Depth { get; set; }
        public HashSet<Point> Visited = new HashSet<Point>();
        public Node(int number, Point point, int depth)
        {
            Number = number;
            Children = new List<Node>();
            Point = point;
            Depth = depth;
            Visited.Add(point);
        }

        public int GetValue()
        {
            var coeff = Depth % 2 == 0 ? 1 : -1;
            if (Children.Count == 0)
            {
                return coeff * Number;
            }

            return coeff * Number + Children.Select(x => x.GetValue()).Max();
        }

        public void AddParent(Node parent)
        {
            Visited.UnionWith(parent.Visited);
            parent.Children.Add(this);
            Parent = parent;
        }

        public override string ToString()
        {
            return $"{Point}, Number: {Number}";
        }
    }
}
