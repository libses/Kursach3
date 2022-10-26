namespace Kursach;

public class Cell
{
    public bool IsVisited { get; set; }
    public int Number { get; set; }

    public Cell(int n)
    {
        Number = n;
        IsVisited = false;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}
