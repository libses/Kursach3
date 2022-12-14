namespace Kursach;

public class Cell
{
    public bool IsVisited;
    public byte Number;

    public Cell(byte n)
    {
        Number = n;
        IsVisited = false;
    }

    public override string ToString()
    {
        return Number.ToString();
    }
}
