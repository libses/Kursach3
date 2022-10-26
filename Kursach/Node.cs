namespace Kursach
{
    public class Node
    {
        public Point Point { get; set; }
        public List<Node> Children { get; set; }
        public Node? Parent { get; set; }
        private int _number;
        public int Number { get
            {
                if (Children.Count == 0) return _number;
                if (_number < 0)
                {
                    return _number + Children.Max(x => x.Number);
                }
                else
                {
                    return _number + Children.Min(x => x.Number);
                }
               
            }
            set
            {
                _number = value;
            }
        }

        public Node(int number)
        {
            Number = number;
            Children = new List<Node>();
        }

        public Node(int number, Point point)
        {
            Number = number;
            Children = new List<Node>();
            Point = point;
        }

        public void AddParent(Node parent)
        {
            parent.Children.Add(this);
            Parent = parent;
        }
    }
}
